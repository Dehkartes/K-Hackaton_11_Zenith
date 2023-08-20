using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Utilities;

namespace ACSClient
{
    // 간편한 Json 데이터 처리를 위해 패키지 다운하여 사용함.(이 프로젝트는 이미 다운이 완료됨)
    public class MyServer
    {
        // 서버 url 설정.
        private static string URL = "http://" + "hsj3925.iptime.org" + ":" + "5000" + "/database";

        public static string callWebRequest()
        {
            string responseFromServer = string.Empty;
            
            try
            {
                // WebRequest로 rest api를 구현.
                WebRequest request = WebRequest.Create(URL);
                // 데이터를 받아오는 목적이므로 GET으로 설정함.
                request.Method = "GET";
                // .GetResponse()함수를 통해 웹서버로부터 response를 받는다
                // .GetResponseStream()를 통해 response의 데이터 내용을 알수있다.
                using (WebResponse response = request.GetResponse())
                using (Stream dataStream = response.GetResponseStream()) 
                using (StreamReader reader = new StreamReader(dataStream))
                {
                    responseFromServer = reader.ReadToEnd();
                }

            }
            catch (Exception e)
            {
                throw e;
            }

            return responseFromServer;
        }
        // json 리스트 데이터를 받아와 ObservableCollection 에 저장
        // 패키지 설치 필수
        public static ObservableCollection<Crackdown> jsonData(string data)
        {
            // string을 바탕으로 JArray 생성
            JArray json = JArray.Parse(data);
            ObservableCollection<Crackdown> list = new ObservableCollection<Crackdown>();
            foreach (JObject obj in json)
            {
                // Json array에 있는 각각의 json 데이터를 Crackdown으로 형변환한다.
                list.Add(obj.ToObject<Crackdown>()); 
            }
            return list;
        }
    }
}
