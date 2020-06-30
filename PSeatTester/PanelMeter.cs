using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PSeatTester
{
    public class PanelMeter
    {
        private MyInterface mControl = null;
        private SerialPort SerialPort1 = new SerialPort();
        private SerialPort SerialPort2 = new SerialPort();
        private long AdReadTimeFirst1 = 0;
        private long AdReadTimeLast1 = 0;
        private long AdReadTimeFirst2 = 0;
        private long AdReadTimeLast2 = 0;
        private Timer timer1 = new Timer();

        public PanelMeter(MyInterface mControl)
        {
            this.mControl = mControl;

            SerialPort1.DataReceived += new SerialDataReceivedEventHandler(DataCatch1);
            SerialPort2.DataReceived += new SerialDataReceivedEventHandler(DataCatch2);
            timer1.Interval = 10;
            timer1.Enabled = true;
            timer1.Tick += new EventHandler(timer1_tick);
            return;
        }

        private void timer1_tick(object sernder, EventArgs e)
        {
            try
            {
                timer1.Enabled = false;
                if (MessageDisplayFlag1 == false)
                {
                    ADReadMessage(0);
                }
                else 
                {
                    AdReadTimeLast1 = mControl.공용함수.timeGetTimems();
                    if(150 <= (AdReadTimeLast1 - AdReadTimeFirst1))
                    {
                        MessageDisplayFlag1 = false;
                        PanelMeterReadPos1++;
                        //if (1 < PanelMeterReadPos1) PanelMeterReadPos1 = 0;
                    }
                }
                if (MessageDisplayFlag2 == false)
                {
                    ADReadMessage(1);
                }
                else
                {
                    AdReadTimeLast2 = mControl.공용함수.timeGetTimems();
                    if (150 <= (AdReadTimeLast2 - AdReadTimeFirst2))
                    {
                        MessageDisplayFlag2 = false;
                        PanelMeterReadPos2++;
                        //if (1 < PanelMeterReadPos2) PanelMeterReadPos2 = 0;
                    }
                }
            }
            catch
            {

            }
            finally
            {
                timer1.Enabled = !mControl.isExit;
            }
        }
        private bool OpenFlag = false;
        public bool Open(__Port__ sPort1, __Port__ sPort2)
        {
            bool Flag1 = false;
            bool Flag2 = false;
            if ((sPort1.Port != null) && (sPort1.Port != string.Empty))
            {
                string[] PortName = mControl.공용함수.GetComName(SerialPort.GetPortNames());

                //if (Array.IndexOf(PortName, Config.PanelMeter.Port) == 0)
                if (PortName.Contains(sPort1.Port) == true)
                {
                    if (SerialPort1.IsOpen == true) SerialPort1.Close();
                    SerialPort1.PortName = sPort1.Port;

                    switch (sPort1.Speed)
                    {
                        case 0: SerialPort1.BaudRate = 2400; break;
                        case 1: SerialPort1.BaudRate = 4800; break;
                        case 2: SerialPort1.BaudRate = 9600; break;
                        case 3: SerialPort1.BaudRate = 11400; break;
                        case 4: SerialPort1.BaudRate = 19200; break;
                        case 5: SerialPort1.BaudRate = 38400; break;
                        case 6: SerialPort1.BaudRate = 57600; break;
                        default: SerialPort1.BaudRate = 115200; break;
                    }
                }
                SerialPort1.ReadTimeout = 500;
                SerialPort1.WriteTimeout = 500;
                SerialPort1.ReceivedBytesThreshold = 13;
                SerialPort1.Open();
                if (SerialPort1.IsOpen == true)
                {
                    Flag1 = true;
                }
                else
                {
                    Flag1 = false;
                }
            }

            if ((sPort2.Port != null) && (sPort2.Port != string.Empty))
            {
                string[] PortName = mControl.공용함수.GetComName(SerialPort.GetPortNames());

                //if (Array.IndexOf(PortName, Config.PanelMeter.Port) == 0)
                if (PortName.Contains(sPort2.Port) == true)
                {
                    if (SerialPort2.IsOpen == true) SerialPort2.Close();
                    SerialPort2.PortName = sPort2.Port;

                    switch (sPort2.Speed)
                    {
                        case 0: SerialPort2.BaudRate = 2400; break;
                        case 1: SerialPort2.BaudRate = 4800; break;
                        case 2: SerialPort2.BaudRate = 9600; break;
                        case 3: SerialPort2.BaudRate = 11400; break;
                        case 4: SerialPort2.BaudRate = 19200; break;
                        case 5: SerialPort2.BaudRate = 38400; break;
                        case 6: SerialPort2.BaudRate = 57600; break;
                        default: SerialPort2.BaudRate = 115200; break;
                    }
                }
                SerialPort2.ReadTimeout = 500;
                SerialPort2.WriteTimeout = 500;
                SerialPort2.ReceivedBytesThreshold = 13;
                SerialPort2.Open();
                if (SerialPort2.IsOpen == true)
                {
                    Flag2 = true;
                }
                else
                {
                    Flag2 = false;
                }
            }
            OpenFlag = Flag1 & Flag2;
            return Flag1 & Flag2;
        }

        public bool Open(__Port__ sPort1)
        {
            bool Flag1 = false;
            bool Flag2 = false;
            if ((sPort1.Port != null) && (sPort1.Port != string.Empty))
            {
                string[] PortName = mControl.공용함수.GetComName(SerialPort.GetPortNames());

                //if (Array.IndexOf(PortName, Config.PanelMeter.Port) == 0)
                if (PortName.Contains(sPort1.Port) == true)
                {
                    if (SerialPort1.IsOpen == true) SerialPort1.Close();
                    SerialPort1.PortName = sPort1.Port;

                    switch (sPort1.Speed)
                    {
                        case 0: SerialPort1.BaudRate = 2400; break;
                        case 1: SerialPort1.BaudRate = 4800; break;
                        case 2: SerialPort1.BaudRate = 9600; break;
                        case 3: SerialPort1.BaudRate = 11400; break;
                        case 4: SerialPort1.BaudRate = 19200; break;
                        case 5: SerialPort1.BaudRate = 38400; break;
                        case 6: SerialPort1.BaudRate = 57600; break;
                        default: SerialPort1.BaudRate = 115200; break;
                    }
                }
                SerialPort1.ReadTimeout = 500;
                SerialPort1.WriteTimeout = 500;
                SerialPort1.ReceivedBytesThreshold = 13;
                SerialPort1.Open();
                if (SerialPort1.IsOpen == true)
                {
                    Flag1 = true;
                }
                else
                {
                    Flag1 = false;
                }
            }
                        
            OpenFlag = Flag1;
            return Flag1 & Flag2;
        }
        public void Close()
        {
            if (SerialPort1.IsOpen == true) SerialPort1.Close();
            OpenFlag = false;
            return;
        }
        private void DataCatch1(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                mControl.공용함수.timedelay(30);
                
                int Length = SerialPort1.BytesToRead;

                AdReadTimeFirst1 = mControl.공용함수.timeGetTimems();
                AdReadTimeLast1 = mControl.공용함수.timeGetTimems();

                if (Length == 0)
                {
                    SerialPort1.DiscardInBuffer();
                    return;
                }

                byte[] buffer = new byte[Length + 10];

                SerialPort1.Read(buffer, 0, Length);

                if (13 < Length) Length = 13;
                if ((0 < buffer[0]) && (buffer[1] == READ_COMMAND) && (0 < buffer[2]))
                {
                    float Value;
                    short Value1;
                    short Value2;
                    int Point;
                    float Point1;
                    //ushort Data;
                    ushort crc16;

                    byte crc16_high, crc16_low;

                    unchecked //overflow 검출하지 않는다. 여산중 overflow가 발생하더라도 무시한다.
                    {
                        crc16 = CRC16(buffer, Length - 2);

                        crc16_high = (byte)((crc16 >> 0) & 0x00ff);
                        crc16_low = (byte)((crc16 >> 8) & 0x00ff);

                        if (crc16_high == buffer[Length - 2] && crc16_low == buffer[Length - 1])
                        {
                            Value1 = (short)((buffer[3] << 8) & 0xff00);
                            Value2 = (short)((buffer[4] << 0) & 0x00ff);

                            Value = Value1 | Value2;

                            Point = (int)(((byte)buffer[6] << 0) & 0x00ff);
                            if (Point <= 7)
                            {
                                Point1 = (float)(Math.Pow(10.0, (double)Point));

                                if ((0 < Point1) && (Point1 < 10000.0))
                                    Value = (float)Value / Point1;
                                else Value = (float)0.0;
                            }
                            else
                            {
                                Value = (float)0.0;
                            }

                            if (mControl.GetConfig.BattID == buffer[0])
                                Batt = Value;
                            else if (mControl.GetConfig.CurrID == buffer[0])
                                PSeat = Value;
                            
                            PanelMeterReadPos1++;
                            if (1 < PanelMeterReadPos1) PanelMeterReadPos1 = 0;
                            MessageDisplayFlag1 = false;
                        }
                    }
                    return;
                }
                SerialPort1.DiscardInBuffer();
            }
            catch
            {

            }
            finally
            {
            }
            return;
        }

        private void DataCatch2(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                mControl.공용함수.timedelay(30);

                int Length = SerialPort2.BytesToRead;

                AdReadTimeFirst2 = mControl.공용함수.timeGetTimems();
                AdReadTimeLast2 = mControl.공용함수.timeGetTimems();

                if (Length == 0)
                {
                    SerialPort2.DiscardInBuffer();
                    return;
                }

                byte[] buffer = new byte[Length + 10];

                SerialPort2.Read(buffer, 0, Length);

                if (13 < Length) Length = 13;
                if ((0 < buffer[0]) && (buffer[1] == READ_COMMAND) && (0 < buffer[2]))
                {
                    float Value;
                    short Value1;
                    short Value2;
                    int Point;
                    float Point1;
                    //ushort Data;
                    ushort crc16;

                    byte crc16_high, crc16_low;

                    unchecked //overflow 검출하지 않는다. 여산중 overflow가 발생하더라도 무시한다.
                    {
                        crc16 = CRC16(buffer, Length - 2);

                        crc16_high = (byte)((crc16 >> 0) & 0x00ff);
                        crc16_low = (byte)((crc16 >> 8) & 0x00ff);

                        if (crc16_high == buffer[Length - 2] && crc16_low == buffer[Length - 1])
                        {
                            Value1 = (short)((buffer[3] << 8) & 0xff00);
                            Value2 = (short)((buffer[4] << 0) & 0x00ff);

                            Value = Value1 | Value2;

                            Point = (int)(((byte)buffer[6] << 0) & 0x00ff);
                            if (Point <= 7)
                            {
                                Point1 = (float)(Math.Pow(10.0, (double)Point));

                                if ((0 < Point1) && (Point1 < 10000.0))
                                    Value = (float)Value / Point1;
                                else Value = (float)0.0;
                            }
                            else
                            {
                                Value = (float)0.0;
                            }

                            //if (mControl.GetConfig.CURR_ID == buffer[0])
                            //    Curr = Value;
                            //else if (mControl.GetConfig.PSEAT_ID == buffer[0])
                            //    PSeat = Value;

                            PanelMeterReadPos2++;
                            if (1 < PanelMeterReadPos2) PanelMeterReadPos2 = 0;
                            MessageDisplayFlag2 = false;
                        }
                    }
                    return;
                }
                SerialPort2.DiscardInBuffer();
            }
            catch
            {

            }
            finally
            {
            }
            return;
        }

        public bool isOpen
        {
            get { return OpenFlag; }
        }

        private float Batt = 0;
        //private float NctBatt = 0;
        //private float Curr = 0;
        private float PSeat = 0;

        private short PanelMeterReadPos1 = 0;
        private short PanelMeterReadPos2 = 0;
        private const int POLYNORMAL = 0xa001;
        private const short READ_COMMAND = 4;
        private bool MessageDisplayFlag1 = false;
        private bool MessageDisplayFlag2 = false;

        /// <summary>
        /// ad 데이타 리드 메시지 전송 
        /// 데이타는 베터리 , LH /RH 전류이며 순차적으로 읽어 온다.
        /// </summary>
        private void ADReadMessage(short Mode)
        {
            byte ID = 0xff;
            byte[] Data = new byte[20];
            ushort CRC;

            try
            {
                if (Mode == 0)
                {
                    if (AdReadTimeFirst1 == 0)
                    {
                        AdReadTimeFirst1 = mControl.공용함수.timeGetTimems();
                        AdReadTimeLast1 = mControl.공용함수.timeGetTimems();
                    }

                    if ((PanelMeterReadPos1 < 0) || (1 < PanelMeterReadPos1)) PanelMeterReadPos1 = 0;
                    switch (PanelMeterReadPos1)
                    {
                        case 0:
                            ID = (byte)mControl.GetConfig.BattID;
                            break;
                        case 1:
                            ID = (byte)mControl.GetConfig.CurrID;
                            break;
                        default:
                            ID = (byte)mControl.GetConfig.BattID;                            
                            break;
                    }
                    if (SerialPort1.IsOpen == true) SerialPort1.DiscardInBuffer();                    
                }
                else
                {
                    //if (AdReadTimeFirst2 == 0)
                    //{
                    //    AdReadTimeFirst2 = mControl.공용함수.timeGetTimems();
                    //    AdReadTimeLast2 = mControl.공용함수.timeGetTimems();
                    //}

                    //if ((PanelMeterReadPos2 < 0) || (1 < PanelMeterReadPos2)) PanelMeterReadPos2 = 0;
                    //switch (PanelMeterReadPos2)
                    //{
                    //    case 0:
                    //        ID = (byte)mControl.GetConfig.CURR_ID;
                    //        break;
                    //    case 1:
                    //        ID = (byte)mControl.GetConfig.PSEAT_ID;
                    //        break;
                    //    default:
                    //        ID = (byte)mControl.GetConfig.CURR_ID;
                    //        break;
                    //}
                    //if (SerialPort2.IsOpen == true) SerialPort2.DiscardInBuffer();
                }


                if ((0 < ID) && (ID != 0xff))
                {
                    Array.Clear(Data, 0, 20);

                    Data[0] = ID;
                    Data[1] = (byte)READ_COMMAND;
                    Data[2] = 0x00; // Read Start Data Address High
                    Data[3] = 0x00; // Read Start Data Address Low
                    Data[4] = 0x00; // Read Data 개수 High
                    Data[5] = 0x04; // Read Data 개수 Low

                    CRC = CRC16(Data, 6);// 6 -> Length
                    Data[6] = (byte)((CRC >> 0) & 0x00ff);
                    Data[7] = (byte)((CRC >> 8) & 0x00ff);
                    if (Mode == 0)
                    {
                        if (SerialPort1.IsOpen == true) SerialPort1.Write(Data, 0, 8);
                    }
                    else
                    {
                        if (SerialPort2.IsOpen == true) SerialPort2.Write(Data, 0, 8);
                    }
                }
                if (Mode == 0)
                {
                    MessageDisplayFlag1 = true;
                    AdReadTimeFirst1 = mControl.공용함수.timeGetTimems();
                    AdReadTimeLast1 = mControl.공용함수.timeGetTimems();
                }
                else
                {
                    MessageDisplayFlag2 = true;
                    AdReadTimeFirst2 = mControl.공용함수.timeGetTimems();
                    AdReadTimeLast2 = mControl.공용함수.timeGetTimems();
                }
            }
            catch (Exception ex)
            {
#if PROGRAM_RUNNING
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
#else
                if (ex.Data == null) throw;
#endif
            }
            return;
        }


        //public float GetCurr
        //{
        //    get { return Curr; }
        //}
        public float GetPSeat
        {
            get { return PSeat; }
        }

        public float GetBatt
        {
            get { return Batt; }
        }
        //public float GetNct
        //{
        //    get { return NctBatt; }
        //}

        /// <summary>
        /// CRC 를 구하는 루틴 - 판넬 메타 입, 출력 관련
        /// </summary>
        /// <param name="Msg"></param>
        /// <param name="Length"></param>
        /// <returns></returns>
        private ushort CRC16(byte[] Msg, int Length)
        {
            ushort CRC;
            ushort flag;
            byte i;
            short j;

            CRC = 0xffff;

            j = 0;
            while (0 < Length)
            {
                CRC ^= Msg[j++];

                for (i = 0; i < 8; i++)
                {
                    flag = (ushort)(CRC & 0x0001);
                    CRC >>= 1;
                    if (flag != 0x00) CRC ^= POLYNORMAL;
                }
                Length--;
            }

            return CRC;
        }
    }
}
