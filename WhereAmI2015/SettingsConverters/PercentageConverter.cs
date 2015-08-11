using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recoding.WhereAmI2015.SettingsConverters
{
    internal class PercentageConverter : DoubleConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            double parsed = 1;

            Double.TryParse(value.ToString(), out parsed);
            if (parsed > 1)
                value = 1;

            if (parsed < 0)
                value = 0;

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
