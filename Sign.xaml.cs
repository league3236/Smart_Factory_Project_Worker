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
using MySql.Data;
using MySql.Data.MySqlClient;
using System.IO;
using System.Windows.Markup;
using System.Windows.Ink;
using System.ComponentModel;
using System.Data;



namespace Project_Worker
{
    /// <summary>
    /// Page4.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Page4 : Page
    {
        int message = 0;
        string filesize = null;
        String filePath = null;
        int i = 0;
        int id = NowPage_Class.Instance.Recognized_Label_get();
        string workName = NowPage_Class.Instance.Selected_Workname_get();


        public Page4()
        {

            InitializeComponent();
        
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

        //파일 크기 구하기
        private string GetFileSize(double byteCount)
        {
            string size = "0 Bytes";

            size = byteCount.ToString();

            /* if (byteCount >= 1073741824.0)
                 size = String.Format("{0:##.##}", byteCount / 1073741824.0) + " GB";

             else if (byteCount >= 1048576.0)
                 size = String.Format("{0:##.##}", byteCount / 1048576.0) + " MB";

             else if (byteCount >= 1024.0)
                 size = String.Format("{0:##.##}", byteCount / 1024.0) + " KB";

             else if (byteCount > 0 && byteCount < 1024.0)
                 size = byteCount.ToString() + " Bytes";
*/
            return size;
        }


        public void SaveCanvas(Page window, InkCanvas canvas, int dpi, string filename)
        {
            Size sizes = new Size(window.Width, window.Height);
            canvas.Measure(sizes);
            //canvas.Arrange(new Rect(size));

            var rtb = new RenderTargetBitmap(
                (int)window.Width, //width 
                (int)window.Height, //height 
                dpi, //dpi x 
                dpi, //dpi y 
                PixelFormats.Pbgra32 // pixelformat 
                );
            rtb.Render(canvas);

            SaveRTBAsPNG(rtb, filename);
        }


        private void SaveRTBAsPNG(RenderTargetBitmap bmp, string filename)
        {
            var enc = new System.Windows.Media.Imaging.PngBitmapEncoder();
            enc.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(bmp));

            using (var stm = System.IO.File.Create(filename))
            {
                enc.Save(stm);  //sign 저장하는 부분
                Console.WriteLine("저장함??");

            }
        }


        private void Finish_Click(object sender, RoutedEventArgs e)
        {
            /// </summary>
            if (this.signCanvas.Strokes.Count > 0)
            {

                FileInfo thisFile = null;
                string temp = AppDomain.CurrentDomain.BaseDirectory;
                filePath = temp+"\\sign\\" + id + "_" + workName + ".jpg";
                try
                {
                    thisFile = new FileInfo(filePath);
                }
                catch (FileNotFoundException es)
                {
                    Console.WriteLine("es : " + es.Message);
                }

                Console.WriteLine("filepath : " + filePath);
                SaveCanvas(this, this.signCanvas, 96, filePath);

                byte[] buf = File.ReadAllBytes(filePath);
                //  Console.Write("BUFFER : ");
                //for (int i = 0; i < buf.Length; i++)
                //{
                //    Console.Write((char)buf[i]);
                //    if (i % 20 == 19) Console.WriteLine("");
                //}

                //Console.WriteLine("============================================");

                //filesize = GetFileSize(thisFile.Length);   //필요없음


                socket_set();
                NowPage_Class.Instance.sock_set.setMESSAGE(PROTOCOL.HSP_MESSAGE_SIGN_UPLOAD);
                sock_send();
                socket_get();

                //StreamReader file = new StreamReader(filePath);
                //Console.WriteLine("sign fiel : " + file);

                //string line = null;
                //String str = GetFileSize(thisFile.Length);
                //while ((line = file.ReadLine()) != null)
                //{
                //    Console.Write(line);

                //}


                Console.WriteLine();
                ///<summary>
                ///PROTOCOL.HSP_MESSAGE_SIGN_UPLOAD에 대한 대답에 대해서
                /// </summary>
                if (message == PROTOCOL.HSP_MESSAGE_OK)
                {
                    socket_set();
                    NowPage_Class.Instance.sock_set.setMESSAGE(PROTOCOL.HSP_MESSAGE_IMAGE_UPLOAD);

                    String str = GetFileSize(thisFile.Length);
                    for (int i = 0; i < str.Length; i++)
                    {
                        NowPage_Class.Instance.sock_set.setChar(i + 10, str.ElementAt(i));
                        NowPage_Class.Instance.sock_set.setChar(i + 11, '\0');
                    }

                    //사인 이미지 이름은 id+workName+s로 이루어짐.
                    string fileName = id + NowPage_Class.Instance.Selected_Workname_get() + "s";
                    for (int a = 0; a < fileName.Length; a++)
                    {
                        NowPage_Class.Instance.sock_set.setChar(a + 30, fileName.ElementAt(a));
                        NowPage_Class.Instance.sock_set.setChar(a + 31, '\0');
                    }


                    sock_send();
                    socket_get();

                    ///<summary>
                    ///PROTOCOL.HSP_MESSAGE_IMAGE_UPLOAD에 대한 대답에 대해서
                    /// </summary>
                    if (message == PROTOCOL.HSP_MESSAGE_OK)
                    {
                        //실질적으로 파일을 보내는 부분 + db_update


                        NowPage_Class.Instance.sock_conn.send(buf);

                        socket_get();

                        if (message == PROTOCOL.HSP_MESSAGE_OK) { 

                            MySqlConnection conn2 = new MySqlConnection();
                            conn2.ConnectionString = "Server=192.168.0.5;Database=proj;Uid=root;Pwd=4321;";
                            conn2.Open();
                            //  MessageBox.Show(string.Format("connection is '{0}'", conn.State));
                            String query = "update work set signCheck = 1 , done = 1 where workName = '" + NowPage_Class.Instance.Selected_Workname_get() + "' and ID = " + id + ";";
                            MySqlCommand cmd = new MySqlCommand(query, conn2);


                            cmd.ExecuteNonQuery();

                            Page5 p5 = new Page5();
                            this.NavigationService.Navigate(p5);
                            Console.WriteLine("sign aaa");
                            NowPage_Class.Instance.Page_set(5);
                            Console.WriteLine("sign b ");
                        }

                    }
                }
               




            }
            else
            {
                MessageBox.Show("서명 후에 눌러주세요.");
            }
        }

        private void Cancel_Clcik(object sender, RoutedEventArgs e)
        {
            Page3 p3 = new Page3();
            this.NavigationService.Navigate(p3);
            NowPage_Class.Instance.Page_set(3);
        }
    }


}


