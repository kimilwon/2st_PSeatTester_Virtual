#define PROGRAM_RUNNING
#define THREAD_RUN

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Threading;
using System.ComponentModel;

namespace PSeatTester
{
    public class IOControl
    {
        private MyInterface mControl = null;
        private IPEndPoint ep;
        private Socket server;
        private EndPoint remoteEP;
        private EndPoint remoteEP2;
        private byte[] rBuffer1 = new byte[1024];
        private __TcpIP__ Board;
        private __TcpIP__ PC;
        private ulong[] InData = { 0x0000000000000000, 0x0000000000000000, 0x0000000000000000 };
        private byte[,] OutData = { { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } };
        private float CurrData = 0;
#if !THREAD_RUN
        private Timer timer1 = new Timer();
#endif
        public IOControl()
        {
        }
        public IOControl(MyInterface mControl, __TcpIP__ Board, __TcpIP__ PC)
        {
            this.PC = PC;
            this.Board = Board;
#if !THREAD_RUN
            timer1.Interval = 10;
            timer1.Tick += timer1_tick;
#endif
            this.mControl = mControl;
            //timer1.Enabled = true;
        }

        public void Open()
        {
            if (UdpOpen() == false)
            {
                MessageBox.Show("마이컴 제어용 통신 포트를 오픈하지 못했습니다.");
            }
            else
            {
                UdpCanCommunicationInit();
                UdpRead();
#if !THREAD_RUN
                timer1.Enabled = true;
#else
                ThreadSetting();
#endif
            }
        }

        private bool UdpOpen()
        {
            bool Flag;
            string strIP;
            int port1;

            Flag = false;
            if ((PC.IP != "") && (PC.IP != null))
            {
                //종점 생성
                strIP = PC.IP;
                IPAddress ip = IPAddress.Parse(strIP);
                port1 = PC.Port;
                ep = new IPEndPoint(ip, port1);

                server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                try
                {
                    server.Bind(ep);
                    isOpen = false;
                    isConnection = false;

                    remoteEP = (EndPoint)new IPEndPoint(ip, port1);

                    if ((Board.IP != "") && (PC.IP != null))
                    {
                        remoteEP2 = (EndPoint)new IPEndPoint(IPAddress.Parse(Board.IP), Board.Port);
                        isOpen = true;
                        isConnection = true;
                        Flag = isOpen;
                    }
                }
                catch// (Exception exp)                
                {
                    //MessageBox.Show("I/O Card 와 Bind 가 되지 않습니다. (이더넷 케이블 확인)");
                    isOpen = false;
                    isConnection = false;
                }
                finally
                {
                    Flag = isOpen;
                }
                server.ReceiveBufferSize = 4096;
                server.SendBufferSize = 4096;
            }

            //int port2 = Config.Board.Port;

            //UDP Socket 생성


            return Flag;
        }

        public ulong[] GetInData
        {
            get { return InData; }
        }

        public bool isOpen { get; set; }
        //{
        //    get { return isOpen; }
        //}

        public bool isConnection { get; set; }
        //{
        //    get { return isConnection; }
        //}
        public void UdpClose()
        {
            //소켓 닫기
#if PROGRAM_RUNNING
            if (isOpen == true) server.Close();
            isOpen = false;
#endif

            return;
        }


        private void UdpWrite(int addr, byte[] Data, int Length)
        {
            //데이터 입력
#if PROGRAM_RUNNING
            int SendLength;
            //인코딩(byte[])
            byte[] sBuffer = new byte[100];
            //byte[] sBuffer = Encoding.UTF8.GetBytes(data); // data 가 string 이어야 한다.

            //보내기            

            //sBuffer = Encoding.UTF8.GetBytes(data);// data 가 string 이어야 한다.

            /*
            union_r r = new union_r();

            r.Addr = addr;

            sBuffer[0] = r.c1;
            sBuffer[1] = r.c2;
            sBuffer[2] = r.c3;
            sBuffer[3] = r.c4;
            */
            SendLength = 0;
            sBuffer[SendLength++] = (byte)((addr & 0xff000000) >> 24);
            sBuffer[SendLength++] = (byte)((addr & 0x00ff0000) >> 16);
            sBuffer[SendLength++] = (byte)((addr & 0x0000ff00) >> 8);
            sBuffer[SendLength++] = (byte)((addr & 0x000000ff) >> 0);
            sBuffer[SendLength++] = (byte)Length;

            for (int i = 0; i < Length; i++) sBuffer[SendLength++] = Data[i];

            //if ((isOpen[Ch] == true) && (isConnection[Ch] == true))
            if (isOpen == true)
            {
                try
                {
                    server.SendTo(sBuffer, SendLength, SocketFlags.DontRoute, remoteEP2);
                }
                catch
                {
                }
                finally
                {
                }
            }
#endif
            return;
        }
        private void UpdWrite2()
        {
            byte[] Data1 = { OutData[1, 0], OutData[1, 1], OutData[1, 2], OutData[1, 3], OutData[1, 4], OutData[1, 5], OutData[1, 6], OutData[1, 7] };
            byte[] Data2 = { OutData[0, 0], OutData[0, 1], OutData[0, 2], OutData[0, 3], OutData[0, 4], OutData[0, 5], OutData[0, 6], OutData[0, 7] };
            UdpWrite(0x151, Data1, 8);
            UdpWrite(0x101, Data2, 8);
            OutPos = 2;
            OutPosFirst = mControl.공용함수.timeGetTimems();
            OutPosLast = mControl.공용함수.timeGetTimems();
            return;
        }

        private void UpdWrite(short Pos)
        {
            byte[] Data = { OutData[Pos, 0], OutData[Pos, 1], OutData[Pos, 2], OutData[Pos, 3], OutData[Pos, 4], OutData[Pos, 5], OutData[Pos, 6], OutData[Pos, 7] };
            int Addr = 0;

            Addr = 0x151 + (Pos * 0x10);
            UdpWrite(Addr, Data, 8);
            OutPos = 5; 
            OutPosFirst = mControl.공용함수.timeGetTimems();
            OutPosLast = mControl.공용함수.timeGetTimems();
            return;
        }

        public void UdpCanCommunicationInit()
        {
#if PROGRAM_RUNNING
            byte[] sBuffer = { 0xfc, 0x03, 0xff };
            if (remoteEP2 != null) server.SendTo(sBuffer, 3, SocketFlags.DontRoute, remoteEP2);
            mControl.공용함수.timedelay(10);
            if (remoteEP2 != null) server.SendTo(sBuffer, 3, SocketFlags.DontRoute, remoteEP2);
            mControl.공용함수.timedelay(10);
            if (remoteEP2 != null) server.SendTo(sBuffer, 3, SocketFlags.DontRoute, remoteEP2);
#endif
            return;
        }

        private string UdpRead()
        {
            //데이터 받기
            string result = "";

            try
            {
#if PROGRAM_RUNNING
                server.BeginReceiveFrom(rBuffer1, 0, rBuffer1.Length, SocketFlags.None, ref remoteEP, new AsyncCallback(ReceiveUdp), remoteEP);
#endif
            }
            catch
            {
            }
            finally
            {
            }
            return result;
        }

        private void ReceiveUdp(IAsyncResult _AR)
        {
            try
            {
                EndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
                // 클라이언트로부터 메시지를 받는다.            
                isConnection = true;
                int ReceivedSize = server.EndReceiveFrom(_AR, ref remoteEP);

                if (0 < ReceivedSize)
                {
                    if (mControl.isExit == false)
                    {
                        //CheckUdpData(rBuffer1, 0, ReceivedSize);                                                
                        CheckUdpData(rBuffer1, ReceivedSize);
                    }
                }
                UdpComCheckFirst = mControl.공용함수.timeGetTimems();
                UdpComCheckLast = mControl.공용함수.timeGetTimems();
                //ReceivedSize[0] = 0;
                server.BeginReceiveFrom(rBuffer1, 0, rBuffer1.Length, SocketFlags.None, ref remoteEP, new AsyncCallback(ReceiveUdp), remoteEP);

            }
            catch// (Exception exp)
            {
                //throw exp;
                //MessageBox.Show(exp.Message);                    
            }

            return;
        }

        //private long UdpFirst = 0;
        //private long UdpLast = 0;
        //private bool isOpen = false;
        //private bool isConnection = false;
        

        private void CheckUdpData(byte[] data, int Length)
        {
            try
            {
                //this.Text = data;
                if (0 < Length)
                {
                    int CanID = 0;
                    //int CanID2 = 0;
                    float Ad;
                    ushort x = 0;
                    
                   
                    int DataLength;
                    ulong[] cData = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

                    CanID = (int)(((data[0] & 0xff) << 24) & 0xff000000);
                    CanID |= (int)(((data[1] & 0xff) << 16) & 0x00ff0000);
                    CanID |= (int)(((data[2] & 0xff) << 8) & 0x0000ff00);
                    CanID |= (int)(((data[3] & 0xff) << 0) & 0x000000ff);
                    DataLength = data[4];

                    for (int i = 0; i < DataLength; i++)
                    {
                        if (i < 8)
                            cData[i] = data[i + 5];
                        else break;
                    }

                    switch (CanID)
                    {
                        case 0x150: // Product In
                            InData[0] = (cData[4] << 0) | (cData[5] << 8) | (cData[6] << 16) | (cData[7] << 24);
                            InData[2] = (cData[0] << 0) | (cData[1] << 8) | (cData[2] << 16) | (cData[3] << 24);

                            break;
                        case 0x100: // 3232 In                            
                            InData[1] = (cData[4] << 0) | (cData[5] << 8) | (cData[6] << 16) | (cData[7] << 24);

                            //string s = string.Format("{0:X} {1:X} {2:X} {3:X} ", cData[4], cData[5], cData[6], cData[7]);
                            //this.Text = s;
                            break;
                        case 0x160:
                            x = (ushort)((cData[1] << 0) | (cData[2] << 8));

                            Ad = (float)(x * (5.0 / 4096.0));
                            //0 V일때 2.6V 가 뜬다고 함

                            if (2.6 <= CurrData)
                            {
                                Ad = (float)((CurrData - 2.6) * (30.0 / 2.4)); //2.5V 30A
                            }
                            else
                            {
                                Ad = (float)((2.6 - CurrData) * (-30.0 / 2.4)); //2.5V 30A
                            }
                            CurrData = Ad;
                            break;
                    }


                    //Application.DoEvents() 를 사용하면 에러 발생 (이 함수고 Callback 으로 호출되어서 그런다.
                    //Application.DoEvents();
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message + "\n" + exp.StackTrace);
            }

            return;
        }
        public static int OutPos = 0;
        private long OutPosFirst;
        private long OutPosLast;

        public float ADRead
        {
            get { return CurrData; }
        }

        public void outportb(int Out, bool OnOff)
        {
            byte Data = 0x00;

            int Pos = Out / 8;
            int dPos = Out % 8;

            Data = (byte)(0x01 << dPos);

            if (OnOff == true)
                OutData[0, Pos] |= Data;
            else OutData[0, Pos] &= (byte)(~Data);
            //OutPos = 1;
            UpdWrite2();
            return;
        }

        public void Function_outportb(short Out, bool OnOff)
        {
            byte Data = 0x00;

            int Pos = (int)Out / 8;
            int dPos = (int)Out % 8;

            Data = (byte)(0x01 << dPos);

            if (OnOff == true)
                OutData[1, Pos] |= Data;
            else OutData[1, Pos] &= (byte)(~Data);
            //OutPos = 1;
            UpdWrite2();
            return;
        }

        public void Function_outportb2(short Out, bool OnOff)
        {
            byte Data = 0x00;

            int Pos = (int)Out / 8;
            int dPos = (int)Out % 8;

            Data = (byte)(0x01 << dPos);

            if (OnOff == true)
                OutData[1, Pos] |= Data;
            else OutData[1, Pos] &= (byte)(~Data);
            //OutPos = 1;
            //UpdWrite2();
            return;
        }

        public bool GetOutputCheck(int Out)
        {
            byte Data = 0x00;

            int Pos = Out / 8;
            int dPos = Out % 8;

            Data = (byte)(0x01 << dPos);


            if ((OutData[0, Pos] & Data) == Data)
                return true;
            else return false;
        }

        //public void Function_outportb(short Out, bool OnOff)
        //{
        //    byte Data = 0x00;

        //    short IOOut = (short)(Out % 16); //보드 하나당 8 접점 16Bit 지원 (제품 6개 핀 번호 지원)
        //    short Card = (short)(Out / 16);

        //    int Pos = (int)IOOut / 8; //보드 하나당 8 접점 16Bit 지원 (제품 6개 핀 번호 지원)
        //    int dPos = (int)IOOut % 8;

        //    Data = (byte)(0x01 << dPos);

        //    if (OnOff == true)
        //        OutData[Card + 1, Pos] |= Data;
        //    else OutData[Card + 1, Pos] &= (byte)(~Data);
        //    //OutPos = 1;
        //    UpdWrite(Card);
        //    return;
        //}



        public void IOInit()
        {
            for (int i = 0; i < 8; i++)
            {
                OutData[0, i] = 0x00;
                OutData[1, i] = 0x00;
                OutData[2, i] = 0x00;
                OutData[3, i] = 0x00;
            }
            return;
        }

        public void FunctionIOInit()
        {
            for (int i = 0; i < 8; i++)
            {
                OutData[1, i] = 0x00;
                OutData[2, i] = 0x00;
                OutData[3, i] = 0x00;
            }
            return;
        }


        /// <summary>
        /// P32C32 지정된 포트의 I/O 위치를 알아낸다.
        /// </summary>
        /// <param name="Pos"></param>
        /// <returns></returns>
        public __IOData__ IOCheck(short Pos)
        {
            __IOData__ value = new __IOData__();

            int OPos = (int)Pos / 8;
            byte Data = (byte)(0x01 << ((int)Pos % 8));

            value.Card = (short)(OPos / 8);
            value.Pos = (short)(OPos % 8);
            value.Data = Data;

            return value;
        }

        private long UdpComCheckFirst;
        private long UdpComCheckLast;
#if THREAD_RUN
        private BackgroundWorker backgroundWorker1 = null;
        private void ThreadSetting()
        {
            backgroundWorker1 = new BackgroundWorker();

            //ReportProgress메소드를 호출하기 위해서 반드시 true로 설정, false일 경우 ReportProgress메소드를 호출하면 exception 발생
            backgroundWorker1.WorkerReportsProgress = true;
            //스레드에서 취소 지원 여부
            backgroundWorker1.WorkerSupportsCancellation = true;
            //스레드가 run시에 호출되는 핸들러 등록
            backgroundWorker1.DoWork += new DoWorkEventHandler(BackgroundWorker1_DoWork);
            // ReportProgress메소드 호출시 호출되는 핸들러 등록
            backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(BackgroundWorker1_ProgressChanged);
            // 스레드 완료(종료)시 호출되는 핸들러 동록
            backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BackgroundWorker1_RunWorkerCompleted);


            // 스레드가 Busy(즉, run)가 아니라면
            if (backgroundWorker1.IsBusy != true)
            {
                // 스레드 작동!! 아래 함수 호출 시 위에서 bw.DoWork += new DoWorkEventHandler(bw_DoWork); 에 등록한 핸들러가
                // 호출 됩니다.

                backgroundWorker1.RunWorkerAsync();
            }
            return;
        }

        private void BackgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //바로 위에서 worker.ReportProgress((i * 10));호출 시 
            // bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged); 등록한 핸들러가 호출 된다고
            // 하였는데요.. 이 부분에서는 기존 Thread에서 처럼 Dispatcher를 이용하지 않아도 됩니다. 
            // 즉 아래처럼!!사용이 가능합니다.
            //this.tbProgress.Text = (e.ProgressPercentage.ToString() + "%");

            // 기존의 Thread클래스에서 아래와 같이 UI 엘리먼트를 갱신하려면
            // Dispatcher.BeginInvoke(delegate() 
            // {
            //        this.tbProgress.Text = (e.ProgressPercentage.ToString() + "%");
            // )};
            //처럼 처리해야 할 것입니다. 그러나 바로 UI 엘리먼트를 업데이트 하고 있죠??
        }


        //스레드의 run함수가 종료될 경우 해당 핸들러가 호출됩니다.
        private void BackgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            //스레드가 종료한 이유(사용자 취소, 완료, 에러)에 맞쳐 처리하면 됩니다.
            if ((e.Cancelled == true))
            {
            }
            else if (!(e.Error == null))
            {

            }
            else
            {

            }
        }


        private void BackgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            do
            {
                //CancellationPending 속성이 true로 set되었다면(위에서 CancelAsync 메소드 호출 시 true로 set된다고 하였죠?
                if ((worker.CancellationPending == true))
                {
                    //루프를 break한다.(즉 스레드 run 핸들러를 벗어나겠죠)
                    e.Cancel = true;
                    break;
                }
                else
                {
                    // 이곳에는 스레드에서 처리할 연산을 넣으시면 됩니다.

                    Processing();

                    Thread.Sleep(5);
                    //await Task.Delay(1000);
                    // 스레드 진행상태 보고 - 이 메소드를 호출 시 위에서 
                    // bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged); 등록한 핸들러가 호출 됩니다.
                    worker.ReportProgress(10);
                }
                if (mControl.isExit == true)
                {
                    worker.CancelAsync();
                }
            } while (true);
            //while (ExitFlag == false);
        }


        private void Processing()
        {
            try
            {
                if (isOpen == true)
                {
                    UdpComCheckLast = mControl.공용함수.timeGetTimems();
                    if (isConnection == true)
                    {
                        // 컨넥션 후 1초 이상 통신이 없으면 컨넥션이 끊어진 것으로 간주 한다.
                        if (1500 <= (UdpComCheckLast - UdpComCheckFirst))
                        {
                            isConnection = false;
                            UdpComCheckFirst = mControl.공용함수.timeGetTimems();
                            UdpComCheckLast = mControl.공용함수.timeGetTimems();
                        }
                    }
                    else
                    {
                        if (500 <= (UdpComCheckLast - UdpComCheckFirst))
                        {
                            UdpCanCommunicationInit();
                            UdpComCheckFirst = mControl.공용함수.timeGetTimems();
                            UdpComCheckLast = mControl.공용함수.timeGetTimems();
                        }
                    }
                }
                if (OutPos == 1)
                {
                    byte[] Data1 = { OutData[1, 0], OutData[1, 1], OutData[1, 2], OutData[1, 3], OutData[1, 4], OutData[1, 5], OutData[1, 6], OutData[1, 7] };
                    byte[] Data2 = { OutData[0, 0], OutData[0, 1], OutData[0, 2], OutData[0, 3], OutData[0, 4], OutData[0, 5], OutData[0, 6], OutData[0, 7] };
                    UdpWrite(0x151, Data1, 8);
                    UdpWrite(0x101, Data2, 8);
                    OutPos = 2;
                    OutPosFirst = mControl.공용함수.timeGetTimems();
                    OutPosLast = mControl.공용함수.timeGetTimems();
                }
                else
                {
                    if (OutPos == 2)
                    {
                        OutPosFirst = mControl.공용함수.timeGetTimems();
                        OutPosLast = mControl.공용함수.timeGetTimems();
                        OutPos = 0;
                    }
                    else if (OutPos == 0)
                    {
                        OutPosLast = mControl.공용함수.timeGetTimems();
                        if (100 <= (OutPosLast - OutPosFirst))
                        {
                            OutPos = 1;
                        }
                    }
                    else
                    {
                        byte[] Data = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                        if (DarkCurrStart == true) Data[0] |= 0x01;
                        UdpWrite(0x140, Data, 8);
                        OutPos = 0;
                        OutPosFirst = mControl.공용함수.timeGetTimems();
                        OutPosLast = mControl.공용함수.timeGetTimems();
                    }
                }
            }
            catch { }
            finally
            {
            }
        }        
#else
        private void timer1_tick(object sender, EventArgs e)
        {
            try
            {
                timer1.Enabled = false;

                if (isOpen == true)
                {
                    UdpComCheckLast = mControl.공용함수.timeGetTimems();
                    if (isConnection == true)
                    {
                        if (1500 <= (UdpComCheckLast - UdpComCheckFirst))
                        {
                            isConnection = false;
                            UdpComCheckFirst = mControl.공용함수.timeGetTimems();
                            UdpComCheckLast = mControl.공용함수.timeGetTimems();
                        }
                    }
                    else
                    {
                        if (500 <= (UdpComCheckLast - UdpComCheckFirst))
                        {
                            UdpCanCommunicationInit();
                            UdpComCheckFirst = mControl.공용함수.timeGetTimems();
                            UdpComCheckLast = mControl.공용함수.timeGetTimems();
                        }
                    }
                }
                if (OutPos == 1)
                {
                    byte[] Data1 = { OutData[1, 0], OutData[1, 1], OutData[1, 2], OutData[1, 3], OutData[1, 4], OutData[1, 5], OutData[1, 6], OutData[1, 7] };
                    byte[] Data2 = { OutData[0, 0], OutData[0, 1], OutData[0, 2], OutData[0, 3], OutData[0, 4], OutData[0, 5], OutData[0, 6], OutData[0, 7] };
                    UdpWrite(0x151, Data1, 8);
                    UdpWrite(0x101, Data2, 8);
                    OutPos = 2;
                    OutPosFirst = mControl.공용함수.timeGetTimems();
                    OutPosLast = mControl.공용함수.timeGetTimems();
                }
                else
                {
                    if (OutPos == 2)
                    {
                        OutPosFirst = mControl.공용함수.timeGetTimems();
                        OutPosLast = mControl.공용함수.timeGetTimems();
                        OutPos = 0;
                    }
                    else if (OutPos == 0)
                    {
                        OutPosLast = mControl.공용함수.timeGetTimems();
                        if (100 <= (OutPosLast - OutPosFirst))
                        {
                            OutPos = 1;
                        }
                    }
                    else
                    {
                        byte[] Data = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                        if (DarkCurrStart == true) Data[0] |= 0x01;
                        UdpWrite(0x140, Data, 8);
                        OutPos = 0;
                        OutPosFirst = mControl.공용함수.timeGetTimems();
                        OutPosLast = mControl.공용함수.timeGetTimems();
                    }
                }
            }
            catch { }
            finally
            {
                timer1.Enabled = !mControl.isExit;
            }
        }
#endif

        private bool DarkCurrStart = false;
        public bool DarkCurrentReadStart
        {
            get
            {
                return DarkCurrStart;
            }
            set
            {
                if (DarkCurrStart != value)
                {
                    DarkCurrStart = value;
                    OutPos = 3;
                }
            }
        }

        

        public bool GetStartSw
        {
            get
            {
                //__IOData__ Pos = IOCheck(IO_IN.PASS);

                ulong Data = (ulong)0x01 << IO_IN.PASS;

                if ((InData[1] & Data) == Data)
                    return true;
                else return false;
            }
        }
        public bool GetResetSw
        {
            get
            {
                //__IOData__ Pos = IOCheck(IO_IN.RESET);

                //ulong Data = (ulong)Pos.Data << Pos.Pos;
                ulong Data = (ulong)0x01 << IO_IN.RESET;

                if ((InData[1] & Data) == Data)
                    return true;
                else return false;
            }
        }
        public bool GetAuto
        {
            get
            {
                //__IOData__ Pos = IOCheck(IO_IN.AUTO);

                //ulong Data = (ulong)Pos.Data << Pos.Pos;
                ulong Data = (ulong)0x01 << IO_IN.AUTO;

#if PROGRAM_RUNNING
                if ((InData[1] & Data) != Data)
                    return true;
                else return false;
#else
                if ((InData[1] & Data) == Data)
                    return true;
                else return false;
#endif
            }
        }

        public bool GetJigUp
        {
            get
            {
                //__IOData__ Pos = IOCheck(IO_IN.JIG_UP);

                //ulong Data = (ulong)Pos.Data << Pos.Pos;
                ulong Data = (ulong)0x01 << IO_IN.JIG_UP;

                if ((InData[1] & Data) == Data)
                    return true;
                else return false;
            }
        }

        public bool GetProductIn
        {
            get
            {
                //__IOData__ Pos = IOCheck(IO_IN.PRODUCT);

                //ulong Data = (ulong)Pos.Data << Pos.Pos;

                ulong Data = (ulong)0x01 << IO_IN.PRODUCT;

                if ((InData[1] & Data) == Data)
                    return true;
                else return false;
            }
        }

        public bool GetRHSelect
        {
            get
            {
                //__IOData__ Pos = IOCheck(IO_IN.LH_SELECT);

                //ulong Data = (ulong)Pos.Data << Pos.Pos;
                ulong Data = (ulong)0x01 << IO_IN.RH_SELECT;

                if ((InData[1] & Data) == Data)
                    return true;
                else return false;
            }
        }

        //public bool GetRHSelect
        //{
        //    get
        //    {
        //        __IOData__ Pos = IOCheck(IO_IN.RH_SELECT);

        //        ulong Data = (ulong)Pos.Data << Pos.Pos;

        //        if ((InData[0] & Data) == Data)
        //            return true;
        //        else return false;
        //    }
        //}
        public bool GetTestSetMode
        {
            get
            {
                ulong Data = (ulong)0x01 << IO_IN.TEST_SELECT;

                if ((InData[1] & Data) == Data)
                    return true;
                else return false;
            }
        }

        //public bool GetM1Sw
        //{
        //    get
        //    {
        //        //__IOData__ Pos = IOCheck(IO_IN.IMS_M1_SW);

        //        //ulong Data = (ulong)Pos.Data << Pos.Pos;
        //        ulong Data = (ulong)0x01 << IO_IN.IMS_M1_SW;

        //        if ((InData[0] & Data) == Data)
        //            return true;
        //        else return false;
        //    }
        //}
        //public bool GetM2Sw
        //{
        //    get
        //    {
        //        //__IOData__ Pos = IOCheck(IO_IN.IMS_M2_SW);

        //        //ulong Data = (ulong)Pos.Data << Pos.Pos;
        //        ulong Data = (ulong)0x01 << IO_IN.IMS_M2_SW;

        //        if ((InData[0] & Data) == Data)
        //            return true;
        //        else return false;
        //    }
        //}

        public bool GetSeatRelax
        {
            get
            {
                ulong Data = (ulong)0x01 << IO_IN.SEAT_RELAX;

                if ((InData[1] & Data) == Data)

                    return true;
                else return false;
            }
        }

        //public bool GetSeatPower
        //{
        //    get
        //    {
        //        __IOData__ Pos = IOCheck(IO_IN.SEAT_POWER);
        //        ulong Data = (ulong)Pos.Data << Pos.Pos;

        //        if ((InData[0] & Data) == Data)

        //            return true;
        //        else return false;
        //    }
        //}
        //public bool GetSeatManual
        //{
        //    get
        //    {
        //        __IOData__ Pos = IOCheck(IO_IN.SEAT_MANUAL);
        //        ulong Data = (ulong)Pos.Data << Pos.Pos;

        //        if ((InData[0] & Data) == Data)

        //            return true;
        //        else return false;
        //    }
        //}

        //public bool GetPSeat12Way
        //{
        //    get
        //    {
        //        __IOData__ Pos = IOCheck(IO_IN.PSEAT_12WAY);
        //        ulong Data = (ulong)Pos.Data << Pos.Pos;

        //        if ((InData[0] & Data) == Data)

        //            return true;
        //        else return false;
        //    }
        //}

        //public bool GetPSeat10Way
        //{
        //    get
        //    {
        //        __IOData__ Pos = IOCheck(IO_IN.PSEAT_10WAY);
        //        ulong Data = (ulong)Pos.Data << Pos.Pos;

        //        if ((InData[0] & Data) == Data)

        //            return true;
        //        else return false;
        //    }
        //}

        //public bool GetPSeat8Way
        //{
        //    get
        //    {
        //        __IOData__ Pos = IOCheck(IO_IN.PSEAT_8WAY);
        //        ulong Data = (ulong)Pos.Data << Pos.Pos;

        //        if ((InData[0] & Data) == Data)

        //            return true;
        //        else return false;
        //    }
        //}

        //public bool GetPSeat4Way
        //{
        //    get
        //    {
        //        __IOData__ Pos = IOCheck(IO_IN.PSEAT_4WAY);
        //        ulong Data = (ulong)Pos.Data << Pos.Pos;

        //        if ((InData[0] & Data) == Data)

        //            return true;
        //        else return false;
        //    }
        //}

        //public bool GetPSeat2Way
        //{
        //    get
        //    {
        //        __IOData__ Pos = IOCheck(IO_IN.PSEAT_2WAY);
        //        ulong Data = (ulong)Pos.Data << Pos.Pos;

        //        if ((InData[0] & Data) == Data)

        //            return true;
        //        else return false;
        //    }
        //}

        public bool GetRelax_Relax
        {
            get
            {
                //__IOData__ Pos = IOCheck(IO_IN.SLIDE_FWD);
                //ulong Data = (ulong)Pos.Data << Pos.Pos;
                ulong Data = (ulong)0x01 << IO_IN.RELAX_RELAX;

                if ((InData[1] & Data) == Data)
                    return true;
                else return false;
            }
        }
        public bool GetRelaxReturn
        {
            get
            {
                //__IOData__ Pos = IOCheck(IO_IN.SLIDE_BWD);
                //ulong Data = (ulong)Pos.Data << Pos.Pos;
                ulong Data = (ulong)0x01 << IO_IN.RELAX_RETURN;

                if ((InData[1] & Data) == Data)
                    return true;
                else return false;
            }
        }
        public bool GetReclinerFwd
        {
            get
            {
                //__IOData__ Pos = IOCheck(IO_IN.RECLINE_FWD);
                //ulong Data = (ulong)Pos.Data << Pos.Pos;
                ulong Data = (ulong)0x01 << IO_IN.RECLINE_FWD;
                if ((InData[1] & Data) == Data)
                    return true;
                else return false;
            }
        }
        public bool GetReclinerBwd
        {
            get
            {
                //__IOData__ Pos = IOCheck(IO_IN.RECLINE_BWD);
                //ulong Data = (ulong)Pos.Data << Pos.Pos;
                ulong Data = (ulong)0x01 << IO_IN.RECLINE_BWD;

                if ((InData[1] & Data) == Data)
                    return true;
                else return false;
            }
        }
        public bool GetLegrest_Rest
        {
            get
            {
                //__IOData__ Pos = IOCheck(IO_IN.TILT_UP);
                //ulong Data = (ulong)Pos.Data << Pos.Pos;
                ulong Data = (ulong)0x01 << IO_IN.LEGREST;

                if ((InData[1] & Data) == Data)
                    return true;
                else return false;
            }
        }
        public bool GetLegrest_Return
        {
            get
            {
                //__IOData__ Pos = IOCheck(IO_IN.TILT_DN);
                //ulong Data = (ulong)Pos.Data << Pos.Pos;
                ulong Data = (ulong)0x01 << IO_IN.LEGREST_RETURN;

                if ((InData[1] & Data) == Data)
                    return true;
                else return false;
            }
        }

        public bool GetHeightUp
        {
            get
            {
                //__IOData__ Pos = IOCheck(IO_IN.HEIGHT_UP);
                //ulong Data = (ulong)Pos.Data << Pos.Pos;
                ulong Data = (ulong)0x01 << IO_IN.HEIGHT_UP;
                if ((InData[1] & Data) == Data)
                    return true;
                else return false;
            }
        }

        public bool GetHeightDn
        {
            get
            {
                //__IOData__ Pos = IOCheck(IO_IN.HEIGHT_DN);
                //ulong Data = (ulong)Pos.Data << Pos.Pos;
                ulong Data = (ulong)0x01 << IO_IN.HEIGHT_DN;
                if ((InData[1] & Data) == Data)
                    return true;
                else return false;
            }
        }

        public bool GetLegrestExt
        {
            get
            {
                //__IOData__ Pos = IOCheck(IO_IN.LUMBER_FWD);
                //ulong Data = (ulong)Pos.Data << Pos.Pos;
                ulong Data = (ulong)0x01 << IO_IN.LEGREST_EXT;

                if ((InData[1] & Data) == Data)
                    return true;
                else return false;
            }
        }
        public bool GetLegrestExtReturn
        {
            get
            {
                //__IOData__ Pos = IOCheck(IO_IN.LUMBER_BWD);
                //ulong Data = (ulong)Pos.Data << Pos.Pos;
                ulong Data = (ulong)0x01 << IO_IN.LEGREST_EXT_RETURN;
                if ((InData[1] & Data) == Data)

                    return true;
                else return false;
            }
        }

        public bool BuzzerOnOff
        {
            set
            {
                outportb(IO_OUT.BUZZER, value);
                UpdWrite2();
            }
            get
            {
                return GetOutputCheck(IO_OUT.BUZZER);
            }
        }

        public bool YellowLampOnOff
        {
            set
            {
                outportb(IO_OUT.YELLOW, value);
                UpdWrite2();                   
            }
            get
            {
                return GetOutputCheck(IO_OUT.YELLOW);
            }
        }
        public bool RedLampOnOff
        {
            set
            {
                outportb(IO_OUT.RED, value);
                UpdWrite2();
            }
            get
            {
                return GetOutputCheck(IO_OUT.RED);
            }
        }

        public bool GreenLampOnOff
        {
            set
            {
                outportb(IO_OUT.GREEN, value);
                UpdWrite2();
            }
            get
            {
                return GetOutputCheck(IO_OUT.GREEN);
            }
        }

        public bool ProductInOut
        {
            set
            {
                outportb(IO_OUT.PRODUCT, value);
                UpdWrite2();
            }
            get
            {
                return GetOutputCheck(IO_OUT.PRODUCT);
            }
        }

        public bool TestOKOnOff
        {
            set
            {
                outportb(IO_OUT.TEST_OK, value);
                UpdWrite2();
            }
            get
            {
                return GetOutputCheck(IO_OUT.TEST_OK);
            }
        }

        //public bool SetJigDownOnOff
        //{
        //    set
        //    {
        //        outportb(IO_OUT.TEST_JIG_DOWN, value);
        //        UpdWrite2();
        //    }
        //    get
        //    {
        //        return GetOutputCheck(IO_OUT.TEST_JIG_DOWN);
        //    }
        //}

        public bool SetDoorOpen
        {
            set
            {
                outportb(IO_OUT.DOOR_OPEN, value);
                UpdWrite2();
            }
            get
            {
                return GetOutputCheck(IO_OUT.DOOR_OPEN);
            }
        }

        public bool TestINGOnOff
        {
            set
            {
                outportb(IO_OUT.TEST_ING, value);
                UpdWrite2();
            }
            get
            {
                return GetOutputCheck(IO_OUT.TEST_ING);
            }
        }

        public bool PinConnectionOnOff
        {
            set
            {
                outportb(IO_OUT.PIN_CONNECTION, value);
                UpdWrite2();
            }
            get
            {
                return GetOutputCheck(IO_OUT.PIN_CONNECTION);
            }
        }
        public bool GetPinConnectSw
        {
            
            get
            {
                //__IOData__ Pos = IOCheck(IO_IN.LUMBER_FWD);
                //ulong Data = (ulong)Pos.Data << Pos.Pos;
                ulong Data = (ulong)0x01 << IO_IN.PIN_CONNECTION_SW;

                if ((InData[1] & Data) == Data)
                    return true;
                else return false;
            }
        }
        public bool GetPinConnectFwd
        {

            get
            {
                //__IOData__ Pos = IOCheck(IO_IN.LUMBER_FWD);
                //ulong Data = (ulong)Pos.Data << Pos.Pos;
                ulong Data = (ulong)0x01 << IO_IN.PIN_CONNECTION_FWD;

                if ((InData[1] & Data) == Data)
                    return true;
                else return false;
            }
        }
        public bool GetPinConnectBwd
        {

            get
            {
                //__IOData__ Pos = IOCheck(IO_IN.LUMBER_FWD);
                //ulong Data = (ulong)Pos.Data << Pos.Pos;
                ulong Data = (ulong)0x01 << IO_IN.PIN_CONNECTION_BWD;

                if ((InData[1] & Data) == Data)
                    return true;
                else return false;
            }
        }

        public bool GetReclineToRelax납입센서
        {
            get
            {
                ulong Data = (ulong)0x01 << IO_IN.SlideDeliveryPosSensor;

                if ((InData[1] & Data) != Data)
                    return true;
                else return false;
            }
        }
        public bool GetRecline납입센서
        {
            get
            {
                ulong Data = (ulong)0x01 << IO_IN.ReclineeDeliveryPosSensor;

                if ((InData[1] & Data) != Data)
                    return true;
                else return false;
            }
        }

        public bool SetPSeatBatt
        {
            set
            {
                Function_outportb(IO_FUNCTION_OUT.PSEAT_BATT, value);
            }
            get
            {
                return GetFunctionOut(IO_FUNCTION_OUT.PSEAT_BATT);
            }
        }

        public bool GetFunctionOut(short Out)
        {
            byte Data = 0x00;

            short IOOut = (short)(IO_FUNCTION_OUT.PSEAT_BATT % 16); //보드 하나당 8 접점 16Bit 지원 (제품 6개 핀 번호 지원)
            short Card = (short)(IO_FUNCTION_OUT.PSEAT_BATT / 16);

            int Pos = (int)IOOut / 8; //보드 하나당 8 접점 16Bit 지원 (제품 6개 핀 번호 지원)
            int dPos = (int)IOOut % 8;

            Data = (byte)(0x01 << dPos);


            if ((OutData[Card + 1, Pos] & Data) == Data)
                return true;
            else return false;
        }

        public bool SetIgn1
        {
            set
            {
                Function_outportb(IO_FUNCTION_OUT.IGN1, value);                
            }
            get
            {
                return GetFunctionOut(IO_FUNCTION_OUT.IGN1);
            }
        }
        public bool SetIgn2
        {
            set
            {
                Function_outportb(IO_FUNCTION_OUT.IGN2, value);
            }
            get
            {
                return GetFunctionOut(IO_FUNCTION_OUT.IGN2);
            }
        }

        public bool PSeatPowerOnOff
        {
            set
            {
                if (value == true)
                {
                    Function_outportb(IO_FUNCTION_OUT.PSEAT_BATT, value);
                    //mControl.공용함수.timedelay(500);
                    Function_outportb2(IO_FUNCTION_OUT.IGN1, value);
                    Function_outportb(IO_FUNCTION_OUT.IGN2, value);
                }
                else
                {
                    Function_outportb2(IO_FUNCTION_OUT.IGN1, value);
                    Function_outportb(IO_FUNCTION_OUT.IGN2, value);
                    //mControl.공용함수.timedelay(500);
                    Function_outportb(IO_FUNCTION_OUT.PSEAT_BATT, value);
                }
            }
        }

        //public bool SetRelax
        //{
        //    set
        //    {
        //        Function_outportb(IO_FUNCTION_OUT.RELAX, value);
        //    }
        //    get
        //    {
        //        return GetFunctionOut(IO_FUNCTION_OUT.RELAX);
        //    }
        //}

        //public bool SetRelaxReturn
        //{
        //    set
        //    {
        //        Function_outportb(IO_FUNCTION_OUT.RELAX_RETURN, value);
        //    }
        //    get
        //    {
        //        return GetFunctionOut(IO_FUNCTION_OUT.RELAX_RETURN);
        //    }
        //}

        //public bool SetReclineFwd
        //{
        //    set
        //    {
        //        outportb(IO_OUT.RECLINER_FWD, value);                
        //    }
        //    get
        //    {
        //        return GetOutputCheck(IO_OUT.RECLINER_FWD);
        //    }
        //}

        //public bool SetReclineBwd
        //{
        //    set
        //    {
        //        outportb(IO_OUT.RECLINER_BWD, value);                
        //    }
        //    get
        //    {
        //        return GetOutputCheck(IO_OUT.RECLINER_BWD);
        //    }
        //}

        //public bool SetHeightUp
        //{
        //    set
        //    {
        //        Function_outportb(IO_FUNCTION_OUT.HEIGHT_UP, value);
        //    }
        //    get
        //    {
        //        return GetFunctionOut(IO_FUNCTION_OUT.HEIGHT_UP);
        //    }
        //}
        //public bool SetHeightDown
        //{
        //    set
        //    {
        //        Function_outportb(IO_FUNCTION_OUT.HEIGHT_DN, value);
        //    }
        //    get
        //    {
        //        return GetFunctionOut(IO_FUNCTION_OUT.HEIGHT_DN);
        //    }
        //}

        //public bool SetLegrest
        //{
        //    set
        //    {
        //        Function_outportb(IO_FUNCTION_OUT.LEGREST, value);
        //    }
        //    get
        //    {
        //        return GetFunctionOut(IO_FUNCTION_OUT.LEGREST);
        //    }
        //}
        //public bool SetLegrestReturn
        //{
        //    set
        //    {
        //        Function_outportb(IO_FUNCTION_OUT.LEGREST_RETURN, value);
        //    }
        //    get
        //    {
        //        return GetFunctionOut(IO_FUNCTION_OUT.LEGREST_RETURN);
        //    }
        //}
        //public bool SetLegrestExt
        //{
        //    set
        //    {
        //        Function_outportb(IO_FUNCTION_OUT.LEGRESTEXT, value);
        //    }
        //    get
        //    {
        //        return GetFunctionOut(IO_FUNCTION_OUT.LEGRESTEXT);
        //    }
        //}
        //public bool SetLegrestExtReturn
        //{
        //    set
        //    {
        //        Function_outportb(IO_FUNCTION_OUT.LEGRESTEXT_RETURN, value);
        //    }
        //    get
        //    {
        //        return GetFunctionOut(IO_FUNCTION_OUT.LEGRESTEXT_RETURN);
        //    }
        //}

        ~IOControl()
        {

        }
    }
}
