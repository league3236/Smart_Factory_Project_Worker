using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
namespace Project_Worker.Classes
{
    class FSPSocket
    {
        Socket socks;
        string ip;
        int port;
        bool Connect_Succeed;

        public FSPSocket(string ip, int port)
        {
            socks = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.ip = ip;
            this.port = port;
        }


        public void set_Connect_Succeed(bool value)
        {
            Connect_Succeed = value;
        }

        public Boolean get_Connect_Succeed()
        {
            return Connect_Succeed;
        }

        public void connect()
        {
            try
            {
                socks.Connect(ip, port);
                Connect_Succeed = true;
            }
            catch (SocketException e)
            {
                Console.WriteLine("에러 코드 : " + e.NativeErrorCode);
                
                Connect_Succeed = false;
            }

        }

        public void close()
        {
            socks.Close();
        }

        public void sendFSP()
        {
            byte[] buf = new byte[2];
            buf[0] = facePROTOCOL.C_Shop_exit;
            
            socks.Send(buf);
           
        }

        public int receiveFSP(FSP fsp)
        {
            byte[] buf = new byte[3];

            int res = socks.Receive(buf);

            if (res >= 0)
                fsp.faceBuffer = buf;

            return res;
        }

    }
}
