using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LiveCharts;
using LiveCharts.Wpf;

namespace Genius_Business.MainPages.Pàgines
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    /// 

    public partial class Home : Page
    {

    public Func<ChartPoint, string> PointLabel { get; set; }
    public SeriesCollection SeriesCollection { get; set; }
    public string[] Labels { get; set; }
    public Func<double, string> YFormatter { get; set; }
        public Home()
        {
            InitializeComponent();


            this.chart();
            this.datosbacis();
        }


        public void chart()
        {
            PointLabel = chartPoint =>
             string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);

            DataContext = this;
        }

        private void Chart_OnDataClick(object sender, ChartPoint chartpoint)
        {
            var chart = (LiveCharts.Wpf.PieChart)chartpoint.ChartView;

            //clear selected slice.
            foreach (PieSeries series in chart.Series)
                series.PushOut = 0;

            var selectedSeries = (PieSeries)chartpoint.SeriesView;
            selectedSeries.PushOut = 8;
        }

        public void datosbacis()
        {

            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Ventas",
                    Values = new ChartValues<double> { 400, 600, 500,500  }
                },
                new LineSeries
                {
                    Title = "Compras",
                    Values = new ChartValues<double> { 600, 700, 300, 40 },
                    PointGeometry = null
                },
                new LineSeries
                {
                    Title = "Producción",
                    Values = new ChartValues<double> { 400,200,300,600 },
                    PointGeometry = DefaultGeometries.Square,
                    PointGeometrySize = 15
                }
            };

            Labels = new[] { "Mayo", "Junio", "Julio", "Agosto" };
            YFormatter = value => value.ToString("C");

            //modifying the series collection will animate and update the chart
            SeriesCollection.Add(new LineSeries
            {
                Title = "Comprobación",
                Values = new ChartValues<double> { 5, 3, 2 },
                LineSmoothness = 0, //0: straight lines, 1: really smooth lines
                PointGeometry = Geometry.Parse("m 25 70.36218 20 -28 -20 22 -8 -6 z"),
                PointGeometrySize = 50,
                PointForeground = Brushes.Gray
            });


            SeriesCollection[3].Values.Add(5d);

            DataContext = this;
        }

    }
}
