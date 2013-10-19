using System;

using Windows.UI.Xaml.Controls;

namespace Veritas.Controls
{
    /// <summary>
    /// Author: Tobias Oskarsson & Adnan Dervisevic
    /// </summary>
    public sealed partial class UptimeControl : UserControl
    {

        #region Constructors

        /// <summary>
        /// Konstrurerar en ny Uptime kontroller
        /// </summary>
        public UptimeControl()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Uppdaterar tiden i sekunder.
        /// </summary>
        /// <param name="uptimeinseconds"></param>
        public void updateUptime(int uptimeinseconds)
        {
            TimeSpan time = TimeSpan.FromSeconds(uptimeinseconds);

            daysLbl.Text = time.Days.ToString();
            hoursLbl.Text = time.Hours.ToString();
            minutesLbl.Text = time.Minutes.ToString();
        }

        #endregion

    }
}
