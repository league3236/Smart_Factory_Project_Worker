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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Windows.Threading;

namespace Project_Worker
{

    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : NavigationWindow
    {
        //오픈시브이 서버와 연결할 클래스생성
        Classes.FSP face_set = new Classes.FSP();
        Classes.FSPSocket face_conn = new Classes.FSPSocket("127.0.0.1", 555);
        //Classes.FSPSocket face_conn = new Classes.FSPSocket("192.168.0.3", 555);

        //Thread mTHread;
        Thread Opencv_SV;

        public MainWindow()
        {
            InitializeComponent();
            this.NavigationService.Navigated += new NavigatedEventHandler(NavigationService_Navigated);

            face_conn.set_Connect_Succeed(false);
            face_conn.connect();
            /*
            if (face_conn.get_Connect_Succeed() == false)
            {
                MessageBox.Show("얼굴인식 서버와 접속이 실패하였습니다.");
                this.Close();
            }*/
            //UI관련된것들 Init
            Init();


            Opencv_SV = new Thread(Opencv_SV_Receive);
            Opencv_SV.Start();

            //mTHread = new Thread(Communication);
            //mTHread.Start();



            //얼굴인식 서버와 접속
            
            

        }

        void Opencv_SV_Receive()  //opencv 서버에서 메세지받는 역할
        {
            int Receive_Check;
            
            while (face_conn.get_Connect_Succeed() == true)
            {
                Receive_Check = face_conn.receiveFSP(face_set);
                if (Receive_Check < 0)
                {
                    face_conn.set_Connect_Succeed(false);
                
                    MessageBox.Show("Opencv 서버로 부터 메세지를 받을 수 없습니다. 프로그램을 종료 후 다시 실행 해주세요.");
                   
                    NowPage_Class.Instance.Local_Server_num = 5; //프로그램 강제종료 
                   
                }
                else
                {
                    NowPage_Class.Instance.Local_Server_num = face_set.getMESSAGE();
                }

                //받은 메세지 처리
                Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                {

                    //사용할 메서드 및 동작
                    switch (NowPage_Class.Instance.Local_Server_num)
                    {
                        case 1:

                            Page2 p2 = new Page2();
                            this.NavigationService.Navigate(p2);
                            NowPage_Class.Instance.Page_set(2);
                            NowPage_Class.Instance.Local_Server_num = 0;
                            break;

                        case 2:
                           
                            Page1 p1 = new Page1();
                            this.NavigationService.Navigate(p1);
                            NowPage_Class.Instance.Page_set(1);
                            NowPage_Class.Instance.Local_Server_num = 0;
                            NowPage_Class.Instance.Recognized_Label_set(0);
                            NowPage_Class.Instance.Selected_Workname_set(null);
                            break;

                        case 3:
                            
                            NowPage_Class.Instance.Page_set(3);
                            NowPage_Class.Instance.Local_Server_num = 0;

                            //서버로부터 인식된 사람 라벨을 세팅하는 곳
                            int Label = face_set.getID();
                            NowPage_Class.Instance.Recognized_Label_set(Label);

                            Page3 p3 = new Page3();
                            this.NavigationService.Navigate(p3);
                            break;
                        case 4:

                            MessageBox.Show("얼굴인식 서버와 접속이 끊어졌습니다. 프로그램을 재시작하세요.");
                            NowPage_Class.Instance.Page_set(0);
                            NowPage_Class.Instance.Local_Server_num = 0;

                            UserControl1.Instance.PDF.Abort();

                            face_conn.set_Connect_Succeed(false);
                            face_conn.close();
                            Environment.Exit(0);
                            break;

                        case 5:
                            MessageBox.Show("프로그램이 종료되었습니다.");
                            UserControl1.Instance.PDF.Abort();

                            Environment.Exit(0);
                            return;

                        default:
                            break;
                    }
                }));

            }
        }

   

        /// /////////////////////////////////  이 아래는 컨트롤러 이벤트 관련 함수들 //////////////////////

        //backspace key 방지
        private void NavigationService_Navigated(object sender, NavigationEventArgs e)
        {
            this.NavigationService.RemoveBackEntry();
        }

        void Init() //Mainwindow 초기화
        {
            //종료시 발생 이벤트 추가를 위해 Closing 생성자에 추가
            this.Closing += new System.ComponentModel.CancelEventHandler(Window_Closing);

            //page관리하는  싱글톤 클래스
            NowPage_Class.Instance.Page_set(1);
            NowPage_Class.Instance.Local_Server_num = 0;
            NowPage_Class.Instance.Recognized_Label_set(0);
            NowPage_Class.Instance.Selected_Workname_set(null);
        }


        //종료시 발생 이벤트
        void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show("종료 하시겠습니까?", "", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No, MessageBoxOptions.None) == MessageBoxResult.No)
            {
            
                e.Cancel = true;
            }
            else
            {
                // mTHread.Abort();  //윈도우 종료시 스레드 종료
                UserControl1.Instance.PDF.Abort(); //PDF 관리하는 스레드 종료
             
                Opencv_SV.Abort();
               
                if (face_conn.get_Connect_Succeed() == true)
                {                
                    face_conn.sendFSP();
                    face_conn.close();
                }
                   
            }
        }

        //키보드 이벤트 발생
        private void WindowOnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                this.Close();
    
        }


    }
}
