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

namespace Veritas.Pages.TrollhättanServer
{

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class thnServer : Page
    {
        XmlDocument xmlDocument;

        public thnServer()
        {
            this.loadXML("http://193.10.237.105");
            this.InitializeComponent();

            DispatcherTimer xmlTimer = new DispatcherTimer();
            xmlTimer.Interval = TimeSpan.FromSeconds(70);
            xmlTimer.Tick += xmlTimer_Tick;
            xmlTimer.Start();
            
            this.headerControl.Foreground = Colors.DodgerBlue;
            this.cpuControl.ChangeColor(Colors.DodgerBlue);

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
                
                // Om den är på England servern
                if (id == 2)
                {
                    name = "";
                    name += node.GetElementsByTagName("name").Item(0).InnerText;
                    name += " " + node.GetElementsByTagName("machineType").Item(0).InnerText;

                    double memFree = Convert.ToInt32(((XmlElement)info).GetElementsByTagName("memoryFree").Item(0).InnerText);
                    double memTotal = Convert.ToInt32(((XmlElement)info).GetElementsByTagName("memoryTotal").Item(0).InnerText);

                    int swapUse = Convert.ToInt32(((XmlElement)info).GetElementsByTagName("swapUse").Item(0).InnerText);
                    int swapTotal = Convert.ToInt32(((XmlElement)info).GetElementsByTagName("swapTotal").Item(0).InnerText);
                    int diskFree = Convert.ToInt32(((XmlElement)info).GetElementsByTagName("diskFree").Item(0).InnerText);
                    int diskTotal = Convert.ToInt32(((XmlElement)info).GetElementsByTagName("diskTotal").Item(0).InnerText);

                    string date = ((XmlElement)info).GetAttribute("date");
                    double cpuIdle = double.Parse((((XmlElement)info).GetElementsByTagName("cpuIdle").Item(0).InnerText).TrimEnd(new Char[] { '%' }));
                    string weatherType = ((XmlElement)((XmlElement)info).GetElementsByTagName("weather").Item(0)).GetElementsByTagName("type").Item(0).InnerText;
                    int weatherTemp = int.Parse(((XmlElement)((XmlElement)info).GetElementsByTagName("weather").Item(0)).GetElementsByTagName("temp").Item(0).InnerText);
                    double cpuUsage = cpuIdle / 100;

                    double netIn = double.Parse(((XmlElement)info).GetElementsByTagName("netIn").Item(0).InnerText);
                    double netOut = double.Parse(((XmlElement)info).GetElementsByTagName("netOut").Item(0).InnerText);


                    time = Convert.ToDateTime(date);

                    // Beräknar swapminne ledigt och använt minne
                    int swapFree = swapTotal - swapUse;
                    double memUsed = memTotal - memFree;
                    
                    // UPDATE
                    this.headerControl.ServerName = name;
                    this.lastUpdate.Text = "Last Update: " + date;
                    this.cpuControl.stapleGraph(1.00-cpuUsage);

                    this.memControl.SetTitle("Memory Usage: " + Math.Round(memUsed / (memFree + memUsed) * 100) + "%");
                    this.memControl.SetItems(new NameValueItem("Used", (int)memUsed), new NameValueItem("Unused", (int)memFree));

                    this.hddControl.SetTitle("HDD Usage: " + Math.Round(((double)diskTotal-(double)diskFree) / (double)diskTotal * 100) + "%");
                    this.hddControl.SetItems(new NameValueItem("Used", diskTotal-diskFree), new NameValueItem("Unused", diskFree));
                        
                    this.swapChart.SetTitle("SwapMem Usage: " + Math.Round((double)swapUse / (double)swapTotal * 100, 2) + "%");
                    this.swapChart.SetItems(new NameValueItem("Used", swapUse), new NameValueItem("Unused", swapFree));

                    this.netIN.Text = "Network in: " + netIn;
                    
                    this.netOUT.Text = "Network out: " + netOut;

                    this.weatherLbl.Text = weatherType;
                    this.weatherTempLbl.Text = weatherTemp.ToString() + "°C";
                }
            }
        }

        void headerControl_OnHistoryClick(object sender, EventArgs e)
        {
            if (this.Frame != null)
                this.Frame.Navigate(typeof(ThnHistory));
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
