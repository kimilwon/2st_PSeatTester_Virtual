using System;
using System.Drawing;
using System.Runtime.InteropServices;
using MES;

namespace PSeatTester
{
    [StructLayout(LayoutKind.Explicit)]
    public struct union_r
    {
        [FieldOffset(0)]
        public int i;
        [FieldOffset(0)]
        public byte c1;
        [FieldOffset(1)]
        public byte c2;
        [FieldOffset(2)]
        public byte c3;
        [FieldOffset(3)]
        public byte c4;
    };
      
  
    public enum MENU
    {
        NONE,
        AGING_TESTING,
        PERFORMANCE_TESTING,
        AGING_SETTING,
        PERFORMANCE_SETTING,
        OPTION,
        LIN1,
        LIN2,
        CAN,
        PASSWORD,
        AGING_DATAVIEW,
        PERFORMANCE_DATAVIEW,
        SELF
    }


    public class RESULT
    {
        public const short READY = 0;
        public const short PASS = 1;
        public const short NG = 2;
        public const short REJECT = 2;
        public const short END = 3;
        public const short STOP = 4;
        public const short CLEAR = 5;
        public const short TEST = 6;
        public const short NOT_TEST = 7;
        public const short TEST_STANDBY = 8;        
    };


    public class IO_IN
    {
        public const short PASS = 0;
        public const short RESET = 1;
        public const short RH_SELECT = 2;
        public const short SEAT_RELAX = 3; 
        //public const short SEAT_POWER = 4;
        public const short AUTO = 5;

        /// <summary>
        /// TEST_SELECT 가 선택되어 있으면 검사 여부 선택으로 사용하고
        /// 선택되어 있지 않으면 각 fwd , bwd, up, dn 스위치 동작을 하면 된다.
        /// </summary>
        public const short TEST_SELECT = 6;
        //public const short MANUAL_SELECT = 7;

        public const short RELAX_RELAX = 8;
        public const short RELAX_RETURN = 9;
        public const short RECLINE_FWD = 10;
        public const short RECLINE_BWD = 11;
        public const short HEIGHT_UP = 12;
        public const short HEIGHT_DN = 13;
        public const short LEGREST_EXT = 14;
        public const short LEGREST_EXT_RETURN = 20;
        public const short LEGREST = 22;
        public const short LEGREST_RETURN = 21;
        public const short PRODUCT = 18;
        public const short JIG_UP = 19;
        public const short SlideDeliveryPosSensor = 17;
        public const short ReclineeDeliveryPosSensor = 16;

        public const short PIN_CONNECTION_FWD = 24;
        public const short PIN_CONNECTION_BWD = 25;
        public const short PIN_CONNECTION_SW = 7;
    }


    public class IO_OUT
    {
        public const short RED = 0;
        public const short YELLOW = 1;
        public const short GREEN = 2;
        public const short BUZZER = 3;
        public const short PRODUCT = 4;
        public const short TEST_OK = 5;
        //public const short TEST_JIG_DOWN = 6;
        public const short DOOR_OPEN = 6;
        public const short TEST_ING = 7;
        
        public const short RH_SELECT = 7;
        //public const short RECLINER_FWD = 8;
        //public const short RECLINER_BWD = 9;
        //public const short PSEAT_IGN = 8;        
        //public const short PRODUCT_HORIZENTAL = 9;
        //public const short AIRBAG_RESI_SELECT = 10;

        public const short PIN_CONNECTION = 24;
        public const short MAX = 11;
    }

    public class IO_FUNCTION_OUT
    {
        public const short PSEAT_BATT = 0;
        public const short IGN1 = 10;
        public const short IGN2 = 12;

        
        //public const short RELAX = 10;
        //public const short RELAX_RETURN = 11;
        //public const short HEIGHT_UP = 12;
        //public const short HEIGHT_DN = 13;
        //public const short LEGREST = 14;
        //public const short LEGREST_RETURN = 15;
        //public const short LEGRESTEXT = 16;
        //public const short LEGRESTEXT_RETURN = 17;
    }

    public enum LH_RH
    {
        LH,
        RH
    }
    //public enum PSEAT_SELECT
    //{
    //    PSEAT_8WAY_4W,
    //    PSEAT_8WAY,
    //    PSEAT_8WAY_2W,
    //    PSEAT_8WAY_WALKIN
    //}
    
    [Serializable, StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]//CharSet = CharSet.Unicode를 선언해 주지 않으면 한글 처리할 때 파일에 저장하거나 할 경우 에러가 발생한다.
    public struct __MinMax__
    {
        public double Min;
        public double Max;
    }

    public struct __MinMaxToInt__
    {
        public int Min;
        public int Max;
    }

    public struct __Port__
    {
        public string Port;
        public int Speed;
    }


    public struct __LinDevice__
    {
        public short Device;
        public int Speed;
    }
    public struct __CanDevice__
    {
        public short Device;
        public short Channel;
        public short ID;
        public int Speed;
    }

    public struct __TcpIP__
    {
        public string IP;
        public int Port;
    }

    [Serializable, StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]//CharSet = CharSet.Unicode를 선언해 주지 않으면 한글 처리할 때 파일에 저장하거나 할 경우 에러가 발생한다.
    public struct __Config__
    {
        /// <summary>
        /// Master
        /// </summary>
        //public __LinDevice__ Lin;
        public __CanDevice__ Can;
        public __Port__ NoiseMeter;
        public __Port__ Stroke;
        public __TCPIP__ Client;
        public __TCPIP__ Server;
        public __TcpIP__ Board;
        public __TcpIP__ PC;
        public __Port__ Power;
        public __Port__ Panel;
        public short BattID;
        public short CurrID;
        public float PinConnectionDelay;
        public bool AutoConnection;
        public float KalmanSpeed;
    }
    
    public struct __Time__
    {
        public int Hour;
        public short Min;
        public short Sec;
        public short mSec;
    }


    [Serializable, StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]//CharSet = CharSet.Unicode를 선언해 주지 않으면 한글 처리할 때 파일에 저장하거나 할 경우 에러가 발생한다.
    //[StructLayout(LayoutKind.Sequential, Pack = 1)]    
    public struct __CanData__
    {
        public short Data;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
        public char[] Title; //[50];
    };

    [Serializable, StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]//CharSet = CharSet.Unicode를 선언해 주지 않으면 한글 처리할 때 파일에 저장하거나 할 경우 에러가 발생한다.
    public struct __ItemCan__
    {
        public short StartBit;
        public short Size;
        public short Mode; // 0이면 일반데이타가 1 이면 숫치 데이타가 들어가는 항목임 2 이면 아스키 데이타가 들어간다.
        public short DataCounter;
        public short Length;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
        public char[] Title; //[50];
        public int CanID;
        public short S_ID;
        public short ReceiveTime; // 전송 간격을 갖는다.
        public bool CanLin; // true can, false Lin
        public bool InOut; // true 이면 output mode

        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 40)]
        public __CanData__[] Data;//[40];
    }

    public struct __Can__
    {
        public short ItemCounter;

        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 700)]
        public __ItemCan__[] Item; // [700]
    };
    public struct __sCan__
    {
        public bool Run;
        public short sID;
        public short dBit;
    }

    public struct __CanMsg
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] DATA;// [8];
        public int Length;
        public int ID;
    }


    public struct __SendCan__
    {
        public int ID;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] Data; //[8]
        public int Length;
        public long first;
        public long last;
        public long sendtime;
        //public byte AliveCnt;
    }

    public struct __InOutCanMsg__
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public __SendCan__[] Send; // [20]
        public int Max;
    }

    public struct CanInOutStruct
    {
        public __InOutCanMsg__ Can;
    }
    public struct LinInOutStruct
    {
        public __InOutCanMsg__ Lin;
    }

    public struct __InOutCan__
    {
        public CanInOutStruct In;
        public CanInOutStruct Out;
    }
    public struct __InOutLin__
    {
        public LinInOutStruct In;
        public LinInOutStruct Out;
    }


    public struct __IOData__
    {
        public short Card;
        public short Pos;
        public byte Data;
    }

    public struct __LinOutPos
    {
        /// <summary>
        /// 데이타
        /// </summary>
        public byte Data;
        /// <summary>
        /// 초기화 데이타 
        /// </summary>
        public byte Mask;
        //시작 위치
        public short Byte;
        /// <summary>
        /// 비트 위치
        /// </summary>
        public short Pos;
        /// <summary>
        /// Lin FID/PID
        /// </summary>
        public byte ID;
    }
    public struct __LinInPos
    {
        /// <summary>
        /// 데이타
        /// </summary>
        public byte Length;
        /// <summary>
        /// 초기화 데이타 
        /// </summary>
        public byte Mask;
        //시작 위치
        public short Byte;
        /// <summary>
        /// 비트 위치
        /// </summary>
        public short Pos;
        /// <summary>
        /// Lin FID/PID
        /// </summary>
        public byte ID;
    }

    [Serializable, StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]//CharSet = CharSet.Unicode를 선언해 주지 않으면 한글 처리할 때 파일에 저장하거나 할 경우 에러가 발생한다.
    public struct __TestDataItem__
    {
        public short Result;
        public float Data;
        public bool Test;
    }

    public struct __TestDataItemToString__
    {
        public short Result;
        public string Data;
        public bool Test;
    }

    public struct __TestDataItem2__
    {
        public short Result1;
        public short Result2;
        public float Data1;
        public float Data2;
        public bool Test1;
        public bool Test2;
    }
    public struct __SoundDataItem__
    {
        public short ResultStart;
        public short ResultRun;
        public float StartData;
        public float RunData;
        public bool Test;
    }

    [Serializable, StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]//CharSet = CharSet.Unicode를 선언해 주지 않으면 한글 처리할 때 파일에 저장하거나 할 경우 에러가 발생한다.
    public struct __TestCanLinDataItem__
    {
        public short Result;
        public byte Data;
        public short Message;
        public bool Test;
    }

    [Serializable, StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]//CharSet = CharSet.Unicode를 선언해 주지 않으면 한글 처리할 때 파일에 저장하거나 할 경우 에러가 발생한다.
    public struct __TestData__
    {
        public short Result;
        public short InComingResult;
        public string Time;
                
        public __TestDataItem2__ Recline;
        public __TestDataItem2__ Relax;
        public __TestDataItem2__ Height;
        public __TestDataItem2__ Legrest;
        public __TestDataItem2__ LegrestExt;
        public __SoundDataItem__ ReclineFwdSound;
        public __SoundDataItem__ ReclineBwdSound;
        public __SoundDataItem__ RelaxSound;
        public __SoundDataItem__ RelaxReturnSound;
        public __SoundDataItem__ HeightUpSound;
        public __SoundDataItem__ HeightDnSound;
        public __SoundDataItem__ LegrestSound;
        public __SoundDataItem__ LegrestReturnSound;
        public __SoundDataItem__ LegrestExtSound;
        public __SoundDataItem__ LegrestExtReturnSound;


        public __TestDataItem__ RelaxTime;
        public __TestDataItem__ RelaxReturnTime;
        public __TestDataItem__ ReclineFwdTime;
        public __TestDataItem__ ReclineBwdTime;
        public __TestDataItem__ HeightUpTime;
        public __TestDataItem__ HeightDnTime;
        public __TestDataItem__ LegrestTime;
        public __TestDataItem__ LegrestReturnTime;
        public __TestDataItem__ LegrestExtTime;
        public __TestDataItem__ LegrestExtReturnTime;
        public __TestDataItemToString__ SWVer;
    }

    public struct __Infor__ 
    {
        public string Date;
        public string DataName;
        public int TotalCount;
        public int OkCount;
        public int NgCount;
        public bool ReBootingFlag;
    }
    public struct __CanLin__
    {
        public __InOutCan__ Can;
        public __InOutLin__ Lin;
    }
    public struct __CanInPos
    {
        /// <summary>
        /// 데이타
        /// </summary>
        public byte Length;
        /// <summary>
        /// 초기화 데이타 
        /// </summary>
        public byte Mask;
        //시작 위치
        public short Byte;
        /// <summary>
        /// 비트 위치
        /// </summary>
        public short Pos;
        /// <summary>
        /// Lin FID/PID
        /// </summary>
        public int ID;
    }
    public struct __CanOutMessage
    {
        /// <summary>
        /// 데이타
        /// </summary>
        public byte[] Data;
        /// <summary>
        /// Lin FID/PID
        /// </summary>
        public int ID;
    }
    public struct __CanOutPos
    {
        /// <summary>
        /// 데이타
        /// </summary>
        public byte Data;
        /// <summary>
        /// 초기화 데이타 
        /// </summary>
        public byte Mask;
        //시작 위치
        public short Byte;
        /// <summary>
        /// 비트 위치
        /// </summary>
        public short Pos;
        /// <summary>
        /// Lin FID/PID
        /// </summary>
        public int ID;
    }

    public struct PSeatPowrItem
    {
        public int Batt;
        public int Gnd;
    }

    public struct PSeatPower
    {
        public PSeatPowrItem Batt1;
        public PSeatPowrItem Batt2;
    }
    public struct PinMapItem
    {
        public int PinNo;
        public int Mode; // 0 - B+ , 1 - Gnd
    }

    public class PSeatRuNMode
    {
        public const short Batt = 0;
        public const short Gnd = 1;
    }

    
    public enum LHD_RHD
    {
        LHD,
        RHD
    }
    public enum PSEAT_TYPE
    {
        IMS,
        POWER,
        MANUAL
    }
    public struct __CheckItem__
    {
        public bool Recline;
        public bool Height;
        public bool Relax;
        public bool Legrest;
        public bool LegrestExt;
        public bool Sound기동;
        public bool Sound구동;
        public bool VirtualLimit;

        public bool Can;
        public bool ProductTestRunFlag;
        public LH_RH LhRh;
        public LHD_RHD LhdRhd;
        public bool RelaxSeat;
    }
    public struct __SpecItem__
    {
        public __MinMax__ Legrest;
        public __MinMax__ Relax;
        public __MinMax__ Height;
        public __MinMax__ Recliner;
        public __MinMax__ LegrestExt;
    }
    public struct __SpecItem2__
    {
        public __MinMax__ Legrest;
        public __MinMax__ LegrestReturn;
        public __MinMax__ Relax;
        public __MinMax__ RelaxReturn;
        public __MinMax__ HeightUp;
        public __MinMax__ HeightDn;
        public __MinMax__ ReclinerFwd;
        public __MinMax__ ReclinerBwd;
        public __MinMax__ LegrestExt;
        public __MinMax__ LegrestExtReturn;
    }
    public struct __SpecItem3__
    {
        public float Legrest;
        public float Relax;
        public float Height;
        public float Recliner;
        public float LegrestExt;
    }

    public struct __DeliveryPos__
    {
        public short Legrest;
        public short Relax;
        public short Height;
        public short Recliner;
        public short RelaxToRecliner;
        public short LegrestExt;
        public float RelaxStroke;
        public float PowerReclinerStroke;

        public float MinStroke;
        public float MaxStroke;
    }

    public struct __SoundSpec__
    {
        public float StartMax;
        public float RunMax;
        /// <summary>
        /// 소음 측정시 Max 범위를 벗어났을 경우 그 소음이 SoundCheckRange 이사 유지 되면 인식한다.
        /// </summary>
        //public float 기도음구동음간격시간;
        public float StartTime;
        public int LimteCount;
        public bool RMSMode;        
    }

    public struct __Spec__
    {
        public string ModelName;

        public __SpecItem__ Current;
        public __SpecItem2__ MovingSpeed;
        public __SpecItem3__ MovingStroke;
        public __SoundSpec__ Sound;        
        public __DeliveryPos__ DeliveryPos;
        //-----------------------------
        public float RelaxLimitTime;
        public float RelaxToReclineLimitTime;
        public float LegrestLimitTime;
        public float LegrestExtLimitTime;
        public float HeightLimitTime;
        public float ReclinerLimitTime;
        //----------------------------- //전류 측정 시간
        public float RelaxTestTime;
        public float LegrestTestTime;
        public float LegrestExtTestTime;
        public float HeightTestTime;
        public float ReclinerTestTime;
        //-----------------------------
        //public float SoundCheckTimeRange;
        public float LegrestSoundCheckTime;
        public float LegrestExtSoundCheckTime;
        public float HeightSoundCheckTime;
        public float ReclinerFwdSoundCheckTime;
        public float ReclinerBwdSoundCheckTime;
        public float RelaxSoundCheckTime;
        //-----------------------------
        public float LegrestLimitCurr;
        public float LegrestExtLimitCurr;
        public float RelaxLimitCurr;
        public float HeightLimitCurr;
        public float ReclinerLimitCurr;
        public float RelaxSwOnTime;
        public float TestVolt;
        public float InComingVolt;
        public string SWVer;
        //public bool Can;
    }

    public class SoundLevePos
    {
        public const short SLIDE_FWD = 0;
        public const short SLIDE_BWD = 1;
        public const short RECLINE_FWD = 2;
        public const short RECLINE_BWD = 3;
        public const short HEIGHT_UP = 4;
        public const short HEIGHT_DN = 5;
        public const short TILT_UP = 6;
        public const short TILT_DN = 7;
        public const short RELAX = 8;
        public const short RELAX_RETURN = 9;
        public const short LEGREST = 10;
        public const short LEGREST_RETURN = 11;
        public const short LEGEXT = 12;
        public const short LEGEXT_RETURN = 13;
    }
}