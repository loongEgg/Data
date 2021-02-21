using System.Collections.ObjectModel;

namespace LoongEgg.Data
{
    /// <summary>
    /// 数据序列
    /// </summary>
    public class DataSeries : ObservableCollection<Point> {
         public string Name { get; set; }
    }
}
