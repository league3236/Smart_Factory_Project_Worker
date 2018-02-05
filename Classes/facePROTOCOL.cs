using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Worker.Classes
{
    class facePROTOCOL
    {
        /// <summary> 
        /// 얼굴인식 시작 = 1
        /// </summary> 
        public const int Ls_face_detect_start = 1;
        /// <summary>
        ///사람이 사라짐 = 2
        /// </summary> 
        public const int Ls_detection_end = 2;
        /// <summary>
        ///id 값 = 3
        /// </summary> 
        public const int Ls_Label = 3;

        /// <summary>
        ///얼굴인식 프로그램이 종료됨
        /// </summary> 
        public const int Ls_exit = 4;

        //cshop프로그램이 종료됨
        public const int C_Shop_exit = 5;
    }
}
