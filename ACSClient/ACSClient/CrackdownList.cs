using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACSClient
{
    // 리스트뷰에 연결할 리스트들 저장하고, 관리하는 클래스.
    public class CrackdownList
    {
        public ObservableCollection<Crackdown> DataList = new ObservableCollection<Crackdown>();
        public List<Crackdown> SelectList = new List<Crackdown>();

        public CrackdownList()
        {
            
        }
        // 데이터베이스와 연결하여 DataList에 저장
        public void DataBaseContect()
        {
            MyDataBase dataBase = new MyDataBase();
            DataList = dataBase.SelectCrackdownData();
            
        }

        // 서버와 연결하여 DataList에 저장
        public void ServerContect()
        {
            string wbRequestResult = MyServer.callWebRequest();
            DataList = MyServer.jsonData(wbRequestResult);
        }
    }
}