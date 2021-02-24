using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace LoongEgg.Data
{
    /// <summary>
    /// 数据序列
    /// </summary>
    public class DataSeries : ObservableCollection<Point>
    {
        /// <summary>
        /// Name of this DataSeries
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 横坐标范围
        /// </summary>
        public Range Xrange { get; set; }

        /// <summary>
        /// 纵坐标范围
        /// </summary>
        public Range Yrange { get; set; }

        /// <summary>
        /// 分组编号
        /// </summary>
        public int GroupId => Group == null ? 0 : Group.Id;

        /// <summary>
        /// 所在组
        /// </summary>
        public DataSeriesCollection Group { get; set; }
         
        /// <summary>
        /// 重置为指定的集合
        /// </summary>
        /// <param name="points"></param>
        public void Reset(IEnumerable<Point> points)
        {
            if (points == null) return;
            Items.Clear();

            foreach (var p in points)
            {
                Items.Add(p);
            }
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    }
}
