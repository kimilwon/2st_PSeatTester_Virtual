using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PSeatTester
{
    public class SoundMeter
    {
        private SerialPort SoundPort = null;
        private Stopwatch STOP_WATCH = new Stopwatch();
        private Timer timer1 = new Timer();
        private bool SendFlag = false;
        private long First;
        private long Last;
        private bool Bswa308_309 = false;

        public const byte STX = 0x02;
        public const byte ETX = 0x03;
        public const byte CR = 0x0d;
        public const byte LF = 0x0a;
        public const byte ID = 1;
        public const byte SET_COMMAND = 0x43;
        public const byte READ_COMMAND = 0x41;
        public const byte ACK = 0x06;
        public const byte NAK = 0x15;
        public bool SoundNotChangeFlag = false;
        public bool SoundNotChangeFlag2 = false;
        public long SoundNotChangeTimeToFirst = 0;
        public long SoundNotChangeTimeToLast = 0;
        public float OldSound = 0;
        public short FirstSoundcheckCount = 0;

        public SoundMeter()
        {
            if (SoundPort == null) SoundPort = new SerialPort();
            SoundPort.BaudRate = 9600;
            SoundPort.Parity = Parity.None;
            SoundPort.DataBits = 8;
            SoundPort.StopBits = StopBits.One;
            SoundPort.DataReceived += new SerialDataReceivedEventHandler(SoundPortReceive);
            STOP_WATCH.Start();

            timer1.Interval = 10;
            timer1.Tick += new EventHandler(time1_tick);
            timer1.Enabled = true;
            return;
        }

        public SoundMeter(string PortName, bool Bswa308_309 = false)
        {
            this.Bswa308_309 = Bswa308_309;
            if (SoundPort == null) SoundPort = new SerialPort();

            if ((PortName != null) && (PortName != "") && (PortName != string.Empty))
            {
                string[] sName = System.IO.Ports.SerialPort.GetPortNames();

                if (sName.Contains(PortName) == true)
                {
                    SoundPort.PortName = PortName;

                    SoundPort.BaudRate = 9600;

                    SoundPort.Parity = Parity.None;
                    SoundPort.DataBits = 8;
                    SoundPort.StopBits = StopBits.One;
                    SoundPort.DataReceived += new SerialDataReceivedEventHandler(SoundPortReceive);
                }
                else
                {
                    MessageBox.Show("소음기를 찾지 못횄습니다.");
                }
            }
            STOP_WATCH.Start();
                        
            timer1.Interval = 10;
            timer1.Tick += new EventHandler(time1_tick);
            timer1.Enabled = true;
            
            return;
        }

        private bool ComOpenToFirstFlag = false;
        private short ComCloseToCount = 0;
        private long ComCloseTimeToFirst = 0;
        private long ComCloseTimeToLast = 0;

        byte[] Request = { 0x02, 0x41, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03 };
        public void time1_tick(object sender, EventArgs e)
        {
            try
            {
                timer1.Enabled = false;

                if (SoundPort.IsOpen == true)
                {
                    if (ComOpenToFirstFlag == false) ComOpenToFirstFlag = true;
                    if (0 < ComCloseToCount) ComCloseToCount = 0;

                    ComCloseTimeToFirst = timeGetTimems();
                    ComCloseTimeToLast = timeGetTimems();

                    if (Bswa308_309 == false)
                    {
                        if (SendFlag == false)
                        {
                            SoundPort.Write(Request, 0, Request.Length);
                            SendFlag = true;
                            First = timeGetTimems();
                            Last = timeGetTimems();
                        }
                        else
                        {
                            Last = timeGetTimems();
                            if (500 <= (Last - First))
                            {
                                SendFlag = false;
                            }
                        }
                    }
                    //else
                    //{
                        //연속으로 데이타를 보내는 코드를 전송할 수 있으나 메타 파워를 껏다 켰을때를 대비해서 싱글 데이타 전송 명령을 사용함.
                        //if (SendFlag == false)
                        //{
                        //    DataReadToSingleReturn();
                        //    SendFlag = true;
                        //    First = timeGetTimems();
                        //    Last = timeGetTimems();
                        //}
                        //else
                        //{
                        //    Last = timeGetTimems();
                        //    if (1000 <= (Last - First))
                        //    {
                        //        SendFlag = false;
                        //    }
                        //}
                    //}


                    //처음 프로그램이 시작 되었을때 사운드 값이 변하지 않는경우 (즉 통신 초기화가 잘 안됬을떄) 다시한번 초기화를 하기 위한 장치
                    if (SoundNotChangeFlag == true)
                    {
                        SoundNotChangeTimeToLast = timeGetTimems();
                        if (SoundNotChangeFlag2 == true)
                        {
                            if (1000 <= (SoundNotChangeTimeToLast - SoundNotChangeTimeToFirst))
                            {
                                SoundNotChangeTimeToFirst = timeGetTimems();
                                SoundNotChangeTimeToLast = timeGetTimems();
                                SoundNotChangeFlag2 = false;
                            }

                            OldSound = SoundData;
                        }
                        else
                        {
                            if (4000 <= (SoundNotChangeTimeToLast - SoundNotChangeTimeToFirst))
                            {
                                SoundNotChangeFlag = false;
                                StartStopMesurement(false);
                                timedelay(4000);
                                SoundInit();
                                timedelay(2000);
                            }
                            else
                            {
                                if (OldSound != SoundData)
                                {
                                    FirstSoundcheckCount++;
                                    if (2 <= FirstSoundcheckCount)
                                    {
                                        SoundNotChangeFlag = false;
                                    }
                                    OldSound = SoundData;
                                }
                            }
                        }
                    }
                    //else
                    //{
                    //    if (OldSound != SoundData)
                    //    {
                    //        SoundNotChangeTimeToFirst = timeGetTimems();
                    //        SoundNotChangeTimeToLast = timeGetTimems();
                    //    }
                    //    else
                    //    {
                    //        SoundNotChangeTimeToLast = timeGetTimems();
                    //        if (60000 <= (SoundNotChangeTimeToLast - SoundNotChangeTimeToFirst))
                    //        {
                    //            StartStopMesurement(false);
                    //            timedelay(4000);
                    //            SoundInit();
                    //            timedelay(2000);
                    //            SoundNotChangeTimeToFirst = timeGetTimems();
                    //            SoundNotChangeTimeToLast = timeGetTimems();
                    //        }
                    //    }
                    //    if (OldSound != SoundData) OldSound = SoundData;
                    //}
                }
                else
                {
                    if (ComOpenToFirstFlag == true)
                    {
                        if (ComCloseToCount < 10)
                        {
                            ComCloseTimeToLast = timeGetTimems();
                            if (1000 <= (ComCloseTimeToLast - ComCloseTimeToFirst))
                            {
                                SoundPort.Open();
                                if (SoundPort.IsOpen == false)
                                {
                                    ComCloseToCount++;
                                    ComCloseTimeToFirst = timeGetTimems();
                                    ComCloseTimeToLast = timeGetTimems();
                                }
                                else
                                {
                                    ComCloseToCount = 0;
                                    ComCloseTimeToFirst = timeGetTimems();
                                    ComCloseTimeToLast = timeGetTimems();
                                }
                            }
                        }
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

        /// <summary>
        /// 0 = 1/1Octave
        /// 1 = Level meter mode
        /// 2 = 1/3 Octave(Optional)
        /// default = 1
        /// </summary>
        /// <param name="Mode"></param>
        public void SetTheMeasurementMode(short Mode)
        {
            byte[] Data = { STX, ID, SET_COMMAND, (byte)'M', (byte)'E', (byte)'M', 0x00, ETX, 0x00, CR, LF };

            Data[6] = ShortToHex(Mode);
            Data[8] = CRC_CHECk(Data, 8);
            if(SoundPort.IsOpen == true) SoundPort.Write(Data, 0, Data.Length);
            timedelay2(2000);
            return;
        }

        /// <summary>
        /// 2 = 4800
        /// 3 = 9600
        /// 4 = 19200
        /// default = 3
        /// </summary>
        /// <param name="Baudrate"></param>
        public void SetTheBaudRate(short Baudrate)
        {
            byte[] Data = { STX, ID, SET_COMMAND, (byte)'B', (byte)'R', (byte)'T', 0x00, ETX, 0x00, CR, LF };

            Data[6] = ShortToHex(Baudrate);
            Data[8] = CRC_CHECk(Data, 8);
            if (SoundPort.IsOpen == true) SoundPort.Write(Data, 0, Data.Length);
            return;
        }

        /// <summary>
        /// ID = 1 ~ 255
        /// </summary>
        /// <param name="mID"></param>
        public void SetTheID(short mID)
        {
            byte[] Data = { STX, ID, SET_COMMAND, (byte)'I', (byte)'D', (byte)'X', 0x00, ETX, 0x00, CR, LF };

            Data[6] = ShortToHex(mID);
            Data[8] = CRC_CHECk(Data, 8);
            if (SoundPort.IsOpen == true) SoundPort.Write(Data, 0, Data.Length);
            return;
        }
        /// <summary>
        /// bswa-309 start Measurement
        /// </summary>
        public void StartStopMesurement(bool Flag)
        {
            byte[] Data = { STX, ID, SET_COMMAND, (byte)'S', (byte)'T', (byte)'A', 0x00, ETX, 0x00, CR, LF };

            Data[6] = Flag == true ? ShortToHex(1) : ShortToHex(0);
            Data[8] = CRC_CHECk(Data, 8);
            if (SoundPort.IsOpen == true) SoundPort.Write(Data, 0, Data.Length);

            timedelay2(4000);
            return;
        }
        /// <summary>
        /// bswa-309 Auto Power On/Off
        /// 0 = 1분
        /// 1 = 5분
        /// 2 = 10분
        /// 3 = 30분
        /// 4 = OFF
        /// default = 4
        /// <param name="aPwr"></param>
        public void SetAutoPowerOff(short aPwr)
        {
            byte[] Data = { STX, ID, SET_COMMAND, (byte)'P', (byte)'W', (byte)'O', 0x00, ETX, 0x00, CR, LF };

            Data[6] = ShortToHex(aPwr);
            Data[8] = CRC_CHECk(Data, 8);
            if (SoundPort.IsOpen == true) SoundPort.Write(Data, 0, Data.Length);
            return;
        }

        /// <summary>
        /// Group = 1 ~ 14
        /// Filter = 0 :A, 1 : B, 2 : C, 3 : Z
        /// Detector = 0 : Fast, 1 : Slide, 2 : Imp
        /// Mode = 
        ///    0 : SPL,
        ///    1 : SD,
        ///    2 : SEL,
        ///    3 : E,
        ///    4 : MAX,
        ///    5 : MIN,
        ///    6 : PEAK,
        ///    7 : LEQ,
        ///    8 ~ 17 : LN1 ~ LN10
        /// </summary>
        /// <param name="Group"></param>
        /// <param name="Filter"></param>
        /// <param name="Detector"></param>
        /// <param name="Mode"></param>
        public void SetCustomMeasure(short Group, short Filter, short Detector, short Mode)
        {
            byte[] Data = { STX, ID, SET_COMMAND, (byte)'C', (byte)'U', (byte)'S', 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, ETX, 0x00, CR, LF };

            Data[6] = ShortToHex(Group);
            Data[7] = 0x20;
            Data[8] = ShortToHex(Filter);
            Data[9] = 0x20;
            Data[10] = ShortToHex(Detector);
            Data[11] = 0x20;
            Data[12] = ShortToHex(Mode);
            Data[14] = CRC_CHECk(Data, 14);
            if (SoundPort.IsOpen == true) SoundPort.Write(Data, 0, Data.Length);
            return;
        }

        /// <summary>
        /// Group = 1 ~ 14
        /// Filter = 0 :A, 1 : B, 2 : C, 3 : Z
        /// Detector = 0 : Fast, 1 : Slide, 2 : Imp
        /// Mode = 
        ///    0 : SPL,
        ///    1 : SD,
        ///    2 : SEL,
        ///    3 : E,
        ///    4 : MAX,
        ///    5 : MIN,
        ///    6 : PEAK,
        ///    7 : LEQ,
        ///    8 ~ 17 : LN1 ~ LN10
        /// </summary>
        /// <param name="Filter"></param>
        /// <param name="Detector"></param>
        /// <param name="Mode"></param>
        public void SetOutput(short Filter, short Detector, short Mode)
        {
            byte[] Data = { STX, ID, SET_COMMAND, (byte)'O', (byte)'U', (byte)'T', 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x30, ETX, 0x00, CR, LF };

            Data[6] = ShortToHex(Filter);
            Data[7] = 0x20;
            Data[8] = ShortToHex(Detector);
            Data[9] = 0x20;
            Data[10] = ShortToHex(Mode);
            Data[11] = 0x20;
            Data[14] = CRC_CHECk(Data, 14);
            if (SoundPort.IsOpen == true) SoundPort.Write(Data, 0, Data.Length);
                        
            return;
        }

        private byte CRC_CHECk(byte[] Data, short Length)
        {
            byte Crc = 0x00;

            for (int i = 0; i < Length; i++)
            {
                //if(i == 1)
                    //Crc = Data[i];
                //else 
                Crc ^= Data[i];
            }
            return Crc;
        }

        public void SetUsbMode(short Mode)
        {
            byte[] Data = { STX, ID, SET_COMMAND, (byte)'U', (byte)'M', (byte)'D', 0x00, ETX, 0x00, CR, LF };

            Data[6] = ShortToHex(Mode);
            Data[8] = CRC_CHECk(Data, 8);
            if (SoundPort.IsOpen == true) SoundPort.Write(Data, 0, Data.Length);
            timedelay2(2000);
            return;
        }

        public bool Open(string PortName)
        {
            if (SoundPort.IsOpen == false)
            {
                SoundPort.PortName = PortName;
                SoundPort.BaudRate = 9600;
                SoundPort.Open();
            }

            SoundInit();
            SoundNotChangeFlag = true;
            SoundNotChangeFlag2 = true;
            SoundNotChangeTimeToFirst = timeGetTimems();
            SoundNotChangeTimeToLast = timeGetTimems();
            return SoundPort.IsOpen;
        }

        public bool Open(string PortName, bool Bswa308_309 = false)
        {
            if (SoundPort.IsOpen == false)
            {
                SoundPort.PortName = PortName;
                SoundPort.BaudRate = 9600;
                SoundPort.Open();
                this.Bswa308_309 = Bswa308_309;
            }

            SoundInit(); 
            SoundNotChangeFlag = true;
            SoundNotChangeFlag2 = true;
            SoundNotChangeTimeToFirst = timeGetTimems();
            SoundNotChangeTimeToLast = timeGetTimems();
            return SoundPort.IsOpen;
        }

        public bool Open()
        {
            if (SoundPort.IsOpen == false) SoundPort.Open();
            SoundInit();
            SoundNotChangeFlag = true;
            SoundNotChangeFlag2 = true;
            SoundNotChangeTimeToFirst = timeGetTimems();
            SoundNotChangeTimeToLast = timeGetTimems();
            return SoundPort.IsOpen;
        }

        private void SoundInit()
        {
            if ((Bswa308_309 == true) && (SoundPort.IsOpen == true))
            {
                SetUsbMode(2);
                SetTheMeasurementMode(1);
                //Filter 값에 따라 측정 값이 이 달라진다.
                //또한 Integration 와 SWNLoger 에 따라 실시간 데이타중 샘플링한 데이타중 RMS 데이타, 최대, 최소, Peak 값 등을 전송한다.
                //Integration == 2 -> LEQ 모드 (RMS)
                SetProfile(File: 1, Filter: 0, Detector: 0, Integration: 2, SWNLoger: 0);
                //Period 값이 클수록 떨어지는 소음 측정이 상승후 늦게 떨어진다. 단 0이면 정말 느리다.
                SetMeasurementSetup(Delay: 1, Period: 1, Repeat: 0, SWNLogger: 0, SWNLogStep: 3, CSDLogger: 0, CSDLogStep: 59);
                StartStopMesurement(true);
                DataReadToContinueReturnStart(true);
            }
            return;
        }

        public void Close()
        {
            if (SoundPort.IsOpen == true)
            {
                if (Bswa308_309 == true)
                {
                    //SetTheBaudRate(3);
                    //timedelay(100);
                    StartStopMesurement(false);
                    //timedelay(100);                    
                    //DataReadToContinueReturnStart(true);
                }
                timedelay(1000);
                SoundPort.Close();
            }
            return;
        }

        public long timeGetTimems()
        {
            double ticks = STOP_WATCH.ElapsedTicks;

            //return STOP_WATCH.ElapsedMilliseconds;
            return (long)((ticks / Stopwatch.Frequency) * 1000);
        }

        public long timeGetNanoTimes()
        {
            //1ticks == 100 nanosecond
            //return STOP_WATCH.Elapsed.Ticks;// ElapsedTicks;
            double ticks = STOP_WATCH.ElapsedTicks;
            return (long)((ticks / Stopwatch.Frequency) * 1000000000);
        }

        public void DataReadToSingleReturn()
        {
            byte[] Data = { STX, ID, SET_COMMAND, (byte)'D', (byte)'M', (byte)'A', 0x00, 0x20, 0x3F, ETX, 0x00, CR, LF };

            Data[6] = ShortToHex(1);
            Data[10] = CRC_CHECk(Data, 10);
            if (SoundPort.IsOpen == true) SoundPort.Write(Data, 0, Data.Length);
            return;
        }

        public void DataReadToContinueReturnStart(bool StartStop)
        {
            byte[] Data = { STX, ID, SET_COMMAND, (byte)'D', (byte)'M', (byte)'A', 0x00, 0x20, 0x3F, ETX, 0x00, CR, LF };

            if (StartStop == true)
                Data[6] = ShortToHex(2);
            else Data[6] = ShortToHex(0);

            Data[10] = CRC_CHECk(Data, 10);
            if (SoundPort.IsOpen == true) SoundPort.Write(Data, 0, Data.Length);

            timedelay2(2000);
            return;
        }

        public void SetMeasurementSetup(short Delay = 1, short Period = 0, short Repeat = 0, short SWNLogger = 0, short SWNLogStep = 3, short CSDLogger = 0, short CSDLogStep = 59)
        {
            byte[] Data = { STX, ID, SET_COMMAND, (byte)'B', (byte)'S', (byte)'E', 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, ETX, 0x00, CR, LF };

            if (Delay == 0) Delay = 1;
            Data[6] = ShortToHex(Delay);
            Data[7] = 0x20;

            Data[8] = ShortToHex(Period);
            Data[9] = 0x20;

            Data[10] = ShortToHex(Repeat);
            Data[11] = 0x20;

            Data[12] = ShortToHex(SWNLogger);
            Data[13] = 0x20;

            Data[14] = ShortToHex(SWNLogStep);
            Data[15] = 0x20;

            Data[16] = ShortToHex(CSDLogger);
            Data[17] = 0x20;

            Data[18] = ShortToHex(CSDLogStep);

            Data[20] = CRC_CHECk(Data, 20);


            if (SoundPort.IsOpen == true) SoundPort.Write(Data, 0, Data.Length);
            timedelay2(2000);
            return;
        }

        /// <summary>
        /// Filter
        ///     0 = A
        ///     1 = B
        ///     2 = C
        ///     3 = Z
        ///     Default = 0
        /// Detector
        ///     0 = Fast
        ///     1 = Slow
        ///     2 = Imp
        ///     Default = 0
        /// Integration mode
        ///     0 = SPL
        ///     1 = PEAK
        ///     2 = LEQ
        ///     3 = MAX
        ///     4 = MIN
        ///     defalut = 0
        /// SWN Logger
        ///     0 = LEQ
        ///     1 = PEAK
        ///     2 = MAX
        ///     3 = MIN
        ///     defult = 0
        /// </summary>
        /// <param name="File"></param>
        /// <param name="Filter"></param>
        /// <param name="Detector"></param>
        /// <param name="Integration"></param>
        /// <param name=""></param>
        public void SetProfile(short File = 1, short Filter = 0, short Detector = 0, short Integration = 0, short SWNLoger = 0)
        {
            byte[] Data = { STX, ID, SET_COMMAND, (byte)'P', (byte)'R', ShortToHex(File), 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, ETX, 0x00, CR, LF };

            Data[6] = ShortToHex(Filter);
            Data[7] = 0x20;

            Data[8] = ShortToHex(Detector);
            Data[9] = 0x20;

            Data[10] = ShortToHex(Integration);
            Data[11] = 0x20;

            Data[12] = ShortToHex(SWNLoger);
            Data[14] = CRC_CHECk(Data, 14);

            if (SoundPort.IsOpen == true) SoundPort.Write(Data, 0, Data.Length);
            timedelay2(2000);
            return;
        }


        private byte ShortToHex(short Data)
        {
            byte[] hex = ASCIIEncoding.ASCII.GetBytes(Data.ToString());

            return hex[0];
        }

        private bool GetReceive { set; get; }
        private float SoundData = 0;
        private void SoundPortReceive(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                timedelay(30);
                int Length = SoundPort.BytesToRead;
                byte[] buffer = new byte[Length + 10];

                SoundPort.Read(buffer, 0, Length);
                                
                if (Bswa308_309 == false)
                {
                    if ((buffer[0] == STX) && (buffer[Length - 1] == ETX))
                    {
                        ushort Data = (ushort)((buffer[5] << 8) | (buffer[6] << 0));
                        SoundData = (float)Data * 0.1F;
                    }
                }
                else
                {
                    GetReceive = true;
                    //02 01 41 31 2C 31 2C 32 2C 30 36 36 2E 31 03 70 0D 0A -> DATA = 36 36 2E 31

                    //ReadByteData = Encoding.Default.GetString(buffer);
                    string ssx = "";
                    foreach (byte d in buffer) ssx += string.Format("{0:X2}, ", d);

                    ReadByteData = ssx; 

                    if ((buffer[0] == STX) && (buffer[2] == READ_COMMAND))
                    {
                        int Count = 0;
                        //1,1,2,066.1ETX
                        byte[] Data = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                        for (int i = 9; i < Length; i++)
                        {
                            if (buffer[i] == ETX) break;
                            Data[Count++] = buffer[i];
                        }

                        if (0 < Count)
                        {
                            string s = Encoding.Default.GetString(Data);
                            float Sound = 0;
                            if (0 <= s.IndexOf(','))
                            {
                                string[] sd = s.Split(',');

                                if (float.TryParse(sd[0], out Sound) == false)
                                    Sound = 0;
                                else SoundData = Sound;
                            }
                            else
                            {
                                if (float.TryParse(s, out Sound) == false)
                                    Sound = 0;
                                else SoundData = Sound;
                            }
                        }
                    }
                }
                SoundPort.DiscardInBuffer();
                SendFlag = false;
            }
            catch
            {

            }
            finally
            {
                SendFlag = false;
            }
            return;
        }

        private string ReadByteData = "";

        private void timedelay(long time)
        {
            long first;
            long last;

            first = timeGetTimems();
            last = timeGetTimems();
            do
            {
                Application.DoEvents();
                last = timeGetTimems();
            } while ((last - first) < time);
            return;
        }

        private void timedelay2(long time)
        {
            long first;
            long last;

            GetReceive = false;
            first = timeGetTimems();
            last = timeGetTimems();
            do
            {
                Application.DoEvents();
                last = timeGetTimems();
                if (GetReceive == true) return;
            } while ((last - first) < time);
            return;
        }

        public float GetSound
        {
            get { return SoundData; }
        }

        public bool Connection
        {
            get { return SoundPort.IsOpen; }
        }

        public string GetAcciiData
        {
            get { return ReadByteData; }
        }
        ~SoundMeter()
        {
            STOP_WATCH.Stop();
        }
    }
}
