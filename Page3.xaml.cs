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
using System.ComponentModel;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace Project_Worker
{
    /// <summary>
    /// Page3.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Page3 : Page
    {
        #region 변수선언부분
        int rowCount = 0;
        int done = 0;
        int numberOfPage = 0;
        int signCheck = 99;
        string workName = null;
        string content = null;

        #endregion
        
        HSP sock_set = new HSP();
       HSP sock_get = new HSP();
        int message = 0;
        int id = NowPage_Class.Instance.Recognized_Label_get();
        HSPSocket sock_conn = new HSPSocket("192.168.0.5", 666);

        public Page3()
        {
            NowPage_Class.Instance.sock_conn = sock_conn;
            NowPage_Class.Instance.sock_set = sock_set;
            NowPage_Class.Instance.sock_get = sock_get;

            InitializeComponent();
            sock_conn.connect();
            //작업지시서 선택 초기화
            NowPage_Class.Instance.Selected_Workname_set(null);


            //db에서 인식된 사람을 읽어오는 부분


            //인식된 사람이름 변수
            socket_set();
            NowPage_Class.Instance.sock_set.setMESSAGE(PROTOCOL.HSP_MESSAGE_LOGIN_START);


            sock_send(); //Error Line

            
            socket_get();

            if (message == PROTOCOL.HSP_MESSAGE_OK)
            {
                int Worker_name = NowPage_Class.Instance.Recognized_Label_get();

                MySqlConnection conn_Name = new MySqlConnection();
                conn_Name.ConnectionString = "Server=192.168.0.5;Database=proj;Uid=root;Pwd=4321;";
                conn_Name.Open();
                String Namequery = "select name from user where ID = " + Worker_name + "; "; //"select * from user where ID = '" + this.ID + "';";
                                                                                             // string userNamequery = "select name from user ID = " + Worker_name + ";";

                MySqlCommand cmds = new MySqlCommand(Namequery, conn_Name);
                cmds.ExecuteNonQuery();
                MySqlDataReader read = cmds.ExecuteReader();
                while (read.Read())
                {
                    Page3_text.Text = Convert.ToString(read["name"]) + " 님의 작업지시서 목록";
                }
                // 리스트뷰 생성
                List<Work_Order> mWork_Orders = new List<Work_Order>();


                //db connect부분

                MySqlConnection conn = new MySqlConnection();
                conn.ConnectionString = "Server=192.168.0.5;Database=proj;Uid=root;Pwd=4321;";

                try
                {

                    conn.Open();
                    //  MessageBox.Show(string.Format("connection is '{0}'", conn.State));
                    String query = "select * from work where ID = " + Worker_name + "; "; //"select * from user where ID = '" + this.ID + "';";
                                                                                          // string userNamequery = "select name from user ID = " + Worker_name + ";";

                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    // MySqlCommand cmds = new MySqlCommand(userNamequery, conn);


                    cmd.ExecuteNonQuery();
                    MySqlDataReader rs = cmd.ExecuteReader();
                    while (rs.Read())
                    {
                        done = Convert.ToInt16(rs["done"]);
                        signCheck = Convert.ToInt16(rs["signCheck"]);
                        numberOfPage = Convert.ToInt16(rs["page"]);
                        workName = Convert.ToString(rs["workName"]);
                        content = Convert.ToString(rs["content"]);
                        Console.WriteLine("done : " + done + " \nsigncheck : " + signCheck + "\nnumberofpage : " + numberOfPage + "\nworkName : " + workName);
                        rowCount++;
                        /*if (done != numberOfPage)
                             break;*/

                        //Db에서 작업지시서 정보 읽어와 Listview에 추가하는 곳

                        Work_Order mWork_Order = new Work_Order();
                        mWork_Order.Workname = workName;
                        mWork_Order.Work_content = content;
                        mWork_Order.Work_signCheck = Convert.ToBoolean(signCheck);
                        switch (done)
                        {
                            case 0:
                                mWork_Order.Work_state = "X";
                                break;
                            case 1:
                                mWork_Order.Work_state = "△";
                                break;
                            case 2:
                                mWork_Order.Work_state = "○";
                                break;
                        }

                        mWork_Orders.Add(mWork_Order);

                    }
                    Console.WriteLine("rowcoung : " + rowCount);
                }
                catch (MySqlException e)
                {
                    Console.WriteLine("e : " + e.StackTrace);
                    Console.WriteLine("e msg : " + e.Message);
                }


                Work_Order_List.ItemsSource = mWork_Orders;

                //정렬
                // Work_Order_List.Items.SortDescriptions.Add(new SortDescription("Workname", ListSortDirection.Ascending));
            }
            else
            {
                MessageBox.Show("서버 연결x");
            }


        }


        public void socket_set()
        {
            NowPage_Class.Instance.sock_set.setHSP(PROTOCOL.HSP_HSP_USEHSP);
            NowPage_Class.Instance.sock_set.setDEVICE(PROTOCOL.HSP_DEVICE_DESKTOP);
            NowPage_Class.Instance.sock_set.setSERVICEID(PROTOCOL.HSP_SERVICEID_USER);
            NowPage_Class.Instance.sock_set.setID(id);

        }

        //소켓과 send
        public void sock_send()
        {
            System.Console.WriteLine("SEND ??????????????");
            NowPage_Class.Instance.sock_conn.sendHSP(sock_set); //Error Line
            System.Console.WriteLine("SEND ???");
        }

        //data를 get하는 부분
        public void socket_get()
        {
            NowPage_Class.Instance.sock_conn.receiveHSP(sock_get);
            Console.WriteLine(" 소켓에서 get옴???");
            message = sock_get.getMESSAGE();

            Console.WriteLine("서버에서 온 메시지 : " + message);
            // MessageBox.Show("서버에서 온 메시지 : " + message); 
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {

            DependencyObject dep = (DependencyObject)e.OriginalSource;

            while((dep!=null)&&!(dep is ListViewItem))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }
            if (dep == null)
                return;

            Work_Order tempData = new Work_Order();
            tempData = (Work_Order)Work_Order_List.ItemContainerGenerator.ItemFromContainer(dep);

            NowPage_Class.Instance.Selected_Workname_set(tempData.Workname);
            NowPage_Class.Instance.signSave =   tempData.Work_signCheck; 
            Page3_text.Text = tempData.Workname+" 를 선택하셨습니다.";
            
        }

        private void OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //db추가할때 삭제할 코드
           

            if (!NowPage_Class.Instance.signSave)
            {
                Page4 p4 = new Page4();
                this.NavigationService.Navigate(p4);
                NowPage_Class.Instance.Page_set(4);
            }
            else if (NowPage_Class.Instance.signSave)
            {
                Page5 p5 = new Page5();
                this.NavigationService.Navigate(p5);
                NowPage_Class.Instance.Page_set(5);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (NowPage_Class.Instance.Selected_Workname_get() != null)
            {
                //db추가할때 삭제할 코드
            
                if (!NowPage_Class.Instance.signSave)
                {
                    Page4 p4 = new Page4();
                    this.NavigationService.Navigate(p4);
                    NowPage_Class.Instance.Page_set(4);
                }
                else if (NowPage_Class.Instance.signSave)
                {
                    Page5 p5 = new Page5();
                    this.NavigationService.Navigate(p5);
                    NowPage_Class.Instance.Page_set(5);
                }
            }
            else
                Page3_text.Text = "작업지시서를 먼저 선택하세요 !!";
           

           
        }
    }
}
