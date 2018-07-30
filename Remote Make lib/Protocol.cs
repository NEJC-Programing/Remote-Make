using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Net;
using System.Runtime.InteropServices;

namespace Remote_Make
{
    class Protocol
    {
        private static int ProtocolPort = 6060;
        private static string Server;
        private static bool Setup = false;
        public void Init(bool server)
        {
            if (server)
                NetCat.Listen(IPAddress.Parse("127.0.0.1"), ProtocolPort);
            else
                NetCat.Connect(Server, ProtocolPort);
            Setup = true;
        }
        public void Init(int port)
        {
            ProtocolPort = port;
            Init(true);
        }
        public void Init(int port,string host)
        {
            ProtocolPort = port;
            Server = host;
            Init(false);
        }
        public void Init(string host)
        {
            Server = host;
            Init(false);
        }
        public void Send(Packed packed)
        {
            if (!Setup)
                return;

            int size = Marshal.SizeOf(packed);
            byte[] arr = new byte[size];

            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(packed, ptr, true);
            Marshal.Copy(ptr, arr, 0, size);
            Marshal.FreeHGlobal(ptr);


            int ss = arr.Length;
            NetCat.SendBytes(BitConverter.GetBytes(ss));
            NetCat.SendBytes(arr);
        }
        public Packed Recieve()
        {
            if (!Setup)
                return new Packed();
            Packed buff = new Packed();
            int psize = BitConverter.ToInt32(NetCat.ReceiveBytes(sizeof(int)),0);
            byte[] arr = NetCat.ReceiveBytes(psize);
            int size = Marshal.SizeOf(buff);

            IntPtr ptr = Marshal.AllocHGlobal(size);

            Marshal.Copy(arr, 0, ptr, size);

            buff = (Packed)Marshal.PtrToStructure(ptr, buff.GetType());
            Marshal.FreeHGlobal(ptr);
            return buff;
        }
        private static class NetCat
        {
            private static NetworkStream stream;
            private static StreamWriter streamw;
            private static StreamReader streamr;
            private static TcpClient myclient = new TcpClient();
            private static TcpListener myserver;
            private static Thread server;

            public static void Connect(string host, int port)
            {
                myclient.Connect(host, port);
                stream = myclient.GetStream();
                streamw = new StreamWriter(stream);
                streamr = new StreamReader(stream);
            }
            public static void Listen(IPAddress host, int port)
            {
                myserver = new TcpListener(host, port);
                myserver.Start();
                server = new Thread(lserver)
                {
                    Priority = ThreadPriority.BelowNormal
                };
                server.Start();
                Thread.Sleep(100);
            }
            private static void lserver()
            {
                while (true)
                {
                    myclient = myserver.AcceptTcpClient();
                    stream = myclient.GetStream();
                    streamw = new StreamWriter(stream);
                    streamr = new StreamReader(stream);
                }
            }
            public static void Send(string text)
            {
                streamw.WriteLine(text);
                streamw.Flush();
            }
            private static void Sendbyte(byte b)
            {
                streamw.Write(b);
                streamw.Flush();
            }
            public static void SendBytes(byte[] data)
            {
                foreach (var item in data)
                {
                    Sendbyte(item);
                }
            }
            public static string ReceiveLine()
            {
                return streamr.ReadLine();
            }
            public static byte ReceiveByte()
            {
                return (byte)streamr.Read();
            }
            public static byte[] ReceiveBytes(int bytes)
            {
                List<byte> buff = new List<byte>();
                for (int i = 0; i <= bytes; i++)
                {
                    buff.Add(ReceiveByte());
                }
                return buff.ToArray();
            }
        }
    }
}
