﻿using System;
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

namespace Project_Worker
{
    
    public partial class Page1 : Page
    {
        public Page1()
        {
            InitializeComponent();
            
         
            medie_video.Play();
                   
        }

        private void medie_video_MediaEnded(object sender, RoutedEventArgs e)
        {
           
            medie_video.Stop();
            medie_video.Play();
        }

    }
}
