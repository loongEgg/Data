using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace LoongEgg.Data.Net
{ 

    /// <summary>
    /// <see cref="ObservableCollection{T}"/> of <see cref="Signal"/>
    /// </summary>
    public class SignalGroup : ObservableCollection<Signal>, IDisposable
    {
        public readonly static List<SignalGroup> Instances = new List<SignalGroup>();
         
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

        public SignalGroup()
        {
            Instances.Add(this);
            CollectionChanged += SignalGroup_CollectionChanged;
        }

        private void SignalGroup_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var collection = e.OldItems as IEnumerable<Signal>;
            if (collection != null)
            {
                foreach (var item in collection)
                {
                    item.GroupId = 0;
                }
            }
            collection = e.NewItems as IEnumerable<Signal>;
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

        public DataSeriesCollection ToDataSeriesCollection()
        {
            var collection = new DataSeriesCollection
            {
                Xrange = this.Xrange,
                Yrange = this.Yrange
            };

            foreach (var item in Items)
            {
                collection.Add(item);
            }

            return collection;
        }

        #region GC
        private bool _Diposed = false;
        ~SignalGroup()
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
