using LoongEgg.Core;
using System;
using System.ComponentModel;
using System.Diagnostics;

namespace LoongEgg.Data
{
    /// <summary>
    /// 数据范围定义
    /// </summary>
    [TypeConverter(typeof(RangeConverter))]
    public class Range : BindableObject
    {
        #region public get only properties

        public double Max { get; private set; }
        public double Min { get; private set; }
        public bool IsAscending => To > From;
        public double Distance { get; private set; }

        #endregion

        #region properties

        public double From
        {
            get { return _From; }
            set { if (SetProperty(ref _From, value)) UpdateMembers(); }
        }
        private double _From;

        public double To
        {
            get { return _To; }
            set { if (SetProperty(ref _To, value)) UpdateMembers(); }
        }
        private double _To;

        #endregion

        #region ctor

        public Range(double from, double to)
        {
            From = from;
            To = to;
        }

        public Range(string from, string to) : this(double.Parse(from), double.Parse(to)) { }

        #endregion

        #region public static methods

        /// <summary>
        /// 将字符串转换成<seealso cref="Range"/>实例
        /// </summary>
        /// <param name="txt">"from, to"</param>
        /// <returns><seealso cref="Range"/>实例</returns>
        public static Range Parse(string txt)
        {
            var items = txt.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (items == null || items.Length != 2)
            {
                Debug.WriteLine(($"{nameof(Range)}类型的字符串必须由两个可以转换为double类型成员构成"));
                return null;
            }

            return new Range(items[0], items[1]);
        }

        #endregion

        #region private methods

        private void UpdateMembers()
        {
            Max = Math.Max(From, To);
            Min = Math.Min(From, To);
            Distance = Max - Min;
            RaisePropertyChanged(nameof(Max));
            RaisePropertyChanged(nameof(Min));
            RaisePropertyChanged(nameof(Distance));
            RaisePropertyChanged(nameof(IsAscending));
        }

        #endregion

    }
}
