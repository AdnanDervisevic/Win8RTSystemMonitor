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

namespace Veritas.Pages.London
{

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LondonServer : Page
    {
        XmlDocument xmlDocument;

        public LondonServer()
        {
            this.loadXML("http://193.10.237.105");
            this.InitializeComponent();

            DispatcherTimer xmlTimer = new DispatcherTimer();
            xmlTimer.Interval = TimeSpan.FromSeconds(70);
            xmlTimer.Tick += xmlTimer_Tick;
            xmlTimer.Start();
            
            this.headerControl.Foreground = Colors.Crimson;
            this.cpuControl.ChangeColor(Colors.DarkSlateBlue);

            this.networkControl.HideHistory();

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
                if (id == 1)
                {
                    

                    name = "";
                    // Skapar namnet med hjälp av typ och namn på servern. Ändrar sedan titeln inne i noden
                    name += node.GetElementsByTagName("name").Item(0).InnerText;
                    name += " " + node.GetElementsByTagName("machineType").Item(0).InnerText;
                    

                    //Hämtar fritt minne, fritt swapminne, fritt diskutrymme för att skapa ett larm
                    double memFree = Convert.ToDouble(((XmlElement)info).GetElementsByTagName("memoryFree").Item(0).InnerText);
                    double memUsed = Convert.ToDouble(((XmlElement)info).GetElementsByTagName("memoryUse").Item(0).InnerText);

                    string swap = ((XmlElement)info).GetElementsByTagName("swapFree").Item(0).InnerText;
                    string disk = ((XmlElement)info).GetElementsByTagName("diskUse").Item(0).InnerText;
                    int uptime = int.Parse(((XmlElement)info).GetElementsByTagName("upTime").Item(0).InnerText);
                    double cpuUsage = double.Parse(((XmlElement)info).GetElementsByTagName("cpuLoad").Item(0).InnerText);

                    // Hämtar datumet som sen konverteras till timedate
                    string date = ((XmlElement)info).GetAttribute("date");                   
                    time = Convert.ToDateTime(date);

                    //Trimmar % och konverterar swap
                    int diskUse = Convert.ToInt16(disk.TrimEnd(new Char[] { '%' }));
                    int swapFree = Convert.ToInt32(swap);

                    long rxBytes = long.Parse(((XmlElement)info).GetElementsByTagName("rxBytes").Item(0).InnerText);
                    long txBytes = long.Parse(((XmlElement)info).GetElementsByTagName("txBytes").Item(0).InnerText);



                    string weatherType = ((XmlElement)((XmlElement)info).GetElementsByTagName("weather").Item(0)).GetElementsByTagName("type").Item(0).InnerText;
                    int weatherTemp = int.Parse(((XmlElement)((XmlElement)info).GetElementsByTagName("weather").Item(0)).GetElementsByTagName("temp").Item(0).InnerText);

                    // UPDATE

                    this.headerControl.ServerName = name;
                    this.lastUpdate.Text = "Last Update: " + date;
                    this.uptimeControl.updateUptime(uptime);
                    this.cpuControl.stapleGraph(cpuUsage);
                    this.memControl.SetTitle("Memory Usage: " + Math.Round(memUsed / (memFree + memUsed) * 100) + "%");
                    this.memControl.SetItems(new NameValueItem("Used", (int)memUsed), new NameValueItem("Unused", (int)memFree));

                    this.hddControl.SetPercentage("HDD Usage: " + diskUse.ToString() + "%", diskUse);
                    

                    this.networkControl.AddRxBytes(time, rxBytes);
                    this.networkControl.changeRXrecieved();

                    this.networkControl.AddTxBytes(time, txBytes);
                    this.networkControl.changeTXsent();

                    this.swapMemoryLbl.Text = swapFree.ToString();

                    this.weatherLbl.Text = weatherType;
                    this.weatherTempLbl.Text = weatherTemp.ToString() + "°C";
                }
            }
        }

        void headerControl_OnHistoryClick(object sender, EventArgs e)
        {
            if (this.Frame != null)
                this.Frame.Navigate(typeof(LondonHistory));
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
