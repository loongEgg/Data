using System;
using System.ComponentModel;
using System.Globalization;

namespace LoongEgg.Data
{
    /// <summary>
    /// Converter for <see cref="Range"/>
    /// </summary>
    public class RangeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
                return Range.Parse((string)value);
            return base.ConvertFrom(context, culture, value);
        }
    }
}
