using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ACSClient
{
    // 데이터베이스로부터 type값을 읽어서 얻은 List를 리스트뷰에 출력할 시 사용.
    // 만약 type가 0이면 무단 투기라고, 1이면 담배라고  리스트뷰에 출력한다.
    public class CrackdownTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int type = (int)value;
            if(type == 0)
            {
                return "무단 투기";
            } else if(type ==1)
            {
                return "담배";
            } else
            {
                return "type 없음";
            }
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
