using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Worker.Classes
{
    class FSP
    {
        public byte[] faceBuffer;

        public FSP()
        {
            faceBuffer = new byte[2];
        }

        public void setMESSAGE(int message)
        {
            faceBuffer[0] = (byte)message;
        }

        public void setID(int id)
        {
            faceBuffer[1] = (byte)id;
        }

        public int getMESSAGE()
        {
            return (int)faceBuffer[0];
        }

        public int getID()
        {
            return (int)faceBuffer[1];
        }
    }
}
