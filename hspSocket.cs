using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Project_Worker
{

    public class PROTOCOL
    {


        /// <summary>
        ///무조건 111
        /// </summary>
        public const int HSP_HSP_USEHSP = 111;



        /// <summary>
        ///장치가 서버인경우=0
        /// </summary>
        public const int HSP_DEVICE_SERVER = 0;

        /// <summary>
        ///장치가 모바일인경우=1
        /// </summary>
        public const int HSP_DEVICE_MOBILE = 1;

        public const int HSP_DEVICE_DESKTOP = 2;




        public const int HSP_SERVICEID_SERVER = 0;

        public const int HSP_SERVICEID_ADMIN = 1;

        public const int HSP_SERVICEID_USER = 2;



        /// <summary>
        ///로그인 하기 전에 ID = 100
        /// </summary> 
        public const int HSP_ID_UNKNOWN = 100;

        /// <summary>
        ///서버의 ID = 99
        /// </summary> 
        public const int HSP_ID_SERVER = 99;




        /// <summary>
        ///message OK = 1
        /// </summary>          
        public const int HSP_MESSAGE_OK = 1;

        /// <summary>
        ///message FAIL=2
        /// </summary>   
        public const int HSP_MESSAGE_FAIL = 2;

        /// <summary>
        ///message 거부=3
        /// </summary>         
        public const int HSP_MESSAGE_DENIED = 3;

        /// <summary>
        ///프로그램 시작합니다.=4
        /// </summary>   
        public const int HSP_MESSAGE_PROGRAM_START = 4;

        /// <summary>
        ///로그인합니다=5
        /// </summary> 
        public const int HSP_MESSAGE_LOGIN_START = 5;

        /// <summary>
        ///안면인식시작=6
        /// </summary> 
        public const int HSP_MESSAGE_FACEDETECTING_START = 6;

        /// <summary>
        ///안면인식 유저=7
        /// </summary> 
        public const int HSP_MESSAGE_FACEDETECTING_IS_USER = 7;

        /// <summary>
        ///안면인식 관리자=8
        /// </summary> 
        public const int HSP_MESSAGE_FACEDETECTING_IS_ADMIN = 8;

        /// <summary>
        ///안면인식 알수없는 사람=9
        /// </summary> 
        public const int HSP_MESSAGE_FACEDETECTING_UNKNOWNPERSON = 9;

        /// <summary>
        ///안면인식 종료=10
        /// </summary> 
        public const int HSP_MESSAGE_FACEDETECTING_END = 10;

        /// <summary>
        ///사인필요해요=11
        /// </summary>
        public const int HSP_MESSAGE_REQUIRED_SIGN = 11;

        /// <summary>
        ///사인업로드할게요=12
        /// </summary> 
        public const int HSP_MESSAGE_SIGN_UPLOAD = 12;

        /// <summary>
        ///이미지업로드할게요=13
        /// </summary> 
        public const int HSP_MESSAGE_IMAGE_UPLOAD = 13;

        /// <summary>
        ///done을 누름=14
        /// </summary> 
        public const int HSP_MESSAGE_PAGE_DONE = 14;

        /// <summary>
        ///로그아웃=15
        /// </summary> 

        public const int HSP_MESSAGE_LOGOUT = 15;

        /// <summary>
        ///업무지시서주세요=16
        /// </summary> 
        public const int HSP_MESSAGE_REQUEST_FILE = 16;//파일 요구






        const int HSP_MESSAGE_UPLOAD = 1;

        const int HSP_MESSAGE_DOWNLOAD = 1;





    }



   public class HSP
    {
        public byte[] hspBuffer;

        public HSP()
        {

            hspBuffer = new byte[100];



        }



        public HSP(byte[] hsp)
        {

            hspBuffer = hsp;

        }



        public byte[] getByteArray()
        {

            return hspBuffer;

        }







        public void setHSP(int hsp)
        {

            hspBuffer[0] = (byte)hsp;

        }



        public void setDEVICE(int device)
        {

            hspBuffer[1] = (byte)device;

        }



        public void setSERVICEID(int serviceID)
        {

            hspBuffer[2] = (byte)serviceID;

        }



        public void setID(int id)
        {

            hspBuffer[3] = (byte)id;

        }



        public void setMESSAGE(int message)
        {

            hspBuffer[4] = (byte)message;

        }



        public void setBUFFER(int n, int setting)
        {



        }

        public void setChar(int n, char c)
        {
            hspBuffer[n] = (byte)c;
        }



        public void setDATA(int n, byte[] data)
        {

            if (n > 100) return;

            if (n + data.Length > 10) return;

            int bufIndex = n;

            for (int i = 0; i < data.Length; i++)
            {

                hspBuffer[bufIndex] = data[i];

            }



        }



        public int getMESSAGE()
        {

            return (int)hspBuffer[4];

        }









    }



   public class HSPSocket
    {

        Socket sock;

        string ip;

        int port;



        public HSPSocket(string ip, int port)
        {

            sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            this.ip = ip;

            this.port = port;

        }



        public void connect()
        {
            try
            {
                sock.Connect(ip, 666);
            }
            catch (SocketException e)
            {
                Console.WriteLine("에러 코드 : " + e.NativeErrorCode);
            }




        }



        public void close()
        {

            sock.Close();

        }





        public int sendHSP(HSP hsp)
        {
            byte[] buf = hsp.getByteArray();
            

            int res = sock.Send(buf);

            return res;
            
        }

        public int send(byte[] BUF)
        {

            byte[] buf = BUF;


            int res = sock.Send(buf);

            return res;

        }



        public int receiveHSP(HSP hsp)
        {

            byte[] buf = new byte[100];

            int res = sock.Receive(buf);

            if (res >= 0)

                hsp.hspBuffer = buf;

            return res;

        }

        public int receive(byte[] buf)
        {
            return sock.Receive(buf);
           

        }

    }
 }
