using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace ACSClient
{
    // MySQL 접속을 위해 패키지 다운하여 사용함.(이 프로젝트는 이미 다운이 완료됨)
    /// <summary>
    /// MySQL 데이터 베이스 접속 관련 클래스
    /// </summary>
    public class MyDataBase
    {
        // 연결 string. 수정 불가능. ip주소, 포트번호, 데이터베이스 이름, id, 비밀번호 설정
        private readonly string ConnectionString = "Server=192.168.55.78;Port=3306;Database=db01;Uid=user01;Pwd=user01;";
        private MySqlConnection mySqlConnection;

        public MyDataBase() {
            mySqlConnection = new MySqlConnection(ConnectionString);
        }

        private void Open()
        {
            try
            {
                mySqlConnection.Open();
            } catch (Exception ex) { throw ex; }
            
        }

        // 데이터베이스 연결하는 함수. 내부에 'SELECT * FROM -' 쿼리문 있음.
        public ObservableCollection<Crackdown> SelectCrackdownData()
        {
            // 쿼리문 str
            string query = "SELECT * FROM crackdown";
            // 데이터베이스로부터 얻은 리스트 받아서 리턴
            ObservableCollection<Crackdown> list = new ObservableCollection<Crackdown>();

            try
            {
                Open();
                MySqlCommand cmd = new MySqlCommand(query);
                cmd.Connection = mySqlConnection;
                // 데이터베이스로부터 얻은 테이블 받기
                MySqlDataReader table = cmd.ExecuteReader();
                while (table.Read())
                {
                    // 데이터를 리스트에 저장
                    list.Add(new Crackdown(int.Parse(table["Crackdown_ID"].ToString()), int.Parse(table["CCTV_ID"].ToString()), DateTime.Parse(table["Date"].ToString()), int.Parse(table["Location_ID"].ToString()), int.Parse(table["Crackdown_Type"].ToString())));
                }
                Close();
                return list;

            }
            catch (Exception ex) { throw ex; }
        }

        private void Close()
        {
            try
            {
                mySqlConnection.Close();
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
