using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Veritas.Classes;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Veritas.Pages.TrollhättanRouter
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ThnRouterHistory : Page
    {
        const string XML = "http://k11.dt.hv.se/~SGDAS034/generatexml.php";

        XmlDocument xmlDocument;
        DateTime previousDateTime;

        Dictionary<int, List<NameValueItem>> interfaces = new Dictionary<int, List<NameValueItem>>();

        bool loadTimer;

        DateTime from;
        DateTime to;

        private void AddToList(int i, NameValueItem item)
        {
            if (interfaces.ContainsKey(i))
            {
                if (this.interfaces[i].Count == 30)
                {
                    this.interfaces[i].RemoveAt(0);
                }

                this.interfaces[i].Add(item);
            }
            else
            {
                interfaces.Add(i, new List<NameValueItem>());
                this.interfaces[i].Add(item);
            }
        }

        long previousTxBytes;
        bool loaded;

        public ThnRouterHistory()
        {
            this.loadTimer = true;
            from = DateTime.Now.AddHours(-3);
            to = DateTime.Now;
            this.loadXML(XML);
            this.InitializeComponent();

            this.updatesText.Text = "latest updates";
            this.selectedDate.Text = "";

            DispatcherTimer xmlTimer = new DispatcherTimer();
            xmlTimer.Interval = TimeSpan.FromSeconds(70);
            xmlTimer.Tick += xmlTimer_Tick;
            xmlTimer.Start();

            this.headerControl.OnBackClick += headerControl_OnBackClick;
            this.headerControl.historyBtn.Visibility = Visibility.Collapsed;
            this.headerControl.Foreground = Colors.HotPink;
            this.headerControl.ServerName = "Trollhättan Router";

            this.txDeltaChart.SetTitle("TX Bytes Increase");
            this.calenderControl.OnSelectedDateTime += calenderControl_OnSelectedDateTime;
        }

        void calenderControl_OnSelectedDateTime(DateTime dateTime)
        {
            this.calenderControl.Visibility = Visibility.Collapsed;
            this.from = dateTime;
            this.to = dateTime.AddHours(3);
            this.loadTimer = false;
            this.loadXML(XML);
            this.updatesText.Text = "updates from";
            this.selectedDate.Text = dateTime.ToString("yyyy-MM-dd HH:mm");
        }

        private void xmlTimer_Tick(object sender, object e)
        {
            if (loadTimer)
                this.loadXML(XML);
        }

        public async void loadXML(string URL)
        {
            string xmlFile = XML + "?machine=4&from=" + from.ToString("yyyyMMddHH:mm") + "&to=" + to.ToString("yyyyMMddHH:mm");

            XmlLoadSettings loadSettings = new XmlLoadSettings();
            loadSettings.ProhibitDtd = false;
            loadSettings.ResolveExternals = true;

            xmlDocument = await XmlDocument.LoadFromUriAsync(new Uri(xmlFile), loadSettings);

            XmlNodeList machineNodes = xmlDocument.GetElementsByTagName("Machine");

            this.txDeltaChart.Clear();
            this.previousTxBytes = 0;

            for (int i = 0; i < machineNodes.Length; i++)
            {
                DateTime dateTime = Convert.ToDateTime(
                    ((XmlElement)machineNodes[i]).GetElementsByTagName("Date_").Item(0).InnerText + " " +
                    ((XmlElement)machineNodes[i]).GetElementsByTagName("time").Item(0).InnerText);

                if (previousDateTime == dateTime)
                    continue;

                previousDateTime = dateTime;

                XmlNodeList interfaceNodes = ((XmlElement)machineNodes[i]).GetElementsByTagName("interface");

                if (i == 0)
                {
                    for (int x = 0; x < interfaceNodes.Length; x++)
                    {
                        string interfaceName = ((XmlElement)interfaceNodes[x]).GetElementsByTagName("interfaceName").Item(0).InnerText;

                        ethernetCombobox.Items.Add(interfaceName);
                    }
                    ethernetCombobox.SelectedIndex = 0;
                }

                string bytesSent = ((XmlElement)interfaceNodes[0]).GetElementsByTagName("bytesSent").Item(0).InnerText;
                long txBytes = 0;

                if (!bytesSent.Trim().Equals(string.Empty))
                    txBytes = TXDelta(long.Parse(bytesSent));

                
                this.txDeltaChart.AddDataPoints(new NameValueItem(dateTime, txBytes));

                if (!loadTimer)
                    if (this.txDeltaChart.Count() == 30)
                        break;
            }

            loaded = true;
            CheckInterfaces();
        }

        private void CheckInterfaces()
        {
            if (loaded)
            {
                XmlNodeList machineNodes = xmlDocument.GetElementsByTagName("Machine");
                previousDateTime = DateTime.Now;
                this.txDeltaChart.Clear();
                this.previousTxBytes = 0;
                int i = -1;
                for (i = 0; i < machineNodes.Length; i++)
                {
                    DateTime dateTime = Convert.ToDateTime(
                        ((XmlElement)machineNodes[i]).GetElementsByTagName("Date_").Item(0).InnerText + " " +
                        ((XmlElement)machineNodes[i]).GetElementsByTagName("time").Item(0).InnerText);

                    if (previousDateTime == dateTime)
                        continue;

                    previousDateTime = dateTime;

                    XmlNodeList interfaceNodes = ((XmlElement)machineNodes[i]).GetElementsByTagName("interface");

                    string interfaceName = ((XmlElement)interfaceNodes[ethernetCombobox.SelectedIndex]).GetElementsByTagName("interfaceName").Item(0).InnerText;
                    string bytesSent = ((XmlElement)interfaceNodes[ethernetCombobox.SelectedIndex]).GetElementsByTagName("bytesSent").Item(0).InnerText;
                    long txBytes = 0;

                    if (!bytesSent.Trim().Equals(string.Empty))
                        txBytes = TXDelta(long.Parse(bytesSent));
                    
                    this.txDeltaChart.AddDataPoints(new NameValueItem(dateTime, txBytes));

                    if (!loadTimer)
                        if (this.txDeltaChart.Count() == 30)
                            break;
                }

                if (i > 0)
                {
                    this.txDeltaChart.SetLeftLabel(this.txDeltaChart.GetItem(0).DateTime.ToString("HH:mm"));
                    this.txDeltaChart.SetRightLabel(this.txDeltaChart.GetLastItem().DateTime.ToString("HH:mm"));
                }
            }
        }

        public long TXDelta(long txBytes)
        {
            if (previousTxBytes > 0)
            {
                long r = txBytes - previousTxBytes;

                previousTxBytes = txBytes;

                return r;
            }
            else
            {
                previousTxBytes = txBytes;
                return 0;
            }
        }

        private void ethernetCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckInterfaces();
        }

        void headerControl_OnBackClick(object sender, EventArgs e)
        {
            if (this.Frame != null && this.Frame.CanGoBack)
                this.Frame.GoBack();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void calender_Click(object sender, RoutedEventArgs e)
        {
            if (this.calenderControl.Visibility == Visibility.Collapsed)
                this.calenderControl.Visibility = Visibility.Visible;
            else
                this.calenderControl.Visibility = Visibility.Collapsed;
        }
    }
}