using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using Project_Worker;

namespace Project_Worker
{
    public partial class UserControl1 : UserControl
    {
        public string filename;
        public Boolean LeftBtn;
        public Boolean RightBtn;
        public Thread PDF;

        private static volatile UserControl1 instance;
        private static object syncRoot = new Object();

        public UserControl1()
        {
            InitializeComponent();

            PDF = new Thread(PDF_Control);
            PDF.Start();



        }

        //Sington 객체 생성
        public static UserControl1 Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new UserControl1();
                    }
                }
                return instance;
            }
        }


        //thread function
        void PDF_Control()
        {
          // int lastpage = (int) Convert.ChangeType(axAcroPDF1.gotoLastPage(), typeof(int));
            
            while (true)
            {
                this.axAcroPDF1.Invoke(new Action(() =>
                {

                    if (LeftBtn == true)  //왼쪽에 있는 버튼 눌린상태 이고 작업스레드 라면
                    {

                        this.axAcroPDF1.gotoPreviousPage();

                        LeftBtn = false;
                    }
                    if (RightBtn == true)
                    {

                        this.axAcroPDF1.gotoNextPage();
                        RightBtn = false;
                    }
                }));
            }

        }

        public void Init() //PDF Setting
        {
            this.axAcroPDF1.LoadFile(filename);
            this.axAcroPDF1.setLayoutMode("SinglePage");
            this.axAcroPDF1.setCurrentPage(NowPage_Class.Instance.Page_Num);
            this.axAcroPDF1.setShowScrollbars(false);
            this.axAcroPDF1.setShowToolbar(false);
            this.axAcroPDF1.setZoom(70);
            
        }

    }
}
