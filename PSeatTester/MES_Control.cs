#define PROGRAM_RUNNING

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SDKNetLib;
using SDKNetLib.Impl;
using PSeatTester;
using FarPoint.Excel;

namespace MES
{
    public struct __TCPIP__
    {
        public string IP;
        public int Port;
    }

    public class ComHeater
    {
        public const byte RS = 0x1e;
        public const byte GS = 0x1d;
        public const byte EOT = 0x04;
        public const byte STX = 0x02;
        public const byte ETX = 0x03;
    }

    public class MES_Control
    {
        private MyInterface mControl = null;
        private __TCPIP__ ServerIpAddr;
        private __TCPIP__ ClientIpAddr;
        private bool ClientConnection = false;
        private bool ServerConnection = false;
        private IAsyncSocketClient client;
        private IAsyncSocketServer server;

        private Timer timer1 = new Timer();

        public struct __ReadMesData__
        {
            /// <summary>
            /// 컴퓨터 날짜 를 같는다.
            /// </summary>
            public string Date;
            /// <summary>
            /// 라인 코드를 같는다.
            /// </summary>
            public string LineCode;
            /// <summary>
            /// 기기번호를 같는다.
            /// </summary>
            public string MachineNo;
            public string Barcode;
            /// <summary>
            /// 검사 여부를 같는다.
            /// </summary>
            public string Check;
        }

        private __ReadMesData__ ReadData = new __ReadMesData__()
        {
            Barcode = null,
            Check = null,
            Date = null,
            LineCode = null,
            MachineNo = null
        };



        public MES_Control()
        {

        }
        public MES_Control(MyInterface mControl, __TCPIP__ ServerIp, __TCPIP__ ClientIp)
        {
            this.mControl = mControl;
            this.ServerIpAddr = ServerIp;
            this.ClientIpAddr = ClientIp;

            timer1.Interval = 1000;
            timer1.Tick += Timer1_Tick;
            timer1.Enabled = true;
        }

        private short CheckToPingCount = 0;
        private bool CheckToPing = true;

        private long PingFirst = 0;
        private long PingLast = 0;

        private void Timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                timer1.Enabled = false;

                if (mControl.isRunning == false)
                {
                    if (ClientConnection == false)
                    {
                        if (CheckToPing == true)
                        {
                            PingLast = mControl.공용함수.timeGetTimems();

                            if (3000 <= (PingLast - PingFirst))
                            {
                                Ping ping = new Ping();
                                PingReply r = ping.Send(ClientIpAddr.IP);
                                if (r.Status == IPStatus.Success)
                                {
                                    //CheckToPingCount++;

                                    //if (CheckToPingCount <= 10)
                                    //    TCPOpen();
                                    //else CheckToPing = false;

                                    //if (CheckToPing == false)
                                    //{
                                    //    if (MessageBox.Show("서버PC 와 통신 연결이 되지 않습니다.\n연결 작업을 다시 진행하시겠습니까?", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                    //    {
                                    //        CheckToPingCount = 0;
                                    //        CheckToPing = true;
                                    //    }
                                    //}

                                    TCPOpen();
                                    CheckToPingCount = 0;
                                }
                                else
                                {
                                    if (100 < CheckToPingCount)
                                    {
                                        CheckToPing = false;
                                        //5분간 Ping 이 붇지 않는다면
                                    }

                                    //if (CheckToPing == false)
                                    //{
                                    //    if (MessageBox.Show("서버PC 와 통신 연결이 되지 않습니다.\n연결 작업을 다시 진행하시겠습니까?", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                    //    {
                                    //        CheckToPingCount = 0;
                                    //        CheckToPing = true;
                                    //    }
                                    //}
                                }
                                PingFirst = mControl.공용함수.timeGetTimems();
                                PingLast = mControl.공용함수.timeGetTimems();
                            }
                        }
                        else
                        {
                            long OneMinth = (1000 * 60); //1분

                            PingLast = mControl.공용함수.timeGetTimems();

                            //15분 후 다시 ping 을 시도한다.
                            if ((OneMinth * 15) <= (PingLast - PingFirst))
                            {
                                CheckToPingCount = 0;
                                CheckToPing = true;
                                PingFirst = mControl.공용함수.timeGetTimems();
                                PingLast = mControl.공용함수.timeGetTimems();
                            }
                        }
                    }
                    else
                    {
                        if (0 < CheckToPingCount) CheckToPingCount = 0;
                    }
                }
            }
            catch
            {

            }
            finally
            {
                timer1.Enabled = true;
            }
            return;
        }
        public void Open()
        {
            TCPOpen();
            return;
        }

        public void Close()
        {
            if (client.IsConnected == true) client.Close();
            server.Close();
            return;
        }

        public bool isClientConnection
        {
            get { return ClientConnection; }
        }
        public bool isServerConnection
        {
            get { return ServerConnection; }
        }

        public string Write
        {
            set { TCPWriteClient(STX + value + ETX); }
        }

        public string ServerSend
        {
            set { TCPWriteServer(STX + value + ETX); }
        }


        private long TCPComCheckFirst;
        private long TCPComCheckLast;
        /// <summary>
        /// 소켓 통신 오픈 작업을 진행한다.
        /// </summary>
        /// <returns></returns>
        private bool TCPOpen()
        {
            //UDP Socket Client
            //시스템 포트 : 0~1023
            //사용자 포트 : 1024~49151
            //동적 포트   : 49152~65535

            //int Port;

            client = new AdvancedAsyncSocketClient(Encoding.UTF8.GetBytes(ETX));

            client.Connect(ClientIpAddr.IP, ClientIpAddr.Port);

            client.OnConnect += new SDKNetLib.Event.AsyncSocketConnectEventHandler(client_OnConnect);
            client.OnReceive += new SDKNetLib.Event.AsyncSocketReceiveEventHandler(client_OnReceive);
            client.OnSend += new SDKNetLib.Event.AsyncSocketSendEventHandler(client_OnSend);
            client.OnClose += new SDKNetLib.Event.AsyncSocketCloseEventHandler(client_OnClose);
            client.OnError += new SDKNetLib.Event.AsyncSocketErrorEventHandler(client_OnError);

            server = new AdvancedAsyncSocketServer(ServerIpAddr.Port, Encoding.UTF8.GetBytes(ETX));
            server.OnAccept += new SDKNetLib.Event.AsyncSocketAcceptEventHandler(server_OnAccept);
            server.OnError += new SDKNetLib.Event.AsyncSocketErrorEventHandler(server_OnError);
            server.OnClose += new SDKNetLib.Event.AsyncSocketCloseEventHandler(server_OnClose);

            server.OnClientReceive += new SDKNetLib.Event.AsyncSocketReceiveEventHandler(server_OnClientReceive);
            server.OnClientSend += new SDKNetLib.Event.AsyncSocketSendEventHandler(server_OnClientSend);
            server.OnClientClose += new SDKNetLib.Event.AsyncSocketCloseEventHandler(server_OnClientClose);
            server.OnClientError += new SDKNetLib.Event.AsyncSocketErrorEventHandler(server_OnClientError);

            server.Listen();

            TCPComCheckFirst = mControl.공용함수.timeGetTimems();
            TCPComCheckLast = mControl.공용함수.timeGetTimems();
            return true;
        }

        private bool TCPOpenToClient()
        {
            //UDP Socket Client
            //시스템 포트 : 0~1023
            //사용자 포트 : 1024~49151
            //동적 포트   : 49152~65535

            //int Port;

            //client = new AdvancedAsyncSocketClient(Encoding.UTF8.GetBytes(ETX));

            client.Connect(ClientIpAddr.IP, ClientIpAddr.Port);

            //client.OnConnect += new SDKNetLib.Event.AsyncSocketConnectEventHandler(`);
            //client.OnReceive += new SDKNetLib.Event.AsyncSocketReceiveEventHandler(client_OnReceive);
            //client.OnSend += new SDKNetLib.Event.AsyncSocketSendEventHandler(client_OnSend);
            //client.OnClose += new SDKNetLib.Event.AsyncSocketCloseEventHandler(client_OnClose);
            //client.OnError += new SDKNetLib.Event.AsyncSocketErrorEventHandler(client_OnError);

            TCPComCheckFirst = mControl.공용함수.timeGetTimems();
            TCPComCheckLast = mControl.공용함수.timeGetTimems();

            return true;
        }
        /// <summary>
        /// 클라이언트 소켓에서 에러가 발생하면 호출 되는 함수 - 통신이 끊혔을때 호출됨
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void client_OnError(object sender, SDKNetLib.Event.AsyncSocketErrorEventArgs e)
        {
            try
            {
                //LogMessage("client_OnError[" + e.AsyncSocketException.Message + "]");
                ClientConnection = false;
            }
            catch
            {
                MessageBox.Show("클라이언트 통신에러 'client_OnError'");
            }
            return;
        }

        /// <summary>
        /// 클라이언트 통신을 닫으면 호출되는 홤수
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void client_OnClose(object sender, SDKNetLib.Event.AsyncSocketConnectionEventArgs e)
        {
            try
            {
                //LogMessage("client_OnClose");
                ClientConnection = false;
            }
            catch
            {
                MessageBox.Show("클라이언트 통신에러 'client_OnClose'");
            }
            return;
        }

        /// <summary>
        /// 클라이언트 소켓으로 데이터를 송신하면 호출되는 함수
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void client_OnSend(object sender, SDKNetLib.Event.AsyncSocketSendEventArgs e)
        {
            //LogMessage("client_OnSend");
            return;
        }

        /// <summary>
        /// 클라이언트 소켓으로 데이타가 수신되면 호출되는 함수
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void client_OnReceive(object sender, SDKNetLib.Event.AsyncSocketReceiveEventArgs e)
        {
            try
            {
                //LogMessage("client_OnReceive[" + Encoding.UTF8.GetString(e.ReceiveByte, 0, e.ReceiveByteSize) + "]");

                if (e.ReceiveByteSize == 0) return;
                //CheckTcpData(Encoding.UTF8.GetString(e.ReceiveByte, 0, e.ReceiveByteSize));

                string s = Encoding.UTF8.GetString(e.ReceiveByte, 0, e.ReceiveByteSize);
                //if (0 <= s.IndexOf("NG"))
                //{
                //    TCPWriteServer(SendPopDataOld);
                //}
                //else
                //{
                //    PopReadData = s;
                //    //PopReadData = Encoding.UTF8.GetString(e.ReceiveByte, 0, e.ReceiveByteSize);
                //    CheckPopData();
                //}
            }
            catch
            {
                MessageBox.Show("클라이언트 통신에러 'client_OnReceive'");
            }
            return;
        }

        public string SendPopDataOld = "";

        /// <summary>
        /// 클라이언트 소켓이 서버에 접속되면 호출되는 함수
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void client_OnConnect(object sender, SDKNetLib.Event.AsyncSocketConnectionEventArgs e)
        {
            try
            {
                //            LogMessage("client_OnConnect");
                ClientConnection = true;
            }
            catch
            {
                MessageBox.Show("클라이언트 통신에러 'client_OnConnect'");
            }
            return;
        }

        /// <summary>
        /// 서버 소켓을 닫으면 호출되는 함수
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void server_OnClose(object sender, SDKNetLib.Event.AsyncSocketConnectionEventArgs e)
        {
            try
            {
                //LogMessage("server_OnClose[client count:" + server.ClientList.Count + "]");
                ServerConnection = false;
            }
            catch
            {
                MessageBox.Show("서버 통신에러 'server_OnClose'");
            }
            return;
        }

        /// <summary>
        /// 서버 소켓에 에러가 발생하면 호출되는 함수
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void server_OnError(object sender, SDKNetLib.Event.AsyncSocketErrorEventArgs e)
        {
            try
            {
                //LogMessage("server_OnError");
                ServerConnection = false;
            }
            catch
            {
                MessageBox.Show("서버 통신에러 'server_OnError'");
            }
            return;
        }

        /// <summary>
        /// 서버 소켓에 다름 클라이언트가 접속되면 호출되는 함수
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void server_OnAccept(object sender, SDKNetLib.Event.AsyncSocketAcceptEventArgs e)
        {
            //LogMessage("server_OnAccept[client count:" + server.ClientList.Count + "]");
            //IAsyncSocketClient cList = e.ClientSocket;
            //string[] s = cList.ClientId.Split(':');

            //if (Config.Back.Ip == s[0])
            //{
            //    if (cList.IsConnected == true) led2.Value.AsBoolean = true;
            //}
            //else if (Config.Cushtion.Ip == s[0])
            //{
            //    if (cList.IsConnected == true) led1.Value.AsBoolean = true;
            //}
            try
            {
                ServerConnection = true;

                //IAsyncSocketClient cList = e.ClientSocket;
                //string[] s = cList.ClientId.Split(':');

                //if (Config.Back.Ip == s[0])
                //{
                //    if (cList.IsConnected == true) led2.Value.AsBoolean = true;
                //}
                //else if (Config.Cushtion.Ip == s[0])
                //{
                //    if (cList.IsConnected == true) led1.Value.AsBoolean = true;
                //}
            }
            catch
            {
                MessageBox.Show("서버 통신에러 'server_OnAccept'");
            }
            return;
        }

        /// <summary>
        /// 서버에 접속되어 있던 클라이언트 에 문제가 발생하면 호출되는 함수
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void server_OnClientError(object sender, SDKNetLib.Event.AsyncSocketErrorEventArgs e)
        {
            try
            {
                //LogMessage("server_OnClientError[" + e.AsyncSocketException.Message + "]");
                ServerConnection = false;
            }
            catch
            {
                MessageBox.Show("서버 통신에러 'server_OnClientError'");
            }
            return;
        }

        /// <summary>
        /// 서버에 접속되어 있는 클라이언트 소켓이 닫히면 호출되는 함수
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void server_OnClientClose(object sender, SDKNetLib.Event.AsyncSocketConnectionEventArgs e)
        {
            try
            {
                //List<IAsyncSocketClient> cList = new List<IAsyncSocketClient>();

                //cList = server.ClientList;

                //led1.Value.AsBoolean = false;
                //led2.Value.AsBoolean = false;
                //for (int i = 0; i < cList.Count; i++)
                //{
                //    if (Config.Back.Ip == cList[i].ClientId)
                //    {
                //        if (cList[i].IsConnected == true) led2.Value.AsBoolean = true;
                //    }
                //    else if (Config.Cushtion.Ip == cList[i].ClientId)
                //    {
                //        if (cList[i].IsConnected == true) led1.Value.AsBoolean = true;
                //    }
                //}
                ServerConnection = false;

#if PROGRAM_RUNNING
                IAsyncSocketClient cList = e.ClientSocket;
                server.ClientList.Remove(cList);
#endif
            }
            catch
            {
                MessageBox.Show("서버 통신에러 'server_OnClientClose'");
            }
            return;
        }

        /// <summary>
        /// 서버에 접속되어 있는 클라이언트 가 데이터를 송신하면 호출되는 함수
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void server_OnClientSend(object sender, SDKNetLib.Event.AsyncSocketSendEventArgs e)
        {
            //MessageBox.Show("server_OnClientSend");
            return;
        }

        private string PopReadData = null;

        /// <summary>
        /// 서버에 데이타가 수신되면 호출되는 함수
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void server_OnClientReceive(object sender, SDKNetLib.Event.AsyncSocketReceiveEventArgs e)
        {
            try
            {
                //LogMessage("server_OnClientReceive[" + Encoding.UTF8.GetString(e.ReceiveByte, 0, e.ReceiveByteSize) + "]");

                //server.SendToSpecificClient(e.ReceiveByte, ((IAsyncSocketClient)sender).ClientId);

                //server.send

                string ReadData;

                ReadData = Encoding.UTF8.GetString(e.ReceiveByte, 0, e.ReceiveByteSize);
                string[] ReadId = ((IAsyncSocketClient)sender).ClientId.Split(':');

                if (0 <= ReadData.IndexOf("NG"))
                {
                    TCPWriteServer(SendPopDataOld);
                }
                else
                {
                    Write = "OK";
                    PopReadData = ReadData;
                    //PopReadData = Encoding.UTF8.GetString(e.ReceiveByte, 0, e.ReceiveByteSize);
                    CheckPopData();
                }


                if (e.ReceiveByteSize == 0) return;

                //this.Invoke(new EventHandler());
                //PopReadData = Encoding.UTF8.GetString(e.ReceiveByte, 0, e.ReceiveByteSize);
                //CheckPopData();
            }
            catch
            {
                MessageBox.Show("서버 통신에러 'server_OnClientReceive'");
            }
            //ReceiveData.Add(ReadId[0] + "-" + ReadData);
            //this.Text = ReadData;
            return;
        }
        private string STX
        {
            get { return GetHex(ComHeater.STX); }
        }

        private string ETX
        {
            get { return GetHex(ComHeater.ETX); }
        }

        private string GetHex(byte hex)
        {
            byte[] shex = { hex, 0x00 };
            string data = Encoding.Default.GetString(shex, 0, 1);
            return data;
        }



        private void TCPWriteClient(string input)
        {
            byte[] sBuffer = Encoding.UTF8.GetBytes(input);

            //보내기            
            //IPAddress Tcpip = IPAddress.Parse(Config.Client.IP);
            //IPEndPoint Serverep = new IPEndPoint(Tcpip, Config.Client.Port);
            if (ClientConnection == true)
                client.Send(sBuffer);
            else MessageBox.Show("통신 포트가 연결되지 않아 데이타 전송을 할 수 없습니다\n 이더넷을 확인해 주십시오.");
            return;
        }

        private void TCPWriteServer(string input)
        {
            byte[] sBuffer = Encoding.UTF8.GetBytes(input);

            //보내기            
            //IPAddress Tcpip = IPAddress.Parse(Config.Client.IP);
            //IPEndPoint Serverep = new IPEndPoint(Tcpip, Config.Client.Port);
            if (ServerConnection == true)
            {
                if (0 < server.ClientList.Count) server.SendToOtherClient(sBuffer, server.ClientList[0]);
            }

            else MessageBox.Show("통신 포트가 연결되지 않아 데이타 전송을 할 수 없습니다\n 이더넷을 확인해 주십시오.");
            return;
        }



        private bool DataReading = false;
        public __ReadMesData__ GetReadData
        {
            get { return ReadData; }
        }

        public bool isReading
        {
            get { return DataReading; }
            set { DataReading = value; }
        }

        public string CrateData
            (
            //string Date,
            //string LineNo,
            //string MachineNo,
            int Serial,
            int TestCount,
            float Min_1 = -9999,
            float Max_1 = -9999,
            float Min_2 = -9999,
            float Max_2 = -9999,
            float Min_3 = -9999,
            float Max_3 = -9999,
            float Min_4 = -9999,
            float Max_4 = -9999,
            float Min_5 = -9999,
            float Max_5 = -9999,
            float Min_6 = -9999,
            float Max_6 = -9999,
            float Min_7 = -9999,
            float Max_7 = -9999,
            float Min_8 = -9999,
            float Max_8 = -9999,
            float Min_9 = -9999,
            float Max_9 = -9999,
            float Min_10 = -9999,
            float Max_10 = -9999,
            float Min_11 = -9999,
            float Max_11 = -9999,
            float Min_12 = -9999,
            float Max_12 = -9999,
            float Min_13 = -9999,
            float Max_13 = -9999,
            float Min_14 = -9999,
            float Max_14 = -9999,
            float Min_15 = -9999,
            float Max_15 = -9999,
            float Value1 = -9999,
            float Value2 = -9999,
            float Value3 = -9999,
            float Value4 = -9999,
            float Value5 = -9999,
            float Value6 = -9999,
            float Value7 = -9999,
            float Value8 = -9999,
            float Value9 = -9999,
            float Value10 = -9999,
            float Value11 = -9999,
            float Value12 = -9999,
            float Value13 = -9999,
            float Value14 = -9999,
            float Value15 = -9999,
            short Result = (short)RESULT.CLEAR
            )
        {
            string SendData = "";

            SendData += STX;
            if (PopReadData != null)
                SendData += PopReadData;
            else SendData += ";;;;;";
            SendData += ReadData.Date + ";";
            SendData += ReadData.LineCode + ";";
            SendData += ReadData.MachineNo + ";";
            SendData += Serial.ToString() + ";";
            SendData += TestCount.ToString() + ";;";

            //---------------------------------------------
            if (Min_1 != -9999)
                SendData += Min_1.ToString("0.00") + ";";
            else SendData += ";";

            if (Max_1 != -9999)
                SendData += Max_1.ToString("0.00") + ";";
            else SendData += ";";
            //---------------------------------------------
            if (Min_2 != -9999)
                SendData += Min_2.ToString("0.00") + ";";
            else SendData += ";";

            if (Max_2 != -9999)
                SendData += Max_2.ToString("0.00") + ";";
            else SendData += ";";

            //---------------------------------------------
            if (Min_3 != -9999)
                SendData += Min_3.ToString("0.00") + ";";
            else SendData += ";";

            if (Max_3 != -9999)
                SendData += Max_3.ToString("0.00") + ";";
            else SendData += ";";

            //---------------------------------------------
            if (Min_4 != -9999)
                SendData += Min_4.ToString("0.00") + ";";
            else SendData += ";";

            if (Max_4 != -9999)
                SendData += Max_4.ToString("0.00") + ";";
            else SendData += ";";

            //---------------------------------------------
            if (Min_5 != -9999)
                SendData += Min_5.ToString("0.00") + ";";
            else SendData += ";";

            if (Max_5 != -9999)
                SendData += Max_5.ToString("0.00") + ";";
            else SendData += ";";

            //---------------------------------------------
            if (Min_6 != -9999)
                SendData += Min_6.ToString("0.00") + ";";
            else SendData += ";";

            if (Max_6 != -9999)
                SendData += Max_6.ToString("0.00") + ";";
            else SendData += ";";

            //---------------------------------------------
            if (Min_7 != -9999)
                SendData += Min_7.ToString("0.00") + ";";
            else SendData += ";";

            if (Max_7 != -9999)
                SendData += Max_7.ToString("0.00") + ";";
            else SendData += ";";

            //---------------------------------------------
            if (Min_8 != -9999)
                SendData += Min_8.ToString("0.00") + ";";
            else SendData += ";";

            if (Max_8 != -9999)
                SendData += Max_8.ToString("0.00") + ";";
            else SendData += ";";

            //---------------------------------------------
            if (Min_9 != -9999)
                SendData += Min_9.ToString("0.00") + ";";
            else SendData += ";";

            if (Max_9 != -9999)
                SendData += Max_9.ToString("0.00") + ";";
            else SendData += ";";

            //---------------------------------------------
            if (Min_10 != -9999)
                SendData += Min_10.ToString("0.00") + ";";
            else SendData += ";";

            if (Max_10 != -9999)
                SendData += Max_10.ToString("0.00") + ";";
            else SendData += ";";
            //---------------------------------------------
            if (Min_11 != -9999)
                SendData += Min_11.ToString("0.00") + ";";
            else SendData += ";";

            if (Max_11 != -9999)
                SendData += Max_11.ToString("0.00") + ";";
            else SendData += ";";
            //---------------------------------------------
            if (Min_12 != -9999)
                SendData += Min_12.ToString("0.00") + ";";
            else SendData += ";";

            if (Max_12 != -9999)
                SendData += Max_12.ToString("0.00") + ";";
            else SendData += ";";

            //---------------------------------------------
            if (Min_13 != -9999)
                SendData += Min_13.ToString("0.00") + ";";
            else SendData += ";";

            if (Max_13 != -9999)
                SendData += Max_13.ToString("0.00") + ";";
            else SendData += ";";

            //---------------------------------------------
            if (Min_14 != -9999)
                SendData += Min_14.ToString("0.00") + ";";
            else SendData += ";";

            if (Max_14 != -9999)
                SendData += Max_14.ToString("0.00") + ";";
            else SendData += ";";

            //---------------------------------------------
            if (Min_15 != -9999)
                SendData += Min_15.ToString("0.00") + ";";
            else SendData += ";";

            if (Max_15 != -9999)
                SendData += Max_15.ToString("0.00") + ";";
            else SendData += ";";

            //---------------------------------------------
            if (Value1 != -9999)
                SendData += Value1.ToString("0.00") + ";";
            else SendData += ";";
            //---------------------------------------------
            if (Value2 != -9999)
                SendData += Value2.ToString("0.00") + ";";
            else SendData += ";";
            //---------------------------------------------
            if (Value3 != -9999)
                SendData += Value3.ToString("0.00") + ";";
            else SendData += ";";
            //---------------------------------------------
            if (Value4 != -9999)
                SendData += Value4.ToString("0.00") + ";";
            else SendData += ";";
            //---------------------------------------------
            if (Value5 != -9999)
                SendData += Value5.ToString("0.00") + ";";
            else SendData += ";";
            //---------------------------------------------
            if (Value6 != -9999)
                SendData += Value6.ToString("0.00") + ";";
            else SendData += ";";
            //---------------------------------------------
            if (Value7 != -9999)
                SendData += Value7.ToString("0.00") + ";";
            else SendData += ";";
            //---------------------------------------------
            if (Value8 != -9999)
                SendData += Value8.ToString("0.00") + ";";
            else SendData += ";";
            //---------------------------------------------
            if (Value9 != -9999)
                SendData += Value9.ToString("0.00") + ";";
            else SendData += ";";
            //---------------------------------------------
            if (Value10 != -9999)
                SendData += Value10.ToString("0.00") + ";";
            else SendData += ";";
            //---------------------------------------------
            if (Value11 != -9999)
                SendData += Value11.ToString("0.00") + ";";
            else SendData += ";";
            //---------------------------------------------
            if (Value12 != -9999)
                SendData += Value12.ToString("0.00") + ";";
            else SendData += ";";
            //---------------------------------------------
            if (Value13 != -9999)
                SendData += Value13.ToString("0.00") + ";";
            else SendData += ";";
            //---------------------------------------------
            if (Value14 != -9999)
                SendData += Value14.ToString("0.00") + ";";
            else SendData += ";";
            //---------------------------------------------
            if (Value15 != -9999)
                SendData += Value15.ToString("0.00") + ";";
            else SendData += ";";

            //---------------------------------------------
            SendData += Result == (short)RESULT.PASS ? "1" : "9";
            SendData += ";";
            SendData += ETX;
            return SendData;
        }

        public void CheckPopData()
        {
            if (0 <= PopReadData.IndexOf(STX))
            {
                PopReadData = PopReadData.Substring(PopReadData.IndexOf(STX) + 1);
            }
            if (0 <= PopReadData.IndexOf(ETX))
            {
                PopReadData = PopReadData.Substring(0, PopReadData.IndexOf(ETX) - 1);
            }

            if (PopReadData.IndexOf(";") < 0) return;

            string[] Split = PopReadData.Split(';');
            ReadData.Date = Split[0];
            ReadData.LineCode = Split[1];
            ReadData.MachineNo = Split[2];
            ReadData.Barcode = Split[3];
            ReadData.Check = Split[4];
            DataReading = true;
            return;
        }

        public string SourceData
        {
            get
            {
                return PopReadData;
            }
        }

        ~MES_Control()
        {

        }
    }
}
