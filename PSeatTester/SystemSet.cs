using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PSeatTester
{
    public partial class SystemSet : Form
    {
        MyInterface mControl = null;
        public SystemSet()
        {
            InitializeComponent();
        }
        public SystemSet(MyInterface mControl)
        {
            InitializeComponent();
            this.mControl = mControl;            
        }


        private void SystemSet_Load(object sender, EventArgs e)
        {
            axComboBox4.Clear();
            axComboBox3.Clear();
            //axComboBox8.Clear();
            //axComboBox7.Clear();

            //if (mControl.GetLin != null)
            //{
            //    string[] Device = mControl.GetLin.GetDevice;

            //    foreach (string sx in Device)
            //    {
            //        axComboBox8.AddItem(sx);
            //    }
            //}

            string Num;
            comboBox1.Items.Clear();
            for (int i = 1; i <= 100; i++)
            {
                Num = (i / 100F).ToString("0.00");
                comboBox1.Items.Add(Num);
            }


            if (mControl.GetCan != null)
            {
                string[] Device = mControl.GetCan.GetDevice;

                foreach (string sx in Device)
                {
                    axComboBox4.AddItem(sx);                    
                }
            }
            //axComboBox7.AddItem("2400");
            //axComboBox7.AddItem("9600");
            //axComboBox7.AddItem("10400");
            //axComboBox7.AddItem("19200");


            axComboBox3.AddItem("5K");
            axComboBox3.AddItem("10K");
            axComboBox3.AddItem("20K");
            axComboBox3.AddItem("33K");
            axComboBox3.AddItem("47K");
            axComboBox3.AddItem("50K");
            axComboBox3.AddItem("83K");
            axComboBox3.AddItem("95K");
            axComboBox3.AddItem("100K");
            axComboBox3.AddItem("125K");
            axComboBox3.AddItem("250K");
            axComboBox3.AddItem("500K");
            axComboBox3.AddItem("800K");
            axComboBox3.AddItem("1M");

            //if ((0 <= mControl.GetConfig.Can.Device) && (0 < axComboBox4.ListCount) && (mControl.GetConfig.Can.Device < axComboBox4.ListCount)) axComboBox4.ListIndex = mControl.GetConfig.Can.Device;

            string s;
            string s1;
            string s2;

            //s1 = "(0x" + mControl.GetConfig.Can.Device.ToString("X2") + ")";
            s1 = "Device=" + mControl.GetConfig.Can.Device.ToString();
            s2 = "Channel=" + mControl.GetConfig.Can.Channel.ToString() + "h";

            for (int i = 0; i < axComboBox4.ListCount; i++)
            {
                s = axComboBox4.get_List(i);

                if (0 <= s.IndexOf(s1))
                {
                    if (0 <= s.IndexOf(s2))
                    {
                        axComboBox4.ListIndex = i;
                        break;
                    }
                }
            }

            //mControl.CanLinPosition


            if ((0 <= mControl.GetConfig.Can.Speed) && (0 < axComboBox3.ListCount) && (mControl.GetConfig.Can.Speed < axComboBox3.ListCount)) axComboBox3.ListIndex = mControl.GetConfig.Can.Speed;

            //s1 = mControl.GetConfig.Lin.Device.ToString() + " - ID";
            //for (int i = 0; i < axComboBox8.ListCount; i++)
            //{
            //    s = axComboBox8.get_List(i);

            //    if (0 <= s.IndexOf(s1))
            //    {
            //        axComboBox8.ListIndex = i;
            //        break;
            //    }
            //}
            //if ((0 <= mControl.GetConfig.Lin.Speed) && (0 < axComboBox7.ListCount) && (mControl.GetConfig.Lin.Speed < axComboBox7.ListCount)) axComboBox7.ListIndex = mControl.GetConfig.Lin.Speed;


            axComboBox1.Clear();
            axComboBox2.Clear();
            axComboBox5.Clear();
            axComboBox6.Clear();
            axComboBox11.Clear();
            axComboBox12.Clear();
                        
            axComboBox1.AddItem("2400");
            axComboBox1.AddItem("4800");
            axComboBox1.AddItem("9600");
            axComboBox1.AddItem("11400");
            axComboBox1.AddItem("19200");
            axComboBox1.AddItem("38400");
            axComboBox1.AddItem("57600");
            axComboBox1.AddItem("115200");

            axComboBox11.AddItem("2400");
            axComboBox11.AddItem("4800");
            axComboBox11.AddItem("9600");
            axComboBox11.AddItem("11400");
            axComboBox11.AddItem("19200");
            axComboBox11.AddItem("38400");
            axComboBox11.AddItem("57600");
            axComboBox11.AddItem("115200");

            axComboBox5.AddItem("2400");
            axComboBox5.AddItem("4800");
            axComboBox5.AddItem("9600");
            axComboBox5.AddItem("11400");
            axComboBox5.AddItem("19200");
            axComboBox5.AddItem("38400");
            axComboBox5.AddItem("57600");
            axComboBox5.AddItem("115200");

            axComboBox7.AddItem("2400");
            axComboBox7.AddItem("4800");
            axComboBox7.AddItem("9600");
            axComboBox7.AddItem("11400");
            axComboBox7.AddItem("19200");
            axComboBox7.AddItem("38400");
            axComboBox7.AddItem("57600");
            axComboBox7.AddItem("115200");

            string[] PortName = mControl.공용함수.GetComName(SerialPort.GetPortNames());
            
            foreach (string pName in PortName)
            {
                axComboBox2.AddItem(pName);
                axComboBox6.AddItem(pName);
                axComboBox12.AddItem(pName);
                axComboBox8.AddItem(pName);
            }

            if (mControl.GetConfig.NoiseMeter.Port != "")
            {
                if (PortName.Contains(mControl.GetConfig.NoiseMeter.Port) == true)
                {
                    int Pos = axComboBox12.FindItem(0, mControl.GetConfig.NoiseMeter.Port, true);
                    axComboBox12.ListIndex = Pos;
                }
                else
                {
                    axComboBox12.ListIndex = -1;
                }
            }
            else
            {
                axComboBox12.ListIndex = -1;
            }

            if ((0 <= mControl.GetConfig.NoiseMeter.Speed) && (0 < axComboBox11.ListCount) && (mControl.GetConfig.NoiseMeter.Speed < axComboBox11.ListCount)) axComboBox11.ListIndex = mControl.GetConfig.NoiseMeter.Speed;



            if (mControl.GetConfig.Panel.Port != "")
            {
                if (PortName.Contains(mControl.GetConfig.Panel.Port) == true)
                {
                    int Pos = axComboBox2.FindItem(0, mControl.GetConfig.Panel.Port, true);
                    axComboBox2.ListIndex = Pos;
                }
                else
                {
                    axComboBox2.ListIndex = -1;
                }
            }
            else
            {
                axComboBox2.ListIndex = -1;
            }

            if ((0 <= mControl.GetConfig.Panel.Speed) && (0 < axComboBox1.ListCount) && (mControl.GetConfig.Panel.Speed < axComboBox1.ListCount)) axComboBox1.ListIndex = mControl.GetConfig.Panel.Speed;


            if (mControl.GetConfig.Power.Port != "")
            {
                if (PortName.Contains(mControl.GetConfig.Power.Port) == true)
                {
                    int Pos = axComboBox6.FindItem(0, mControl.GetConfig.Power.Port, true);
                    axComboBox6.ListIndex = Pos;
                }
                else
                {
                    axComboBox6.ListIndex = -1;
                }
            }
            else
            {
                axComboBox6.ListIndex = -1;
            }

            if ((0 <= mControl.GetConfig.Power.Speed) && (0 < axComboBox5.ListCount) && (mControl.GetConfig.Power.Speed < axComboBox5.ListCount)) axComboBox5.ListIndex = mControl.GetConfig.Power.Speed;


            if (mControl.GetConfig.Stroke.Port != "")
            {
                if (PortName.Contains(mControl.GetConfig.Stroke.Port) == true)
                {
                    int Pos = axComboBox8.FindItem(0, mControl.GetConfig.Stroke.Port, true);
                    axComboBox8.ListIndex = Pos;
                }
                else
                {
                    axComboBox8.ListIndex = -1;
                }
            }
            else
            {
                axComboBox8.ListIndex = -1;
            }

            if ((0 <= mControl.GetConfig.Stroke.Speed) && (0 < axComboBox7.ListCount) && (mControl.GetConfig.Stroke.Speed < axComboBox7.ListCount)) axComboBox7.ListIndex = mControl.GetConfig.Stroke.Speed;


            textBox6.Text = mControl.GetConfig.Client.IP;
            textBox2.Text = mControl.GetConfig.Client.Port.ToString();

            textBox8.Text = mControl.GetConfig.Server.IP;
            textBox3.Text = mControl.GetConfig.Server.Port.ToString();

            textBox9.Text = mControl.GetConfig.PC.IP;
            textBox5.Text = mControl.GetConfig.PC.Port.ToString();

            textBox10.Text = mControl.GetConfig.Board.IP;
            textBox7.Text = mControl.GetConfig.Board.Port.ToString();

            textBox1.Text = mControl.GetConfig.BattID.ToString();
            textBox4.Text = mControl.GetConfig.CurrID.ToString();
            textBox11.Text = mControl.GetConfig.PinConnectionDelay.ToString("0.0");
            checkBox1.Checked = mControl.GetConfig.AutoConnection;

            comboBox1.SelectedItem = mControl.GetConfig.KalmanSpeed.ToString("0.00");

            return;
        }

        private void MoveSpec()
        {
            __Config__ Config = mControl.GetConfig;

            if (0 <= axComboBox4.ListIndex)
            {
                string[] s = axComboBox4.Text.Split(':');
                string[] s2 = s[1].Split(',');

                string ss1 = s2[0].Substring(s2[0].IndexOf("Device=") + "Device=".Length);
                string ss2 = s2[2].Substring(s2[2].IndexOf("Channel=") + "Channel=".Length);

                ss2 = ss2.Replace("h", null);

                if (short.TryParse(ss1, out Config.Can.Device) == false) Config.Can.Device = -1;
                if (short.TryParse(ss2, out Config.Can.Channel) == false) Config.Can.Channel = -1;


                ss1 = s2[1].Substring(s2[1].IndexOf("ID=") + "ID=".Length);

                ss2 = ss1.Replace("(", null);
                ss2 = ss2.Replace(")", null);
                Config.Can.ID = (short)mControl.공용함수.StringToHex(ss2);
            }

            if (0 <= axComboBox3.ListIndex) Config.Can.Speed = axComboBox3.ListIndex;

            //if (0 <= axComboBox8.ListIndex)
            //{                
            //    string[] s = axComboBox8.Text.Split(',');

            //    if (2 <= s.Length)
            //    {
            //        string[] s2 = s[0].Trim().Split('.');

            //        if (2 <= s2.Length)
            //        {
            //            string[] ID = s2[1].Trim().Split('-');
            //            ID[0] = ID[0].Trim();
            //            if (short.TryParse(ID[0], out Config.Lin.Device) == false) Config.Lin.Device = 0;
            //        }
            //        //Config.Can.Device = (short)axComboBox4.ListIndex;
            //    }
            //}

            if (0 <= axComboBox2.ListIndex) Config.Panel.Port = axComboBox2.Text;
            if (0 <= axComboBox1.ListIndex) Config.Panel.Speed = axComboBox1.ListIndex;

            if (0 <= axComboBox6.ListIndex) Config.Power.Port = axComboBox6.Text;
            if (0 <= axComboBox5.ListIndex) Config.Power.Speed = axComboBox5.ListIndex;

            if (0 <= axComboBox12.ListIndex) Config.NoiseMeter.Port = axComboBox12.Text;
            if (0 <= axComboBox11.ListIndex) Config.NoiseMeter.Speed = axComboBox11.ListIndex;

            if (0 <= axComboBox8.ListIndex) Config.Stroke.Port = axComboBox8.Text;
            if (0 <= axComboBox7.ListIndex) Config.Stroke.Speed = axComboBox7.ListIndex;

            Config.Client.IP = textBox6.Text;
            if (int.TryParse(textBox2.Text, out Config.Client.Port) == false) Config.Client.Port = 0;

            Config.Server.IP = textBox8.Text;
            if (int.TryParse(textBox3.Text, out Config.Server.Port) == false) Config.Server.Port = 0;


            Config.PC.IP = textBox9.Text;
            if (int.TryParse(textBox5.Text, out Config.PC.Port) == false) Config.PC.Port = 0;

            Config.Board.IP = textBox10.Text;
            if (int.TryParse(textBox7.Text, out Config.Board.Port) == false) Config.Board.Port = 0;

            if (short.TryParse(textBox1.Text, out Config.BattID) == false) Config.BattID = 0;
            if (short.TryParse(textBox4.Text, out Config.CurrID) == false) Config.CurrID = 0;

            if (float.TryParse(textBox11.Text, out Config.PinConnectionDelay) == false) Config.PinConnectionDelay = 1.5F;
            Config.AutoConnection = checkBox1.Checked;

            string Num;

            if (comboBox1.SelectedItem != null)
                Num = comboBox1.SelectedItem.ToString();
            else Num = "0.15";

            if (float.TryParse(Num, out Config.KalmanSpeed) == false) Config.KalmanSpeed = 0.15F;

            mControl.GetConfig = Config;
            return;
        }
        

        private void ImageButton1_Click(object sender, EventArgs e)
        {
            MoveSpec();
            //ConfigSetting GetConfig = new ConfigSetting();
            //GetConfig.ReadWriteConfig = mControl.GetConfig;
            return;
        }

        private void ImageButton2_Click(object sender, EventArgs e)
        {
            this.Close();
            return;
        }
    }
}
