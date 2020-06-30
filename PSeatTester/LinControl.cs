#define PROGRAM_RUNNING

using PSeatTester.Properties;
using Peak.Lin;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace PSeatTester
{
    public partial class LinControl : Form
    {
        MyInterface mControl;

        public const short MAX_DEVICE = 8;

        /// <summary>
        /// The received LIN message
        /// </summary>
        //private TLINRcvMsg m_Msg;
        /// <summary>
        /// Timestamp of a previously received message
        /// </summary>
        //private ulong m_oldTimeStamp;
        /// <summary>
        /// index of the message in the ListView component
        /// </summary>
        //private int m_iIndex;
        /// <summary>
        /// Number of LIN message received with the same frame ID
        /// </summary>
        //private int m_Count;
        /// <summary>
        /// Defines if the timestamp is displayed as a period
        /// </summary>
        //private bool m_bShowPeriod;
        /// <summary>
        /// Defines if the message has been modified and its display needs to be updated
        /// </summary>
        //private bool m_bWasChanged;


        /// <summary>
        /// Client handle
        /// </summary>
        private byte[] m_hClient = new byte[MAX_DEVICE];
        /// <summary>
        /// Hardware handle
        /// </summary>
        private ushort[] m_hHw = new ushort[MAX_DEVICE];
        /// <summary>
        /// LIN Hardware Modus (Master/Slave)
        /// </summary>
        private TLINHardwareMode[] m_HwMode = new TLINHardwareMode[MAX_DEVICE];
        /// <summary>
        /// Client filter mask
        /// </summary>
        private ulong[] m_lMask = new ulong[MAX_DEVICE];
        /// <summary>
        /// Baudrate Index of Hardware
        /// </summary>
        private ushort[] m_wBaudrate = new ushort[MAX_DEVICE];
        /// <summary>
        /// Last LIN error
        /// </summary>
        private TLINError m_LastLINErr;
        /// <summary>
        /// Constant value that indicate the mask of the client filter (64bit)
        /// </summary>
        private const ulong FRAME_FILTER_MASK = 0xFFFFFFFFFFFFFFFF;

        private CGlobalFrameTable[] m_pGFT = new CGlobalFrameTable[MAX_DEVICE];
        /// <summary>
        /// Stores the status of received messages for its display
        /// </summary>
        private System.Collections.ArrayList[] m_LastMsgsList = new System.Collections.ArrayList[MAX_DEVICE];
        private ushort LinChannel = 0;
        //private ushort LinID;

        //Lin 통신 옵션(방향)
        private List<ComboBoxItem>[] cbbDirection = new List<ComboBoxItem>[MAX_DEVICE];
        //채크섬 방식
        private List<ComboBoxItem>[] cbbCST = new List<ComboBoxItem>[MAX_DEVICE];
        //린 채널
        private List<ComboBoxItem> cbbChannel = new List<ComboBoxItem>();

        //ID List
        private List<ComboBoxItem>[] cbbID = new List<ComboBoxItem>[MAX_DEVICE];

        //public List<TLINDirection> cbbDirection = new List<TLINDirection>();
        //public List<TLINChecksumType> cbbCST = new List<TLINChecksumType>();


        public enum Direction : short
        {
            dirDisabled,
            dirPublisher,
            dirSubscriber,
            dirSubscriberAutoLength
        }

        public enum ChecksumType : short
        {
            cstCustom,
            cstClassic,
            cstEnhanced,
            cstAuto
        }


        //private bool UserMode = false;

        //        public LinControl()
        //        {
        //            RefreshHardware();

        //            for (short i = 0; i < MAX_DEVICE; i++)
        //            {
        //#if PROGRAM_RUNNING
        //                //InitializeComponent();
        //                // Create the Global Frame Table and add property change handler on its items

        //                cbbCST[i] = new List<ComboBoxItem>();
        //                cbbID[i] = new List<ComboBoxItem>();
        //                cbbDirection[i] = new List<ComboBoxItem>();
        //                m_LastMsgsList[i] = new System.Collections.ArrayList();
        //                m_HwMode[i] = new TLINHardwareMode();


        //                m_pGFT[i] = new CGlobalFrameTable(this);

        //                switch (i)
        //                {
        //                    case 0:
        //                        m_pGFT[i].OnPropertyChange += new PropertyChangeEventHandler(PropertyChange1);
        //                        break;
        //                    case 1:
        //                        m_pGFT[i].OnPropertyChange += new PropertyChangeEventHandler(PropertyChange2);
        //                        break;
        //                    case 2:
        //                        m_pGFT[i].OnPropertyChange += new PropertyChangeEventHandler(PropertyChange3);
        //                        break;
        //                    case 3:
        //                        m_pGFT[i].OnPropertyChange += new PropertyChangeEventHandler(PropertyChange4);
        //                        break;
        //                    case 4:
        //                        m_pGFT[i].OnPropertyChange += new PropertyChangeEventHandler(PropertyChange5);
        //                        break;
        //                    case 5:
        //                        m_pGFT[i].OnPropertyChange += new PropertyChangeEventHandler(PropertyChange6);
        //                        break;
        //                    case 6:
        //                        m_pGFT[i].OnPropertyChange += new PropertyChangeEventHandler(PropertyChange7);
        //                        break;
        //                    case 7:
        //                        m_pGFT[i].OnPropertyChange += new PropertyChangeEventHandler(PropertyChange8);
        //                        break;
        //                }

        //                UpdateFrameIds(i);
        //                // Populates FrameID combobox with global frame IDs
        //                cbbDirection[i].Clear();
        //                cbbDirection[i].Add(new ComboBoxItem(Resources.SLinDirectionDisabled, TLINDirection.dirDisabled));
        //                cbbDirection[i].Add(new ComboBoxItem(Resources.SLinDirectionPublisher, TLINDirection.dirPublisher));
        //                cbbDirection[i].Add(new ComboBoxItem(Resources.SLinDirectionSubscriber, TLINDirection.dirSubscriber));
        //                cbbDirection[i].Add(new ComboBoxItem(Resources.SLinDirectionAuto, TLINDirection.dirSubscriberAutoLength));
        //                // Populates ChecksumType combobox
        //                cbbCST[i].Clear();
        //                cbbCST[i].Add(new ComboBoxItem(Resources.SLinCSTAuto, TLINChecksumType.cstAuto));
        //                cbbCST[i].Add(new ComboBoxItem(Resources.SLinCSTClassic, TLINChecksumType.cstClassic));
        //                cbbCST[i].Add(new ComboBoxItem(Resources.SLinCSTEnhanced, TLINChecksumType.cstEnhanced));
        //                cbbCST[i].Add(new ComboBoxItem(Resources.SLinCSTCustom, TLINChecksumType.cstCustom));
        //#endif
        //                m_hClient[i] = 0;
        //                m_hHw[i] = 0;
        //            }
        //            return;
        //        }
        /// <summary>
        /// 전달되는 상수 값이 true 일 경우 ChecksumType = cstEnhanced , Direction = dirPublisher 로 설정 된다.
        /// </summary>
        /// <param name="UserMode"></param>
        public LinControl(bool UserMode, MyInterface mControl)
        {
            RefreshHardware();

            //            for (short i = 0; i < MAX_DEVICE; i++)
            //            {
            //#if PROGRAM_RUNNING
            //                //InitializeComponent();
            //                // Create the Global Frame Table and add property change handler on its items

            //                cbbCST[i] = new List<ComboBoxItem>();
            //                cbbID[i] = new List<ComboBoxItem>();
            //                cbbDirection[i] = new List<ComboBoxItem>();
            //                m_LastMsgsList[i] = new System.Collections.ArrayList();
            //                m_HwMode[i] = new TLINHardwareMode();

            //                m_pGFT[i] = new CGlobalFrameTable(this);
            //                switch (i)
            //                {
            //                    case 0:
            //                        m_pGFT[i].OnPropertyChange += new PropertyChangeEventHandler(PropertyChange1);
            //                        break;
            //                    case 1:
            //                        m_pGFT[i].OnPropertyChange += new PropertyChangeEventHandler(PropertyChange2);
            //                        break;
            //                    case 2:
            //                        m_pGFT[i].OnPropertyChange += new PropertyChangeEventHandler(PropertyChange3);
            //                        break;
            //                    case 3:
            //                        m_pGFT[i].OnPropertyChange += new PropertyChangeEventHandler(PropertyChange4);
            //                        break;
            //                    case 4:
            //                        m_pGFT[i].OnPropertyChange += new PropertyChangeEventHandler(PropertyChange5);
            //                        break;
            //                    case 5:
            //                        m_pGFT[i].OnPropertyChange += new PropertyChangeEventHandler(PropertyChange6);
            //                        break;
            //                    case 6:
            //                        m_pGFT[i].OnPropertyChange += new PropertyChangeEventHandler(PropertyChange7);
            //                        break;
            //                    case 7:
            //                        m_pGFT[i].OnPropertyChange += new PropertyChangeEventHandler(PropertyChange8);
            //                        break;
            //                }

            //                UpdateFrameIds(i);
            //                // Populates FrameID combobox with global frame IDs
            //                cbbDirection[i].Clear();
            //                cbbDirection[i].Add(new ComboBoxItem(Resources.SLinDirectionDisabled, TLINDirection.dirDisabled));
            //                cbbDirection[i].Add(new ComboBoxItem(Resources.SLinDirectionPublisher, TLINDirection.dirPublisher));
            //                cbbDirection[i].Add(new ComboBoxItem(Resources.SLinDirectionSubscriber, TLINDirection.dirSubscriber));
            //                cbbDirection[i].Add(new ComboBoxItem(Resources.SLinDirectionAuto, TLINDirection.dirSubscriberAutoLength));
            //                // Populates ChecksumType combobox
            //                cbbCST[i].Clear();
            //                cbbCST[i].Add(new ComboBoxItem(Resources.SLinCSTAuto, TLINChecksumType.cstAuto));
            //                cbbCST[i].Add(new ComboBoxItem(Resources.SLinCSTClassic, TLINChecksumType.cstClassic));
            //                cbbCST[i].Add(new ComboBoxItem(Resources.SLinCSTEnhanced, TLINChecksumType.cstEnhanced));
            //                cbbCST[i].Add(new ComboBoxItem(Resources.SLinCSTCustom, TLINChecksumType.cstCustom));
            //#endif
            //                m_hClient[i] = 0;
            //                m_hHw[i] = 0;
            //            }
#if PROGRAM_RUNNING
            this.mControl = mControl;
#endif
            return;
        }


        public enum HW_MODE : short
        {
            NONE = 0,
            SLAVE = 1,
            MASTER = 2
        }

        private bool[] Open = { false, false, false, false, false, false, false, false };

        public bool isOpen(short Ch)
        {
            if (Ch == -1) return false;
            else return Open[Ch];
        }

        public TLINHardwareMode GetHwMode(short Ch)
        {
            return m_HwMode[Ch];
        }


        /// <summary>
        /// Lin 통신을 오픈한다.
        /// </summary>
        /// <param name="Channel"></param>
        /// <param name="ID"></param>
        /// <param name="Master"></param>
        /// <param name="Speed"></param>
        /// <returns></returns>
        public void LinOpen(int Channel, HW_MODE HwMode, int Speed)
        {
#if PROGRAM_RUNNING
            Open[Channel] = false;

            //InitializeComponent();
            // Create the Global Frame Table and add property change handler on its items

            cbbCST[Channel] = new List<ComboBoxItem>();
            cbbID[Channel] = new List<ComboBoxItem>();
            cbbDirection[Channel] = new List<ComboBoxItem>();
            m_LastMsgsList[Channel] = new System.Collections.ArrayList();
            m_HwMode[Channel] = new TLINHardwareMode();

            m_pGFT[Channel] = new CGlobalFrameTable(this);
            switch (Channel)
            {
                case 0:
                    m_pGFT[Channel].OnPropertyChange += new PropertyChangeEventHandler(PropertyChange1);
                    break;
                case 1:
                    m_pGFT[Channel].OnPropertyChange += new PropertyChangeEventHandler(PropertyChange2);
                    break;
                case 2:
                    m_pGFT[Channel].OnPropertyChange += new PropertyChangeEventHandler(PropertyChange3);
                    break;
                //defailt:
                case 3:
                    m_pGFT[Channel].OnPropertyChange += new PropertyChangeEventHandler(PropertyChange4);
                    break;
                    //case 4:
                    //    m_pGFT[Channel].OnPropertyChange += new PropertyChangeEventHandler(PropertyChange5);
                    //    break;
                    //case 5:
                    //    m_pGFT[Channel].OnPropertyChange += new PropertyChangeEventHandler(PropertyChange6);
                    //    break;
                    //case 6:
                    //    m_pGFT[Channel].OnPropertyChange += new PropertyChangeEventHandler(PropertyChange7);
                    //    break;
                    //case 7:
                    //    m_pGFT[Channel].OnPropertyChange += new PropertyChangeEventHandler(PropertyChange8);
                    //    break;
            }

            //UpdateFrameIds((short)Channel);
            // Populates FrameID combobox with global frame IDs
            cbbDirection[Channel].Clear();
            cbbDirection[Channel].Add(new ComboBoxItem(Resources.SLinDirectionDisabled, TLINDirection.dirDisabled));
            cbbDirection[Channel].Add(new ComboBoxItem(Resources.SLinDirectionPublisher, TLINDirection.dirPublisher));
            cbbDirection[Channel].Add(new ComboBoxItem(Resources.SLinDirectionSubscriber, TLINDirection.dirSubscriber));
            cbbDirection[Channel].Add(new ComboBoxItem(Resources.SLinDirectionAuto, TLINDirection.dirSubscriberAutoLength));
            // Populates ChecksumType combobox
            cbbCST[Channel].Clear();
            cbbCST[Channel].Add(new ComboBoxItem(Resources.SLinCSTAuto, TLINChecksumType.cstAuto));
            cbbCST[Channel].Add(new ComboBoxItem(Resources.SLinCSTClassic, TLINChecksumType.cstClassic));
            cbbCST[Channel].Add(new ComboBoxItem(Resources.SLinCSTEnhanced, TLINChecksumType.cstEnhanced));
            cbbCST[Channel].Add(new ComboBoxItem(Resources.SLinCSTCustom, TLINChecksumType.cstCustom));

            m_hClient[Channel] = 0;
            m_hHw[Channel] = 0;


            m_lMask[Channel] = FRAME_FILTER_MASK;

            if (HwMode == HW_MODE.MASTER)
                m_HwMode[Channel] = TLINHardwareMode.modMaster;
            else if (HwMode == HW_MODE.SLAVE)
                m_HwMode[Channel] = TLINHardwareMode.modSlave;
            else m_HwMode[Channel] = TLINHardwareMode.modNone;

            switch (Speed)
            {
                case 0:
                    m_wBaudrate[Channel] = (ushort)2400;
                    break;
                case 1:
                    m_wBaudrate[Channel] = (ushort)9600;
                    break;
                case 2:
                    m_wBaudrate[Channel] = (ushort)10400;
                    break;
                default:
                    m_wBaudrate[Channel] = (ushort)19200;
                    break;
            }

            LinChannel = (ushort)Channel;
            //LinID = ID;

            // Populates Direction combobox
            //m_LastMsgsList = new System.Collections.ArrayList();

            if (DoLinConnect() == false) return;

            Open[Channel] = true;
#endif
            return;
        }

        //        public void LinOpen()
        //        {
        //#if PROGRAM_RUNNING
        //            Open[0] = false;
        //            m_lMask[0] = FRAME_FILTER_MASK;

        //            //if (HwMode == HW_MODE.MASTER)
        //            //    m_HwMode = TLINHardwareMode.modMaster;
        //            //else if (HwMode == HW_MODE.SLAVE)
        //            //    m_HwMode = TLINHardwareMode.modSlave;
        //            //else m_HwMode = TLINHardwareMode.modNone;

        //            //switch (Speed)
        //            //{
        //            //    case 0:
        //            //        m_wBaudrate = (ushort)2400;
        //            //        break;
        //            //    case 1:
        //            //        m_wBaudrate = (ushort)9600;
        //            //        break;
        //            //    case 2:
        //            //        m_wBaudrate = (ushort)10400;
        //            //        break;
        //            //    default:
        //            //        m_wBaudrate = (ushort)19200;
        //            //        break;
        //            //}

        //            LinChannel = 0;
        //            //LinID = ID;

        //            // Populates Direction combobox
        //            m_LastMsgsList[0] = new System.Collections.ArrayList();

        //            if (DoLinConnect() == false) return;

        //            Open[0] = true;
        //#endif
        //            return;
        //        }

        /// <summary>
        /// Connects to the hardware with the setting values
        /// from the connection groupbox.
        /// </summary>
        /// <returns>
        /// Returns true if the function finished successfull. Otherwise
        /// returns false.
        /// </returns>

        private bool DoLinConnect()
        {
#if PROGRAM_RUNNING
            bool fRet;
            ushort lwHw, lwBaud;
            int lnMode, lnCurrBaud;
            byte[] lhClients = new byte[255];
            TLINHardwareMode lHwMode;

            // initialisation
            fRet = false;

            if (m_hHw[LinChannel] != 0)
            {
                // If a connection to hardware already exits
                // disconnect this connection first.
                if (DoLinDisconnect(LinChannel) == false) return fRet;
            }


            //m_hHw = 0;

            // Get the selected Hardware handle from the comboboxItem
            lwHw = (ushort)((ComboBoxItem)cbbChannel[LinChannel]).Value;
            if (lwHw != 0)
            {
                PLinApi.IdentifyHardware(lwHw);

                if (m_hClient[LinChannel] == 0)
                {
                    // Register this application with LIN as client.
                    //if (LinChannel == 0)
                    //m_LastLINErr = PLinApi.RegisterClient(Resources.SPLinClientName, Handle, out m_hClient[LinChannel]);

                    m_hClient[LinChannel] = (byte)(LinChannel + 1);


                    switch (LinChannel)
                    {
                        case 0:
                            m_LastLINErr = PLinApi.RegisterClient(Resources.SPLinClientName, Handle, out m_hClient[LinChannel]);
                            break;
                        case 1:
                            m_LastLINErr = PLinApi.RegisterClient(Resources.SPLinClientName, Handle, out m_hClient[LinChannel]);
                            break;
                        case 2:
                            m_LastLINErr = PLinApi.RegisterClient(Resources.SPLinClientName, Handle, out m_hClient[LinChannel]);
                            break;
                        //case 3:
                        default:
                            m_LastLINErr = PLinApi.RegisterClient(Resources.SPLinClientName, Handle, out m_hClient[LinChannel]);
                            break;
                            //case 4:
                            //    m_LastLINErr = PLinApi.RegisterClient(Resources.SPLinClientName5, Handle, out m_hClient[LinChannel]);
                            //    break;
                            //case 5:
                            //    m_LastLINErr = PLinApi.RegisterClient(Resources.SPLinClientName6, Handle, out m_hClient[LinChannel]);
                            //    break;
                            //case 6:
                            //    m_LastLINErr = PLinApi.RegisterClient(Resources.SPLinClientName7, Handle, out m_hClient[LinChannel]);
                            //    break;
                            //default:
                            //    m_LastLINErr = PLinApi.RegisterClient(Resources.SPLinClientName8, Handle, out m_hClient[LinChannel]);
                            //    break;
                    }
                    //else m_LastLINErr = PLinApi.RegisterClient(Resources.SPLinClientName2, Handle, out m_hClient);
                }
                m_LastLINErr = PLinApi.GetHardwareParam(lwHw, TLINHardwareParam.hwpConnectedClients, lhClients, 255);

                // The local hardware handle is valid.
                // Get the current mode of the hardware
                PLinApi.GetHardwareParam(lwHw, TLINHardwareParam.hwpMode, out lnMode, 0);
                // Get the current baudrate of the hardware
                PLinApi.GetHardwareParam(lwHw, TLINHardwareParam.hwpBaudrate, out lnCurrBaud, 0);
                // Try to connect the application client to the
                // hardware with the local handle.
                m_LastLINErr = PLinApi.ConnectClient(m_hClient[LinChannel], lwHw);
                if (m_LastLINErr == TLINError.errOK)
                {
                    // If the connection successfull
                    // assign the local handle to the
                    // member handle.
                    m_hHw[LinChannel] = lwHw;
                    // Get the selected hardware mode.
                    //if (cbbHwMode.SelectedIndex == 1)
                    //    lHwMode = TLINHardwareMode.modMaster;
                    //else
                    //    lHwMode = TLINHardwareMode.modSlave;
                    lHwMode = m_HwMode[LinChannel];
                    // Get the selected baudrate
                    try
                    {
                        //lwBaud = Convert.ToUInt16(cbbBaudrates.Text);
                        lwBaud = m_wBaudrate[LinChannel];
                    }
                    catch
                    {
                        lwBaud = 0;
                    }

                    // Get the selected hardware channel
                    //하드웨어를 다시 초기호한다. (다른 클라이언트에 의해 하드웨어가 초기화 된 상태일 경우
                    if (((TLINHardwareMode)lnMode == TLINHardwareMode.modNone) || (Convert.ToUInt16(lnCurrBaud) != lwBaud))
                    {
                        // Only if the current hardware is not initialize
                        // try to Intialize the hardware with mode and baudrate
                        m_LastLINErr = PLinApi.InitializeHardware(m_hClient[LinChannel], m_hHw[LinChannel], lHwMode, lwBaud);
                    }
                    if (m_LastLINErr == TLINError.errOK)
                    {
                        // Assign the Hardware Mode to member attribut
                        m_HwMode[LinChannel] = lHwMode;
                        // Assign the baudrate index to member attribut
                        m_wBaudrate[LinChannel] = lwBaud;
                        // Set the client filter with the mask.
                        m_LastLINErr = PLinApi.SetClientFilter(m_hClient[LinChannel], m_hHw[LinChannel], m_lMask[LinChannel]);
                        // Read the frame table from the connected hardware.
                        ReadFrameTableFromHw();
                        // Reset the last LIN error code to default.
                        m_LastLINErr = TLINError.errOK;
                        fRet = true;
                    }
                    else
                    {
                        // An error occured while initializing hardware.
                        // Set the member variable to default.
                        m_hHw[LinChannel] = 0;
                        fRet = false;
                    }
                }
                else
                {
                    // The local hardware handle is invalid
                    // and/or an error occurs while connecting
                    // hardware with client.
                    // Set the member variable to default.
                    m_hHw[LinChannel] = 0;
                    fRet = false;
                }

                // Check if any LIN error code was received.
                if (m_LastLINErr != TLINError.errOK)
                {
                    MessageBox.Show(GetFormatedError(m_LastLINErr), "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    fRet = false;
                }

            }
            else // Should never occur
            {
                m_hHw[LinChannel] = 0; // But if it occurs, set handle to default
            }

            return fRet;
#else
            return false;
#endif
        }

        /// <summary>
        /// Disconnects an existing connection to a LIN hardware and returns
        /// true if disconnection finished succesfully or if no connection exists.
        /// Returns false if the current connection can not be disconnected.
        /// </summary>
        /// <returns>
        /// Returns true if the function finished successfull. Otherwise
        /// returns false.
        /// </returns>
        public bool DoLinDisconnect(ushort Channel)
        {
#if PROGRAM_RUNNING
            // If the application was registered with LIN as client.
            if (m_hHw[Channel] != 0)
            {
                Open[Channel] = false;
                // The client was connected to a LIN hardware.
                // Before disconnect from the hardware check
                // the connected clients and determine if the
                // hardware configuration have to reset or not.

                // Initialize the locale variables.
                bool lfOtherClient = false;
                bool lfOwnClient = false;
                byte[] lhClients = new byte[255];

                // Get the connected clients from the LIN hardware.
                m_LastLINErr = PLinApi.GetHardwareParam(m_hHw[Channel], TLINHardwareParam.hwpConnectedClients, lhClients, 255);
                if (m_LastLINErr == TLINError.errOK)
                {
                    // No errors !
                    // Check all client handles.
                    for (int i = 0; i < lhClients.Length; i++)
                    {
                        // If client handle is invalid
                        if (lhClients[i] == 0) continue;
                        // Set the boolean to true if the handle isn't the
                        // handle of this application.
                        // Even the boolean is set to true it can never
                        // set to false.
                        lfOtherClient = lfOtherClient || (lhClients[i] != m_hClient[Channel]);
                        // Set the boolean to true if the handle is the
                        // handle of this application.
                        // Even the boolean is set to true it can never
                        // set to false.
                        lfOwnClient = lfOwnClient || (lhClients[i] == m_hClient[Channel]);
                    }
                }
                // If another application is also connected to
                // the LIN hardware do not reset the configuration.
                if (lfOtherClient == false)
                {
                    // No other application connected !
                    // Reset the configuration of the LIN hardware.
                    PLinApi.ResetHardwareConfig(m_hClient[Channel], m_hHw[Channel]);
                }
                // If this application is connected to the hardware
                // then disconnect the client. Otherwise not.
                if (lfOwnClient == true)
                {
                    // Disconnect if the application was connected to a LIN hardware.
                    m_LastLINErr = PLinApi.DisconnectClient(m_hClient[Channel], m_hHw[Channel]);
                    if (m_LastLINErr == TLINError.errOK)
                    {
                        m_hClient[Channel] = 0;
                        m_hHw[Channel] = 0;
                        return true;
                    }
                    else
                    {
                        // Error while disconnecting from hardware.
                        //MessageBox.Show(GetFormatedError(m_LastLINErr), Resources.SAppCaptionError);
                        MessageBox.Show(GetFormatedError(m_LastLINErr), "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
#else 
            return true;
#endif
        }

        /// <summary>
        /// Help Function used to get an error as text
        /// </summary>
        /// <param name="error">Error code to be translated</param>
        /// <returns>A text with the translated error</returns>
        private string GetFormatedError(TLINError error)
        {
#if PROGRAM_RUNNING
            StringBuilder sErrText = new StringBuilder(255);
            // If any error are occured
            // display the error text in a message box.
            // 0x00 = Neutral
            // 0x07 = Language German
            // 0x09 = Language English
            if (PLinApi.GetErrorText(error, 0x09, sErrText, 255) != TLINError.errOK)
                return string.Format("An error occurred. Error-code's text ({0}) couldn't be retrieved", error);
            return sErrText.ToString();
#else
            return null;
#endif
        }

        /// <summary>
        /// Reads all values from the frame table of the hardware
        /// and assign it to the GlobalFrameTable. Also refresh
        /// the Global Frame Table ListView with that values.
        /// </summary>
        private void ReadFrameTableFromHw()
        {
#if PROGRAM_RUNNING
            int i, lnCnt;
            TLINFrameEntry lFrameEntry;
            TLINError lErr;
            ulong llMask;

            // Create a LIN frame entry object
            lFrameEntry = new TLINFrameEntry();
            // Get the count of Frame Definition from the
            // Global Frame Table.
            lnCnt = m_pGFT[LinChannel].Count;
            // Initialize the member attribute for the
            // client mask with 0.
            m_lMask[LinChannel] = 0;
            // Each Frame Definition
            for (i = 0; i < lnCnt; i++)
            {
                // Before a frame entry can be read from the
                // hardware, the Frame-ID of the wanted entry 
                // must be set
                lFrameEntry.FrameId = m_pGFT[LinChannel][i].IdAsByte;
                // Read the information of the specified frame entry from the hardware.
                lErr = PLinApi.GetFrameEntry(m_hHw[LinChannel], ref lFrameEntry);
                // Check the result value of the LinApi function call.
                if (lErr == TLINError.errOK)
                {
                    // LinApi function call successfull.
                    // Copy the frame entry information to the Frame Definition.
                    m_pGFT[LinChannel][i].m_nLength = Convert.ToInt32(lFrameEntry.Length);
                    m_pGFT[LinChannel][i].m_bDirection = lFrameEntry.Direction;
                    m_pGFT[LinChannel][i].m_nChecksumType = lFrameEntry.ChecksumType;
                    if (m_pGFT[LinChannel][i].Direction != TLINDirection.dirDisabled)
                    {
                        // If the direction is not disabled then set the
                        // bit in the client filter mask.
                        llMask = ((ulong)1 << i) & FRAME_FILTER_MASK;
                        m_lMask[LinChannel] = m_lMask[LinChannel] | llMask;
                    }
                }
            }
            // If the Client and Hardware handles are valid.
            if ((m_hClient[LinChannel] != 0) && (m_hHw[LinChannel] != 0))
            {
                // Set the client filter.
                PLinApi.SetClientFilter(m_hClient[LinChannel], m_hHw[LinChannel], m_lMask[LinChannel]);
            }
            // Updates the displayed frameIds
            UpdateFrameIds((short)LinChannel);
#endif
            return;
        }


        //private List<CGlobalFrameTable> cbbID = new List<CGlobalFrameTable>();

        private void UpdateFrameIds(short Channel)
        {
#if PROGRAM_RUNNING
            //ComboBoxItem lItem;
            //string lID;

            // Retrieves selected ID if it exist
            //lItem = (ComboBoxItem)cbbID.SelectedItem;
            //lID = (lItem != null) ? lItem.Text : null;

            // Clears and populates FrameID combobox with global frame IDs
            cbbID[Channel].Clear();
            for (int i = 0; i < m_pGFT[Channel].Count; i++)
            {
                // add only non disabled frames
                if (m_pGFT[Channel][i].Direction == TLINDirection.dirDisabled)
                {
                    continue;
                }
                else
                {
                    //if (UserMode == true)
                    //{
                    //    if (m_pGFT[i].ID == "30h")
                    //    {
                    //        m_pGFT[i].ChecksumType = TLINChecksumType.cstEnhanced;
                    //        m_pGFT[i].Direction = TLINDirection.dirPublisher;
                    //    }
                    //}

                    if (m_HwMode[Channel] == TLINHardwareMode.modSlave)
                    {
                        if ((m_pGFT[Channel][i].ID == "3Ch") || (m_pGFT[Channel][i].ID == "04h") || (m_pGFT[Channel][i].ID == "38h") || (m_pGFT[Channel][i].ID == "08h")/* || (m_pGFT[i].ID == "38h")*/)
                        {
                            if (m_pGFT[Channel][i].ChecksumType != TLINChecksumType.cstEnhanced) m_pGFT[Channel][i].ChecksumType = TLINChecksumType.cstEnhanced;
                            if (m_pGFT[Channel][i].Direction != TLINDirection.dirPublisher) m_pGFT[Channel][i].Direction = TLINDirection.dirPublisher;

                            if ((m_pGFT[Channel][i].ID == "3Ch") || (m_pGFT[Channel][i].ID == "38h"))
                            {
                                if (m_pGFT[Channel][i].Length != 5) m_pGFT[Channel][i].Length = 8;
                            }
                            else
                            {
                                if (m_pGFT[Channel][i].Length != 4) m_pGFT[Channel][i].Length = 2;
                            }
                        }
                    }
                    else
                    {
                        if (m_pGFT[Channel][i].ID == "25h")/* || (m_pGFT[i].ID == "34h") || (m_pGFT[i].ID == "36h") || (m_pGFT[i].ID == "38h")*/
                        {
                            if (m_pGFT[Channel][i].ChecksumType != TLINChecksumType.cstEnhanced) m_pGFT[Channel][i].ChecksumType = TLINChecksumType.cstEnhanced;
                            if (m_pGFT[Channel][i].Direction != TLINDirection.dirPublisher) m_pGFT[Channel][i].Direction = TLINDirection.dirPublisher;
                            if (m_pGFT[Channel][i].Length != 5) m_pGFT[Channel][i].Length = 5;
                        }
                        else
                        {
                            if ((m_pGFT[Channel][i].ID == "1Fh") || (m_pGFT[Channel][i].ID == "3Ch")/* || (m_pGFT[i].ID == "34h") || (m_pGFT[i].ID == "36h") || (m_pGFT[i].ID == "38h")*/)
                            {
                                if (m_pGFT[Channel][i].ChecksumType != TLINChecksumType.cstAuto) m_pGFT[Channel][i].ChecksumType = TLINChecksumType.cstAuto;
                                if (m_pGFT[Channel][i].Direction != TLINDirection.dirSubscriberAutoLength) m_pGFT[Channel][i].Direction = TLINDirection.dirSubscriberAutoLength;

                                if (m_pGFT[Channel][i].ID == "1Fh")
                                {
                                    if (m_pGFT[Channel][i].Length != 4) m_pGFT[Channel][i].Length = 4;
                                }
                                else
                                {
                                    if (m_pGFT[Channel][i].Length != 8) m_pGFT[Channel][i].Length = 8;
                                }
                            }
                        }
                    }

                    cbbID[Channel].Add(new ComboBoxItem(m_pGFT[Channel][i].ID, m_pGFT[Channel][i]));
                }
                // check if the new item was selected before the update
                //if (lID == m_pGFT[i].ID)
                //    cbbID.SelectedIndex = cbbID.Items.Count - 1;
            }
#endif
            return;
        }


        public void LinConfigSetting(short Channel)
        {
#if PROGRAM_RUNNING
            // Open the "Global Frame Table" Dialog
            Frame_Dlg dlg = new Frame_Dlg(m_pGFT[Channel]);
            dlg.ShowDialog();
            // Output filter information (as changes to Global Frame Table items modify it)
            //btnFilterQuery_Click(this, new EventArgs());
            UInt64 pRcvMask = 0;
            // Retrieves the filter corresponding to the current Client-Hardware pair
            if (m_hHw[Channel] != 0)
            {
                if (PLinApi.GetClientFilter(m_hClient[Channel], m_hHw[Channel], out pRcvMask) == TLINError.errOK)
                {
                    //IncludeTextMessage(string.Format("The Status of the filter is {0}.", Convert.ToString((long)pRcvMask, 2).PadLeft(64, '0')));
                }
            }
            // Update the available frame ids (i.e. the IDs combobox in the "write message" UI group)
            UpdateFrameIds(Channel);
#endif
            return;
        }

        /// <summary>
        /// Occurs before a property value of at least one CFrameDefinition object changes.
        /// </summary>
        /// <param name="s">The source of the event</param>
        /// <param name="e">A PropertyValueChangeEventArg that contains the event data.</param>
        private void PropertyChange1(object s, PropertyChangeEventArg e)
        {
#if PROGRAM_RUNNING            
            short Channel = 0;
            CFrameDefinition lFD;
            TLINFrameEntry lFrameEntry;
            TLINError lErr;
            ulong lMask;

            // Try to get the sender as CFrameDefinition
            lFD = (CFrameDefinition)s;
            // The sender of this event is the CFrameDefinition that
            // property should be change by User.
            if (lFD != null)
            {
                // If data length is to be set, check the value.
                if (e.Type == EProperty.Length) e.Allowed = (e.Value >= 0) && (e.Value <= 8);// Only a value between 0 and 8 are valid.
                // If DO NOT allow then return.
                if (!e.Allowed) return;

                // Only if the hardware was initialized as Slave
                // set the direction of the LIN-Frame.
                // By the Master the direction will be used with
                // the LIN_Write and do not have to set here.
                if (m_HwMode[Channel] == TLINHardwareMode.modSlave)
                {
                    // Temporary set Allowed to false is to easy
                    // return only later.
                    e.Allowed = false;

                    // Create a Frame Entry object to get
                    // and set the direction.
                    lFrameEntry = new TLINFrameEntry();
                    // Set the Frame-ID of the Frame to get and set.
                    // The ID have to set before get the entry.
                    lFrameEntry.FrameId = lFD.IdAsByte;
                    // Get the Frame Entry with the Frame-ID from
                    // the Hardware via the LinApi.
                    lErr = PLinApi.GetFrameEntry(m_hHw[Channel], ref lFrameEntry);
                    // If an error occurs do not allow to change
                    // the property and return.
                    // The Allowed parameter was set some lines before.
                    if (lErr != TLINError.errOK) return;

                    // Switch between the different kind of property types.
                    switch (e.Type)
                    {
                        // Direction property should be set
                        case EProperty.Direction:
                            lFrameEntry.Direction = (TLINDirection)e.Value;
                            break;
                        // Length property should be set
                        case EProperty.Length:
                            lFrameEntry.Length = Convert.ToByte(e.Value);
                            break;
                        // ChecksumType property should be set
                        case EProperty.ChecksumType:
                            lFrameEntry.ChecksumType = (TLINChecksumType)e.Value;
                            break;
                    }
                    lFrameEntry.Flags = PLinApi.FRAME_FLAG_RESPONSE_ENABLE;
                    lErr = PLinApi.SetFrameEntry(m_hClient[Channel], m_hHw[Channel], ref lFrameEntry);
                    // If an error occurs do not allow to change
                    // the property and return.
                    // The Allowed parameter was set some lines before.
                    if (lErr != TLINError.errOK) return;

                    // Temporary set Allowed to true for next check.
                    // The action was successfull on this line.
                    e.Allowed = true;
                }

                // If the property 'Direction' of one
                // CFrameDefinition will be changed,
                // here we need a special request to set
                // the client filter.
                if (e.Type == EProperty.Direction)
                {
                    // If the new value for the property 'Direction'
                    // should NOT be 'Disabled' check first if
                    // the CFrameDefinition is defined already with some
                    // other value then 'Disabled'.
                    if ((TLINDirection)e.Value != TLINDirection.dirDisabled)
                    {
                        if (lFD.Direction == TLINDirection.dirDisabled)
                        {
                            // If the old property of CFrameDefintion
                            // was set to 'Disabled' the new value
                            // means that the Frame-ID have to add to
                            // the client filter by the LinApi.
                            // Set the client filter.
                            // The Filter is a bit mask.
                            lMask = (ulong)1 << lFD.IdAsInt;
                            m_lMask[Channel] = m_lMask[Channel] | lMask;
                            lErr = PLinApi.SetClientFilter(m_hClient[Channel], m_hHw[Channel], m_lMask[Channel]);
                            // Only allow to change the property if the Frame-ID
                            // was added successfull to the Filter.
                            e.Allowed = lErr == TLINError.errOK;
                        }
                    }
                    else
                    {
                        // If the value of direction should set on 'disable'
                        // Remove the Frame-ID from the client filter.
                        lMask = (ulong)1 << lFD.IdAsInt;
                        m_lMask[Channel] = m_lMask[Channel] & ~lMask;
                        lErr = PLinApi.SetClientFilter(m_hClient[Channel], m_hHw[Channel], m_lMask[Channel]);
                        // Only allow to change the property if the Frame-ID
                        // was removed successfull from the Filter.
                        e.Allowed = lErr == TLINError.errOK;
                    }
                }
            }
#endif
        }


        /// <summary>
        /// Occurs before a property value of at least one CFrameDefinition object changes.
        /// </summary>
        /// <param name="s">The source of the event</param>
        /// <param name="e">A PropertyValueChangeEventArg that contains the event data.</param>
        private void PropertyChange2(object s, PropertyChangeEventArg e)
        {
#if PROGRAM_RUNNING
            short Channel = 1;
            CFrameDefinition lFD;
            TLINFrameEntry lFrameEntry;
            TLINError lErr;
            ulong lMask;

            // Try to get the sender as CFrameDefinition
            lFD = (CFrameDefinition)s;
            // The sender of this event is the CFrameDefinition that
            // property should be change by User.
            if (lFD != null)
            {
                // If data length is to be set, check the value.
                if (e.Type == EProperty.Length) e.Allowed = (e.Value >= 0) && (e.Value <= 8);// Only a value between 0 and 8 are valid.
                // If DO NOT allow then return.
                if (!e.Allowed) return;

                // Only if the hardware was initialized as Slave
                // set the direction of the LIN-Frame.
                // By the Master the direction will be used with
                // the LIN_Write and do not have to set here.
                if (m_HwMode[Channel] == TLINHardwareMode.modSlave)
                {
                    // Temporary set Allowed to false is to easy
                    // return only later.
                    e.Allowed = false;

                    // Create a Frame Entry object to get
                    // and set the direction.
                    lFrameEntry = new TLINFrameEntry();
                    // Set the Frame-ID of the Frame to get and set.
                    // The ID have to set before get the entry.
                    lFrameEntry.FrameId = lFD.IdAsByte;
                    // Get the Frame Entry with the Frame-ID from
                    // the Hardware via the LinApi.
                    lErr = PLinApi.GetFrameEntry(m_hHw[Channel], ref lFrameEntry);
                    // If an error occurs do not allow to change
                    // the property and return.
                    // The Allowed parameter was set some lines before.
                    if (lErr != TLINError.errOK) return;

                    // Switch between the different kind of property types.
                    switch (e.Type)
                    {
                        // Direction property should be set
                        case EProperty.Direction:
                            lFrameEntry.Direction = (TLINDirection)e.Value;
                            break;
                        // Length property should be set
                        case EProperty.Length:
                            lFrameEntry.Length = Convert.ToByte(e.Value);
                            break;
                        // ChecksumType property should be set
                        case EProperty.ChecksumType:
                            lFrameEntry.ChecksumType = (TLINChecksumType)e.Value;
                            break;
                    }
                    lFrameEntry.Flags = PLinApi.FRAME_FLAG_RESPONSE_ENABLE;
                    lErr = PLinApi.SetFrameEntry(m_hClient[Channel], m_hHw[Channel], ref lFrameEntry);
                    // If an error occurs do not allow to change
                    // the property and return.
                    // The Allowed parameter was set some lines before.
                    if (lErr != TLINError.errOK) return;

                    // Temporary set Allowed to true for next check.
                    // The action was successfull on this line.
                    e.Allowed = true;
                }

                // If the property 'Direction' of one
                // CFrameDefinition will be changed,
                // here we need a special request to set
                // the client filter.
                if (e.Type == EProperty.Direction)
                {
                    // If the new value for the property 'Direction'
                    // should NOT be 'Disabled' check first if
                    // the CFrameDefinition is defined already with some
                    // other value then 'Disabled'.
                    if ((TLINDirection)e.Value != TLINDirection.dirDisabled)
                    {
                        if (lFD.Direction == TLINDirection.dirDisabled)
                        {
                            // If the old property of CFrameDefintion
                            // was set to 'Disabled' the new value
                            // means that the Frame-ID have to add to
                            // the client filter by the LinApi.
                            // Set the client filter.
                            // The Filter is a bit mask.
                            lMask = (ulong)1 << lFD.IdAsInt;
                            m_lMask[Channel] = m_lMask[Channel] | lMask;
                            lErr = PLinApi.SetClientFilter(m_hClient[Channel], m_hHw[Channel], m_lMask[Channel]);
                            // Only allow to change the property if the Frame-ID
                            // was added successfull to the Filter.
                            e.Allowed = lErr == TLINError.errOK;
                        }
                    }
                    else
                    {
                        // If the value of direction should set on 'disable'
                        // Remove the Frame-ID from the client filter.
                        lMask = (ulong)1 << lFD.IdAsInt;
                        m_lMask[Channel] = m_lMask[Channel] & ~lMask;
                        lErr = PLinApi.SetClientFilter(m_hClient[Channel], m_hHw[Channel], m_lMask[Channel]);
                        // Only allow to change the property if the Frame-ID
                        // was removed successfull from the Filter.
                        e.Allowed = lErr == TLINError.errOK;
                    }
                }
            }
#endif
        }


        /// <summary>
        /// Occurs before a property value of at least one CFrameDefinition object changes.
        /// </summary>
        /// <param name="s">The source of the event</param>
        /// <param name="e">A PropertyValueChangeEventArg that contains the event data.</param>
        private void PropertyChange3(object s, PropertyChangeEventArg e)
        {
#if PROGRAM_RUNNING
            short Channel = 2;
            CFrameDefinition lFD;
            TLINFrameEntry lFrameEntry;
            TLINError lErr;
            ulong lMask;

            // Try to get the sender as CFrameDefinition
            lFD = (CFrameDefinition)s;
            // The sender of this event is the CFrameDefinition that
            // property should be change by User.
            if (lFD != null)
            {
                // If data length is to be set, check the value.
                if (e.Type == EProperty.Length) e.Allowed = (e.Value >= 0) && (e.Value <= 8);// Only a value between 0 and 8 are valid.
                // If DO NOT allow then return.
                if (!e.Allowed) return;

                // Only if the hardware was initialized as Slave
                // set the direction of the LIN-Frame.
                // By the Master the direction will be used with
                // the LIN_Write and do not have to set here.
                if (m_HwMode[Channel] == TLINHardwareMode.modSlave)
                {
                    // Temporary set Allowed to false is to easy
                    // return only later.
                    e.Allowed = false;

                    // Create a Frame Entry object to get
                    // and set the direction.
                    lFrameEntry = new TLINFrameEntry();
                    // Set the Frame-ID of the Frame to get and set.
                    // The ID have to set before get the entry.
                    lFrameEntry.FrameId = lFD.IdAsByte;
                    // Get the Frame Entry with the Frame-ID from
                    // the Hardware via the LinApi.
                    lErr = PLinApi.GetFrameEntry(m_hHw[Channel], ref lFrameEntry);
                    // If an error occurs do not allow to change
                    // the property and return.
                    // The Allowed parameter was set some lines before.
                    if (lErr != TLINError.errOK) return;

                    // Switch between the different kind of property types.
                    switch (e.Type)
                    {
                        // Direction property should be set
                        case EProperty.Direction:
                            lFrameEntry.Direction = (TLINDirection)e.Value;
                            break;
                        // Length property should be set
                        case EProperty.Length:
                            lFrameEntry.Length = Convert.ToByte(e.Value);
                            break;
                        // ChecksumType property should be set
                        case EProperty.ChecksumType:
                            lFrameEntry.ChecksumType = (TLINChecksumType)e.Value;
                            break;
                    }
                    lFrameEntry.Flags = PLinApi.FRAME_FLAG_RESPONSE_ENABLE;
                    lErr = PLinApi.SetFrameEntry(m_hClient[Channel], m_hHw[Channel], ref lFrameEntry);
                    // If an error occurs do not allow to change
                    // the property and return.
                    // The Allowed parameter was set some lines before.
                    if (lErr != TLINError.errOK) return;

                    // Temporary set Allowed to true for next check.
                    // The action was successfull on this line.
                    e.Allowed = true;
                }

                // If the property 'Direction' of one
                // CFrameDefinition will be changed,
                // here we need a special request to set
                // the client filter.
                if (e.Type == EProperty.Direction)
                {
                    // If the new value for the property 'Direction'
                    // should NOT be 'Disabled' check first if
                    // the CFrameDefinition is defined already with some
                    // other value then 'Disabled'.
                    if ((TLINDirection)e.Value != TLINDirection.dirDisabled)
                    {
                        if (lFD.Direction == TLINDirection.dirDisabled)
                        {
                            // If the old property of CFrameDefintion
                            // was set to 'Disabled' the new value
                            // means that the Frame-ID have to add to
                            // the client filter by the LinApi.
                            // Set the client filter.
                            // The Filter is a bit mask.
                            lMask = (ulong)1 << lFD.IdAsInt;
                            m_lMask[Channel] = m_lMask[Channel] | lMask;
                            lErr = PLinApi.SetClientFilter(m_hClient[Channel], m_hHw[Channel], m_lMask[Channel]);
                            // Only allow to change the property if the Frame-ID
                            // was added successfull to the Filter.
                            e.Allowed = lErr == TLINError.errOK;
                        }
                    }
                    else
                    {
                        // If the value of direction should set on 'disable'
                        // Remove the Frame-ID from the client filter.
                        lMask = (ulong)1 << lFD.IdAsInt;
                        m_lMask[Channel] = m_lMask[Channel] & ~lMask;
                        lErr = PLinApi.SetClientFilter(m_hClient[Channel], m_hHw[Channel], m_lMask[Channel]);
                        // Only allow to change the property if the Frame-ID
                        // was removed successfull from the Filter.
                        e.Allowed = lErr == TLINError.errOK;
                    }
                }
            }
#endif
        }

        /// <summary>
        /// Occurs before a property value of at least one CFrameDefinition object changes.
        /// </summary>
        /// <param name="s">The source of the event</param>
        /// <param name="e">A PropertyValueChangeEventArg that contains the event data.</param>
        private void PropertyChange4(object s, PropertyChangeEventArg e)
        {
#if PROGRAM_RUNNING
            short Channel = 3;
            CFrameDefinition lFD;
            TLINFrameEntry lFrameEntry;
            TLINError lErr;
            ulong lMask;

            // Try to get the sender as CFrameDefinition
            lFD = (CFrameDefinition)s;
            // The sender of this event is the CFrameDefinition that
            // property should be change by User.
            if (lFD != null)
            {
                // If data length is to be set, check the value.
                if (e.Type == EProperty.Length) e.Allowed = (e.Value >= 0) && (e.Value <= 8);// Only a value between 0 and 8 are valid.
                // If DO NOT allow then return.
                if (!e.Allowed) return;

                // Only if the hardware was initialized as Slave
                // set the direction of the LIN-Frame.
                // By the Master the direction will be used with
                // the LIN_Write and do not have to set here.
                if (m_HwMode[Channel] == TLINHardwareMode.modSlave)
                {
                    // Temporary set Allowed to false is to easy
                    // return only later.
                    e.Allowed = false;

                    // Create a Frame Entry object to get
                    // and set the direction.
                    lFrameEntry = new TLINFrameEntry();
                    // Set the Frame-ID of the Frame to get and set.
                    // The ID have to set before get the entry.
                    lFrameEntry.FrameId = lFD.IdAsByte;
                    // Get the Frame Entry with the Frame-ID from
                    // the Hardware via the LinApi.
                    lErr = PLinApi.GetFrameEntry(m_hHw[Channel], ref lFrameEntry);
                    // If an error occurs do not allow to change
                    // the property and return.
                    // The Allowed parameter was set some lines before.
                    if (lErr != TLINError.errOK) return;

                    // Switch between the different kind of property types.
                    switch (e.Type)
                    {
                        // Direction property should be set
                        case EProperty.Direction:
                            lFrameEntry.Direction = (TLINDirection)e.Value;
                            break;
                        // Length property should be set
                        case EProperty.Length:
                            lFrameEntry.Length = Convert.ToByte(e.Value);
                            break;
                        // ChecksumType property should be set
                        case EProperty.ChecksumType:
                            lFrameEntry.ChecksumType = (TLINChecksumType)e.Value;
                            break;
                    }
                    lFrameEntry.Flags = PLinApi.FRAME_FLAG_RESPONSE_ENABLE;
                    lErr = PLinApi.SetFrameEntry(m_hClient[Channel], m_hHw[Channel], ref lFrameEntry);
                    // If an error occurs do not allow to change
                    // the property and return.
                    // The Allowed parameter was set some lines before.
                    if (lErr != TLINError.errOK) return;

                    // Temporary set Allowed to true for next check.
                    // The action was successfull on this line.
                    e.Allowed = true;
                }

                // If the property 'Direction' of one
                // CFrameDefinition will be changed,
                // here we need a special request to set
                // the client filter.
                if (e.Type == EProperty.Direction)
                {
                    // If the new value for the property 'Direction'
                    // should NOT be 'Disabled' check first if
                    // the CFrameDefinition is defined already with some
                    // other value then 'Disabled'.
                    if ((TLINDirection)e.Value != TLINDirection.dirDisabled)
                    {
                        if (lFD.Direction == TLINDirection.dirDisabled)
                        {
                            // If the old property of CFrameDefintion
                            // was set to 'Disabled' the new value
                            // means that the Frame-ID have to add to
                            // the client filter by the LinApi.
                            // Set the client filter.
                            // The Filter is a bit mask.
                            lMask = (ulong)1 << lFD.IdAsInt;
                            m_lMask[Channel] = m_lMask[Channel] | lMask;
                            lErr = PLinApi.SetClientFilter(m_hClient[Channel], m_hHw[Channel], m_lMask[Channel]);
                            // Only allow to change the property if the Frame-ID
                            // was added successfull to the Filter.
                            e.Allowed = lErr == TLINError.errOK;
                        }
                    }
                    else
                    {
                        // If the value of direction should set on 'disable'
                        // Remove the Frame-ID from the client filter.
                        lMask = (ulong)1 << lFD.IdAsInt;
                        m_lMask[Channel] = m_lMask[Channel] & ~lMask;
                        lErr = PLinApi.SetClientFilter(m_hClient[Channel], m_hHw[Channel], m_lMask[Channel]);
                        // Only allow to change the property if the Frame-ID
                        // was removed successfull from the Filter.
                        e.Allowed = lErr == TLINError.errOK;
                    }
                }
            }
#endif
        }


        /// <summary>
        /// Occurs before a property value of at least one CFrameDefinition object changes.
        /// </summary>
        /// <param name="s">The source of the event</param>
        /// <param name="e">A PropertyValueChangeEventArg that contains the event data.</param>
        private void PropertyChange5(object s, PropertyChangeEventArg e)
        {
#if PROGRAM_RUNNING
            short Channel = 4;
            CFrameDefinition lFD;
            TLINFrameEntry lFrameEntry;
            TLINError lErr;
            ulong lMask;

            // Try to get the sender as CFrameDefinition
            lFD = (CFrameDefinition)s;
            // The sender of this event is the CFrameDefinition that
            // property should be change by User.
            if (lFD != null)
            {
                // If data length is to be set, check the value.
                if (e.Type == EProperty.Length) e.Allowed = (e.Value >= 0) && (e.Value <= 8);// Only a value between 0 and 8 are valid.
                // If DO NOT allow then return.
                if (!e.Allowed) return;

                // Only if the hardware was initialized as Slave
                // set the direction of the LIN-Frame.
                // By the Master the direction will be used with
                // the LIN_Write and do not have to set here.
                if (m_HwMode[Channel] == TLINHardwareMode.modSlave)
                {
                    // Temporary set Allowed to false is to easy
                    // return only later.
                    e.Allowed = false;

                    // Create a Frame Entry object to get
                    // and set the direction.
                    lFrameEntry = new TLINFrameEntry();
                    // Set the Frame-ID of the Frame to get and set.
                    // The ID have to set before get the entry.
                    lFrameEntry.FrameId = lFD.IdAsByte;
                    // Get the Frame Entry with the Frame-ID from
                    // the Hardware via the LinApi.
                    lErr = PLinApi.GetFrameEntry(m_hHw[Channel], ref lFrameEntry);
                    // If an error occurs do not allow to change
                    // the property and return.
                    // The Allowed parameter was set some lines before.
                    if (lErr != TLINError.errOK) return;

                    // Switch between the different kind of property types.
                    switch (e.Type)
                    {
                        // Direction property should be set
                        case EProperty.Direction:
                            lFrameEntry.Direction = (TLINDirection)e.Value;
                            break;
                        // Length property should be set
                        case EProperty.Length:
                            lFrameEntry.Length = Convert.ToByte(e.Value);
                            break;
                        // ChecksumType property should be set
                        case EProperty.ChecksumType:
                            lFrameEntry.ChecksumType = (TLINChecksumType)e.Value;
                            break;
                    }
                    lFrameEntry.Flags = PLinApi.FRAME_FLAG_RESPONSE_ENABLE;
                    lErr = PLinApi.SetFrameEntry(m_hClient[Channel], m_hHw[Channel], ref lFrameEntry);
                    // If an error occurs do not allow to change
                    // the property and return.
                    // The Allowed parameter was set some lines before.
                    if (lErr != TLINError.errOK) return;

                    // Temporary set Allowed to true for next check.
                    // The action was successfull on this line.
                    e.Allowed = true;
                }

                // If the property 'Direction' of one
                // CFrameDefinition will be changed,
                // here we need a special request to set
                // the client filter.
                if (e.Type == EProperty.Direction)
                {
                    // If the new value for the property 'Direction'
                    // should NOT be 'Disabled' check first if
                    // the CFrameDefinition is defined already with some
                    // other value then 'Disabled'.
                    if ((TLINDirection)e.Value != TLINDirection.dirDisabled)
                    {
                        if (lFD.Direction == TLINDirection.dirDisabled)
                        {
                            // If the old property of CFrameDefintion
                            // was set to 'Disabled' the new value
                            // means that the Frame-ID have to add to
                            // the client filter by the LinApi.
                            // Set the client filter.
                            // The Filter is a bit mask.
                            lMask = (ulong)1 << lFD.IdAsInt;
                            m_lMask[Channel] = m_lMask[Channel] | lMask;
                            lErr = PLinApi.SetClientFilter(m_hClient[Channel], m_hHw[Channel], m_lMask[Channel]);
                            // Only allow to change the property if the Frame-ID
                            // was added successfull to the Filter.
                            e.Allowed = lErr == TLINError.errOK;
                        }
                    }
                    else
                    {
                        // If the value of direction should set on 'disable'
                        // Remove the Frame-ID from the client filter.
                        lMask = (ulong)1 << lFD.IdAsInt;
                        m_lMask[Channel] = m_lMask[Channel] & ~lMask;
                        lErr = PLinApi.SetClientFilter(m_hClient[Channel], m_hHw[Channel], m_lMask[Channel]);
                        // Only allow to change the property if the Frame-ID
                        // was removed successfull from the Filter.
                        e.Allowed = lErr == TLINError.errOK;
                    }
                }
            }
#endif
        }


        /// <summary>
        /// Occurs before a property value of at least one CFrameDefinition object changes.
        /// </summary>
        /// <param name="s">The source of the event</param>
        /// <param name="e">A PropertyValueChangeEventArg that contains the event data.</param>
        private void PropertyChange6(object s, PropertyChangeEventArg e)
        {
#if PROGRAM_RUNNING
            short Channel = 5;
            CFrameDefinition lFD;
            TLINFrameEntry lFrameEntry;
            TLINError lErr;
            ulong lMask;

            // Try to get the sender as CFrameDefinition
            lFD = (CFrameDefinition)s;
            // The sender of this event is the CFrameDefinition that
            // property should be change by User.
            if (lFD != null)
            {
                // If data length is to be set, check the value.
                if (e.Type == EProperty.Length) e.Allowed = (e.Value >= 0) && (e.Value <= 8);// Only a value between 0 and 8 are valid.
                // If DO NOT allow then return.
                if (!e.Allowed) return;

                // Only if the hardware was initialized as Slave
                // set the direction of the LIN-Frame.
                // By the Master the direction will be used with
                // the LIN_Write and do not have to set here.
                if (m_HwMode[Channel] == TLINHardwareMode.modSlave)
                {
                    // Temporary set Allowed to false is to easy
                    // return only later.
                    e.Allowed = false;

                    // Create a Frame Entry object to get
                    // and set the direction.
                    lFrameEntry = new TLINFrameEntry();
                    // Set the Frame-ID of the Frame to get and set.
                    // The ID have to set before get the entry.
                    lFrameEntry.FrameId = lFD.IdAsByte;
                    // Get the Frame Entry with the Frame-ID from
                    // the Hardware via the LinApi.
                    lErr = PLinApi.GetFrameEntry(m_hHw[Channel], ref lFrameEntry);
                    // If an error occurs do not allow to change
                    // the property and return.
                    // The Allowed parameter was set some lines before.
                    if (lErr != TLINError.errOK) return;

                    // Switch between the different kind of property types.
                    switch (e.Type)
                    {
                        // Direction property should be set
                        case EProperty.Direction:
                            lFrameEntry.Direction = (TLINDirection)e.Value;
                            break;
                        // Length property should be set
                        case EProperty.Length:
                            lFrameEntry.Length = Convert.ToByte(e.Value);
                            break;
                        // ChecksumType property should be set
                        case EProperty.ChecksumType:
                            lFrameEntry.ChecksumType = (TLINChecksumType)e.Value;
                            break;
                    }
                    lFrameEntry.Flags = PLinApi.FRAME_FLAG_RESPONSE_ENABLE;
                    lErr = PLinApi.SetFrameEntry(m_hClient[Channel], m_hHw[Channel], ref lFrameEntry);
                    // If an error occurs do not allow to change
                    // the property and return.
                    // The Allowed parameter was set some lines before.
                    if (lErr != TLINError.errOK) return;

                    // Temporary set Allowed to true for next check.
                    // The action was successfull on this line.
                    e.Allowed = true;
                }

                // If the property 'Direction' of one
                // CFrameDefinition will be changed,
                // here we need a special request to set
                // the client filter.
                if (e.Type == EProperty.Direction)
                {
                    // If the new value for the property 'Direction'
                    // should NOT be 'Disabled' check first if
                    // the CFrameDefinition is defined already with some
                    // other value then 'Disabled'.
                    if ((TLINDirection)e.Value != TLINDirection.dirDisabled)
                    {
                        if (lFD.Direction == TLINDirection.dirDisabled)
                        {
                            // If the old property of CFrameDefintion
                            // was set to 'Disabled' the new value
                            // means that the Frame-ID have to add to
                            // the client filter by the LinApi.
                            // Set the client filter.
                            // The Filter is a bit mask.
                            lMask = (ulong)1 << lFD.IdAsInt;
                            m_lMask[Channel] = m_lMask[Channel] | lMask;
                            lErr = PLinApi.SetClientFilter(m_hClient[Channel], m_hHw[Channel], m_lMask[Channel]);
                            // Only allow to change the property if the Frame-ID
                            // was added successfull to the Filter.
                            e.Allowed = lErr == TLINError.errOK;
                        }
                    }
                    else
                    {
                        // If the value of direction should set on 'disable'
                        // Remove the Frame-ID from the client filter.
                        lMask = (ulong)1 << lFD.IdAsInt;
                        m_lMask[Channel] = m_lMask[Channel] & ~lMask;
                        lErr = PLinApi.SetClientFilter(m_hClient[Channel], m_hHw[Channel], m_lMask[Channel]);
                        // Only allow to change the property if the Frame-ID
                        // was removed successfull from the Filter.
                        e.Allowed = lErr == TLINError.errOK;
                    }
                }
            }
#endif
        }


        /// <summary>
        /// Occurs before a property value of at least one CFrameDefinition object changes.
        /// </summary>
        /// <param name="s">The source of the event</param>
        /// <param name="e">A PropertyValueChangeEventArg that contains the event data.</param>
        private void PropertyChange7(object s, PropertyChangeEventArg e)
        {
#if PROGRAM_RUNNING
            short Channel = 6;
            CFrameDefinition lFD;
            TLINFrameEntry lFrameEntry;
            TLINError lErr;
            ulong lMask;

            // Try to get the sender as CFrameDefinition
            lFD = (CFrameDefinition)s;
            // The sender of this event is the CFrameDefinition that
            // property should be change by User.
            if (lFD != null)
            {
                // If data length is to be set, check the value.
                if (e.Type == EProperty.Length) e.Allowed = (e.Value >= 0) && (e.Value <= 8);// Only a value between 0 and 8 are valid.
                // If DO NOT allow then return.
                if (!e.Allowed) return;

                // Only if the hardware was initialized as Slave
                // set the direction of the LIN-Frame.
                // By the Master the direction will be used with
                // the LIN_Write and do not have to set here.
                if (m_HwMode[Channel] == TLINHardwareMode.modSlave)
                {
                    // Temporary set Allowed to false is to easy
                    // return only later.
                    e.Allowed = false;

                    // Create a Frame Entry object to get
                    // and set the direction.
                    lFrameEntry = new TLINFrameEntry();
                    // Set the Frame-ID of the Frame to get and set.
                    // The ID have to set before get the entry.
                    lFrameEntry.FrameId = lFD.IdAsByte;
                    // Get the Frame Entry with the Frame-ID from
                    // the Hardware via the LinApi.
                    lErr = PLinApi.GetFrameEntry(m_hHw[Channel], ref lFrameEntry);
                    // If an error occurs do not allow to change
                    // the property and return.
                    // The Allowed parameter was set some lines before.
                    if (lErr != TLINError.errOK) return;

                    // Switch between the different kind of property types.
                    switch (e.Type)
                    {
                        // Direction property should be set
                        case EProperty.Direction:
                            lFrameEntry.Direction = (TLINDirection)e.Value;
                            break;
                        // Length property should be set
                        case EProperty.Length:
                            lFrameEntry.Length = Convert.ToByte(e.Value);
                            break;
                        // ChecksumType property should be set
                        case EProperty.ChecksumType:
                            lFrameEntry.ChecksumType = (TLINChecksumType)e.Value;
                            break;
                    }
                    lFrameEntry.Flags = PLinApi.FRAME_FLAG_RESPONSE_ENABLE;
                    lErr = PLinApi.SetFrameEntry(m_hClient[Channel], m_hHw[Channel], ref lFrameEntry);
                    // If an error occurs do not allow to change
                    // the property and return.
                    // The Allowed parameter was set some lines before.
                    if (lErr != TLINError.errOK) return;

                    // Temporary set Allowed to true for next check.
                    // The action was successfull on this line.
                    e.Allowed = true;
                }

                // If the property 'Direction' of one
                // CFrameDefinition will be changed,
                // here we need a special request to set
                // the client filter.
                if (e.Type == EProperty.Direction)
                {
                    // If the new value for the property 'Direction'
                    // should NOT be 'Disabled' check first if
                    // the CFrameDefinition is defined already with some
                    // other value then 'Disabled'.
                    if ((TLINDirection)e.Value != TLINDirection.dirDisabled)
                    {
                        if (lFD.Direction == TLINDirection.dirDisabled)
                        {
                            // If the old property of CFrameDefintion
                            // was set to 'Disabled' the new value
                            // means that the Frame-ID have to add to
                            // the client filter by the LinApi.
                            // Set the client filter.
                            // The Filter is a bit mask.
                            lMask = (ulong)1 << lFD.IdAsInt;
                            m_lMask[Channel] = m_lMask[Channel] | lMask;
                            lErr = PLinApi.SetClientFilter(m_hClient[Channel], m_hHw[Channel], m_lMask[Channel]);
                            // Only allow to change the property if the Frame-ID
                            // was added successfull to the Filter.
                            e.Allowed = lErr == TLINError.errOK;
                        }
                    }
                    else
                    {
                        // If the value of direction should set on 'disable'
                        // Remove the Frame-ID from the client filter.
                        lMask = (ulong)1 << lFD.IdAsInt;
                        m_lMask[Channel] = m_lMask[Channel] & ~lMask;
                        lErr = PLinApi.SetClientFilter(m_hClient[Channel], m_hHw[Channel], m_lMask[Channel]);
                        // Only allow to change the property if the Frame-ID
                        // was removed successfull from the Filter.
                        e.Allowed = lErr == TLINError.errOK;
                    }
                }
            }
#endif
        }


        /// <summary>
        /// Occurs before a property value of at least one CFrameDefinition object changes.
        /// </summary>
        /// <param name="s">The source of the event</param>
        /// <param name="e">A PropertyValueChangeEventArg that contains the event data.</param>
        private void PropertyChange8(object s, PropertyChangeEventArg e)
        {
#if PROGRAM_RUNNING
            short Channel = 7;
            CFrameDefinition lFD;
            TLINFrameEntry lFrameEntry;
            TLINError lErr;
            ulong lMask;

            // Try to get the sender as CFrameDefinition
            lFD = (CFrameDefinition)s;
            // The sender of this event is the CFrameDefinition that
            // property should be change by User.
            if (lFD != null)
            {
                // If data length is to be set, check the value.
                if (e.Type == EProperty.Length) e.Allowed = (e.Value >= 0) && (e.Value <= 8);// Only a value between 0 and 8 are valid.
                // If DO NOT allow then return.
                if (!e.Allowed) return;

                // Only if the hardware was initialized as Slave
                // set the direction of the LIN-Frame.
                // By the Master the direction will be used with
                // the LIN_Write and do not have to set here.
                if (m_HwMode[Channel] == TLINHardwareMode.modSlave)
                {
                    // Temporary set Allowed to false is to easy
                    // return only later.
                    e.Allowed = false;

                    // Create a Frame Entry object to get
                    // and set the direction.
                    lFrameEntry = new TLINFrameEntry();
                    // Set the Frame-ID of the Frame to get and set.
                    // The ID have to set before get the entry.
                    lFrameEntry.FrameId = lFD.IdAsByte;
                    // Get the Frame Entry with the Frame-ID from
                    // the Hardware via the LinApi.
                    lErr = PLinApi.GetFrameEntry(m_hHw[Channel], ref lFrameEntry);
                    // If an error occurs do not allow to change
                    // the property and return.
                    // The Allowed parameter was set some lines before.
                    if (lErr != TLINError.errOK) return;

                    // Switch between the different kind of property types.
                    switch (e.Type)
                    {
                        // Direction property should be set
                        case EProperty.Direction:
                            lFrameEntry.Direction = (TLINDirection)e.Value;
                            break;
                        // Length property should be set
                        case EProperty.Length:
                            lFrameEntry.Length = Convert.ToByte(e.Value);
                            break;
                        // ChecksumType property should be set
                        case EProperty.ChecksumType:
                            lFrameEntry.ChecksumType = (TLINChecksumType)e.Value;
                            break;
                    }
                    lFrameEntry.Flags = PLinApi.FRAME_FLAG_RESPONSE_ENABLE;
                    lErr = PLinApi.SetFrameEntry(m_hClient[Channel], m_hHw[Channel], ref lFrameEntry);
                    // If an error occurs do not allow to change
                    // the property and return.
                    // The Allowed parameter was set some lines before.
                    if (lErr != TLINError.errOK) return;

                    // Temporary set Allowed to true for next check.
                    // The action was successfull on this line.
                    e.Allowed = true;
                }

                // If the property 'Direction' of one
                // CFrameDefinition will be changed,
                // here we need a special request to set
                // the client filter.
                if (e.Type == EProperty.Direction)
                {
                    // If the new value for the property 'Direction'
                    // should NOT be 'Disabled' check first if
                    // the CFrameDefinition is defined already with some
                    // other value then 'Disabled'.
                    if ((TLINDirection)e.Value != TLINDirection.dirDisabled)
                    {
                        if (lFD.Direction == TLINDirection.dirDisabled)
                        {
                            // If the old property of CFrameDefintion
                            // was set to 'Disabled' the new value
                            // means that the Frame-ID have to add to
                            // the client filter by the LinApi.
                            // Set the client filter.
                            // The Filter is a bit mask.
                            lMask = (ulong)1 << lFD.IdAsInt;
                            m_lMask[Channel] = m_lMask[Channel] | lMask;
                            lErr = PLinApi.SetClientFilter(m_hClient[Channel], m_hHw[Channel], m_lMask[Channel]);
                            // Only allow to change the property if the Frame-ID
                            // was added successfull to the Filter.
                            e.Allowed = lErr == TLINError.errOK;
                        }
                    }
                    else
                    {
                        // If the value of direction should set on 'disable'
                        // Remove the Frame-ID from the client filter.
                        lMask = (ulong)1 << lFD.IdAsInt;
                        m_lMask[Channel] = m_lMask[Channel] & ~lMask;
                        lErr = PLinApi.SetClientFilter(m_hClient[Channel], m_hHw[Channel], m_lMask[Channel]);
                        // Only allow to change the property if the Frame-ID
                        // was removed successfull from the Filter.
                        e.Allowed = lErr == TLINError.errOK;
                    }
                }
            }
#endif
        }

        /// <summary>
        /// Updates the combobox 'cbbChannel' with currently available hardwares
        /// </summary>
        private void RefreshHardware()
        {
#if PROGRAM_RUNNING
            byte i;
            ushort[] lwHwHandles;
            ushort lwBuffSize, lwCount;
            int lnHwType, lnDevNo, lnChannel, lnMode, HwId;
            TLINError lLINErr;
            ushort lwHw;
            String str;

            // Get the buffer length needed...
            lwCount = 0;
            lLINErr = PLinApi.GetAvailableHardware(new ushort[0], 0, out lwCount);

            // use default value if either no hw is connected or an unexpected error occured
            if (lwCount == 0) lwCount = 16;
            lwHwHandles = new ushort[lwCount];
            lwBuffSize = Convert.ToUInt16(lwCount * sizeof(ushort));

            // Get all available LIN hardware.
            lLINErr = PLinApi.GetAvailableHardware(lwHwHandles, lwBuffSize, out lwCount);
            if (lLINErr == TLINError.errOK)
            {
                cbbChannel.Clear();
                // If no error occurs
                if (lwCount == 0)
                {
                    // No LIN hardware was found.
                    // Show an empty entry
                    lwHw = 0;
                    cbbChannel.Add(new ComboBoxItem(Resources.SHardwareNothingFound, (ushort)0));
                }
                // For each founded LIN hardware
                for (i = 0; i < lwCount; i++)
                {
                    // Get the handle of the hardware.
                    lwHw = lwHwHandles[i];
                    // Read the type of the hardware with the handle lwHw.
                    PLinApi.GetHardwareParam(lwHw, TLINHardwareParam.hwpType, out lnHwType, 0);
                    // Read the device number of the hardware with the handle lwHw.
                    PLinApi.GetHardwareParam(lwHw, TLINHardwareParam.hwpDeviceNumber, out lnDevNo, 0);
                    // Read the channel number of the hardware with the handle lwHw.
                    PLinApi.GetHardwareParam(lwHw, TLINHardwareParam.hwpChannelNumber, out lnChannel, 0);
                    // Read the mode of the hardware with the handle lwHw (Master, Slave or None).
                    PLinApi.GetHardwareParam(lwHw, TLINHardwareParam.hwpMode, out lnMode, 0);

                    PLinApi.GetHardwareParam(lwHw, TLINHardwareParam.hwpIdNumber, out HwId, 0);

                    // Create a comboboxItem
                    // If the hardware type is a knowing hardware
                    // show the name of that in the label of the entry.
                    switch (lnHwType)
                    {
                        case PLinApi.LIN_HW_TYPE_USB_PRO:
                            str = Resources.SHardwareTypeLIN;
                            break;
                        case PLinApi.LIN_HW_TYPE_USB_PRO_FD:
                            str = Resources.SHardwareTypeLINFD;
                            break;
                        case PLinApi.LIN_HW_TYPE_PLIN_USB:
                            str = Resources.SHardwarePLIN;
                            break;
                        default:
                            // Show as unknown hardware
                            str = Resources.SHardwareTypeUnkown;
                            break;
                    }
                    str = string.Format("{0} - dev. {1} - ID, chan. {2}", str, HwId, lnChannel);
                    cbbChannel.Add(new ComboBoxItem(str, lwHw));

                }
            }
            else
            {
                MessageBox.Show(GetFormatedError(lLINErr), "Error");
            }
#endif
            return;
        }

        public string[] GetDevice
        {
            get
            {
                string[] Value = new string[cbbChannel.Count];
                int Cnt = 0;

                foreach (ComboBoxItem Item in cbbChannel)
                {
                    Value[Cnt++] = Item.Text;
                }
                return Value;
            }
        }

        private void LinControl_FormClosing(object sender, FormClosingEventArgs e)
        {
#if PROGRAM_RUNNING
            for (short i = 0; i < MAX_DEVICE; i++)
            {
                if (m_hClient[i] != 0)
                {
                    DoLinDisconnect((ushort)i);
                    m_hHw[i] = 0;
                    // Unregister the application
                    PLinApi.RemoveClient(m_hClient[i]);
                    m_hClient[i] = 0;
                }
            }
#endif
            return;
        }

        public void LinClose()
        {
#if PROGRAM_RUNNING
            for (short i = 0; i < MAX_DEVICE; i++)
            {
                if (m_hClient[i] != 0)
                {
                    DoLinDisconnect((ushort)i);
                    m_hHw[i] = 0;
                    // Unregister the application
                    PLinApi.RemoveClient(m_hClient[i]);
                    m_hClient[i] = 0;
                }
            }
            this.Close();
#endif
            return;

        }

        /// <summary>
        /// Function for reading PLIN messages
        /// </summary>
        public void ReadMessages(short Channel)
        {
#if PROGRAM_RUNNING
            TLINRcvMsg lpMsg;

            // We read at least one time the queue looking for messages.
            // If a message is found, we look again trying to find more.
            // If the queue is empty or an error occurs, we get out from
            // the dowhile statement.
            //	
            do
            {
                m_LastLINErr = PLinApi.Read(m_hClient[Channel], out lpMsg);
                // If at least one Frame is received by the LinApi.
                // Check if the received frame is a standard type.
                // If it is not a standard type than ignore it.
                if (lpMsg.Type != TLINMsgType.mstStandard) continue;
                if (m_LastLINErr == TLINError.errOK) ProcessMessage(Channel: Channel, linMsg: lpMsg);
            } while (!Convert.ToBoolean(m_LastLINErr & TLINError.errRcvQueueEmpty));
#endif
            return;
        }

        public void ReadMultiMessage(short Channel)
        {
#if PROGRAM_RUNNING
            TLINRcvMsg[] lpMsg = new TLINRcvMsg[10];
            int rCount = 0;

            do
            {
                m_LastLINErr = PLinApi.ReadMulti(m_hClient[Channel], lpMsg, 10, out rCount);
                // If at least one Frame is received by the LinApi.
                // Check if the received frame is a standard type.
                // If it is not a standard type than ignore it.                
            } while (!Convert.ToBoolean(m_LastLINErr & TLINError.errRcvQueueEmpty));

            if (rCount == 0) return;

            for (int i = 0; i < rCount; i++)
            {
                if (lpMsg[i].Type != TLINMsgType.mstStandard) continue;
                if (m_LastLINErr == TLINError.errOK) ProcessMessage(Channel: Channel, linMsg: lpMsg[i]);
            }
#endif
            return;
        }

        /// <summary>
        /// Processes a received message, in order to show it in the Message-ListView
        /// </summary>
        /// <param name="linMsg">The received PLIN message</param>
        private void ProcessMessage(TLINRcvMsg linMsg, short Channel)
        {
#if PROGRAM_RUNNING
            // We search if a message (Same ID and Type) has 
            // already been received or if this is a new message

            foreach (MessageStatus msg in m_LastMsgsList[Channel])
            {
                if (msg.LINMsg.FrameId == linMsg.FrameId)
                {
                    // Modify the message and exit
                    //
                    msg.Update(linMsg);
                    return;
                }
            }
            // Message not found. It will be created
            //
            InsertMsgEntry(Channel, linMsg);
#endif
            return;
        }

        public System.Collections.ArrayList GetLinData(short Channel)
        {
            return m_LastMsgsList[Channel];
        }

        public void SetLinData(short Channel, System.Collections.ArrayList value)
        {
            m_LastMsgsList[Channel] = value;
            return;
        }


        public __CanMsg GetLin(short Channel)
        {
            __CanMsg Msg = new __CanMsg()
            {
                ID = 0,
                Length = 0,
                DATA = new byte[8]
            };

            if (0 < m_LastMsgsList[Channel].Count)
            {
                MessageStatus Lmsg = (MessageStatus)m_LastMsgsList[Channel][0];

                Msg.ID = (int)Lmsg.LINMsg.FrameId;
                Msg.Length = Lmsg.LINMsg.Length;
                Array.Copy(Lmsg.LINMsg.Data, Msg.DATA, 8);
                m_LastMsgsList[Channel].RemoveAt(0);
            }
            else
            {
                Array.Clear(Msg.DATA, 0, 8);
            }
            return Msg;
        }

        public bool isData(short Channel)
        {
            if (0 < m_LastMsgsList[Channel].Count)
                return true;
            else return false;
        }


        /// <summary>
        /// Inserts a new entry for a new message in the Message-ListView
        /// </summary>
        /// <param name="newMsg">The messasge to be inserted</param>
        private void InsertMsgEntry(short Channel, TLINRcvMsg newMsg)
        {
#if PROGRAM_RUNNING
            MessageStatus msgStsCurrentMsg;
            //string strId;

            // We add this status in the last message list
            //
            //msgStsCurrentMsg = new MessageStatus(newMsg, lstMessages.Items.Count);
            msgStsCurrentMsg = new MessageStatus(newMsg, 20);
            m_LastMsgsList[Channel].Add(msgStsCurrentMsg);

            //// Search and retrieve the ID in the global frame table associated with the frame Protected-ID
            ////strId = "";
            //for (int i = 0; i < m_pGFT.Count; i++)
            //{
            //    if (msgStsCurrentMsg.LINMsg.FrameId == m_pGFT[i].ProtectedIdAsInt)
            //    {
            //        strId = m_pGFT[i].ID;
            //        break;
            //    }
            //}
            //// Add the new ListView Item with the ID of the message
            ////	
            //lviCurrentItem = lstMessages.Items.Add(strId);
            //// We set the length of the message
            ////
            //lviCurrentItem.SubItems.Add(msgStsCurrentMsg.LINMsg.Length.ToString());
            //// We set the data of the message. 	
            ////
            //lviCurrentItem.SubItems.Add(msgStsCurrentMsg.DataString);
            //// we set the message count message (this is the First, so count is 1)            
            ////
            //lviCurrentItem.SubItems.Add(msgStsCurrentMsg.Count.ToString());
            //// Add time stamp information if needed
            ////
            //lviCurrentItem.SubItems.Add(msgStsCurrentMsg.TimeString);
            //// We set the direction of the message
            ////
            //lviCurrentItem.SubItems.Add(msgStsCurrentMsg.DirectionString);
            //// We set the error of the message
            ////
            //lviCurrentItem.SubItems.Add(msgStsCurrentMsg.ErrorString);
            //// We set the CST of the message
            ////
            //lviCurrentItem.SubItems.Add(msgStsCurrentMsg.CSTString);
            //// We set the CRC of the message
            ////
            //lviCurrentItem.SubItems.Add(msgStsCurrentMsg.ChecksumString);
#endif
            return;
        }

        /// <summary>
        /// Displays and updates LIN messages in the Message-ListView
        /// </summary>
        //private void DisplayMessages()
        //{
        //    ListViewItem lviCurrentItem;

        //    foreach (MessageStatus msgStatus in m_LastMsgsList)
        //    {
        //        // Get the data to actualize
        //        //
        //        if (msgStatus.MarkedAsUpdated)
        //        {
        //            msgStatus.MarkedAsUpdated = false;
        //            lviCurrentItem = lstMessages.Items[msgStatus.Position];

        //            lviCurrentItem.SubItems[1].Text = msgStatus.LINMsg.Length.ToString();
        //            lviCurrentItem.SubItems[2].Text = msgStatus.DataString;
        //            lviCurrentItem.SubItems[3].Text = msgStatus.Count.ToString();
        //            lviCurrentItem.SubItems[4].Text = msgStatus.TimeString;
        //            lviCurrentItem.SubItems[5].Text = msgStatus.DirectionString;
        //            lviCurrentItem.SubItems[6].Text = msgStatus.ErrorString;
        //            lviCurrentItem.SubItems[7].Text = msgStatus.CSTString;
        //            lviCurrentItem.SubItems[8].Text = msgStatus.ChecksumString;
        //        }
        //    }
        //}


        private bool SendOK = false;
        public bool LinWrite(short Channel, __CanMsg Msg/*, Direction Dir, ChecksumType Chk*/)
        {
#if PROGRAM_RUNNING
            TLINMsg pMsg;
            CFrameDefinition lFD;

            short Pos = -1;
            short xPos = 0;
            // Get the CFrameDefinition associated to the selected FrameID
            foreach (ComboBoxItem wID in cbbID[Channel])
            {
                CFrameDefinition w_pGFT = (CFrameDefinition)wID.Value;
                if (w_pGFT.IdAsInt == Msg.ID)
                {
                    Pos = xPos;
                    break;
                }
                xPos++;
            }

            if (Pos == -1)
            {
                SendOK = false;
                return false;
            }
            lFD = (CFrameDefinition)(((ComboBoxItem)cbbID[Channel][Pos]).Value);
            // Create a new LIN frame message and copy the data.
            pMsg = new TLINMsg();
            pMsg.Data = new byte[8];
            pMsg.FrameId = Convert.ToByte(lFD.ProtectedIdAsInt);

            pMsg.Direction = (TLINDirection)((ComboBoxItem)cbbDirection[Channel][(short)lFD.Direction]).Value;
            pMsg.ChecksumType = (TLINChecksumType)((ComboBoxItem)cbbCST[Channel][(short)lFD.ChecksumType]).Value;

            pMsg.Length = Convert.ToByte(Msg.Length);
            // Fill data array
            //txtbCurrentTextBox = txtData0;
            for (int i = 0; i < pMsg.Length; i++) pMsg.Data[i] = Msg.DATA[i];

            // Check if the hardware is initialize as master
            if (m_HwMode[Channel] == TLINHardwareMode.modMaster)
            {
                // Calculate the checksum contained with the
                // checksum type that set some line before.
                PLinApi.CalculateChecksum(ref pMsg);
                // Try to send the LIN frame message with LIN_Write.
                m_LastLINErr = PLinApi.Write(m_hClient[Channel], m_hHw[Channel], ref pMsg);
            }
            else
            {
                // If the hardare is initialize as slave
                // only update the data in the LIN frame.
                m_LastLINErr = PLinApi.UpdateByteArray(m_hClient[Channel], m_hHw[Channel], lFD.IdAsByte, (byte)0, pMsg.Length, pMsg.Data);
            }
            // Show error if any
            if (m_LastLINErr != TLINError.errOK)
            {
                SendOK = false;
                return false;
            }
            else
            {
                SendOK = true;
                return true;
            }
#else
            return true;
#endif
        }

        public bool LinIDModeSetting(short Channel, int ID, Direction Dir, ChecksumType Chk)
        {
            short Pos = -1;
            short xPos = 0;
            //CFrameDefinition lFD;

            foreach (ComboBoxItem wID in cbbID[Channel])
            {
                CFrameDefinition w_pGFT = (CFrameDefinition)wID.Value;
                if (w_pGFT.ProtectedIdAsInt == ID)
                {
                    Pos = xPos;
                    break;
                }
                xPos++;
            }

            if (Pos == -1)
            {
                SendOK = false;
                return false;
            }

            m_pGFT[Channel][Pos].m_bDirection = (TLINDirection)Dir;
            m_pGFT[Channel][Pos].m_nChecksumType = (TLINChecksumType)Chk;

            ComboBoxItem Item = cbbID[Channel][Pos];
            Item.Text = m_pGFT[Channel][Pos].ID;
            Item.Value = m_pGFT[Channel][Pos];
            cbbID[Channel][Pos] = Item;
            return true;
        }

        public bool LinWrite(short Channel, __CanMsg Msg, Direction Dir, ChecksumType Chk)
        {
#if PROGRAM_RUNNING
            TLINMsg pMsg;
            CFrameDefinition lFD;

            short Pos = -1;
            short xPos = 0;
            // Get the CFrameDefinition associated to the selected FrameID
            foreach (ComboBoxItem wID in cbbID[Channel])
            {
                CFrameDefinition w_pGFT = (CFrameDefinition)wID.Value;
                if (w_pGFT.ProtectedIdAsInt == Msg.ID)
                {
                    Pos = xPos;
                    break;
                }
                xPos++;
            }

            if (Pos == -1)
            {
                SendOK = false;
                return false;
            }
            lFD = (CFrameDefinition)(((ComboBoxItem)cbbID[Channel][Pos]).Value);
            // Create a new LIN frame message and copy the data.
            pMsg = new TLINMsg();
            pMsg.Data = new byte[8];
            pMsg.FrameId = Convert.ToByte(lFD.ProtectedIdAsInt);

            //pMsg.Direction = (TLINDirection)((ComboBoxItem)cbbDirection[(short)lFD.Direction]).Value;
            //pMsg.ChecksumType = (TLINChecksumType)((ComboBoxItem)cbbCST[(short)lFD.ChecksumType]).Value;

            pMsg.Direction = (TLINDirection)Dir;
            pMsg.ChecksumType = (TLINChecksumType)Chk;

            pMsg.Length = Convert.ToByte(Msg.Length);
            // Fill data array
            //txtbCurrentTextBox = txtData0;
            for (int i = 0; i < pMsg.Length; i++) pMsg.Data[i] = Msg.DATA[i];

            // Check if the hardware is initialize as master
            if (m_HwMode[Channel] == TLINHardwareMode.modMaster)
            {
                // Calculate the checksum contained with the
                // checksum type that set some line before.
                PLinApi.CalculateChecksum(ref pMsg);
                // Try to send the LIN frame message with LIN_Write.
                m_LastLINErr = PLinApi.Write(m_hClient[Channel], m_hHw[Channel], ref pMsg);
            }
            else
            {
                // If the hardare is initialize as slave
                // only update the data in the LIN frame.
                m_LastLINErr = PLinApi.UpdateByteArray(m_hClient[Channel], m_hHw[Channel], lFD.IdAsByte, (byte)0, pMsg.Length, pMsg.Data);
            }
            // Show error if any
            if (m_LastLINErr == TLINError.errOK)
            {
                SendOK = true;
                return true;
            }
            else
            {
                SendOK = false;
                return false;
            }
#else
            return true;
#endif
        }

        public bool GetSendStatus
        {
            get { return SendOK; }
        }
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // LinControl
            // 
            this.ClientSize = new System.Drawing.Size(228, 55);
            this.Name = "LinControl";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LinControl_FormClosing);
            this.ResumeLayout(false);

        }

        public void WakeUpSend(short Channel)
        {
            m_LastLINErr = PLinApi.XmtWakeUp(m_hClient[Channel], m_hHw[Channel]);
            //mControl.공용함수.timedelay(5);
            //PLinApi.XmtWakeUp(m_hClient, m_hHw);
            //mControl.공용함수.timedelay(5);
            //PLinApi.XmtWakeUp(m_hClient, m_hHw);
            //mControl.공용함수.timedelay(5);
            //PLinApi.XmtWakeUp(m_hClient, m_hHw);
            if (m_LastLINErr == TLINError.errOK)
            {

            }
            return;
        }

        private bool LinStop = false;
        public void Stop()
        {
            if (LinStop == true) return;
            //if (Open == false) return;
            //DoLinDisconnect();
            //if (m_hClient != 0)
            //{
            //    DoLinDisconnect();
            //    m_hHw = 0;
            //    // Unregister the application
            //    PLinApi.RemoveClient(m_hClient);
            //    m_hClient = 0;
            //}
            LinStop = true;
            return;
        }

        public void Start()
        {
            if (LinStop == false) return;
            //if (Open == true) return;
            //LinOpen();
            //PLinApi.DisconnectClient(m_hClient, m_hHw);
            LinStop = true;
            return;
        }

        public short Reset
        {
            set
            {
                PLinApi.ResetClient(m_hClient[value]);
            }
        }
    }

    public class MessageStatus
    {
        /// <summary>
        /// The received LIN message
        /// </summary>
        private TLINRcvMsg m_Msg;
        /// <summary>
        /// Timestamp of a previously received message
        /// </summary>
        private ulong m_oldTimeStamp;
        /// <summary>
        /// index of the message in the ListView component
        /// </summary>
        private int m_iIndex;
        /// <summary>
        /// Number of LIN message received with the same frame ID
        /// </summary>
        private int m_Count;
        /// <summary>
        /// Defines if the timestamp is displayed as a period
        /// </summary>
        private bool m_bShowPeriod;
        /// <summary>
        /// Defines if the message has been modified and its display needs to be updated
        /// </summary>
        private bool m_bWasChanged;

        /// <summary>
        /// Creates a new MessageStatus object
        /// </summary>
        /// <param name="linMsg">received LIN message</param>
        /// <param name="listIndex">index of the message in the ListView</param>
        public MessageStatus(TLINRcvMsg linMsg, int listIndex)
        {
            m_Msg = linMsg;
            m_oldTimeStamp = linMsg.TimeStamp;
            m_iIndex = listIndex;
            m_Count = 1;
            m_bShowPeriod = true;
            m_bWasChanged = false;
        }
        /// <summary>
        /// Updates an existing MessageStatus with a new LIN message
        /// </summary>
        /// <param name="linMsg">LIN message to update</param>
        public void Update(TLINRcvMsg linMsg)
        {
            m_oldTimeStamp = m_Msg.TimeStamp;
            m_Msg = linMsg;
            m_Count += 1;
            m_bWasChanged = true;
        }

        //#region Getters and Setters
        /// <summary>
        /// The received LIN message
        /// </summary>
        public TLINRcvMsg LINMsg
        {
            get { return m_Msg; }
        }
        /// <summary>
        /// Index of the message in the ListView
        /// </summary>
        public int Position
        {
            get { return m_iIndex; }
        }
        /// <summary>
        /// Direction of the LIN message as a string
        /// </summary>
        //public string DirectionString
        //{
        //    get { return GetDirectionString(); }
        //}
        /// <summary>
        /// Checksum type of the LIN message as a string
        /// </summary>
        //public string CSTString
        //{
        //    get { return GetCSTString(); }
        //}
        /// <summary>
        /// Checksum of the LIN message as a string
        /// </summary>
        public string ChecksumString
        {
            get { return string.Format("{0:X}h", m_Msg.Checksum); }
        }
        /// <summary>
        /// Error field of the LIN message as a string
        /// </summary>
        public string ErrorString
        {
            get { return GetErrorString(); }
        }
        /// <summary>
        /// Protected ID of the LIN message as a string
        /// </summary>
        public string PIdString
        {
            get { return getProtectedIdString(); }
        }
        /// <summary>
        /// Data fields of the LIN message as a string
        /// </summary>
        public string DataString
        {
            get { return GetDataString(); }
        }
        /// <summary>
        /// Number of LIN messages received with the same frame ID
        /// </summary>
        public int Count
        {
            get { return m_Count; }
        }
        /// <summary>
        /// States wether the timestamp is displayed as a period or not
        /// </summary>
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
        /// <summary>
        /// Defines if the LIN message has been modified
        /// </summary>
        public bool MarkedAsUpdated
        {
            get { return m_bWasChanged; }
            set { m_bWasChanged = value; }
        }
        /// <summary>
        /// The timestamp or period of the LIN message
        /// </summary>
        public string TimeString
        {
            get { return GetTimeString(); }
        }

        //#endregion

        //#region private getters

        /// <summary>
        /// Returns the Protected ID as a string
        /// </summary>
        /// <returns>formatted protected ID</returns>
        private string getProtectedIdString()
        {
            return string.Format("{0:X3}h", m_Msg.FrameId);
        }
        /// <summary>
        /// Returns the Data array as a string
        /// </summary>
        /// <returns>the formatted data array</returns>
        private string GetDataString()
        {
            string strTemp;

            strTemp = "";
            for (int i = 0; i < m_Msg.Length; i++)
            {
                strTemp += string.Format("{0:X2} ", m_Msg.Data[i]);
            }
            return strTemp;
        }
        /// <summary>
        /// Returns the timestamp or the period of the frame
        /// </summary>
        /// <returns>timestamp or period in milliseconds</returns>
        private string GetTimeString()
        {
            ulong time;

            time = m_Msg.TimeStamp;
            if (m_bShowPeriod) time = (time - m_oldTimeStamp) / 1000;

            return time.ToString();
        }
        /// <summary>
        /// Returns the direction as a formatted string
        /// </summary>
        /// <returns>the formatted direction</returns>
        //private string GetDirectionString()
        //{
        //    switch (m_Msg.Direction)
        //    {
        //        case TLINDirection.dirDisabled:
        //            return Resources.SLinDirectionDisabled;
        //        case TLINDirection.dirPublisher:
        //            return Resources.SLinDirectionPublisher;
        //        case TLINDirection.dirSubscriber:
        //            return Resources.SLinDirectionSubscriber;
        //        case TLINDirection.dirSubscriberAutoLength:
        //            return Resources.SLinDirectionAuto;
        //    }
        //    return "";
        //}
        /// <summary>
        /// Returns the Checksum type as a string
        /// </summary>
        /// <returns>formatted checksum type</returns>
        //private string GetCSTString()
        //{
        //    switch (m_Msg.ChecksumType)
        //    {
        //        case TLINChecksumType.cstAuto:
        //            return Resources.SLinCSTAuto;
        //        case TLINChecksumType.cstClassic:
        //            return Resources.SLinCSTClassic;
        //        case TLINChecksumType.cstEnhanced:
        //            return Resources.SLinCSTEnhanced;
        //        case TLINChecksumType.cstCustom:
        //            return Resources.SLinCSTCustom;
        //    }
        //    return "";
        //}
        /// <summary>
        /// Returns the Error field of the LIN message as a string
        /// </summary>
        /// <returns>formatted Error field of the LIN message</returns>
        private string GetErrorString()
        {
            if (m_Msg.ErrorFlags == 0) return "O.k.";

            return m_Msg.ErrorFlags.ToString();
        }
        //#endregion        
    }
}
