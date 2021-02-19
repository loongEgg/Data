using System.Collections.ObjectModel;

namespace LoongEgg.Data
{
    public class DataSeriesCollection: ObservableCollection<DataSeries> { }
    public class DataSeries : ObservableCollection<Point> { }

    public struct Point
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
}
