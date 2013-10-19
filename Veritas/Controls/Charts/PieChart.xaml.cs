using System.Collections.Generic;
using System.Collections.ObjectModel;

using Windows.UI.Xaml.Controls;

using Veritas.Classes;

using WinRTXamlToolkit.Controls.DataVisualization.Charting;

namespace Veritas.Controls.Charts
{
    public sealed partial class PieChart : UserControl
    {
        #region Fields

        /// <summary>
        /// Listan av values som visas i pieCharten.
        /// </summary>
        private ObservableCollection<NameValueItem> values;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new PieChart.
        /// </summary>
        public PieChart()
        {
            this.InitializeComponent();
            values = new ObservableCollection<NameValueItem>();
            ((PieSeries)this.pieChart.Series[0]).ItemsSource = values;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sets the pieCharts title.
        /// </summary>
        /// <param name="title">The title.</param>
        public void SetTitle(string title)
        {
            this.titleLbl.Text = title;
        }

        /// <summary>
        /// Sets the piechart to a percentage.
        /// </summary>
        /// <param name="title">The title of the pieChart.</param>
        /// <param name="percentage">The amount in percentage</param>
        public void SetPercentage(string title, int percentage)
        {
            this.SetItems(new NameValueItem("Used", percentage), new NameValueItem("Unused", 100 - percentage));

            this.titleLbl.Text = title;
        }

        /// <summary>
        /// Sets an array of ints to the list of values.
        /// </summary>
        /// <param name="values">The array of ints to set.</param>
        public void SetItems(params NameValueItem[] values)
        {
            this.values.Clear();
            for (int i = 0; i < values.Length; i++)
                this.values.Add(values[i]);
        }

        /// <summary>
        /// Sets a list of ints to the list of values.
        /// </summary>
        /// <param name="values">The list of ints to set.</param>
        public void SetItems(List<NameValueItem> values)
        {
            this.values.Clear();

            for (int i = 0; i < values.Count; i++)
                this.values.Add(values[i]);
        }

        /// <summary>
        /// Adds an array of ints to the list of values.
        /// </summary>
        /// <param name="values">The array of ints to add.</param>
        public void AddItems(params NameValueItem[] values)
        {
            for (int i = 0; i < values.Length; i++)
                this.values.Add(values[i]);
        }

        /// <summary>
        /// Adds a list of ints to the list of values.
        /// </summary>
        /// <param name="values">The list of ints to add.</param>
        public void AddItems(List<NameValueItem> values)
        {
            for (int i = 0; i < values.Count; i++)
                this.values.Add(values[i]);
        }

        #endregion
    }
}