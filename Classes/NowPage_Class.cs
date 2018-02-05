using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project_Worker;
using MySql.Data.MySqlClient;

namespace Project_Worker
{
   public sealed class NowPage_Class
    {
        //프로그램 시작도 여기서 알림
        //현재 페이지의 정보, 인식된 사람의 라벨, 선택된 작업의 정보를 가진 클래스
        //이 객체는 프로그램에서 한번만 생성됨 
        //모든 클래스에서 한 객체를 공유

        private static volatile NowPage_Class instance;
        private static object syncRoot = new Object();


        


        private NowPage_Class()
        {
        }
        public static NowPage_Class Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new NowPage_Class();
                    }
                }
                return instance;
            }
        }

        //프로그램이 현재 보여주고있는 페이지 정보
        private int Page;

        public int Page_get()
        {
            return Page;
        }
        public void Page_set(int value)
        {
            Page = value;
        }

        //프로그램에서 인식된 라벨 정보
        private int Recognized_Label;

        public int Recognized_Label_get()
        {
            return Recognized_Label;
        }
        public void Recognized_Label_set(int value)
        {
            Recognized_Label = value;
        }

        //프로그램에서 선택된 작업명 
        private string Selected_Workname;

        public string Selected_Workname_get()
        {
            return Selected_Workname;
        }
        public void Selected_Workname_set(string value)
        {
            Selected_Workname = value;
        }

        public int Local_Server_num; //얼굴인식 로컬 서버에서 받은 숫자

        //프로그램의 시작을 알리는 변수;
        public bool Program_Start;

        public Boolean signSave;

       public HSPSocket sock_conn; //117.17.142.98         //127.0.0.1   //117.17.143.229

        public HSP sock_set;
        public HSP sock_get;
        public int Page_Num; //PDF Page 관리
       
    }
}
