using System;
using System.Runtime.InteropServices;	// Add
using System.Text;

/*
 * using System.Runtime.InteropServices를 선언하지 않을 경우
 * 밑에 보이는 1, 2번 문장 중에 1번으로 사용해야 하며
 * 위와 같이 using문으로 선언이 된 경우 2번 문장을 사용해야 한다.
 * using문을 선언하고 1번 문장을 사용해도 상관은 없다.
 * 단 using문을 선언하지 않고 2번 문장과 같이 사용해서는 안된다.
 */

// 1 [System.Runtime.InteropServices.DllImport("kernel32")]
// 2 [DllImport("kernel32")]

namespace PSeatTester
{
    /// <summary>
    /// Create a New INI file to store or load data
    /// </summary>
    public class TIniFile
    {
        string iniPath;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="def"></param>
        /// <param name="retVal"></param>
        /// <param name="size"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section,
            string key, string def, StringBuilder retVal, int size, string filePath);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="size"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section,
            string key, int size, string filePath);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="section"></param>
        /// <param name="LpPairValues"></param>
        /// <param name="size"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section,
            byte[] lpPairValues, int size, string filePath);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lpsection"></param>
        /// <param name="size"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(byte[] lpsection, int size, string filePath);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section,
            string key, string val, string filePath);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section,
            string key, string val);



        public TIniFile(string sName)
        {
            iniPath = sName;
        }

        public string FileName
        {
            get
            {
                return iniPath;
            }
            set
            {
                iniPath = value;
            }
        }

        // INI 값 읽기
        public String ReadString(String Section, String Key)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp, 255, iniPath);
            return temp.ToString();
        }

        public String ReadString(String Section, String Key1, String Key2)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key1 + Key2, "", temp, 255, iniPath);
            return temp.ToString();
        }

        public bool ReadBool(String Section, String Key)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp, 255, iniPath);

            if (temp.ToString() == "1")
                return true;
            else if (temp.ToString() == "True")
                return true;
            else return false;
        }
        public bool ReadBool(String Section, String Key1, String Key2)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key1 + Key2, "", temp, 255, iniPath);

            if (temp.ToString() == "1")
                return true;
            else if (temp.ToString() == "True")
                return true;
            else return false;
        }

        public int ReadInteger(String Section, String Key)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp, 255, iniPath);

            if (temp.Length == 0)
                return 0;
            else return Convert.ToInt32(temp.ToString());
        }
        public int ReadInteger(String Section, String Key1, String Key2)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key1 + Key2, "", temp, 255, iniPath);

            if (temp.Length == 0)
                return 0;
            else return Convert.ToInt32(temp.ToString());
        }

        public long ReadLong(String Section, String Key)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp, 255, iniPath);

            if (temp.Length == 0)
                return 0;
            else return Convert.ToInt64(temp.ToString());
        }
        public long ReadLong(String Section, String Key1, String Key2)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key1 + Key2, "", temp, 255, iniPath);

            if (temp.Length == 0)
                return 0;
            else return Convert.ToInt64(temp.ToString());
        }

        public short ReadShort(String Section, String Key)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp, 255, iniPath);

            if (temp.Length == 0)
                return 0;
            else return Convert.ToInt16(temp.ToString());
        }
        public short ReadShort(String Section, String Key1, String Key2)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key1 + Key2, "", temp, 255, iniPath);

            if (temp.Length == 0)
                return 0;
            else return Convert.ToInt16(temp.ToString());
        }

        public float ReadFloat(String Section, String Key)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp, 255, iniPath);

            if (temp.Length == 0)
                return 0;
            else return float.Parse(temp.ToString());
        }
        public float ReadFloat(String Section, String Key1, String Key2)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key1 + Key2, "", temp, 255, iniPath);

            if (temp.Length == 0)
                return 0;
            else return float.Parse(temp.ToString());
        }

        public double ReadDouble(String Section, String Key)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp, 255, iniPath);

            if (temp.Length == 0)
                return 0;
            else return Convert.ToDouble(temp.ToString());
        }

        public bool ReadDouble(String Section, String Key, ref double Value)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp, 255, iniPath);

            bool Flag;

            if (temp.Length == 0)
            {
                Value = 0;
                Flag = false;
            }
            else
            {
                Value = Convert.ToDouble(temp.ToString());
                Flag = true;
            }
            return Flag;
        }

        public double ReadDouble(String Section, String Key1, String Key2)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key1 + Key2, "", temp, 255, iniPath);

            if (temp.Length == 0)
                return 0;
            else return Convert.ToDouble(temp.ToString());
        }

        public string ReadDateTime(String Section, String Key)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp, 255, iniPath);

            if (temp.Length == 0)
                return "";
            else return temp.ToString();
        }

        public string ReadDateTime(String Section, String Key1, String Key2)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key1 + Key2, "", temp, 255, iniPath);

            if (temp.Length == 0)
                return "";
            else return temp.ToString();
        }

        public bool ReadString(String Section, String Key, ref string Value)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp, 255, iniPath);

            Value = temp.ToString();
            if (0 < i)
                return true;
            else return false;
        }

        public bool ReadString(String Section, String Key1, String Key2, ref string Value)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key1 + Key2, "", temp, 255, iniPath);

            Value = temp.ToString();
            if (0 < temp.Length)
                return true;
            else return false;
        }

        public bool ReadBool(String Section, String Key, ref bool Value)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp, 255, iniPath);

            if (0 < temp.Length)
            {
                if (temp.ToString() == "1")
                    Value = true;
                else if (temp.ToString() == "True")
                    Value = true;
                else Value = false;

                return true;
            }
            else
            {
                return false;
            }
        }
        public bool ReadBool(String Section, String Key1, String Key2, bool Value)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key1 + Key2, "", temp, 255, iniPath);

            if (0 < temp.Length)
            {
                if (temp.ToString() == "1")
                    Value = true;
                else if (temp.ToString() == "True")
                    Value = true;
                else Value = false;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ReadInteger(String Section, String Key, ref int Value)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp, 255, iniPath);

            if (temp.Length == 0)
            {
                return false;
            }
            else
            {
                Value = Convert.ToInt32(temp.ToString());
                return true;
            }
        }
        public bool ReadInteger(String Section, String Key1, String Key2, ref int Value)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key1 + Key2, "", temp, 255, iniPath);

            if (temp.Length == 0)
            {
                return false;
            }
            else
            {
                Value = Convert.ToInt32(temp.ToString());
                return true;
            }
        }

        public bool ReadLong(String Section, String Key, ref long Value)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp, 255, iniPath);

            if (temp.Length == 0)
            {
                return false;
            }
            else
            {
                Value = Convert.ToInt64(temp.ToString());
                return true;
            }
        }
        public bool ReadLong(String Section, String Key1, String Key2, ref long Value)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key1 + Key2, "", temp, 255, iniPath);

            if (temp.Length == 0)
            {
                return false;
            }
            else
            {
                Value = Convert.ToInt64(temp.ToString());
                return true;
            }
        }

        public bool ReadShort(String Section, String Key, ref short Value)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp, 255, iniPath);

            if (temp.Length == 0)
            {
                return false;
            }
            else
            {
                Value = Convert.ToInt16(temp.ToString());
                return true;
            }
        }
        public bool ReadShort(String Section, String Key1, String Key2, ref short Value)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key1 + Key2, "", temp, 255, iniPath);

            if (temp.Length == 0)
            {
                return false;
            }
            else
            {
                Value = Convert.ToInt16(temp.ToString());
                return true;
            }
        }

        public bool ReadByte(String Section, String Key, ref byte Value)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp, 255, iniPath);

            if (temp.Length == 0)
            {
                return false;
            }
            else
            {
                Value = Convert.ToByte(temp.ToString());
                return true;
            }
        }
        public bool ReadByte(String Section, String Key1, String Key2, ref byte Value)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key1 + Key2, "", temp, 255, iniPath);

            if (temp.Length == 0)
            {
                return false;
            }
            else
            {
                Value = Convert.ToByte(temp.ToString());
                return true;
            }
        }

        public bool ReadFloat(String Section, String Key, ref float Value)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp, 255, iniPath);

            if (temp.Length == 0)
            {
                return false;
            }
            else
            {
                Value = (float)Convert.ToDouble(temp.ToString());
                return true;
            }
        }
        public bool ReadFloat(String Section, String Key1, String Key2, ref float Value)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key1 + Key2, "", temp, 255, iniPath);

            if (temp.Length == 0)
            {
                return false;
            }
            else
            {
                Value = (float)Convert.ToDouble(temp.ToString());
                return true;
            }
        }

        public bool ReadFloat(String Section, String Key, ref double Value)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp, 255, iniPath);

            if (temp.Length == 0)
            {
                return false;
            }
            else
            {
                Value = Convert.ToDouble(temp.ToString());
                return true;
            }
        }

        public bool ReadDouble(String Section, String Key1, String Key2, ref double Value)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key1 + Key2, "", temp, 255, iniPath);

            if (temp.Length == 0)
            {
                return false;
            }
            else
            {
                Value = Convert.ToDouble(temp.ToString());
                return true;
            }
        }

        public bool ReadDateTime(String Section, String Key, ref string Value)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp, 255, iniPath);

            if (temp.Length == 0)
            {
                return false;
            }
            else
            {
                Value = temp.ToString();
                return true;
            }
        }

        public bool ReadDateTime(String Section, String Key1, String Key2, ref string Value)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key1 + Key2, "", temp, 255, iniPath);

            if (temp.Length == 0)
            {
                return false;
            }
            else
            {
                Value = temp.ToString();
                return true;
            }
        }


        // INI 값 설정
        public void WriteString(String Section, String Key, String Value)
        {
            WritePrivateProfileString(Section, Key, Value, iniPath);
            return;
        }
        public void WriteString(String Section, String Key, String Key2, String Value)
        {
            WritePrivateProfileString(Section, Key + Key2, Value, iniPath);
            return;
        }

        public void WriteInteger(String Section, String Key, long Value)
        {
            WritePrivateProfileString(Section, Key, Value.ToString(), iniPath);
            return;
        }

        public void WriteInteger(String Section, String Key, String Key2, long Value)
        {
            WritePrivateProfileString(Section, Key + Key2, Value.ToString(), iniPath);
            return;
        }

        public void WriteInteger(String Section, String Key, int Value)
        {
            WritePrivateProfileString(Section, Key, Value.ToString(), iniPath);
            return;
        }

        public void WriteInteger(String Section, String Key, String Key2, int Value)
        {
            WritePrivateProfileString(Section, Key + Key2, Value.ToString(), iniPath);
            return;
        }

        public void WriteInteger(String Section, String Key, short Value)
        {
            WritePrivateProfileString(Section, Key, Value.ToString(), iniPath);
            return;
        }
        public void WriteInteger(String Section, String Key, String Key2, short Value)
        {
            WritePrivateProfileString(Section, Key + Key2, Value.ToString(), iniPath);
            return;
        }

        public void WriteFloat(String Section, String Key, double Value)
        {
            WritePrivateProfileString(Section, Key, Value.ToString(), iniPath);
            return;
        }

        public void WriteFloat(String Section, String Key, String Key2, double Value)
        {
            WritePrivateProfileString(Section, Key + Key2, Value.ToString(), iniPath);
            return;
        }


        public void WriteFloat(String Section, String Key, float Value)
        {
            WritePrivateProfileString(Section, Key, Value.ToString(), iniPath);
            return;
        }
        public void WriteFloat(String Section, String Key, String Key2, float Value)
        {
            WritePrivateProfileString(Section, Key + Key2, Value.ToString(), iniPath);
            return;
        }

        public void WriteBool(String Section, String Key, bool Value)
        {
            WritePrivateProfileString(Section, Key, Value.ToString(), iniPath);
            return;
        }

        public void WriteBool(String Section, String Key, String Key2, bool Value)
        {
            WritePrivateProfileString(Section, Key + Key2, Value.ToString(), iniPath);
            return;
        }

        public void WriteDateTime(String Section, String Key, DateTime Value)
        {
            WritePrivateProfileString(Section, Key, Value.ToString(), iniPath);
            return;
        }

        public void WriteDateTime(String Section, String Key, String Key2, DateTime Value)
        {
            WritePrivateProfileString(Section, Key + Key2, Value.ToString(), iniPath);
            return;
        }
    }
}
