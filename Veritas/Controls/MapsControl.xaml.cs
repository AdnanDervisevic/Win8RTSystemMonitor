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
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Veritas.Controls
{
    public sealed partial class MapsControl : UserControl
    {
        public MapsControl()
        {
            this.InitializeComponent();
        }

        internal void HideWeb()
        {
            this.webView.Visibility = Visibility.Collapsed;
        }

        internal void ShowWeb()
        {
            this.webView.Visibility = Visibility.Visible;
        }

        internal void HideButton()
        {
            this.rect.Visibility = Visibility.Collapsed;
            this.img.Visibility = Visibility.Collapsed;
        }

        internal void ShowButton()
        {
            this.rect.Visibility = Visibility.Visible;
            this.img.Visibility = Visibility.Visible;
        }
    }
}
