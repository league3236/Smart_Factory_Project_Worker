using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Worker
{       //작업 지시서 클래스
    class Work_Order
    {
        public string Workname { get; set; }
        public string Work_content { get; set; }    
        public string Work_state { get; set; }
        public bool Work_signCheck { get; set; }
    }
}
