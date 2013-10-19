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
    public sealed partial class NetworkInfo : UserControl
    {
        Dictionary<DateTime, long> rxByteValues = new Dictionary<DateTime, long>();
        Dictionary<DateTime, long> txByteValues = new Dictionary<DateTime, long>();
        long bytesAdded = 0;

        public NetworkInfo()
        {
            /*
            rxByteValues.Add(DateTime.Now.AddMinutes(-30.0), (long)134325);
            rxByteValues.Add(DateTime.Now.AddDays(-1.00), (long)13432);
            rxByteValues.Add(DateTime.Now.AddDays(-3.00), (long)1343);
            rxByteValues.Add(DateTime.Now.AddMonths(-9), (long)134);
            rxByteValues.Add(DateTime.Now, (long)13432542);
            txByteValues.Add(DateTime.Now, (long)200);
            txByteValues.Add(DateTime.Now.AddMinutes(-30.0), (long)13432542);
            txByteValues.Add(DateTime.Now.AddDays(-1.00), (long)134325);
            txByteValues.Add(DateTime.Now.AddDays(-3.00), (long)1343);
            txByteValues.Add(DateTime.Now.AddMonths(-9), (long)134);
             * */
            this.InitializeComponent();
        }

        public void Clear()
        {
            this.rxByteValues.Clear();
            this.txByteValues.Clear();
        }

        public void AddRxBytes(DateTime date, long rxBytes)
        {
            if (!rxByteValues.ContainsKey(date))
                rxByteValues.Add(date, rxBytes);

            changeRXrecieved();
        }

        public void AddTxBytes(DateTime date, long txBytes)
        {
            if (!txByteValues.ContainsKey(date))
                txByteValues.Add(date, txBytes);

            changeTXsent();
        }

        public void HideHistory()
        {
            rxLabelBytes.Visibility = Visibility.Collapsed;
            rxLabelRecieved.Visibility = Visibility.Collapsed;
            txLabelBytes.Visibility = Visibility.Collapsed;
            txLabelSent.Visibility = Visibility.Collapsed;
            txSentSince.Margin= new Thickness(40, 70, -58, 0);
            txSinceStartBytes.Margin = new Thickness(148,93,0,0);
        }

        public void changeRXrecieved()
        {
            RXrecieved.Text = rxByteValues.Last().Value.ToString();
        }

        public void changeTXsent()
        {
           TXrecieved.Text = txByteValues.Last().Value.ToString();
        }

        public void setRxIncrease(double days, double offset)
        {
            bytesAdded = 0;

            DateTime from = DateTime.Now;

            from = DateTime.Now.AddDays(-days);

            for (int i = 0; i < (rxByteValues.Count - 1); i++)
            {
                if (rxByteValues.ElementAt(i).Key.CompareTo(from.AddDays(-offset)) >= 0)
                {
                    bytesAdded += rxByteValues.ElementAt(i).Value;
                }
            }
            RXbyteIncreasedBy.Text = bytesAdded.ToString();
        }

        public void setTxIncrease(double days, double offset)
        {
            bytesAdded = 0;

            DateTime from = DateTime.Now;

            from = DateTime.Now.AddDays(-days);

            for (int i = 0; i < (txByteValues.Count - 1); i++)
            {
                if (txByteValues.ElementAt(i).Key.CompareTo(from.AddDays(-offset)) >= 0)
                {
                    bytesAdded += txByteValues.ElementAt(i).Value;
                }
            }
            TXbyteIncreasedBy.Text = bytesAdded.ToString();
        }
    }
}
