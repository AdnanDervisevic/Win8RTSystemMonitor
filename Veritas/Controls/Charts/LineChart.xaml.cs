using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Windows.UI.Xaml.Controls;

using Veritas.Classes;

using WinRTXamlToolkit.Controls.DataVisualization.Charting;
using Windows.UI;

namespace Veritas.Controls.Charts
{
    public sealed partial class LineChart : UserControl
    {
        /// <summary>
        /// The ObservableCollection of datapoints.
        /// </summary>
        private ObservableCollection<NameValueItem> datapoints;

        /// <summary>
        /// Gets or sets the max amount of datapoints.
        /// </summary>
        public int MaxDataPoints { get; set; }
        System.Random rand = new System.Random();
        /// <summary>
        /// Constructs a new LineChart.
        /// </summary>
        public LineChart()
        {
            this.InitializeComponent();
            this.datapoints = new ObservableCollection<NameValueItem>();
            this.MaxDataPoints = 30;

            ((LineSeries)this.lineChart.Series[0]).ItemsSource = this.datapoints;
            /*
            SetTitle("CPU Usage");

            for (int i = 0; i < 32; i++)
            {
                if (i < 10)
                    AddDataPoints(new NameValueItem("0" + i, rand.Next(20, 100)));
                else
                    AddDataPoints(new NameValueItem(i.ToString(), rand.Next(20, 100)));
            }*/
            /*
            ((LineSeries)this.lineChart.Series[0]).DependentRangeAxis =
                new LinearAxis
                {
                    Minimum = 0,
                    Maximum = 100,
                    Orientation = AxisOrientation.Y,
                    Interval = 20,
                    ShowGridLines = true
                };*/
        }

        /// <summary>
        /// Reverts all the NameValueItems
        /// </summary>
        public void Reverse()
        {
            List<NameValueItem> tmp = new List<NameValueItem>();

            for (int i = this.datapoints.Count - 1; i >= 0; i--)
                tmp.Add(this.datapoints[i]);

            this.datapoints.Clear();
            this.AddDataPoints(tmp);
        }

        /// <summary>
        /// Gets the nameValue item at a position.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public NameValueItem GetItem(int i)
        {
            if (i < this.datapoints.Count)
                return this.datapoints[i];

            return null;
        }

        /// <summary>
        /// Gets the last NameValue item.
        /// </summary>
        /// <returns></returns>
        public NameValueItem GetLastItem()
        {
            return this.datapoints[this.datapoints.Count - 1];
        }

        /// <summary>
        /// Gets the number of data points.
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return this.datapoints.Count;
        }

        /// <summary>
        /// Removes all the datapoints.
        /// </summary>
        public void Clear()
        {
            this.datapoints.Clear();
        }

        /// <summary>
        /// Sets the linecharts title.
        /// </summary>
        /// <param name="title">The title.</param>
        public void SetTitle(string title)
        {
            this.lineChart.Title = title;
        }

        /// <summary>
        /// Sets the text to the left label.
        /// </summary>
        /// <param name="title"></param>
        public void SetLeftLabel(string title)
        {
            this.firstMonth.Text = title;
        }

        /// <summary>
        /// Sets the text to the right label.
        /// </summary>
        /// <param name="title"></param>
        public void SetRightLabel(string title)
        {
            this.secondMonth.Text = title;
        }

        /// <summary>
        /// Clears the chart and adds an array of datapoints.
        /// If there are more than 31 datapoints only the last 31 will be added.
        /// </summary>
        /// <param name="datapoints">The array of datapoints to add.</param>
        public void SetDataPoints(params NameValueItem[] datapoints)
        {
            this.datapoints.Clear();
            this.AddDataPoints(datapoints.ToList());
        }

        /// <summary>
        /// Clears the chart and adds a list of datapoints.
        /// If there are more than 31 datapoints only the last 31 will be added.
        /// </summary>
        /// <param name="datapoints">The list of datapoints to add.</param>
        public void SetDataPoints(List<NameValueItem> datapoints)
        {
            this.datapoints.Clear();
            this.AddDataPoints(datapoints);
        }

        /// <summary>
        /// Adds an array of datapoints to the chart.
        /// </summary>
        /// <param name="datapoints">The array of datapoints to add.</param>
        public void AddDataPoints(params NameValueItem[] datapoints)
        {
            this.AddDataPoints(datapoints.ToList());
        }

        /// <summary>
        /// Adds a list of datapoints to the chart.
        /// </summary>
        /// <param name="datapoints">The list of datapoints to add.</param>
        public void AddDataPoints(List<NameValueItem> datapoints)
        {
            for (int i = 0; i < datapoints.Count; i++)
            {
                if (this.datapoints.Count == MaxDataPoints)
                {
                    this.datapoints.RemoveAt(0);
                }

                this.datapoints.Add(datapoints[i]);
            }
        }
    }
}