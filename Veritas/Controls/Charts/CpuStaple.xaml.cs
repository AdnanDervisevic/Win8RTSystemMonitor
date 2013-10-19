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
    public sealed partial class CpuStaple : UserControl
    {
        public CpuStaple()
        {
            this.InitializeComponent();
        }

        public void ChangeColor(Color inColor)
        {
            this.frame.Stroke = new SolidColorBrush(inColor);
        }

        public void stapleGraph(double cpuValue)
        {
            double cpuPercantageValue = Math.Round(cpuValue * 100, 3);
            cpuPercentageText.Text = cpuPercantageValue+"%".ToString();

            cpuValue = Math.Round(cpuValue, 2);

            if (cpuValue <= 0.10)
            {
            }

            else if (cpuValue <= 0.20)
            {
                tenRect.Visibility = Visibility.Visible;
            }

            else if (cpuValue <= 0.30)
            {
                tenRect.Visibility = Visibility.Visible;
                twentyRect.Visibility = Visibility.Visible;
            }

            else if (cpuValue <= 0.40)
            {
                tenRect.Visibility = Visibility.Visible;
                twentyRect.Visibility = Visibility.Visible;
                thirtyRect.Visibility = Visibility.Visible;
            }

            else if (cpuValue <= 0.50)
            {
                tenRect.Visibility = Visibility.Visible;
                twentyRect.Visibility = Visibility.Visible;
                thirtyRect.Visibility = Visibility.Visible;
                fourtyRect.Visibility = Visibility.Visible;
            }

            else if (cpuValue <= 0.60)
            {
                tenRect.Visibility = Visibility.Visible;
                twentyRect.Visibility = Visibility.Visible;
                thirtyRect.Visibility = Visibility.Visible;
                fourtyRect.Visibility = Visibility.Visible;
                fiftyRect.Visibility = Visibility.Visible;
            }

            else if (cpuValue <= 0.70)
            {
                tenRect.Visibility = Visibility.Visible;
                twentyRect.Visibility = Visibility.Visible;
                thirtyRect.Visibility = Visibility.Visible;
                fourtyRect.Visibility = Visibility.Visible;
                fiftyRect.Visibility = Visibility.Visible;
                sixtyRect.Visibility = Visibility.Visible;
            }

            else if (cpuValue <= 0.80)
            {
                tenRect.Visibility = Visibility.Visible;
                twentyRect.Visibility = Visibility.Visible;
                thirtyRect.Visibility = Visibility.Visible;
                fourtyRect.Visibility = Visibility.Visible;
                fiftyRect.Visibility = Visibility.Visible;
                sixtyRect.Visibility = Visibility.Visible;
                seventyRect.Visibility = Visibility.Visible;
            }

            else if (cpuValue <= 0.80)
            {
                tenRect.Visibility = Visibility.Visible;
                twentyRect.Visibility = Visibility.Visible;
                thirtyRect.Visibility = Visibility.Visible;
                fourtyRect.Visibility = Visibility.Visible;
                fiftyRect.Visibility = Visibility.Visible;
                sixtyRect.Visibility = Visibility.Visible;
                seventyRect.Visibility = Visibility.Visible;
                eightyRect.Visibility = Visibility.Visible;
            }
            else if (cpuValue <= 0.90)
            {
                tenRect.Visibility = Visibility.Visible;
                twentyRect.Visibility = Visibility.Visible;
                thirtyRect.Visibility = Visibility.Visible;
                fourtyRect.Visibility = Visibility.Visible;
                fiftyRect.Visibility = Visibility.Visible;
                sixtyRect.Visibility = Visibility.Visible;
                seventyRect.Visibility = Visibility.Visible;
                eightyRect.Visibility = Visibility.Visible;
                ninetyRect.Visibility = Visibility.Visible;
            }
            else if (cpuValue <= 1.00)
            {
                tenRect.Visibility = Visibility.Visible;
                twentyRect.Visibility = Visibility.Visible;
                thirtyRect.Visibility = Visibility.Visible;
                fourtyRect.Visibility = Visibility.Visible;
                fiftyRect.Visibility = Visibility.Visible;
                sixtyRect.Visibility = Visibility.Visible;
                seventyRect.Visibility = Visibility.Visible;
                eightyRect.Visibility = Visibility.Visible;
                ninetyRect.Visibility = Visibility.Visible;
                hundredRect.Visibility = Visibility.Visible;
            }
        }
    }
}
