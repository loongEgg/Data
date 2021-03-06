﻿using System;
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

        public Range Xrange
        {
            get;
            set;
        } = new Range(0, 500);

        public Range Yrange
        {
            get;
            set;
        } = new Range(0, 50);

        public int Id => this.GetHashCode();

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
                    item.GroupId = 0;
                }
            }
            collection = e.NewItems as IEnumerable<DataSeries>;
            if (collection != null)
            {

                foreach (var item in collection)
                {
                    item.GroupId = Id;
                    if (item.Xrange != null)
                    {
                        if (Xrange == null)
                        {
                            Xrange = item.Xrange;
                        }
                        else
                        {
                            if (item.Xrange.Max > Xrange.Max)
                                Xrange.To = item.Xrange.Max;
                            if (item.Xrange.Min < Xrange.Min)
                                Xrange.From = item.Xrange.Min;
                        }
                    }
                    if (item.Yrange != null)
                    {
                        if (Yrange == null)
                        {
                            Yrange = item.Yrange;
                        }
                        else
                        {
                            if (item.Yrange.Max > Yrange.Max)
                                Yrange.To = item.Yrange.Max;
                            if (item.Yrange.Min < Yrange.Min)
                                Yrange.From = item.Yrange.Min;
                        }
                    }
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
