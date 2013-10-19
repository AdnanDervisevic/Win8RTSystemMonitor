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

namespace Veritas.Pages.PLC
{

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PLC : Page
    {
        XmlDocument xmlDocument;

        public PLC()
        {
            this.loadXML("http://193.10.237.105");
            this.InitializeComponent();

            DispatcherTimer xmlTimer = new DispatcherTimer();
            xmlTimer.Interval = TimeSpan.FromSeconds(70);
            xmlTimer.Tick += xmlTimer_Tick;
            xmlTimer.Start();

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
                
                if (id == 3)
                {
                    name = "";
                    name += node.GetElementsByTagName("name").Item(0).InnerText;
                    name += " " + node.GetElementsByTagName("machineType").Item(0).InnerText;

                    int larm = Convert.ToInt32(((XmlElement)info).GetElementsByTagName("plcAlarm").Item(0).InnerText);
                    int temp = Convert.ToInt32(((XmlElement)info).GetElementsByTagName("plcTemp").Item(0).InnerText);

                    string weatherType = ((XmlElement)((XmlElement)info).GetElementsByTagName("weather").Item(0)).GetElementsByTagName("type").Item(0).InnerText;
                    int weatherTemp = int.Parse(((XmlElement)((XmlElement)info).GetElementsByTagName("weather").Item(0)).GetElementsByTagName("temp").Item(0).InnerText);

                    string date = ((XmlElement)info).GetAttribute("date");

                    tempController.setTemp(temp);
                    this.headerControl.ServerName = name;
                    

                    this.weatherLbl.Text = weatherType;
                    this.weatherTempLbl.Text = weatherTemp.ToString() + "°C";
                    this.lastUpdate.Text = "Last Update: " + date;

                    time = Convert.ToDateTime(date);

                    if (time < DateTime.Now.AddMinutes(-10.0))
                    {
                        this.Background = new SolidColorBrush(Colors.DarkRed);
                    }

                    if (larm == 1)
                    {
                        larmOff.Visibility = Visibility.Collapsed;
                        larmOn.Visibility = Visibility.Visible;
                    }

                    else if (larm == 0)
                    {
                        larmOn.Visibility = Visibility.Collapsed;
                        larmOff.Visibility = Visibility.Visible;
                    }
                }
            }
        }

        void headerControl_OnHistoryClick(object sender, EventArgs e)
        {
            
             if (this.Frame != null)
                this.Frame.Navigate(typeof(PLCHistory));
             
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
    }
}
