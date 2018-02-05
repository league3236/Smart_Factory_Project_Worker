using System;
using System.Collections.Generic;
using System.Drawing;
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
using System.Timers;
using System.Windows.Threading;

namespace Project_Worker
{

    public partial class Page2 : Page
    {
        int count = 0;
        public Page2()
        {
            InitializeComponent();

            //이미지 업로드
            System.Drawing.Bitmap img = Project_Worker.Properties.Resources.얼굴가이드_사진;
            MemoryStream imgStream = new MemoryStream();
            img.Save(imgStream, System.Drawing.Imaging.ImageFormat.Bmp);
            imgStream.Seek(0, SeekOrigin.Begin);
            BitmapFrame newimg = BitmapFrame.Create(imgStream);
            Face_Guide.Source = newimg;

            //글씨 움직이게하는 타이머
            DispatcherTimer Timer = new DispatcherTimer();
            Timer.Interval = TimeSpan.FromMilliseconds(500);
            Timer.Tick += new EventHandler(Timer_Tick);
            Timer.Start();
        }      

        private void Timer_Tick(object sender,EventArgs e)
        {
            if (count > 10000)
                count = 0;

            count++;

            switch (count % 5)
            {
                case 0:
                    Animation_Text.Text = "얼굴을 인식하고 있습니다";
                    break;
                case 1:
                    Animation_Text.Text = "얼굴을 인식하고 있습니다 .";
                    break;
                case 2:
                    Animation_Text.Text = "얼굴을 인식하고 있습니다 . .";
                    break;
                case 3:
                    Animation_Text.Text = "얼굴을 인식하고 있습니다 . . .";
                    break;
                case 4:
                    Animation_Text.Text = "얼굴을 인식하고 있습니다 . . . .";
                    break;
                default:
                    break;
            }
           
        }
    }
}
