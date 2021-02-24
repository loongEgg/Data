using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace LoongEgg.Data
{
    /// <summary>
    /// 数据系列的集合
    /// </summary>
    public class DataSeriesCollection : ObservableCollection<DataSeries>, IDisposable
    { 
        public readonly static List<DataSeriesCollection> Instances = new List<DataSeriesCollection>();

        public int Id => Instances.IndexOf(this) + 1;

        public DataSeriesCollection()
        {
            Instances.Add(this);
            CollectionChanged += DataSeriesCollection_CollectionChanged;
        }

        private void DataSeriesCollection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var collection = e.OldItems as IEnumerable<DataSeries>;
            if (collection != null)
            {
                foreach (var item in collection)
                {
                    item.Group = null;
                }
            }
            collection = e.NewItems as IEnumerable<DataSeries>;
            if (collection != null)
            {

                foreach (var item in collection)
                {
                    item.Group = this;
                }
            }
        }

        #region GC
        private bool _Diposed = false;
        ~DataSeriesCollection()
        {
            Dispose(false);
        }
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_Diposed)
                return;

            if (disposing)
            {
                // 托管资源释放区
            }
            // 非托管资源释放区
            Instances.Remove(this);
            _Diposed = true;
        }
        #endregion
    }
}
