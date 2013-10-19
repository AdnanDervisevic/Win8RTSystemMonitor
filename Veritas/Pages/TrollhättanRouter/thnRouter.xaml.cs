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
using Windows.System.Threading;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Veritas.Pages.TrollhättanRouter
{

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class thnRouter : Page
    {
        XmlDocument xmlDocument;
        bool loaded;

        public thnRouter()
        {
            this.loadXML("http://193.10.237.105");
            this.InitializeComponent();

            DispatcherTimer xmlTimer = new DispatcherTimer();
            xmlTimer.Interval = TimeSpan.FromSeconds(70);
            xmlTimer.Tick += xmlTimer_Tick;
            xmlTimer.Start();

            networkInfoController.HideHistory();
            networkInfoController.RXRecievedSinceLabel.Visibility = Visibility.Collapsed;
            networkInfoController.lblRXRecieved.Visibility = Visibility.Collapsed;

            this.headerControl.Foreground = Colors.HotPink;

            this.headerControl.OnBackClick += headerControl_OnBackClick;
            this.headerControl.OnHistoryClick += headerControl_OnHistoryClick;
        }

        void xmlTimer_Tick(object sender, object e)
        {
            this.loadXML("http://193.10.237.105");
        }

        public async void loadXML(string URL)
        {
            XmlLoadSettings loadSettings = new XmlLoadSettings();
            // Error annars om man inte enablar DTD
            loadSettings.ProhibitDtd = false;
            loadSettings.ResolveExternals = true;
            
            xmlDocument = await XmlDocument.LoadFromUriAsync(new Uri(URL), loadSettings);
            
            // Kopplar mot XML och hämtar från "machine" 
            XmlNodeList nodelist = xmlDocument.GetElementsByTagName("machine");
            // Lagring för id och startar namn string
            int id;
            String name = "";
            // Tid från server ska in här
            DateTime time;
            
            // Kör igenom alla maskiner
            for (int i = 0; i < nodelist.Length; i++)
            {
                // Skapar en nod från nodlistan för maskin av nummer i
                XmlElement node = (XmlElement)nodelist[i];
                id = Convert.ToInt16((node).GetAttribute("id").ToString());

                // skapar en ny nod för info taggen inne i en maskin
                IXmlNode info = ((XmlElement)nodelist[i]).GetElementsByTagName("info").Item(0);

                if (id == 4)
                {
                    networkInfoController.Clear();
                    string date = ((XmlElement)info).GetAttribute("date");
                    name = "";
                    name += "Trollhättan";
                    name += " " + node.GetElementsByTagName("machineType").Item(0).InnerText;
                    time = Convert.ToDateTime(date);

                    string[] daysSplit = node.GetElementsByTagName("upTime").Item(0).InnerText.Split('d');
                    string[] timeSplit = daysSplit[1].Split(',')[1].Split(':');

                    int days = int.Parse(daysSplit[0].Trim()) * 60 * 60 * 24;
                    int hours = int.Parse(timeSplit[0].Trim()) * 60 * 60;
                    int minutes = int.Parse(timeSplit[1].Trim()) * 60;
                    int seconds = int.Parse(timeSplit[1].Trim());

                    int total = days + hours + minutes + seconds;

                    this.uptimeController.updateUptime(total);
                    this.lastUpdate.Text = "Last Update: " + date;

                    // Kollar igenom all status på alla ethernet interfaces från routern.
                    for (int x = 0; x < ((XmlElement)info).GetElementsByTagName("interface").Count; x++)
                    {
                        string status = ((XmlElement)info).GetElementsByTagName("status").Item((uint)x).InnerText;
                        string interfaceName = ((XmlElement)info).GetElementsByTagName("interfaceName").Item((uint)x).InnerText;
                        long txBytes = long.Parse(((XmlElement)info).GetElementsByTagName("bytesSent").Item((uint)x).InnerText);

                        ethernetCombobox.Items.Add(interfaceName);
                        loaded = false;
                        ethernetCombobox.SelectedIndex = 0;

                        if (x > 0)
                            continue;

                        networkInfoController.AddTxBytes(time, txBytes);

                        if (status.Trim().Equals("up"))
                        {
                            serverStatus.Text = "Up";
                            serverStatus.Foreground = new SolidColorBrush(Colors.Green);
                        }
                        else
                        {
                            serverStatus.Text = "Down";
                            serverStatus.Foreground = new SolidColorBrush(Colors.Red);
                        }
                    }

                    //Hämtning
                    string weatherType = ((XmlElement)((XmlElement)info).GetElementsByTagName("weather").Item(0)).GetElementsByTagName("type").Item(0).InnerText;
                    int weatherTemp = int.Parse(((XmlElement)((XmlElement)info).GetElementsByTagName("weather").Item(0)).GetElementsByTagName("temp").Item(0).InnerText);
                    string ip = ((XmlElement)info).GetElementsByTagName("ipNumber").Item(0).InnerText;


                    //Update
                    this.headerControl.ServerName = name;

                    this.ipNumber.Text = ip;

                    this.weatherLbl.Text = weatherType;
                    this.weatherTempLbl.Text = weatherTemp.ToString() + "°C";

                    loaded = true;
                }
            }
        }

        public void checkInterfaces(string URL)
        { 
            if (loaded)
            {
                XmlNodeList nodelist = xmlDocument.GetElementsByTagName("machine");
                // Lagring för id och startar namn string
                int id;
                // Tid från server ska in här
                DateTime time;

                // Kör igenom alla maskiner
                for (int i = 0; i < nodelist.Length; i++)
                {
                    // Skapar en nod från nodlistan för maskin av nummer i
                    XmlElement node = (XmlElement)nodelist[i];
                    id = Convert.ToInt16((node).GetAttribute("id").ToString());

                    // skapar en ny nod för info taggen inne i en maskin
                    IXmlNode info = ((XmlElement)nodelist[i]).GetElementsByTagName("info").Item(0);
                    string date = ((XmlElement)info).GetAttribute("date");

                    if (id == 4)
                    {
                        networkInfoController.Clear();

                        for (int x = 0; x < ((XmlElement)info).GetElementsByTagName("interface").Count; x++)
                        {
                            string status = ((XmlElement)info).GetElementsByTagName("status").Item((uint)x).InnerText;
                            string interfaceName = ((XmlElement)info).GetElementsByTagName("interfaceName").Item((uint)x).InnerText;
                            long txBytes = long.Parse(((XmlElement)info).GetElementsByTagName("bytesSent").Item((uint)x).InnerText);

                            time = Convert.ToDateTime(date);

                            if (interfaceName.Equals(ethernetCombobox.SelectedItem.ToString()))
                            {
                                networkInfoController.AddTxBytes(time.AddSeconds(x), txBytes);

                                if (status.Trim().Equals("up"))
                                {
                                    serverStatus.Text = "Up";
                                    serverStatus.Foreground = new SolidColorBrush(Colors.Green);
                                }
                                else
                                {
                                    serverStatus.Text = "Down";
                                    serverStatus.Foreground = new SolidColorBrush(Colors.Red);
                                }
                            }
                        }
                    }
                }
            }
        }

        void headerControl_OnHistoryClick(object sender, EventArgs e)
        {
             if (this.Frame != null)
                this.Frame.Navigate(typeof(ThnRouterHistory));
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

        private void ethernetCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            checkInterfaces("http://193.10.237.105");
        }
    }
}
