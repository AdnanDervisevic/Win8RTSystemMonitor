using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Veritas.Controls
{
    public sealed partial class Calendar : UserControl
    {

        public delegate void OnSelectedDateTimeDelegate(DateTime dateTime);
        public event OnSelectedDateTimeDelegate OnSelectedDateTime;

        public Calendar()
        {
            this.InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (OnSelectedDateTime != null)
            {
                DateTime dateTime = this.calendar.SelectedDate;

                dateTime = dateTime.AddHours(double.Parse(this.hours.SelectionBoxItem.ToString()));
                dateTime = dateTime.AddMinutes(double.Parse(this.minutes.SelectionBoxItem.ToString()));

                this.OnSelectedDateTime(dateTime);
            }
        }
    }
}