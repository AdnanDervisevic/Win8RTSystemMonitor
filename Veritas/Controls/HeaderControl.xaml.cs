using System;

using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Veritas.Controls
{
    /// <summary>
    /// Author: Tobias Oskarsson & Adnan Dervisevic
    /// </summary>
    public sealed partial class HeaderControl : UserControl
    {
        #region Fields

        /// <summary>
        /// EventHandler till tillbaka knappen.
        /// </summary>
        public event EventHandler OnBackClick;

        /// <summary>
        /// EventHandler till History knappen.
        /// </summary>
        public event EventHandler OnHistoryClick;

        #endregion

        #region Properties

        /// <summary>
        /// Hämtar eller sätter servernamnet.
        /// </summary>
        public string ServerName
        {
            get { return this.serverName.Text; }
            set { this.serverName.Text = value; }
        }

        /// <summary>
        /// Hämtar eller sätter förgrunden.
        /// </summary>
        public Color Foreground
        {
            get { return (this.serverName.Foreground as SolidColorBrush).Color; }
            set { this.serverName.Foreground = new SolidColorBrush(value); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Konstrurerar en ny Header.
        /// </summary>
        public HeaderControl()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Events

        /// <summary>
        /// Event som körs när man trycker på tillbaka knappen.
        /// </summary>
        /// <param name="sender">Vilket objekt som skickade eventet.</param>
        /// <param name="e">Event argument.</param>
        private void onBackBtn_Click(object sender, RoutedEventArgs e)
        {
            if (this.OnBackClick != null)
                this.OnBackClick(this, EventArgs.Empty);
        }

        /// <summary>
        /// Event som körs när man trycker på history knappen.
        /// </summary>
        /// <param name="sender">Vilket objekt som skickade eventet.</param>
        /// <param name="e">Event argument.</param>
        private void onHistoryBtn_Click(object sender, RoutedEventArgs e)
        {
            if (this.OnHistoryClick != null)
                this.OnHistoryClick(this, EventArgs.Empty);
        }

        #endregion
    }
}