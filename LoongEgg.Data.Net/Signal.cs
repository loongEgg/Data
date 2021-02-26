using LoongEgg.Core;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Threading;

namespace LoongEgg.Data.Net
{
    /// <summary>
    /// 监控信号
    /// </summary>
    public class Signal : DataSeries
    {
        #region event
         
        /// <summary>
        /// 心跳事件
        /// </summary>
        public static event TimeStampTickEventHandler TimeStampTick;
        /// <summary>
        /// 计数重载事件, 时长为TimeRange.Max
        /// </summary>
        public static event TimeStampReloadHandler TimeStampReloaded;

        #endregion

        /// <summary>
        /// 定时器
        /// </summary>
        private static DispatcherTimer Timer;
        private static Stopwatch Watch;

        /// <summary>
        /// 时钟周期, Max代表最大计数时长, Min代表每个周期结束后额外保存的上一周期时长(用负值表示).
        /// Coerec by <see cref="ResetTimer"/>
        /// </summary>
        /// <remarks>
        ///     [ default ]: {-10,000, 60,000}
        ///         [ min ]: [-10,000, 0]
        ///         [ max ]: [1,000, 60,000)
        /// </remarks>
        public readonly static Range TimeRange = new Range(-10000, 60000);

        /// <summary>
        /// 当前的时戳
        /// </summary>
        public static double TimeStamp { get; private set; } = 0;

        /// <summary>
        /// 时间步长(ms), 最小为20ms
        /// </summary>
        public static double TimeTick
        {
            get { return _TimeTick; }
            set
            {
                _TimeTick = Math.Abs(value);
                if (_TimeTick <= 20) _TimeTick = 20;
                ResetTimer();
            }
        }
        private static double _TimeTick = 100;

        /// <summary>
        /// 静态构造器, 
        /// </summary>
        static Signal()
        {
            Watch = new Stopwatch();
            TimeRange.PropertyChanged += (s, e) =>
            {
                ResetTimer();
            };
            ResetTimer();
        }

        /// <summary>
        /// instance of <see cref="Signal"/>
        /// </summary>
        public Signal()
        {
            Xrange = TimeRange;
            TimeStampReloaded += () =>
            {
                Reset();
            };
        }

        /// <summary>
        /// reset points
        /// </summary>
        /// <remarks>
        /// but keep the points of last Math.Abs(TimeRange.Min) ms
        /// </remarks>
        private void Reset()
        {
            var offset = TimeRange.Max + TimeRange.Min;
            var cache = Items.ToList().Where(p => p.X > offset).Select(i => new Data.Point(i.X - TimeRange.Max, i.Y)).ToList();
            if (cache != null && cache.Count > 0)
                Reset(cache);
        }

        /// <summary>
        /// reset the timer
        /// </summary>
        private static void ResetTimer()
        {
            Timer?.Stop();
            if (TimeTick < 1) return;
            if (TimeRange == null) return;
            if (TimeRange.Min < -10000) TimeRange.From = -10000;
            if (TimeRange.Min > 0) TimeRange.From = 0;

            if (TimeRange.Max < 1000) TimeRange.To = 1000;
            if (TimeRange.Max > 60000) TimeRange.To = 60000;

            double timeTotal = TimeRange.Max;

            Timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(TimeTick / 4.0)
            };
            Timer.Tick += OnTick;

            Watch.Restart(); 
            Timer.Start();
        }
         
        /// <summary>
        /// handle <see cref="DispatcherTimer.Tick"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnTick(object sender, EventArgs e)
        {
            if ((Watch.ElapsedMilliseconds - TimeStamp) > TimeTick)
            {
                TimeStamp = Watch.ElapsedMilliseconds;
                TimeStampTick?.Invoke(TimeStamp);
            }
            if (TimeStamp > TimeRange.Max)
            {
                TimeStampReloaded?.Invoke();
                TimeStamp = 0;
                TimeStampTick?.Invoke(TimeStamp);
                Watch.Restart(); 
            }
        }

        /// <summary>
        /// 新值
        /// </summary>
        /// <remarks>
        ///     add a point and raised ObservableCollection.CollectionChanged event every time it's set.
        /// </remarks>
        public double Value
        {
            get { return _Value; }
            set
            {
                _Value = value;
                Add(new Data.Point(TimeStamp, value));
            }
        }
        private double _Value;

    }
}
