using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace FlashGame
{
    [ValueConversion(typeof(string), typeof(string))]
    public class CoverConverter : IValueConverter
    {

        Config cfg = ((App)System.Windows.Application.Current).CurrentConfig;

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string name = value.ToString();

            return string.Format(@cfg.SwfDirectory + @"\game\cover\{0}", name);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
