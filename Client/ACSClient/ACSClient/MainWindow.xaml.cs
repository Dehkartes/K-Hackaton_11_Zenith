using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ACSClient
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {   
        DispatcherTimer timer;
        CrackdownList CrackdownDataList;
        // 콤보박스 아이템 리스트
        List<string> TypeList = new List<string>()
        {
            "전체", "무단 투기", "담배"
        };
        
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                CrackdownDataList = new CrackdownList();
                CategoryComboBox.ItemsSource = TypeList;
                // 타이머 시작..
                DatabaseContectTimer();

                // CrackdownDataList.ServerContect();
                // 데이터베이스에 연결하여 소스 연결
                CrackdownDataList.DataBaseContect();
                DataListView.ItemsSource = CrackdownDataList.DataList;
            } catch(MySqlException) {
                MessageBox.Show("데이터베이스 접속에 실패하였습니다. 네트워크 연결상태를 확인해주십시오");
                DataListView.ItemsSource = CrackdownDataList.DataList;
            } catch {
                MessageBox.Show("프로그램 열기에 실패하였습니다. 자세한 사항은 문의해주십시오");
                DataListView.ItemsSource = CrackdownDataList.DataList;
            }
        }

        #region 이벤트 발생 함수

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AllRefresh();
                CategoryComboBox.SelectedIndex= 0;
                ItemDatePicker.SelectedDate = null;
                AllTimeCheckBox.IsChecked = true;
            }
            catch (MySqlException)
            {
                MessageBox.Show("데이터베이스에 접속하는데에 실패하였습니다. 네트워크 연결상태를 확인해주십시오");
            }
            catch
            {
                MessageBox.Show("새로고침하는데 실패하였습니다. 자세한 사항은 문의해주십시오");
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(AllTimeCheckBox.IsChecked == false && ItemDatePicker.SelectedDate == null) { throw new Exception("검색 오류 : 날짜를 다시 체크해주세요"); }
                SearchListView();
            }
            catch (MySqlException)
            {
                MessageBox.Show("데이터베이스에 접속하는데에 실패하였습니다. 네트워크 연결상태를 확인해주십시오");
            }
            catch
            {
                MessageBox.Show("검색하는데 실패하였습니다. 자세한 사항은 문의해주십시오"); ;
            }
        }

        #endregion

        #region 내부 함수

        // 날짜픽커, 모든날짜체크박스, 유형 콤보박스에 체크 및 선택 유무를 살피고 그에 따라 리스트뷰 출력
        private void SearchListView()
        {
            try
            {
                int select = CategoryComboBox.SelectedIndex;
                string dateTime;
                // 모든 날짜를 체크하지 않고 datepicker로 날짜를 선택한경우
                if (AllTimeCheckBox.IsChecked == false && !ItemDatePicker.SelectedDate.Equals(null))
                {
                    dateTime = ItemDatePicker.SelectedDate.Value.ToString("yy/MM/dd");
                    // 유형이 전체가 아닐때
                    if (!(select == 0))
                    {
                        // CrackdownDataList에 저장되어 있었던 SelectList에 조건에 맞는 리스트를 저장한다.
                        // 이때, where안의 람다식에 '(data.Crackdown_Type==(select-1))'에서 select-1은, 인덱스는 1부터 시작하기 때문에 -1을 함. 
                        CrackdownDataList.SelectList = CrackdownDataList.DataList.Where(data => (data.Crackdown_Type==(select-1)) && data.Date.ToString("yy/MM/dd").Equals(dateTime)).ToList();
                        // ListView의 소스 바꾸기
                        DataListView.ItemsSource = CrackdownDataList.SelectList;
                        DataListView.Items.Refresh();
                    }
                    // 유형이 전체일때
                    else if (select == 0)
                    {
                        CrackdownDataList.SelectList = CrackdownDataList.DataList.Where(data => data.Date.ToString("yy/MM/dd").Equals(dateTime)).ToList();
                        DataListView.ItemsSource = CrackdownDataList.SelectList;
                        DataListView.Items.Refresh();
                    }

                }
                // datepicker로 날짜를 선택했어도 모든 날짜 체크 박스를 체크한 모든 경우
                // 또는 모든 날짜 체크 박스를 하지 않았는데 datepicker로 날짜를 체크하지 않은 경우
                else
                {
                    // 유형이 전체일때
                    if (!(select == 0))
                    {
                        CrackdownDataList.SelectList = CrackdownDataList.DataList.Where(data => data.Crackdown_Type == (select - 1)).ToList();
                        DataListView.ItemsSource = CrackdownDataList.SelectList;
                        DataListView.Items.Refresh();
                    }
                    // 유형이 전체가 아닐때
                    else
                    {
                        DataListView.ItemsSource = CrackdownDataList.DataList;
                        DataListView.Items.Refresh();
                    }
                        
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // 초기화 함수.
        private void AllRefresh()
        {
            try
            {
                // CrackdownDataList.ServerContect();
                CrackdownDataList.DataBaseContect();
                DataListView.ItemsSource = CrackdownDataList.DataList;
                
            } catch (Exception ex)
            {
                throw ex;
            }
        }

        // 비동기로 실행되는 함수.
        /*
         
         private void DatabaseContectTimer()
        {
            try
            {
                var aTimer = new System.Timers.Timer(1000 * 30); // 1000당 1초
                aTimer.Elapsed += (s, e) => { 
                    CrackdownDataList.ServerContect(); 
                    DataListView.Items.Refresh(); 
                };
                aTimer.AutoReset = true;
                aTimer.Start();
            }
            catch (MySqlException)
            {
                MessageBox.Show("데이터베이스에 접속하는데에 실패하였습니다. 네트워크 연결상태를 확인해주십시오");
            }
            catch
            {
                MessageBox.Show("새로고침하는데 실패하였습니다. 자세한 사항은 문의해주십시오");
            }
        }
         */
        

        // UI쓰레드로 5분마다 한번씩 서버와 연결하여 리스트뷰 초기화.
        // 만약 검색을 하여 조건이 있는 경우에는 그 조건에 따라서 검색
        private void DatabaseContectTimer()
        {
            try
            {
                timer = new DispatcherTimer();
                // 5분에 한번씩 함수 호출
                timer.Interval = TimeSpan.FromSeconds(60 * 5);
                // 호출되는 이벤트 함수 (서버에 연결 후, 검색이 되어있는지 확인)
                timer.Tick += new EventHandler(timer_Tick);
                timer.Start();
            }
            catch (MySqlException)
            {
                MessageBox.Show("데이터베이스 접속에 실패하였습니다. 네트워크 연결상태를 확인해주십시오");
            }
            catch
            {
                MessageBox.Show("새로고침에 실패하였습니다. 자세한 사항은 문의해주십시오");
            }
        }

        // 5분마다 호출되는 함수. 데이터베이스에 연결 후 SearchListView()함수를 통해 현재 어떤 조건으로 되어있는지 확인 및 출력
        private void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                // CrackdownDataList.ServerContect();
                CrackdownDataList.DataBaseContect();
                SearchListView();

            }
            catch (MySqlException)
            {
                // 예외가 발생하면 MessageBox가 아닌 유사 Toast박스 윈도우로 출력
                Window Toast = new ToastMessage("데이터베이스 접속에 실패하였습니다. 네트워크 연결상태를 확인해주십시오");
                // MainWindow 창 한가운데에 출력하기 위해 현재 출력되어 있는 윈도우를 Toast의 Owner로 설정.
                // Toast는 Owner 가운데에 출력되도록 설정됨
                Toast.Owner  = Application.Current.MainWindow;
                Toast.Show();
            }
            catch
            {
                Window Toast = new ToastMessage("새로고침에 실패하였습니다. 자세한 사항은 문의해주십시오");
                Toast.Owner = Application.Current.MainWindow;
                Toast.Show();
            }
        }

        #endregion

    }
}