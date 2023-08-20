using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ACSClient
{
    /// <summary>
    /// ToastMessage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ToastMessage : Window
    {
        public ToastMessage(string message)
        {
            InitializeComponent();

            // 생성할 때 인자로 준 string을 메시지로 토스트 출력한다.
            Message.Text = message;

            // 나타날때의 Animation 설정 (0.8초동안 서서히 나타남)
            this.Visibility = Visibility.Visible;
            DoubleAnimation fadeIn = new DoubleAnimation();
            fadeIn.From = 0;
            fadeIn.To = 1;
            fadeIn.Duration = new Duration(TimeSpan.FromSeconds(0.8));

            // 사라질 때의 Animation 설정(0.8초동안 서서히 사라짐)
            DoubleAnimation fadeOut = new DoubleAnimation();
            fadeOut.From = 1;
            fadeOut.To = 0;
            fadeOut.Duration = new Duration(TimeSpan.FromSeconds(0.8));

            // 나타날때의 Animation이 다 끝나고 나면 실행할 함수 설정
            fadeIn.Completed += (s, e) =>
            {
                // 사라질때의 Animation 시작한다.
                this.BeginAnimation(OpacityProperty, fadeOut);
            };
            // 사라질때의 Animation이 다 끝나고 나면 실행할 함수 설정
            fadeOut.Completed += new EventHandler(FadeOutAnimationEndHandler);

            // 나타날때의 Animation 시작
            this.BeginAnimation(OpacityProperty, fadeIn);

        }

        // 사라질때의 Animation 이 다 끝나고 나면 실행할 함수
        private void FadeOutAnimationEndHandler(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
