using LoongEgg.Core;
using System;
using System.Linq;
using System.Windows.Threading;

namespace LoongEgg.Data.Net
{
    public class Signal : BindableObject
    {
        private static DispatcherTimer Timer;

        public static Range TimeRange
        {
            get { return _TimeRange; }
            private set
            {
                if (value == null) return;
                _TimeRange = value;
            }
        }
        private static Range _TimeRange = new Range(-5, 10);

        public static event TimeStampReloadHandler TimeStampReloaded;

        /// <summary>
        /// 当前的时戳
        /// </summary>
        public static double TimeStamp { get; private set; } = 0;

        /// <summary>
        /// 时间步长(ms)
        /// </summary>
        public static double TimeTick
        {
            get { return _TimeTick; }
            set
            {
                _TimeTick = Math.Abs(value);
                if (_TimeTick <= 1) _TimeTick = 1;
                ResetTimer();
            }
        }
        private static double _TimeTick = 10;

        static Signal()
        {
            ResetTimer();
        }

        public Signal()
        {
            TimeStampReloaded += () =>
            {
                Reset();
            };
        }

        private void Reset()
        {
            var offset = TimeRange.Max + TimeRange.Min;
            var cache = DataSeries.ToList().Where(p => p.X > offset).Select(i => new Point(i.X + TimeRange.Min, i.Y));
            DataSeries.Reset(cache);
        }

        private static void ResetTimer()
        {
            Timer?.Stop();
            if (TimeTick < 1) return;
            if (TimeRange == null) return;
            if (TimeRange.Min < -10) TimeRange.From = -10;
            if (TimeRange.Min > 0) TimeRange.From = 0;

            if (TimeRange.Max < 0 || TimeRange.Max > 60000) TimeRange.To = 10000;

            double timeTotal = TimeRange.Max;
            Timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(TimeTick)
            };
            Timer.Tick += (s, e) =>
            {
                TimeStamp += TimeTick;
                if (TimeStamp > TimeRange.Max)
                {
                    TimeStamp = 0;
                    TimeStampReloaded?.Invoke();
                }
            };
            Timer.Start();
        }

        public double Value
        {
            get { return _Value; }
            set
            {
                _Value = value;
                DataSeries.Add(new Point(TimeStamp, value));
            }
        }
        private double _Value;

        public readonly DataSeries DataSeries = new DataSeries();
    }
}
