using System;
using System.Windows;

namespace Project_Worker
{
    internal class RouteEventHandler
    {
        private Action<object, RoutedEventArgs> medie_video_MediaEnded;

        public RouteEventHandler(Action<object, RoutedEventArgs> medie_video_MediaEnded)
        {
            this.medie_video_MediaEnded = medie_video_MediaEnded;
        }
    }
}