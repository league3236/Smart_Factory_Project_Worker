using System;
using System.Collections.Generic;
using System.IO;
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
using MySql.Data;
using MySql.Data.MySqlClient;
using Project_Worker;

namespace Project_Worker
{
    /// <summary>
    /// Page5.xaml에 대한 상호 작용 논리
    /// </summary>
    /// 

    public partial class Page5 : Page
    {
        static string temp = AppDomain.CurrentDomain.BaseDirectory; //현재 디버그폴더  
        string workName = NowPage_Class.Instance.Selected_Workname_get();
        int id = NowPage_Class.Instance.Recognized_Label_get();
        string fileName = null;

        int message = 0;
        public Page5()
        {
            InitializeComponent();

            ////////////////////start db part

            //db에서 page 컬럼에서 값을 불러와 Page_Num에 넣는부분;
            //Page_Num= db page값


            /////////////////////////////// end db part

            //pdf 파일 띄우기   

            MySqlConnection conn3 = new MySqlConnection();
            conn3.ConnectionString = "Server=192.168.0.5;Database=proj;Uid=root;Pwd=4321;";
            conn3.Open();
            //  MessageBox.Show(string.Format("connection is '{0}'", conn.State));
            String query = "select * from work where id = '" + id + "' AND workName = '" + workName + "';";
            MySqlCommand cmdd = new MySqlCommand(query, conn3);
            cmdd.ExecuteNonQuery();
            MySqlDataReader read = cmdd.ExecuteReader();
            while (read.Read())
            {
                fileName = Convert.ToString(read["img"]);
                Console.WriteLine("filename : " + fileName);
                NowPage_Class.Instance.Page_Num = Convert.ToInt16(read["page"]);
            }


            socket_set();
            NowPage_Class.Instance.sock_set.setMESSAGE(PROTOCOL.HSP_MESSAGE_REQUEST_FILE);

            for (int a = 0; a < fileName.Length; a++)
            {
                NowPage_Class.Instance.sock_set.setChar(a + 30, fileName.ElementAt(a));


                NowPage_Class.Instance.sock_set.setChar(a + 31, '\0');


                byte[] bb = NowPage_Class.Instance.sock_set.getByteArray();

                Console.WriteLine("[" + a + "] " + "fn : " + fileName.ElementAt(a) + "hsparray" + (char)bb[a + 30]);
            }


            sock_send();

            socket_get();

            if (message == PROTOCOL.HSP_MESSAGE_DENIED)
            {
                MessageBox.Show("파일명 오류");
            }
            else if (message == PROTOCOL.HSP_MESSAGE_IMAGE_UPLOAD)
            {

                Console.WriteLine("work aaa");
                byte[] arr = NowPage_Class.Instance.sock_get.getByteArray(); //NowPage_Class.Instance.sock_set.getByteArray();
                string sizeStr = "";
                for (int i = 0; i < 10; i++)
                {
                    char temp = (char)arr[i + 10];
                    if (temp == '\0')
                        break;
                    sizeStr += temp;
                }


                Console.WriteLine("siZe : " + sizeStr);

                int size = int.Parse(sizeStr);
                int downloadSize = 0;
                FileStream pdfFile = new FileStream(fileName, FileMode.Create);

                //StreamWriter sr = new StreamWriter("temp.pdf");
                socket_set();
                NowPage_Class.Instance.sock_set.setMESSAGE(PROTOCOL.HSP_MESSAGE_OK);
                sock_send();

                //int req_len = 1024;
                while (downloadSize < size)
                {
                    byte[] buf1 = new byte[1024];
                    //byte[] buf2 = new byte[1024];

                    int nbyte = NowPage_Class.Instance.sock_conn.receive(buf1);
                    
                    /*while(nbyte < req_len)
                    {
                        for (int i = 0; i<= nbyte; i++)
                        {
                            buf2[i] = buf1[i];
                        }
                        req_len -= nbyte;
                        nbyte = NowPage_Class.Instance.sock_conn.receive(buf1);
                    }*/

                    downloadSize += nbyte;
                    Console.WriteLine("Download : " + nbyte + "  (" + downloadSize + "/" + size + ")");
                    pdfFile.Write(buf1, 0, 1024);



                }

                pdfFile.Close();

            }




            //pdf 파일 띄우기    
            this.WinFormHost.Child = UserControl1.Instance;
            UserControl1.Instance.filename = fileName;
            UserControl1.Instance.Init();


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
            NowPage_Class.Instance.sock_conn.sendHSP(NowPage_Class.Instance.sock_set);
            Console.WriteLine("SEND ???");
        }

        //data를 get하는 부분
        public void socket_get()
        {
            NowPage_Class.Instance.sock_conn.receiveHSP(NowPage_Class.Instance.sock_get);
            Console.WriteLine(" 소켓에서 get옴???");
            message = NowPage_Class.Instance.sock_get.getMESSAGE();

            Console.WriteLine("서버에서 온 메시지 : " + message);
            // MessageBox.Show("서버에서 온 메시지 : " + message); 
        }

        private void Left_btn(object sender, RoutedEventArgs e)
        {
            //PDF 이전페이지 보여주는 기능
            UserControl1.Instance.LeftBtn = true;

            //페이지 감소
            if (NowPage_Class.Instance.Page_Num > 1)
                NowPage_Class.Instance.Page_Num--;
            //db에다가 page 컬럼에 Page_Num 넣어야함
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = "Server=192.168.0.5;Database=proj;Uid=root;Pwd=4321;";
            conn.Open();


            //  MessageBox.Show(string.Format("connection is '{0}'", conn.State));
            String query = "update work set done = 1, page = " + NowPage_Class.Instance.Page_Num + " where workName = '" + NowPage_Class.Instance.Selected_Workname_get() + "' and ID = " + NowPage_Class.Instance.Recognized_Label_get() + ";";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.ExecuteNonQuery();

        }

        private void Right_btn(object sender, RoutedEventArgs e)
        {
            //PDF다음 페이지 보여주는 기능
            UserControl1.Instance.RightBtn = true;

            //페이지 증가
            // if (Page_Num < Max_PageNum)
            NowPage_Class.Instance.Page_Num++;
            //db에다가 page 컬럼에 Page_Num 넣어야함

            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = "Server=192.168.0.5;Database=proj;Uid=root;Pwd=4321;";
            conn.Open();

            //  MessageBox.Show(string.Format("connection is '{0}'", conn.State));
            String query = "update work set done = 1, page = " + NowPage_Class.Instance.Page_Num + " where workName = '" + NowPage_Class.Instance.Selected_Workname_get() + "' and ID = " + NowPage_Class.Instance.Recognized_Label_get() + ";";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.ExecuteNonQuery();

        }

        private void Finish_button_Click(object sender, RoutedEventArgs e)
        {


            if (MessageBox.Show("[경고] 모든 페이지의 작업을 완료하였습니까?", "", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No, MessageBoxOptions.None) == MessageBoxResult.No)
            {
                //아니요 눌렀을경우

            }
            else
            {
                //예 눌렀을 경우
                if (NowPage_Class.Instance.Page_get() == 5)
                {
                    //db에다가 done체크 해야함
                    MySqlConnection conn2 = new MySqlConnection();
                    conn2.ConnectionString = "Server=192.168.0.5;Database=proj;Uid=root;Pwd=4321;";
                    conn2.Open();
                    //  MessageBox.Show(string.Format("connection is '{0}'", conn.State));
                    String query = "update work set done = 2 where workName = '" + workName + "' and ID = " + id + ";";
                    MySqlCommand cmd = new MySqlCommand(query, conn2);
                    cmd.ExecuteNonQuery();

                    //페이지 이동
                    Page3 p3 = new Page3();
                    this.NavigationService.Navigate(p3);
                }

            }

        }


        private void List_button(object sender, RoutedEventArgs e)
        {   //취소하고 목록 다시보는 버튼



            Page3 p3 = new Page3();
            this.NavigationService.Navigate(p3);
        }
    }
}
