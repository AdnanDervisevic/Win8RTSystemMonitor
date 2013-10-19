using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Veritas.Controls
{
    public sealed partial class tempController : UserControl
    {
        public tempController()
        {
            this.InitializeComponent();
        }

        public void setTemp(int temperature)
        {
            // Ändrar färg på temp nummer beroende på temparaturskala
            if (temperature < 25)
            {
                this.tempLabel.Foreground = new SolidColorBrush(Colors.DodgerBlue);
            }
            else if (temperature >= 25 && temperature < 40)
            {
                this.tempLabel.Foreground = new SolidColorBrush(Colors.Green);
            }
            else if (temperature >= 40 && temperature < 65)
            {
                this.tempLabel.Foreground = new SolidColorBrush(Colors.DarkOliveGreen);
            }
            else if (temperature >= 65 && temperature < 80)
            {
                this.tempLabel.Foreground = new SolidColorBrush(Colors.DarkOrange);
            }
            else if (temperature >= 80 && temperature < 100)
            {
                this.tempLabel.Foreground = new SolidColorBrush(Colors.OrangeRed);
            }
            else if (temperature >= 100)
            {
                this.tempLabel.Foreground = new SolidColorBrush(Colors.Red);
            }

            //
            this.tempLabel.Text = temperature+"°".ToString();

            double temp = (temperature - 6.5);

            rectTransform.TranslateX = temp * 3.205;
        }
    }
}
