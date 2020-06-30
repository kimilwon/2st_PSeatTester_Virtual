#define PROGRAM_RUNNING
/// <summary>
/// Inclusion of PEAK PCAN-Basic namespace
/// </summary>
using Peak.Can.Basic;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
//using System.Windows.Forms;
using TPCANHandle = System.UInt16;
using TPCANTimestampFD = System.UInt64;

namespace PSeatTester
{
    public class __CanControl
    {
        private MyInterface mControl = null;
        private int OkCount;
        //private int sid;
        //private short sSpeed;
        //private int WriteErrorCount;
        bool NgCountCheckFlag;

        public bool[] Open = { false, false };

        #region Structures
        /// <summary>
        /// Message Status structure used to show CAN Messages
        /// in a ListView
        /// </summary>
        private class MessageStatus
        {
            private TPCANMsg m_Msg;
            private TPCANTimestamp m_TimeStamp;
            private TPCANTimestamp m_oldTimeStamp;
            private int m_iIndex;
            private int m_Count;
            private bool m_bShowPeriod;
            private bool m_bWasChanged;

            public MessageStatus(TPCANMsg canMsg, TPCANTimestamp canTimestamp, int listIndex)
            {
                m_Msg = canMsg;
                m_TimeStamp = canTimestamp;
                m_oldTimeStamp = canTimestamp;
                m_iIndex = listIndex;
                m_Count = 1;
                m_bShowPeriod = true;
                m_bWasChanged = false;
            }

            public void Update(TPCANMsg canMsg, TPCANTimestamp canTimestamp)
            {
                m_Msg = canMsg;
                m_oldTimeStamp = m_TimeStamp;
                m_TimeStamp = canTimestamp;
                m_bWasChanged = true;
                m_Count += 1;
            }

            public TPCANMsg CANMsg
            {
                get { return m_Msg; }
            }

            public TPCANTimestamp Timestamp
            {
                get { return m_TimeStamp; }
            }

            public int Position
            {
                get { return m_iIndex; }
            }

            public string TypeString
            {
                get { return GetMsgTypeString(); }
            }

            public string IdString
            {
                get { return GetIdString(); }
            }

            public string DataString
            {
                get { return GetDataString(); }
            }

            public int Count
            {
                get { return m_Count; }
            }

            public bool ShowingPeriod
            {
                get { return m_bShowPeriod; }
                set
                {
                    if (m_bShowPeriod ^ value)
                    {
                        m_bShowPeriod = value;
                        m_bWasChanged = true;
                    }
                }
            }

            public bool MarkedAsUpdated
            {
                get { return m_bWasChanged; }
                set { m_bWasChanged = value; }
            }

            public string TimeString
            {
                get { return GetTimeString(); }
            }

            private string GetTimeString()
            {
                double fTime;

                fTime = m_TimeStamp.millis + (m_TimeStamp.micros / 1000.0);
                if (m_bShowPeriod) fTime -= (m_oldTimeStamp.millis + (m_oldTimeStamp.micros / 1000.0));
                return fTime.ToString("F1");
            }

            private string GetDataString()
            {
                string strTemp;

                strTemp = "";

                if ((m_Msg.MSGTYPE & TPCANMessageType.PCAN_MESSAGE_RTR) == TPCANMessageType.PCAN_MESSAGE_RTR)
                    return "Remote Request";
                else
                    for (int i = 0; i < m_Msg.LEN; i++)
                        strTemp += string.Format("{0:X2} ", m_Msg.DATA[i]);

                return strTemp;
            }

            private string GetIdString()
            {
                // We format the ID of the message and show it
                //
                if ((m_Msg.MSGTYPE & TPCANMessageType.PCAN_MESSAGE_EXTENDED) == TPCANMessageType.PCAN_MESSAGE_EXTENDED)
                    return string.Format("{0:X8}h", m_Msg.ID);
                else
                    return string.Format("{0:X3}h", m_Msg.ID);
            }

            private string GetMsgTypeString()
            {
                string strTemp;

                if ((m_Msg.MSGTYPE & TPCANMessageType.PCAN_MESSAGE_EXTENDED) == TPCANMessageType.PCAN_MESSAGE_EXTENDED)
                    strTemp = "EXTENDED";
                else
                    strTemp = "STANDARD";

                if ((m_Msg.MSGTYPE & TPCANMessageType.PCAN_MESSAGE_RTR) == TPCANMessageType.PCAN_MESSAGE_RTR)
                    strTemp += "/RTR";

                return strTemp;
            }

        }
        #endregion

        #region Delegates
        /// <summary>
        /// Read-Delegate Handler
        /// </summary>
        private delegate void ReadDelegateHandler();
        #endregion

        #region Members
        /// <summary>
        /// Saves the handle of a PCAN hardware
        /// </summary>
        private TPCANHandle[] m_PcanHandle = { 0, 0 };
        /// <summary>
        /// Saves the baudrate register for a conenction
        /// </summary>
        private TPCANBaudrate[] m_Baudrate = { TPCANBaudrate.PCAN_BAUD_500K, TPCANBaudrate.PCAN_BAUD_500K };
        /// <summary>
        /// Saves the type of a non-plug-and-play hardware
        /// </summary>
        private TPCANType[] m_HwType = { TPCANType.PCAN_TYPE_ISA, TPCANType.PCAN_TYPE_ISA };
        /// <summary>
        /// Stores the status of received messages for its display
        /// </summary>
        //private System.Collections.ArrayList m_LastMsgsList;
        /// <summary>
        /// Read Delegate for calling the function "ReadMessages"
        /// </summary>
        //private ReadDelegateHandler m_ReadDelegate;
        /// <summary>
        /// Receive-Event
        /// </summary>
        //private System.Threading.AutoResetEvent m_ReceiveEvent;
        /// <summary>
        /// Thread for message reading (using events)
        /// </summary>
        //private System.Threading.Thread m_ReadThread;
        /// <summary>
        /// Handles of the current available PCAN-Hardware
        /// </summary>
        private TPCANHandle[] m_HandlesArray = { 0, 0 };
        #endregion

        public __CanControl(MyInterface mControl)
        {
            //WriteErrorCount = 0;
            this.mControl = mControl;

            m_HandlesArray = new TPCANHandle[]
            {
                //PCANBasic.PCAN_ISABUS1,
                //PCANBasic.PCAN_ISABUS2,
                //PCANBasic.PCAN_ISABUS3,
                //PCANBasic.PCAN_ISABUS4,
                //PCANBasic.PCAN_ISABUS5,
                //PCANBasic.PCAN_ISABUS6,
                //PCANBasic.PCAN_ISABUS7,
                //PCANBasic.PCAN_ISABUS8,
                //PCANBasic.PCAN_DNGBUS1,
                PCANBasic.PCAN_PCIBUS1,
                PCANBasic.PCAN_PCIBUS2,
                PCANBasic.PCAN_PCIBUS3,
                PCANBasic.PCAN_PCIBUS4,
                PCANBasic.PCAN_PCIBUS5,
                PCANBasic.PCAN_PCIBUS6,
                PCANBasic.PCAN_PCIBUS7,
                PCANBasic.PCAN_PCIBUS8,
                PCANBasic.PCAN_PCIBUS9,
                PCANBasic.PCAN_PCIBUS10,
                PCANBasic.PCAN_PCIBUS11,
                PCANBasic.PCAN_PCIBUS12,
                PCANBasic.PCAN_PCIBUS13,
                PCANBasic.PCAN_PCIBUS14,
                PCANBasic.PCAN_PCIBUS15,
                PCANBasic.PCAN_PCIBUS16,
                PCANBasic.PCAN_USBBUS1,
                PCANBasic.PCAN_USBBUS2,
                PCANBasic.PCAN_USBBUS3,
                PCANBasic.PCAN_USBBUS4,
                PCANBasic.PCAN_USBBUS5,
                PCANBasic.PCAN_USBBUS6,
                PCANBasic.PCAN_USBBUS7,
                PCANBasic.PCAN_USBBUS8,
                PCANBasic.PCAN_USBBUS9,
                PCANBasic.PCAN_USBBUS10,
                PCANBasic.PCAN_USBBUS11,
                PCANBasic.PCAN_USBBUS12,
                PCANBasic.PCAN_USBBUS13,
                PCANBasic.PCAN_USBBUS14,
                PCANBasic.PCAN_USBBUS15,
                PCANBasic.PCAN_USBBUS16,
                PCANBasic.PCAN_PCCBUS1,
                PCANBasic.PCAN_PCCBUS2,
                PCANBasic.PCAN_LANBUS1,
                PCANBasic.PCAN_LANBUS2,
                PCANBasic.PCAN_LANBUS3,
                PCANBasic.PCAN_LANBUS4,
                PCANBasic.PCAN_LANBUS5,
                PCANBasic.PCAN_LANBUS6,
                PCANBasic.PCAN_LANBUS7,
                PCANBasic.PCAN_LANBUS8,
                PCANBasic.PCAN_LANBUS9,
                PCANBasic.PCAN_LANBUS10,
                PCANBasic.PCAN_LANBUS11,
                PCANBasic.PCAN_LANBUS12,
                PCANBasic.PCAN_LANBUS13,
                PCANBasic.PCAN_LANBUS14,
                PCANBasic.PCAN_LANBUS15,
                PCANBasic.PCAN_LANBUS16,
            };

            GetHwInfor();
        }

        ~__CanControl()
        {
        }

        private void ConfigureLogFile()
        {
            UInt32 iBuffer;

            // Sets the mask to catch all events
            //
            iBuffer = PCANBasic.LOG_FUNCTION_ALL;

            // Configures the log file. 
            // NOTE: The Log capability is to be used with the NONEBUS Handle. Other handle than this will 
            // cause the function fail.
            //
#if PROGRAM_RUNNING
            PCANBasic.SetValue(PCANBasic.PCAN_NONEBUS, TPCANParameter.PCAN_LOG_CONFIGURE, ref iBuffer, sizeof(UInt32));
#endif
            return;
        }

        /// <summary>
        /// Configures the PCAN-Trace file for a PCAN-Basic Channel
        /// </summary>
        private void ConfigureTraceFile(short Ch)
        {
#if PROGRAM_RUNNING
            UInt32 iBuffer;
            TPCANStatus stsResult;

            try
            {
                try
                {
                    // Configure the maximum size of a trace file to 5 megabytes
                    //
                    iBuffer = 5;
                    stsResult = PCANBasic.SetValue(m_PcanHandle[Ch], TPCANParameter.PCAN_TRACE_SIZE, ref iBuffer, sizeof(UInt32));
                    if (stsResult != TPCANStatus.PCAN_ERROR_OK) uMessageBox.Show(promptText: GetFormatedError(stsResult), title: "경고"); //MessageBox.Show(GetFormatedError(stsResult));

                    // Configure the way how trace files are created: 
                    // * Standard name is used
                    // * Existing file is ovewritten, 
                    // * Only one file is created.
                    // * Recording stopts when the file size reaches 5 megabytes.
                    //
                    iBuffer = PCANBasic.TRACE_FILE_SINGLE | PCANBasic.TRACE_FILE_OVERWRITE;
                    stsResult = PCANBasic.SetValue(m_PcanHandle[Ch], TPCANParameter.PCAN_TRACE_CONFIGURE, ref iBuffer, sizeof(UInt32));
                    if (stsResult != TPCANStatus.PCAN_ERROR_OK) uMessageBox.Show(promptText: GetFormatedError(stsResult), title: "경고"); //MessageBox.Show(GetFormatedError(stsResult));
                }
                catch (Exception Msg)
                {
                    //MessageBox.Show(Msg.Message + "\n" + Msg.StackTrace);
                    uMessageBox.Show(promptText: Msg.Message + "\n" + Msg.StackTrace, title: "경고");
                }
            }
            finally
            {
            }
#endif
            return;
        }

        /// <summary>
        /// Help Function used to get an error as text
        /// </summary>
        /// <param name="error">Error code to be translated</param>
        /// <returns>A text with the translated error</returns>


        private string GetFormatedError(TPCANStatus error)
        {
#if PROGRAM_RUNNING
            StringBuilder strTemp;

            // Creates a buffer big enough for a error-text
            //
            strTemp = new StringBuilder(256);
            // Gets the text using the GetErrorText API function
            // If the function success, the translated error is returned. If it fails,
            // a text describing the current error is returned.
            //

            try
            {
                try
                {
                    if (PCANBasic.GetErrorText(error, 0, strTemp) != TPCANStatus.PCAN_ERROR_OK) return string.Format("An error occurred. Error-code's text ({0:X}) couldn't be retrieved", error);

                }
                catch (Exception Msg)
                {
                    //MessageBox.Show(Msg.Message + "\n" + Msg.StackTrace);
                    uMessageBox.Show(promptText: Msg.Message + "\n" + Msg.StackTrace, title: "경고");
                }
            }
            finally
            {

            }
            return strTemp.ToString();
#else
            return "";
#endif
        }


        /// <summary>
        /// Gets the current status of the PCAN-Basic message filter
        /// </summary>
        /// <param name="status">Buffer to retrieve the filter status</param>
        /// <returns>If calling the function was successfull or not</returns>
        private bool GetFilterStatus(short Ch, out uint status)
        {
#if PROGRAM_RUNNING
            TPCANStatus stsResult;

            status = 0;
            try
            {
                try
                {
                    // Tries to get the sttaus of the filter for the current connected hardware
                    //
                    stsResult = PCANBasic.GetValue(m_PcanHandle[Ch], TPCANParameter.PCAN_MESSAGE_FILTER, out status, sizeof(UInt32));

                    // If it fails, a error message is shown
                    //
                    if (stsResult != TPCANStatus.PCAN_ERROR_OK)
                    {
                        //MessageBox.Show(GetFormatedError(stsResult));
                        uMessageBox.Show(promptText: GetFormatedError(stsResult), title: "경고");
                        return false;
                    }
                }
                catch (Exception Msg)
                {
                    //MessageBox.Show(Msg.Message + "\n" + Msg.StackTrace);
                    uMessageBox.Show(promptText: Msg.Message + "\n" + Msg.StackTrace, title: "경고");
                }
            }
            finally
            {
            }
#else
            status = 0;
#endif
            return true;
        }


        public bool OpenCan(short Channel, int ID, short Speed, bool m_IsFD = false)
        {
#if PROGRAM_RUNNING
            TPCANStatus stsResult;
            UInt32 iBuffer;

            //-----------------------------[ Init can board ]
            // Connects a selected PCAN-Basic channel
            //
            try
            {
                try
                {
                    //sid = ID;
                    //sSpeed = Speed;
                    //switch (ID)
                    //{
                    //    case 0: m_PcanHandle = (byte)PCANBasic.PCAN_USBBUS1; break;
                    //    case 1: m_PcanHandle = (byte)PCANBasic.PCAN_USBBUS2; break;
                    //    case 2: m_PcanHandle = (byte)PCANBasic.PCAN_USBBUS3; break;
                    //    case 3: m_PcanHandle = (byte)PCANBasic.PCAN_USBBUS4; break;
                    //    case 4: m_PcanHandle = (byte)PCANBasic.PCAN_USBBUS5; break;
                    //    case 5: m_PcanHandle = (byte)PCANBasic.PCAN_USBBUS6; break;
                    //    case 6: m_PcanHandle = (byte)PCANBasic.PCAN_USBBUS7; break;
                    //    case 7: m_PcanHandle = (byte)PCANBasic.PCAN_USBBUS8; break;
                    //    default: m_PcanHandle = (byte)PCANBasic.PCAN_USBBUS1; break;
                    //}

                    m_PcanHandle[Channel] = (byte)ID;

                    switch (Speed)
                    {
                        case 0: m_Baudrate[Channel] = TPCANBaudrate.PCAN_BAUD_5K; break;
                        case 1: m_Baudrate[Channel] = TPCANBaudrate.PCAN_BAUD_10K; break;
                        case 2: m_Baudrate[Channel] = TPCANBaudrate.PCAN_BAUD_20K; break;
                        case 3: m_Baudrate[Channel] = TPCANBaudrate.PCAN_BAUD_33K; break;
                        case 4: m_Baudrate[Channel] = TPCANBaudrate.PCAN_BAUD_47K; break;
                        case 5: m_Baudrate[Channel] = TPCANBaudrate.PCAN_BAUD_50K; break;
                        case 6: m_Baudrate[Channel] = TPCANBaudrate.PCAN_BAUD_83K; break;
                        case 7: m_Baudrate[Channel] = TPCANBaudrate.PCAN_BAUD_95K; break;
                        case 8: m_Baudrate[Channel] = TPCANBaudrate.PCAN_BAUD_100K; break;
                        case 9: m_Baudrate[Channel] = TPCANBaudrate.PCAN_BAUD_125K; break;
                        case 10: m_Baudrate[Channel] = TPCANBaudrate.PCAN_BAUD_250K; break;
                        case 11: m_Baudrate[Channel] = TPCANBaudrate.PCAN_BAUD_500K; break;
                        case 12: m_Baudrate[Channel] = TPCANBaudrate.PCAN_BAUD_800K; break;
                        case 13: m_Baudrate[Channel] = TPCANBaudrate.PCAN_BAUD_1M; break;
                    }
                    if (m_IsFD)
                    {
                        string s = "f_clock_mhz = 20,nom_brp = 5,nom_tseg1 = 2,nom_tseg2 = 1,nom_sjw = 1,data_brp = 2,data_tseg1 = 3, data_tseg2 = 1,data_sjw = 1";
                        stsResult = PCANBasic.InitializeFD(
                            m_PcanHandle[Channel],
                            s);
                    }
                    else
                    {
                        //m_HwType = TPCANType.PCAN_TYPE_ISA;

                        stsResult = PCANBasic.Initialize(
                            m_PcanHandle[Channel],
                            m_Baudrate[Channel],
                            m_HwType[Channel],
                            0x100,
                            3);
                    }
                    if (stsResult != TPCANStatus.PCAN_ERROR_OK)
                        uMessageBox.Show(promptText: GetFormatedError(stsResult), title: "경고"); //MessageBox.Show(GetFormatedError(stsResult));
                    else
                        // Prepares the PCAN-Basic's PCAN-Trace file
                        //
                        ConfigureTraceFile((short)Channel);


                    //-----------------------------[ Open can board ]
                    // Gets the current status of the message filter
                    //
                    if (!GetFilterStatus((short)Channel, out iBuffer)) return false;

                    // The filter will be full opened or complete closed
                    //
                    iBuffer = PCANBasic.PCAN_FILTER_OPEN;

                    // The filter is configured
                    //
                    stsResult = PCANBasic.SetValue(
                        m_PcanHandle[Channel],
                        TPCANParameter.PCAN_MESSAGE_FILTER,
                        ref iBuffer,
                        sizeof(UInt32));

                    // If success, an information message is written, if it is not, an error message is shown
                    //                    

                    if (stsResult == TPCANStatus.PCAN_ERROR_OK)
                        Open[Channel] = true;
                    else Open[Channel] = false;

                    if (stsResult == TPCANStatus.PCAN_ERROR_OK)
                        return true;
                    else uMessageBox.Show(promptText: GetFormatedError(stsResult), title: "경고"); //MessageBox.Show(GetFormatedError(stsResult));
                }
                catch (Exception Msg)
                {
                    //MessageBox.Show(Msg.Message + "\n" + Msg.StackTrace);
                    uMessageBox.Show(promptText: Msg.Message + "\n" + Msg.StackTrace, title: "경고"); //MessageBox.Show(GetFormatedError(stsResult));
                }
            }
            finally
            {
            }
            return false;
#else
            return true;
#endif
        }


        public bool isOpen(short Ch)
        {
            return Open[Ch];
        }

        public void CanClose(short Ch)
        {
#if PROGRAM_RUNNING
            TPCANStatus stsResult;
            UInt32 iBuffer;

            try
            {
                try
                {
                    Open[Ch] = false;
                    //-----------------------------[ Open can board ]
                    // Gets the current status of the message filter
                    //
                    if (!GetFilterStatus(Ch, out iBuffer)) return;

                    PCANBasic.Reset(m_PcanHandle[Ch]);

                    // The filter will be full opened or complete closed
                    //
                    iBuffer = PCANBasic.PCAN_FILTER_CLOSE;

                    // The filter is configured
                    //
                    stsResult = PCANBasic.SetValue(
                        m_PcanHandle[Ch],
                        TPCANParameter.PCAN_MESSAGE_FILTER,
                        ref iBuffer,
                        sizeof(UInt32));

                    // If success, an information message is written, if it is not, an error message is shown
                    //
                    if (stsResult == TPCANStatus.PCAN_ERROR_OK)
                    {
                        PCANBasic.Uninitialize(m_PcanHandle[Ch]);
                        return;
                    }

                }
                catch (Exception Msg)
                {
                    //MessageBox.Show(Msg.Message + "\n" + Msg.StackTrace);
                    uMessageBox.Show(title: "경고", promptText: Msg.Message + "\n" + Msg.StackTrace); //MessageBox.Show(GetFormatedError(stsResult));
                }
            }
            finally
            {
            }
            return;
#else
            return;
#endif
        }

        private long First = 0;
        private long Last = 0;

        public __CanMsg ReadCan(short Ch, bool FDFlag = false)
        {
            __CanMsg Msg = new __CanMsg();
            TPCANStatus stsResult;

            // We execute the "Read" function of the PCANBasic                
            //
            Msg.DATA = new byte[8];
            Msg.ID = -1;
            Msg.Length = 0;

#if PROGRAM_RUNNING
            try
            {
                try
                {
                    for (int i = 0; i < 8; i++) Msg.DATA[i] = 0x00;

                    if (Open[Ch] == true)
                    {
                        if (FDFlag == false)
                        {
                            TPCANMsg CANMsg;
                            TPCANTimestamp CANTimeStamp;

                            stsResult = PCANBasic.Read(m_PcanHandle[Ch], out CANMsg, out CANTimeStamp);
                            if (stsResult == TPCANStatus.PCAN_ERROR_OK)
                            {
                                Msg.ID = (int)CANMsg.ID;
                                Msg.Length = CANMsg.LEN;

                                for (int i = 0; i < CANMsg.LEN; i++) Msg.DATA[i] = CANMsg.DATA[i];

                                if (NgCountCheckFlag == true)
                                {
                                    if (OkCount < 100) OkCount++;
                                }
                            }

                            if (mControl.공용함수 != null)
                            {
                                Last = mControl.공용함수.timeGetTimems();
                                if (1000 <= (Last - First))
                                {
                                    CanReceiveReset(Ch);
                                    First = mControl.공용함수.timeGetTimems();
                                }
                            }
                        }
                        else
                        {
                            TPCANMsgFD CANMsg;
                            TPCANTimestampFD CANTimeStamp;


                            // We execute the "Read" function of the PCANBasic                
                            //
                            stsResult = PCANBasic.ReadFD(m_PcanHandle[Ch], out CANMsg, out CANTimeStamp);
                            if (stsResult == TPCANStatus.PCAN_ERROR_OK)
                            {
                                Msg.ID = (int)CANMsg.ID;
                                Msg.Length = CANMsg.DLC;

                                for (int i = 0; i < CANMsg.DLC; i++) Msg.DATA[i] = CANMsg.DATA[i];

                                if (NgCountCheckFlag == true)
                                {
                                    if (OkCount < 100) OkCount++;
                                }
                            }
                        }
                    }
                }
                catch (Exception fMsg)
                {
                    //MessageBox.Show(fMsg.Message + "\n" + fMsg.StackTrace);
                    uMessageBox.Show(title: "경고", promptText: fMsg.Message + "\n" + fMsg.StackTrace); //MessageBox.Show(GetFormatedError(stsResult));
                }
            }
            finally
            {
            }
#endif
            return Msg;
        }


        public void WriteCan(short Ch, __CanMsg Data, bool FDFlag = false, bool Extended = false)
        {
#if PROGRAM_RUNNING            
            //TextBox txtbCurrentTextBox;
            TPCANStatus stsResult;

            // We create a TPCANMsg message structure 
            //

            try
            {
                try
                {
                    if (Open[Ch] == true)
                    {
                        if (FDFlag == false)
                        {
                            TPCANMsg wCANMsg = new TPCANMsg();
                            wCANMsg.DATA = new byte[8];

                            // We configurate the Message.  The ID (max 0x1FF),
                            // Length of the Data, Message Type (Standard in 
                            // this example) and die data
                            //

                            wCANMsg.ID = (uint)Data.ID;
                            wCANMsg.LEN = (byte)Data.Length;
                            wCANMsg.MSGTYPE = (Extended == true) ? TPCANMessageType.PCAN_MESSAGE_EXTENDED : TPCANMessageType.PCAN_MESSAGE_STANDARD;
                            //wCANMsg.MSGTYPE = TPCANMessageType.PCAN_MESSAGE_STANDARD;
                            // If a remote frame will be sent, the data bytes are not important.
                            //
                            //if (chbRemote.Checked)
                            //    CANMsg.MSGTYPE |= TPCANMessageType.PCAN_MESSAGE_RTR;
                            //else
                            //{
                            // We get so much data as the Len of the message
                            //

                            for (int i = 0; i < wCANMsg.LEN; i++)
                            {
                                wCANMsg.DATA[i] = Data.DATA[i];
                            }
                            //}

                            // The message is sent to the configured hardware
                            //
                            //stsResult = PCANBasic.GetStatus(m_PcanHandle);

                            //if (stsResult == TPCANStatus.PCAN_ERROR_OK)
                            //{
                            stsResult = PCANBasic.Write(m_PcanHandle[Ch], ref wCANMsg);

                            // The message was successfully sent
                            //
                            if (stsResult == TPCANStatus.PCAN_ERROR_OK)
                            {
                                //WriteErrorCount = 0;
                                return;
                            }
                            else
                            {
                                /*
                                WriteErrorCount++;

                                if (WriteErrorCount < 5)
                                {
                                    PCANBasic.Uninitialize(m_PcanHandle);
                                    OpenCan(sid, sSpeed);
                                }
                                else
                                {
                                    MessageBox.Show(GetFormatedError(stsResult));// An error occurred.  We show the error.      
                                }
                                */
                            }

                            //}
                            //else
                            //{
                            //    return;
                            //}
                        }
                        else
                        {
                            TPCANMsgFD CANMsg;
                            //TextBox txtbCurrentTextBox;
                            int iLength;

                            // We create a TPCANMsgFD message structure 
                            //
                            CANMsg = new TPCANMsgFD();
                            CANMsg.DATA = new byte[64];

                            // We configurate the Message.  The ID,
                            // Length of the Data, Message Type 
                            // and the data
                            //
                            CANMsg.ID = (uint)Data.ID;
                            CANMsg.DLC = Convert.ToByte(Data.Length);
                            CANMsg.MSGTYPE = (Extended == true) ? TPCANMessageType.PCAN_MESSAGE_EXTENDED : TPCANMessageType.PCAN_MESSAGE_STANDARD;
                            CANMsg.MSGTYPE |= TPCANMessageType.PCAN_MESSAGE_FD;
                            //CANMsg.MSGTYPE |= (chbBRS.Checked) ? TPCANMessageType.PCAN_MESSAGE_BRS : TPCANMessageType.PCAN_MESSAGE_STANDARD;

                            // If a remote frame will be sent, the data bytes are not important.
                            //
                            //if (chbRemote.Checked)
                            //    CANMsg.MSGTYPE |= TPCANMessageType.PCAN_MESSAGE_RTR;
                            //else
                            //{
                            // We get so much data as the Len of the message
                            //
                            iLength = GetLengthFromDLC(CANMsg.DLC, (CANMsg.MSGTYPE & TPCANMessageType.PCAN_MESSAGE_FD) == 0);

                            //}

                            // The message is sent to the configured hardware
                            //
                            PCANBasic.WriteFD(m_PcanHandle[Ch], ref CANMsg);
                        }
                    }
                }
                catch (Exception Msg)
                {
                    //MessageBox.Show(Msg.Message + "\n" + Msg.StackTrace);
                    uMessageBox.Show(title: "경고", promptText: Msg.Message + "\n" + Msg.StackTrace); //MessageBox.Show(GetFormatedError(stsResult));
                }
            }
            finally
            {
            }
#endif
            return;
        }

        private int GetLengthFromDLC(int dlc, bool isSTD)
        {
            if (dlc <= 8)
                return dlc;

            if (isSTD)
                return 8;

            switch (dlc)
            {
                case 9: return 12;
                case 10: return 16;
                case 11: return 20;
                case 12: return 24;
                case 13: return 32;
                case 14: return 48;
                case 15: return 64;
                default: return dlc;
            }
        }

        public void ResetFailCount()
        {
            OkCount = 0;
            return;
        }

        public bool NgCheckOnOff
        {
            set { NgCountCheckFlag = value; }
            get { return NgCountCheckFlag; }
        }

        public bool CanReadFail
        {
            get
            {
                if (OkCount < 10)
                    return true;
                else return false;
            }
        }

        private List<string> CanList = new List<string>();

        public void CanReceiveReset(short Ch)
        {
#if PROGRAM_RUNNING
            TPCANStatus stsResult;

            stsResult = PCANBasic.Reset(m_PcanHandle[Ch]);
#endif
            return;
        }
        /// <summary>
        /// Gets the formated text for a PCAN-Basic channel handle
        /// </summary>
        /// <param name="handle">PCAN-Basic Handle to format</param>
        /// <returns>The formatted text for a channel</returns>
        private string FormatChannelName(TPCANHandle handle)
        {
            return FormatChannelName(handle, false, -1, null, 0);
        }

        /// <summary>
        /// Gets the formated text for a PCAN-Basic channel handle
        /// </summary>
        /// <param name="handle">PCAN-Basic Handle to format</param>
        /// <param name="isFD">If the channel is FD capable</param>
        /// <returns>The formatted text for a channel</returns>
        private string FormatChannelName(TPCANHandle handle, bool isFD, Int32 ControllerCh = -1, string HwName = null, UInt32 DeviceID = 0)
        {
            TPCANDevice devDevice;
            byte byChannel;

            // Gets the owner device and channel for a 
            // PCAN-Basic handle
            //
            if (handle < 0x100)
            {
                devDevice = (TPCANDevice)(handle >> 4);
                byChannel = (byte)(handle & 0xF);
            }
            else
            {
                devDevice = (TPCANDevice)(handle >> 8);
                byChannel = (byte)(handle & 0xFF);
            }

            // Constructs the PCAN-Basic Channel name and return it
            //
            if (ControllerCh == -1)
            {
                //if (isFD)
                //    return string.Format("{0}:FD {1} ({2:X2}h)", devDevice, byChannel, handle);
                //else
                //    return string.Format("{0} {1} ({2:X2}h)", devDevice, byChannel, handle);
                return string.Format("{0}:{1} ({2:X2}h)", HwName, byChannel, handle);
            }
            else
            {
                //if (isFD)
                //    return string.Format("{0}:FD {1} (0x{2:X2}) , {3} - Ch", devDevice, byChannel, handle, ControllerCh + 1);
                //else
                //    return string.Format("{0} {1} (0x{2:X2}) , {3} - Ch", devDevice, byChannel, handle, ControllerCh + 1);
                return string.Format("{0}:Device={3}, ID=(0x{1:X2}), Channel={2}h", HwName, handle, ControllerCh + 1, DeviceID);
            }
        }

        private void GetHwInfor()
        {
            UInt32 iBuffer;
            TPCANStatus stsResult;
            bool isFD;
            UInt32 Ch;
            UInt32 DeviceCh;
            StringBuilder HwName = new StringBuilder();
            //StringBuilder HwDevice = new StringBuilder();

            // Clears the Channel combioBox and fill it again with 
            // the PCAN-Basic handles for no-Plug&Play hardware and
            // the detected Plug&Play hardware
            //
            CanList.Clear();
            try
            {
                for (int i = 0; i < m_HandlesArray.Length; i++)
                {
                    // Includes all no-Plug&Play Handles
                    if (m_HandlesArray[i] <= PCANBasic.PCAN_DNGBUS1)
                    {
                        CanList.Add(FormatChannelName(m_HandlesArray[i]));
                    }
                    else
                    {
                        // Checks for a Plug&Play Handle and, according with the return value, includes it
                        // into the list of available hardware channels.
                        //
                        stsResult = PCANBasic.GetValue(m_HandlesArray[i], TPCANParameter.PCAN_CHANNEL_CONDITION, out iBuffer, sizeof(UInt32));
                        if ((stsResult == TPCANStatus.PCAN_ERROR_OK) && ((iBuffer & PCANBasic.PCAN_CHANNEL_AVAILABLE) == PCANBasic.PCAN_CHANNEL_AVAILABLE))
                        {
                            stsResult = PCANBasic.GetValue(m_HandlesArray[i], TPCANParameter.PCAN_CONTROLLER_NUMBER, out Ch, sizeof(UInt32));
                            stsResult = PCANBasic.GetValue(m_HandlesArray[i], TPCANParameter.PCAN_HARDWARE_NAME, HwName, 20);
                            stsResult = PCANBasic.GetValue(m_HandlesArray[i], TPCANParameter.PCAN_CHANNEL_FEATURES, out iBuffer, sizeof(UInt32));
                            isFD = (stsResult == TPCANStatus.PCAN_ERROR_OK) && ((iBuffer & PCANBasic.FEATURE_FD_CAPABLE) == PCANBasic.FEATURE_FD_CAPABLE);

                            if (isFD)
                            {
                                string s = "f_clock_mhz = 20,nom_brp = 5,nom_tseg1 = 2,nom_tseg2 = 1,nom_sjw = 1,data_brp = 2,data_tseg1 = 3, data_tseg2 = 1,data_sjw = 1";
                                stsResult = PCANBasic.InitializeFD(
                                    m_HandlesArray[i],
                                    s);
                            }
                            else
                            {
                                //m_HwType = TPCANType.PCAN_TYPE_ISA;

                                stsResult = PCANBasic.Initialize(
                                    m_HandlesArray[i],
                                    m_Baudrate[i],
                                    m_HwType[i],
                                    0x100,
                                    3);
                            }
                            stsResult = PCANBasic.GetValue(m_HandlesArray[i], TPCANParameter.PCAN_DEVICE_NUMBER, out DeviceCh, sizeof(UInt32));
                            PCANBasic.Uninitialize(m_HandlesArray[i]);
                            //CanList.Add(FormatChannelName(m_HandlesArray[i], isFD, (Int32)Ch));

                            CanList.Add(FormatChannelName(m_HandlesArray[i], isFD, (Int32)Ch, HwName.ToString(), DeviceCh));
                        }
                    }
                }

            }
            catch (DllNotFoundException)
            {
                //MessageBox.Show("Unable to find the library: PCANBasic.dll !", "Error!"/*, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error*/);
                uMessageBox.Show(title: "Error!", "Unable to find the library: PCANBasic.dll !"); //MessageBox.Show(GetFormatedError(stsResult));
                //Environment.Exit(-1);
            }
            return;
        }
        public string[] GetDevice
        {
            get
            {
                return CanList.ToArray();
            }
        }
    }
}
