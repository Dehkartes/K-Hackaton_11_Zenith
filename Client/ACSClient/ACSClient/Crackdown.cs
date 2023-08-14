using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ACSClient
{
    // 속성 데이터가 변화되었을때 사용되는 인터페이스
    public class Crackdown : INotifyPropertyChanged
    {
        #region 프로퍼티

        public event PropertyChangedEventHandler PropertyChanged;

        private int crackdown_id;
        private int cctv_id;
        private int location_id;
        private DateTime date;
        private int crackdown_type;

        public int Crackdown_ID
        {
            get { return crackdown_id; }
            set
            {
                crackdown_id = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Crackdown_ID"));
                }
            }
        }

        public int CCTV_ID
        {
            get { return cctv_id; }
            set
            {
                cctv_id = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("CCTV_ID"));
                }
            }
        }

        public DateTime Date
        {
            get { return date; }
            set
            {
                date = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Date"));
                }
            }
        }

        public int Location_ID
        {
            get { return location_id; }
            set
            {
                location_id = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Location_ID"));
                }
            }
        }

        public int Crackdown_Type
        {
            get { return crackdown_type; }
            set
            {
                crackdown_type = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Crackdown_Type"));
                }
            }
        }

        #endregion

        public Crackdown(int _crackdown_id, int _cctv_id,DateTime _Date, int _Location, int _Type)
        {
            Crackdown_ID = _crackdown_id;
            CCTV_ID = _cctv_id;
            Date = _Date;
            Location_ID = _Location;
            Crackdown_Type = _Type;
        }

        public Crackdown() { }

        public Boolean IsSameData(DateTime _Date)
        {
            return Date == _Date;
        }

        public Boolean IsSameData(int _Type)
        {
            return Crackdown_Type == _Type;
        }

        public string ReturnStr()
        {
            return Crackdown_ID.ToString() + CCTV_ID.ToString() + Date.ToString() + Location_ID.ToString() + Crackdown_Type.ToString();
        }
    }
}
