using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Veritas.Pages;
using Windows.Data.Xml.Dom;
using Windows.Data.Xml;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;
using Veritas.Pages.London;
using Veritas.Pages.TrollhättanServer;
using Veritas.Pages.PLC;
using Veritas.Pages.TrollhättanRouter;
using Windows.UI.Xaml.Media.Animation;

namespace Veritas
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        //XML nodlista och dokument som används i loadXML
        XmlDocument xmlDocument;
        XmlNodeList nodelist;

        bool mapsOpen = false;
        bool mapsFinished = false;

        public MainPage()
        {
            //Laddar XML en gång innan timer startas
            loadXML("http://193.10.237.105");

            //Startar timer som laddar hem från servern var 5:e sekund
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(70);
            timer.Tick += timer_Tick;
            timer.Start();

            this.InitializeComponent();
            
            // Kopplar alla noder till onClick funktioner
            londonNode.onClick += londonNode_onClick;

            thnNode.onClick += thnNode_onClick;

            thnRNode.onClick += thnRNode_onClick;

            PLCNode.onClick += PLCNode_onClick;
        }

        /// <summary>
        /// Timern kör på den här funktionen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void timer_Tick(object sender, object e)
        {
            // Kör LoadXML när hämtning sker
            loadXML("http://193.10.237.105/"); // Laddar in data från url

        }

        /// <summary>
        /// Laddar XML från URL och ändrar startskärmen baserat på 
        /// </summary>
        /// <param name="URL">String med url</param>
        public async void loadXML(string URL)
        {
            XmlLoadSettings loadSettings = new XmlLoadSettings();
            // Error annars om man inte enablar DTD
            loadSettings.ProhibitDtd = false;
            loadSettings.ResolveExternals = true;

            xmlDocument = await XmlDocument.LoadFromUriAsync(new Uri(URL), loadSettings);

            // Kopplar mot XML och hämtar från "machine" 
            nodelist = xmlDocument.GetElementsByTagName("machine");
            // Lagring för id och startar namn string
            int id;
            String name ="";
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
                    // Anta att servern är online, ändra senare om det behövs
                    londonNode.changeServerStatus("On");
                    // Skapar namnet med hjälp av typ och namn på servern. Ändrar sedan titeln inne i noden
                    name += node.GetElementsByTagName("name").Item(0).InnerText;
                    name += " " + node.GetElementsByTagName("machineType").Item(0).InnerText;
                    londonNode.changeServerName(name);

                    //Hämtar fritt minne, fritt swapminne, fritt diskutrymme för att skapa ett larm
                    int memFree = Convert.ToInt32(((XmlElement)info).GetElementsByTagName("memoryFree").Item(0).InnerText);
                    string swap = ((XmlElement)info).GetElementsByTagName("swapFree").Item(0).InnerText;
                    string disk = ((XmlElement)info).GetElementsByTagName("diskUse").Item(0).InnerText;

                    // Hämtar datumet som sen konverteras till timedate
                    string date = ((XmlElement)info).GetAttribute("date");
                    time = Convert.ToDateTime(date);
                    
                    //Trimmar % och konverterar swap
                    int diskUse = Convert.ToInt16(disk.TrimEnd(new Char[] { '%' }));
                    int swapFree = Convert.ToInt32(swap);

                    // Om servern inte har uppdaterats på 10 minuter anta att den är avstängd/något är fel och behöver undersökas
                    if (time < DateTime.Now.AddMinutes(-10.0))
                    {
                        londonNode.changeServerStatus("Off");
                    }

                    // LARM! Om något är väldigt väldigt fel så skapa en alert som säger att användaren direkt måste kolla servern
                    if (swapFree < 5000 || memFree < 5000 || diskUse > 95)
                        londonNode.changeServerStatus("Alert");
                }

                if (id == 2)
                {
                    name = "";
                    thnNode.changeServerStatus("On");
                    name += node.GetElementsByTagName("name").Item(0).InnerText;
                    name += " " + node.GetElementsByTagName("machineType").Item(0).InnerText;
                    thnNode.changeServerName(name);

                    int memFree = Convert.ToInt32(((XmlElement)info).GetElementsByTagName("memoryFree").Item(0).InnerText);
                    int swapUse = Convert.ToInt32(((XmlElement)info).GetElementsByTagName("swapUse").Item(0).InnerText);
                    int swapTotal = Convert.ToInt32(((XmlElement)info).GetElementsByTagName("swapTotal").Item(0).InnerText);
                    int diskFree = Convert.ToInt32(((XmlElement)info).GetElementsByTagName("diskFree").Item(0).InnerText);
                    string date = ((XmlElement)info).GetAttribute("date");

                    time = Convert.ToDateTime(date);

                    // Beräknar swapminne ledigt pga annan arkitektur på xml i denna
                    int swapFree = swapTotal -swapUse;


                    if (time < DateTime.Now.AddMinutes(-10.0))
                    {
                        thnNode.changeServerStatus("Off");
                    }

                    if (swapFree < 5000 || memFree < 5000 || diskFree < 5000)
                        thnNode.changeServerStatus("Alert");
                }

                if (id == 3)
                {
                    name = "";
                    PLCNode.changeServerStatus("On");
                    name += node.GetElementsByTagName("name").Item(0).InnerText;
                    name += " " + node.GetElementsByTagName("machineType").Item(0).InnerText;
                    PLCNode.changeServerName(name);

                    int larm = Convert.ToInt32(((XmlElement)info).GetElementsByTagName("plcAlarm").Item(0).InnerText);
                    int temp = Convert.ToInt32(((XmlElement)info).GetElementsByTagName("temp").Item(0).InnerText);

                    string date = ((XmlElement)info).GetAttribute("date");

                    time = Convert.ToDateTime(date);

                    if (time < DateTime.Now.AddMinutes(-10.0))
                    {
                        PLCNode.changeServerStatus("Off");
                    }

                    if (larm == 1 || temp > 90)
                        PLCNode.changeServerStatus("Alert");
                }

                if (id == 4)
                {
                    name = "";
                    bool larm = false; 
                    thnRNode.changeServerStatus("On");
                    name += "Trollhättan";
                    name += " " + node.GetElementsByTagName("machineType").Item(0).InnerText;
                    thnRNode.changeServerName(name);

                    // Kollar igenom all status på alla ethernet interfaces från routern.
                    for (int x = 0; x < ((XmlElement)info).GetElementsByTagName("status").Count; x++)
                    {
                        string status = ((XmlElement)info).GetElementsByTagName("status").Item((uint)x).InnerText;

                        if (status.Trim().Equals("up"))
                            larm = false;
                        else
                            larm = true;
                    }

                    string date = ((XmlElement)info).GetAttribute("date");

                    time = Convert.ToDateTime(date);

                    if (time < DateTime.Now.AddMinutes(-15.0))
                    {
                        thnRNode.changeServerStatus("Off");
                    }

                    if (larm)
                        thnRNode.changeServerStatus("Alert");
                }
            }
        }

        /// <summary>
        /// OnClick funktioner för alla noder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void londonNode_onClick(object sender, EventArgs e)
        {   
            if (this.Frame != null)
                this.Frame.Navigate(typeof(LondonServer));
        }

        void thnNode_onClick(object sender, EventArgs e)
        {
            if (this.Frame != null)
                this.Frame.Navigate(typeof(thnServer));
        }

        void thnRNode_onClick(object sender, EventArgs e)
        {
            if (this.Frame != null)
                this.Frame.Navigate(typeof(thnRouter));
        }

        void PLCNode_onClick(object sender, EventArgs e)
        {
            if (this.Frame != null)
                this.Frame.Navigate(typeof(PLC));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

	private void mapsControl_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (!this.mapsOpen)
            {
                mapsOpen = true;

                Storyboard sb = new Storyboard();
                
                DoubleAnimation width = new DoubleAnimation();
                DoubleAnimation height = new DoubleAnimation();

                DoubleAnimation moveX = new DoubleAnimation();
                DoubleAnimation moveY = new DoubleAnimation();

                width.EnableDependentAnimation = true;
                height.EnableDependentAnimation = true;

                moveX.EnableDependentAnimation = true;
                moveY.EnableDependentAnimation = true;

                width.Duration = new Duration(TimeSpan.FromSeconds(1));
                height.Duration = new Duration(TimeSpan.FromSeconds(1));

                moveX.Duration = new Duration(TimeSpan.FromSeconds(1));
                moveY.Duration = new Duration(TimeSpan.FromSeconds(1));

                sb.Children.Add(width);
                sb.Children.Add(height);

                sb.Children.Add(moveX);
                sb.Children.Add(moveY);

                Storyboard.SetTarget(width, this.mapsControl);
                Storyboard.SetTarget(height, this.mapsControl);
                Storyboard.SetTarget(moveX, this.mapsControl);
                Storyboard.SetTarget(moveY, this.mapsControl);

                Storyboard.SetTargetProperty(width, "Width");
                Storyboard.SetTargetProperty(height, "Height");
                Storyboard.SetTargetProperty(moveX, "(Canvas.Left)");
                Storyboard.SetTargetProperty(moveY, "(Canvas.Top)");

                moveX.To = 244;
                moveY.To = 104;

                width.To = 847;
                height.To = 596;

                this.mapsControl.HideButton();
                sb.Completed += sb_Completed;
                sb.Begin();
            }
        }

        void sbClose_Completed(object sender, object e)
        {
            this.mapsFinished = false;
            this.mapsOpen = false;
            this.mapsControl.ShowButton();
        }

        void sb_Completed(object sender, object e)
        {
            this.mapsFinished = true;
            this.mapsControl.ShowWeb();
        }

        private void Canvas_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (this.mapsFinished)
            {
                Storyboard sbClose = new Storyboard();

                DoubleAnimation width = new DoubleAnimation();
                DoubleAnimation height = new DoubleAnimation();

                DoubleAnimation moveX = new DoubleAnimation();
                DoubleAnimation moveY = new DoubleAnimation();

                width.EnableDependentAnimation = true;
                height.EnableDependentAnimation = true;

                moveX.EnableDependentAnimation = true;
                moveY.EnableDependentAnimation = true;

                width.Duration = new Duration(TimeSpan.FromSeconds(1));
                height.Duration = new Duration(TimeSpan.FromSeconds(1));

                moveX.Duration = new Duration(TimeSpan.FromSeconds(1));
                moveY.Duration = new Duration(TimeSpan.FromSeconds(1));

                sbClose.Children.Add(width);
                sbClose.Children.Add(height);

                sbClose.Children.Add(moveX);
                sbClose.Children.Add(moveY);

                Storyboard.SetTarget(width, this.mapsControl);
                Storyboard.SetTarget(height, this.mapsControl);
                Storyboard.SetTarget(moveX, this.mapsControl);
                Storyboard.SetTarget(moveY, this.mapsControl);

                Storyboard.SetTargetProperty(width, "Width");
                Storyboard.SetTargetProperty(height, "Height");
                Storyboard.SetTargetProperty(moveX, "(Canvas.Left)");
                Storyboard.SetTargetProperty(moveY, "(Canvas.Top)");

                moveX.To = 536;
                moveY.To = 349;

                width.To = 263;
                height.To = 106;

                this.mapsControl.HideWeb();
                sbClose.Completed += sbClose_Completed;
                sbClose.Begin();
            }
        }
    }
}