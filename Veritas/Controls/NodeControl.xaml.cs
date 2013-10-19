using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Veritas.Controls
{
    public sealed partial class NodeControl : UserControl
    {
        public event EventHandler onClick;

        public NodeControl()
        {
            this.InitializeComponent();
        }

        public void changeServerName(String serverName)
        {
            nodeServerName.Text = serverName;
        }

        // Sätter statustext och ändrar bilder
        public void changeServerStatus(String ServerStatus)
        {
            if (ServerStatus.Equals("On"))
            {
                nodeServerStatus.Text = "Server Status: Online";
                nodeImage.Source = new BitmapImage(new Uri("ms-appx:/Assets/on.png"));
            }

            else if (ServerStatus.Equals("Off"))
            {
                nodeServerStatus.Text = "Server Status: Offline";
                nodeImage.Source = new BitmapImage(new Uri("ms-appx:/Assets/off.png"));
            }

            else if (ServerStatus.Equals("Alert"))
            {
                nodeServerStatus.Text = "ALERT - CHECK SERVER NOW";

                nodeImage.Source = new BitmapImage(new Uri("ms-appx:/Assets/alert.png"));
            }
        }

        private void Grid_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            if (this.onClick != null)
                this.onClick(this, EventArgs.Empty);
        }
    }
}
