using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PSeatTester
{
    public partial class SpecSetting : Form
    {
        private __Spec__ Spec = new __Spec__();
        private MyInterface mControl = null;

        public SpecSetting()
        {
            InitializeComponent();
        }
        public SpecSetting(MyInterface mControl, string mName)
        {
            InitializeComponent();
            this.mControl = mControl;
            this.mName = mName;
        }

        private bool ModelBoxChangeFlag = false;
        private void SpecSetting_Load(object sender, EventArgs e)
        {
            mControl.공용함수.ReadFileListNotExt(Program.SPEC_PATH.ToString(), "*.Spc", COMMON_FUCTION.FileSortMode.FILENAME_ODERBY);
            List<string> FList = mControl.공용함수.GetFileList;

            if (0 < FList.Count)
            {
                ModelBoxChangeFlag = true;
                comboBox1.Items.Clear();
                foreach (string s in FList) comboBox1.Items.Add(s);

                if (0 < comboBox1.Items.Count)
                {
                    if (0 < comboBox1.Items.Count)
                    {
                        if ((mName != null) && (mName != "") && (mName != string.Empty))
                        {
                            if (comboBox1.Items.Contains(mName) == true) comboBox1.SelectedItem = mName;
                        }
                    }

                }
                if (mName != null)
                {
                    string sName = Program.SPEC_PATH.ToString() + "\\" + mName + ".Spc";
                    if(File.Exists(sName) == true) mControl.공용함수.OpenSpec(sName,ref Spec);
                }
            }
            DisplaySpec();
            ModelBoxChangeFlag = false;
            return;
        }

        private string mName;

        private void imageButton1_Click(object sender, EventArgs e)
        {
            //저장
            MoveSpec();

            string sName;
            string ModName = null;
            if (comboBox1.SelectedItem == null)
            {
                if (MessageBox.Show("선택된 모델이 없습니다.\n모델을 생성하시겠습니까?", "경고", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    if (InputBox.Show("모델명 입력", "모델명", ref ModName) == DialogResult.OK)
                    {
                        if ((ModName != null) && (ModName != "") && (ModName != string.Empty))
                        {
                            mName = ModName;
                            sName = Program.SPEC_PATH.ToString() + "\\" + ModName + ".Spc";

                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
            else
            {
                ModName = comboBox1.SelectedItem.ToString();
                sName = Program.SPEC_PATH.ToString() + "\\" + comboBox1.SelectedItem.ToString() + ".Spc";
            }
            Spec.ModelName = ModName;
            mControl.공용함수.SaveSpec(Spec: Spec, Name: sName);
            return;
        }

        private void imageButton2_Click(object sender, EventArgs e)
        {
            //모델 추가
            string ModName = null;
            if (InputBox.Show("모델명 입력", "모델명", ref ModName) == DialogResult.OK)
            {
                if ((ModName != null) && (ModName != "") && (ModName != string.Empty))
                {
                    string sName;
                    mName = ModName;
                    sName = Program.SPEC_PATH.ToString() + "\\" + ModName + ".Spc";

                    ModelBoxChangeFlag = true;
                    comboBox1.Items.Add(ModName);

                    InitSpec();
                    Spec.ModelName = mName;
                    DisplaySpec();
                    mControl.공용함수.SaveSpec(sName, Spec);
                    ModelBoxChangeFlag = false;
                }
            }
            return;
        }

        private void imageButton3_Click(object sender, EventArgs e)
        {
            //삭제
            if (comboBox1.SelectedItem == null) return;
            string sName = Program.SPEC_PATH.ToString() + "\\" + comboBox1.SelectedItem.ToString() + ".Spc";

            if (File.Exists(sName) == true) File.Delete(sName);

            InitSpec();
            DisplaySpec();
            comboBox1.Items.Remove(comboBox1.SelectedItem);
            return;
        }

        private void imageButton4_Click(object sender, EventArgs e)
        {
            //다른 이름으로 저장
            MoveSpec();

            string ModName = null;
            if (InputBox.Show("모델명 입력", "모델명", ref ModName) == DialogResult.OK)
            {
                if ((ModName != null) && (ModName != "") && (ModName != string.Empty))
                {
                    string sName;
                    mName = ModName;
                    sName = Program.SPEC_PATH.ToString() + "\\" + ModName + ".Spc";

                    Spec.ModelName = ModName;
                    mControl.공용함수.SaveSpec(sName, Spec);
                    ModelBoxChangeFlag = true;
                    comboBox1.Items.Add(ModName);
                    comboBox1.SelectedItem = ModName;
                    ModelBoxChangeFlag = false;
                }
            }
            return;
        }
        private void DisplaySpec()
        {
            fpSpread1.Sheets[0].Cells[3, 3].Text = Spec.Current.Recliner.Min.ToString("0.00");
            fpSpread1.Sheets[0].Cells[3, 4].Text = Spec.Current.Recliner.Max.ToString("0.00");
            fpSpread1.Sheets[0].Cells[4, 3].Text = Spec.Current.Relax.Min.ToString("0.00");
            fpSpread1.Sheets[0].Cells[4, 4].Text = Spec.Current.Relax.Max.ToString("0.00");
            fpSpread1.Sheets[0].Cells[5, 3].Text = Spec.Current.Height.Min.ToString("0.00");
            fpSpread1.Sheets[0].Cells[5, 4].Text = Spec.Current.Height.Max.ToString("0.00");
            fpSpread1.Sheets[0].Cells[6, 3].Text = Spec.Current.Legrest.Min.ToString("0.00");
            fpSpread1.Sheets[0].Cells[6, 4].Text = Spec.Current.Legrest.Max.ToString("0.00");
            fpSpread1.Sheets[0].Cells[7, 3].Text = Spec.Current.LegrestExt.Min.ToString("0.00");
            fpSpread1.Sheets[0].Cells[7, 4].Text = Spec.Current.LegrestExt.Max.ToString("0.00");

            fpSpread1.Sheets[0].Cells[8, 3].Text = Spec.MovingSpeed.ReclinerFwd.Min.ToString("0.00");
            fpSpread1.Sheets[0].Cells[8, 4].Text = Spec.MovingSpeed.ReclinerFwd.Max.ToString("0.00");
            fpSpread1.Sheets[0].Cells[9, 3].Text = Spec.MovingSpeed.ReclinerBwd.Min.ToString("0.00");
            fpSpread1.Sheets[0].Cells[9, 4].Text = Spec.MovingSpeed.ReclinerBwd.Max.ToString("0.00");
            fpSpread1.Sheets[0].Cells[10, 3].Text = Spec.MovingSpeed.Relax.Min.ToString("0.00");
            fpSpread1.Sheets[0].Cells[10, 4].Text = Spec.MovingSpeed.Relax.Max.ToString("0.00");
            fpSpread1.Sheets[0].Cells[11, 3].Text = Spec.MovingSpeed.RelaxReturn.Min.ToString("0.00");
            fpSpread1.Sheets[0].Cells[11, 4].Text = Spec.MovingSpeed.RelaxReturn.Max.ToString("0.00");

            fpSpread1.Sheets[0].Cells[12, 3].Text = Spec.MovingSpeed.HeightUp.Min.ToString("0.00");
            fpSpread1.Sheets[0].Cells[12, 4].Text = Spec.MovingSpeed.HeightUp.Max.ToString("0.00");
            fpSpread1.Sheets[0].Cells[13, 3].Text = Spec.MovingSpeed.HeightDn.Min.ToString("0.00");
            fpSpread1.Sheets[0].Cells[13, 4].Text = Spec.MovingSpeed.HeightDn.Max.ToString("0.00");

            fpSpread1.Sheets[0].Cells[14, 3].Text = Spec.MovingSpeed.Legrest.Min.ToString("0.00");
            fpSpread1.Sheets[0].Cells[14, 4].Text = Spec.MovingSpeed.Legrest.Max.ToString("0.00");
            fpSpread1.Sheets[0].Cells[15, 3].Text = Spec.MovingSpeed.LegrestReturn.Min.ToString("0.00");
            fpSpread1.Sheets[0].Cells[15, 4].Text = Spec.MovingSpeed.LegrestReturn.Max.ToString("0.00");

            fpSpread1.Sheets[0].Cells[16, 3].Text = Spec.MovingSpeed.LegrestExt.Min.ToString("0.00");
            fpSpread1.Sheets[0].Cells[16, 4].Text = Spec.MovingSpeed.LegrestExt.Max.ToString("0.00");
            fpSpread1.Sheets[0].Cells[17, 3].Text = Spec.MovingSpeed.LegrestExtReturn.Min.ToString("0.00");
            fpSpread1.Sheets[0].Cells[17, 4].Text = Spec.MovingSpeed.LegrestExtReturn.Max.ToString("0.00");


            fpSpread1.Sheets[0].Cells[18, 3].Text = Spec.Sound.StartMax.ToString("0.00");
            fpSpread1.Sheets[0].Cells[18, 4].Text = Spec.Sound.RunMax.ToString("0.00");
            fpSpread1.Sheets[0].Cells[19, 4].Value = Spec.Sound.RMSMode == true ? "True" : "False";

            fpSpread1.Sheets[0].Cells[20, 4].Text = Spec.Sound.LimteCount.ToString();

            fpSpread1.Sheets[0].Cells[22, 4].Text = Spec.ReclinerLimitTime.ToString("0.0");
            fpSpread1.Sheets[0].Cells[23, 4].Text = Spec.RelaxToReclineLimitTime.ToString("0.0");
            fpSpread1.Sheets[0].Cells[24, 4].Text = Spec.RelaxLimitTime.ToString("0.0");
            fpSpread1.Sheets[0].Cells[25, 4].Text = Spec.LegrestLimitTime.ToString("0.0");
            fpSpread1.Sheets[0].Cells[26, 4].Text = Spec.LegrestExtLimitTime.ToString("0.0");
            fpSpread1.Sheets[0].Cells[27, 4].Text = Spec.HeightLimitTime.ToString("0.0");

            //fpSpread1.Sheets[0].Cells[10, 8].Text = Spec.MovingStroke.Recliner.ToString("0.0");
            //fpSpread1.Sheets[0].Cells[11, 8].Text = Spec.MovingStroke.Relax.ToString("0.0");
            //fpSpread1.Sheets[0].Cells[12, 8].Text = Spec.MovingStroke.Height.ToString("0.0");
            //fpSpread1.Sheets[0].Cells[13, 8].Text = Spec.MovingStroke.Legrest.ToString("0.0");
            //fpSpread1.Sheets[0].Cells[14, 8].Text = Spec.MovingStroke.LegrestExt.ToString("0.0");

            fpSpread1.Sheets[0].Cells[22, 2].Value = Spec.DeliveryPos.Recliner;
            fpSpread1.Sheets[0].Cells[23, 2].Value = Spec.DeliveryPos.RelaxToRecliner;
            fpSpread1.Sheets[0].Cells[24, 2].Value = Spec.DeliveryPos.Relax;
            fpSpread1.Sheets[0].Cells[25, 2].Value = Spec.DeliveryPos.Legrest;
            fpSpread1.Sheets[0].Cells[26, 2].Value = Spec.DeliveryPos.LegrestExt;
            fpSpread1.Sheets[0].Cells[27, 2].Value = Spec.DeliveryPos.Height;

            fpSpread1.Sheets[0].Cells[2, 8].Text = Spec.ReclinerTestTime.ToString("0.0");
            fpSpread1.Sheets[0].Cells[3, 8].Text = Spec.RelaxTestTime.ToString("0.0");
            fpSpread1.Sheets[0].Cells[4, 8].Text = Spec.HeightTestTime.ToString("0.0");
            fpSpread1.Sheets[0].Cells[5, 8].Text = Spec.LegrestTestTime.ToString("0.0");
            fpSpread1.Sheets[0].Cells[6, 8].Text = Spec.LegrestExtTestTime.ToString("0.0");

            fpSpread1.Sheets[0].Cells[13, 8].Text = Spec.Sound.StartTime.ToString("0.00");
            fpSpread1.Sheets[0].Cells[14, 8].Value = Spec.ReclinerFwdSoundCheckTime.ToString("0.0");
            fpSpread1.Sheets[0].Cells[15, 8].Value = Spec.ReclinerBwdSoundCheckTime.ToString("0.0");
            fpSpread1.Sheets[0].Cells[16, 8].Value = Spec.RelaxSoundCheckTime.ToString("0.0");
            fpSpread1.Sheets[0].Cells[17, 8].Value = Spec.HeightSoundCheckTime.ToString("0.0");
            fpSpread1.Sheets[0].Cells[18, 8].Value = Spec.LegrestSoundCheckTime.ToString("0.0");
            fpSpread1.Sheets[0].Cells[19, 8].Value = Spec.LegrestExtSoundCheckTime.ToString("0.0");

            fpSpread1.Sheets[0].Cells[29, 2].Value = Spec.ReclinerLimitCurr.ToString("0.0");
            fpSpread1.Sheets[0].Cells[30, 2].Value = Spec.RelaxLimitCurr.ToString("0.0");
            fpSpread1.Sheets[0].Cells[31, 2].Value = Spec.LegrestLimitCurr.ToString("0.0");
            fpSpread1.Sheets[0].Cells[32, 2].Value = Spec.LegrestExtLimitCurr.ToString("0.0");
            fpSpread1.Sheets[0].Cells[33, 2].Value = Spec.HeightLimitCurr.ToString("0.0");

            fpSpread1.Sheets[0].Cells[21, 8].Value = Spec.InComingVolt.ToString("0.0");
            fpSpread1.Sheets[0].Cells[22, 8].Value = Spec.TestVolt.ToString("0.0");
            fpSpread1.Sheets[0].Cells[23, 8].Value = Spec.RelaxSwOnTime.ToString("0.0");
            fpSpread1.Sheets[0].Cells[26, 8].Value = Spec.DeliveryPos.PowerReclinerStroke.ToString("0.00");
            fpSpread1.Sheets[0].Cells[27, 8].Value = Spec.DeliveryPos.RelaxStroke.ToString("0.00");
            fpSpread1.Sheets[0].Cells[28, 8].Value = Spec.DeliveryPos.MinStroke.ToString("0.00");
            fpSpread1.Sheets[0].Cells[29, 8].Value = Spec.DeliveryPos.MaxStroke.ToString("0.00");

            fpSpread1.Sheets[0].Cells[33, 8].Text = Spec.SWVer;
            return;
        }
        private void MoveSpec()
        {
            InitSpec();
            if (double.TryParse(fpSpread1.Sheets[0].Cells[3, 3].Text, out Spec.Current.Recliner.Min) == false) Spec.Current.Recliner.Min = 0;
            if (double.TryParse(fpSpread1.Sheets[0].Cells[3, 4].Text, out Spec.Current.Recliner.Max) == false) Spec.Current.Recliner.Max = 0;

            if (double.TryParse(fpSpread1.Sheets[0].Cells[4, 3].Text, out Spec.Current.Relax.Min) == false) Spec.Current.Relax.Min = 0;
            if (double.TryParse(fpSpread1.Sheets[0].Cells[4, 4].Text, out Spec.Current.Relax.Max) == false) Spec.Current.Relax.Max = 0;
            if (double.TryParse(fpSpread1.Sheets[0].Cells[5, 3].Text, out Spec.Current.Height.Min) == false) Spec.Current.Height.Min = 0;
            if (double.TryParse(fpSpread1.Sheets[0].Cells[5, 4].Text, out Spec.Current.Height.Max) == false) Spec.Current.Height.Max = 0;
            if (double.TryParse(fpSpread1.Sheets[0].Cells[6, 3].Text, out Spec.Current.Legrest.Min) == false) Spec.Current.Legrest.Min = 0;
            if (double.TryParse(fpSpread1.Sheets[0].Cells[6, 4].Text, out Spec.Current.Legrest.Max) == false) Spec.Current.Legrest.Max = 0;
            if (double.TryParse(fpSpread1.Sheets[0].Cells[7, 3].Text, out Spec.Current.LegrestExt.Min) == false) Spec.Current.LegrestExt.Min = 0;
            if (double.TryParse(fpSpread1.Sheets[0].Cells[7, 4].Text, out Spec.Current.LegrestExt.Max) == false) Spec.Current.LegrestExt.Max = 0;

            if (double.TryParse(fpSpread1.Sheets[0].Cells[8, 3].Text, out Spec.MovingSpeed.ReclinerFwd.Min) == false) Spec.MovingSpeed.ReclinerFwd.Min = 0;
            if (double.TryParse(fpSpread1.Sheets[0].Cells[8, 4].Text, out Spec.MovingSpeed.ReclinerFwd.Max) == false) Spec.MovingSpeed.ReclinerFwd.Max = 0;
            if (double.TryParse(fpSpread1.Sheets[0].Cells[9, 3].Text, out Spec.MovingSpeed.ReclinerBwd.Min) == false) Spec.MovingSpeed.ReclinerBwd.Min = 0;
            if (double.TryParse(fpSpread1.Sheets[0].Cells[9, 4].Text, out Spec.MovingSpeed.ReclinerBwd.Max) == false) Spec.MovingSpeed.ReclinerBwd.Max = 0;
            if (double.TryParse(fpSpread1.Sheets[0].Cells[10, 3].Text, out Spec.MovingSpeed.Relax.Min) == false) Spec.MovingSpeed.Relax.Min = 0;
            if (double.TryParse(fpSpread1.Sheets[0].Cells[10, 4].Text, out Spec.MovingSpeed.Relax.Max) == false) Spec.MovingSpeed.Relax.Max = 0;
            if (double.TryParse(fpSpread1.Sheets[0].Cells[11, 3].Text, out Spec.MovingSpeed.RelaxReturn.Min) == false) Spec.MovingSpeed.RelaxReturn.Min = 0;
            if (double.TryParse(fpSpread1.Sheets[0].Cells[11, 4].Text, out Spec.MovingSpeed.RelaxReturn.Max) == false) Spec.MovingSpeed.RelaxReturn.Max = 0;
            if (double.TryParse(fpSpread1.Sheets[0].Cells[12, 3].Text, out Spec.MovingSpeed.HeightUp.Min) == false) Spec.MovingSpeed.HeightUp.Min = 0;
            if (double.TryParse(fpSpread1.Sheets[0].Cells[12, 4].Text, out Spec.MovingSpeed.HeightUp.Max) == false) Spec.MovingSpeed.HeightUp.Max = 0;
            if (double.TryParse(fpSpread1.Sheets[0].Cells[13, 3].Text, out Spec.MovingSpeed.HeightDn.Min) == false) Spec.MovingSpeed.HeightDn.Min = 0;
            if (double.TryParse(fpSpread1.Sheets[0].Cells[13, 4].Text, out Spec.MovingSpeed.HeightDn.Max) == false) Spec.MovingSpeed.HeightDn.Max = 0;

            if (double.TryParse(fpSpread1.Sheets[0].Cells[14, 3].Text, out Spec.MovingSpeed.Legrest.Min) == false) Spec.MovingSpeed.Legrest.Min = 0;
            if (double.TryParse(fpSpread1.Sheets[0].Cells[14, 4].Text, out Spec.MovingSpeed.Legrest.Max) == false) Spec.MovingSpeed.Legrest.Max = 0;
            if (double.TryParse(fpSpread1.Sheets[0].Cells[15, 3].Text, out Spec.MovingSpeed.LegrestReturn.Min) == false) Spec.MovingSpeed.LegrestReturn.Min = 0;
            if (double.TryParse(fpSpread1.Sheets[0].Cells[15, 4].Text, out Spec.MovingSpeed.LegrestReturn.Max) == false) Spec.MovingSpeed.LegrestReturn.Max = 0;


            if (double.TryParse(fpSpread1.Sheets[0].Cells[16, 3].Text, out Spec.MovingSpeed.LegrestExt.Min) == false) Spec.MovingSpeed.LegrestExt.Min = 0;
            if (double.TryParse(fpSpread1.Sheets[0].Cells[16, 4].Text, out Spec.MovingSpeed.LegrestExt.Max) == false) Spec.MovingSpeed.LegrestExt.Max = 0;
            if (double.TryParse(fpSpread1.Sheets[0].Cells[17, 3].Text, out Spec.MovingSpeed.LegrestExtReturn.Min) == false) Spec.MovingSpeed.LegrestExtReturn.Min = 0;
            if (double.TryParse(fpSpread1.Sheets[0].Cells[17, 4].Text, out Spec.MovingSpeed.LegrestExtReturn.Max) == false) Spec.MovingSpeed.LegrestExtReturn.Max = 0;

            if (float.TryParse(fpSpread1.Sheets[0].Cells[18, 3].Text, out Spec.Sound.StartMax) == false) Spec.Sound.StartMax = 0;
            if (float.TryParse(fpSpread1.Sheets[0].Cells[18, 4].Text, out Spec.Sound.RunMax) == false) Spec.Sound.RunMax = 0;
            Spec.Sound.RMSMode = fpSpread1.Sheets[0].Cells[19, 4].Text == "True" ? true : false;

            if (int.TryParse(fpSpread1.Sheets[0].Cells[20, 4].Text, out Spec.Sound.LimteCount) == false) Spec.Sound.LimteCount = 0;

            if (float.TryParse(fpSpread1.Sheets[0].Cells[22, 4].Text, out Spec.ReclinerLimitTime) == false) Spec.ReclinerLimitTime = 0;
            if (float.TryParse(fpSpread1.Sheets[0].Cells[23, 4].Text, out Spec.RelaxToReclineLimitTime) == false) Spec.RelaxToReclineLimitTime = 0;
            if (float.TryParse(fpSpread1.Sheets[0].Cells[24, 4].Text, out Spec.RelaxLimitTime) == false) Spec.RelaxLimitTime = 0;
            if (float.TryParse(fpSpread1.Sheets[0].Cells[25, 4].Text, out Spec.LegrestLimitTime) == false) Spec.LegrestLimitTime = 0;
            if (float.TryParse(fpSpread1.Sheets[0].Cells[26, 4].Text, out Spec.LegrestExtLimitTime) == false) Spec.LegrestExtLimitTime = 0;
            if (float.TryParse(fpSpread1.Sheets[0].Cells[27, 4].Text, out Spec.HeightLimitTime) == false) Spec.HeightLimitTime = 0;

            //Spec.Can = fpSpread1.Sheets[0].Cells[8, 8].Text == "True" ? true : false;

            //if (float.TryParse(fpSpread1.Sheets[0].Cells[10, 8].Text, out Spec.MovingStroke.Recliner) == false) Spec.MovingStroke.Recliner = 0;
            //if (float.TryParse(fpSpread1.Sheets[0].Cells[11, 8].Text, out Spec.MovingStroke.Relax) == false) Spec.MovingStroke.Relax = 0;
            //if (float.TryParse(fpSpread1.Sheets[0].Cells[12, 8].Text, out Spec.MovingStroke.Height) == false) Spec.MovingStroke.Height = 0;
            //if (float.TryParse(fpSpread1.Sheets[0].Cells[13, 8].Text, out Spec.MovingStroke.Legrest) == false) Spec.MovingStroke.Legrest = 0;
            //if (float.TryParse(fpSpread1.Sheets[0].Cells[14, 8].Text, out Spec.MovingStroke.LegrestExt) == false) Spec.MovingStroke.LegrestExt = 0;

            if (float.TryParse(fpSpread1.Sheets[0].Cells[2, 8].Text, out Spec.ReclinerTestTime) == false) Spec.ReclinerTestTime = 0;
            if (float.TryParse(fpSpread1.Sheets[0].Cells[3, 8].Text, out Spec.RelaxTestTime) == false) Spec.RelaxTestTime = 0;
            if (float.TryParse(fpSpread1.Sheets[0].Cells[4, 8].Text, out Spec.HeightTestTime) == false) Spec.HeightTestTime = 0;
            if (float.TryParse(fpSpread1.Sheets[0].Cells[5, 8].Text, out Spec.LegrestTestTime) == false) Spec.LegrestTestTime = 0;
            if (float.TryParse(fpSpread1.Sheets[0].Cells[6, 8].Text, out Spec.LegrestExtTestTime) == false) Spec.LegrestExtTestTime = 0;

            var s1 = fpSpread1.Sheets[0].Cells[22, 2].Text;
            var s2 = fpSpread1.Sheets[0].Cells[23, 2].Text;
            var s3 = fpSpread1.Sheets[0].Cells[24, 2].Text;
            var s4 = fpSpread1.Sheets[0].Cells[25, 2].Text;
            var s5 = fpSpread1.Sheets[0].Cells[26, 2].Text;
            var s6 = fpSpread1.Sheets[0].Cells[26, 2].Text;

            if ((string)s1 == "FWD")
                Spec.DeliveryPos.Recliner = 0;
            else if ((string)s1 == "BWd")
                Spec.DeliveryPos.Recliner = 2;
            else Spec.DeliveryPos.Recliner = 1;


            if ((string)s2 == "FWD")
                Spec.DeliveryPos.RelaxToRecliner = 0;
            else if ((string)s2 == "BWd")
                Spec.DeliveryPos.RelaxToRecliner = 2;
            else Spec.DeliveryPos.RelaxToRecliner = 1;

            Spec.DeliveryPos.Relax = (string)s3 == "RELAX" ? (short)0 : (short)1;
            Spec.DeliveryPos.Legrest = (string)s4 == "LEGREST" ? (short)0 : (short)1;
            Spec.DeliveryPos.LegrestExt = (string)s5 == "LEGREST" ? (short)0 : (short)1;
            Spec.DeliveryPos.Height = (string)s6 == "UP" ? (short)0 : (short)1;

            if (float.TryParse(fpSpread1.Sheets[0].Cells[13, 8].Text, out Spec.Sound.StartTime) == false) Spec.Sound.StartTime = 0;
            if (float.TryParse(fpSpread1.Sheets[0].Cells[14, 8].Text, out Spec.ReclinerFwdSoundCheckTime) == false) Spec.ReclinerFwdSoundCheckTime = 0;
            if (float.TryParse(fpSpread1.Sheets[0].Cells[15, 8].Text, out Spec.ReclinerBwdSoundCheckTime) == false) Spec.ReclinerBwdSoundCheckTime = 0;
            if (float.TryParse(fpSpread1.Sheets[0].Cells[16, 8].Text, out Spec.RelaxSoundCheckTime) == false) Spec.RelaxSoundCheckTime = 0;
            if (float.TryParse(fpSpread1.Sheets[0].Cells[17, 8].Text, out Spec.HeightSoundCheckTime) == false) Spec.HeightSoundCheckTime = 0;
            if (float.TryParse(fpSpread1.Sheets[0].Cells[18, 8].Text, out Spec.LegrestSoundCheckTime) == false) Spec.LegrestSoundCheckTime = 0;
            if (float.TryParse(fpSpread1.Sheets[0].Cells[19, 8].Text, out Spec.LegrestExtSoundCheckTime) == false) Spec.LegrestExtSoundCheckTime = 0;
            //if (float.TryParse(fpSpread1.Sheets[0].Cells[22, 8].Text, out Spec.Sound.기도음구동음간격시간) == false) Spec.Sound.기도음구동음간격시간 = 0;
            
            if (float.TryParse(fpSpread1.Sheets[0].Cells[29, 2].Text, out Spec.ReclinerLimitCurr) == false) Spec.ReclinerLimitCurr = 0;            
            if (float.TryParse(fpSpread1.Sheets[0].Cells[30, 2].Text, out Spec.RelaxLimitCurr) == false) Spec.RelaxLimitCurr = 0;
            if (float.TryParse(fpSpread1.Sheets[0].Cells[31, 2].Text, out Spec.LegrestLimitCurr) == false) Spec.LegrestLimitCurr = 0;
            if (float.TryParse(fpSpread1.Sheets[0].Cells[32, 2].Text, out Spec.LegrestExtLimitCurr) == false) Spec.LegrestExtLimitCurr = 0;
            if (float.TryParse(fpSpread1.Sheets[0].Cells[33, 2].Text, out Spec.HeightLimitCurr) == false) Spec.HeightLimitCurr = 0;

            if (float.TryParse(fpSpread1.Sheets[0].Cells[21, 8].Text, out Spec.InComingVolt) == false) Spec.InComingVolt = 13.6F;
            if (float.TryParse(fpSpread1.Sheets[0].Cells[22, 8].Text, out Spec.TestVolt) == false) Spec.TestVolt = 13.6F;
            if (float.TryParse(fpSpread1.Sheets[0].Cells[23, 8].Text, out Spec.RelaxSwOnTime) == false) Spec.RelaxSwOnTime = 3F;

            if (float.TryParse(fpSpread1.Sheets[0].Cells[26, 8].Text, out Spec.DeliveryPos.PowerReclinerStroke) == false) Spec.DeliveryPos.PowerReclinerStroke = 3F;
            if (float.TryParse(fpSpread1.Sheets[0].Cells[27, 8].Text, out Spec.DeliveryPos.RelaxStroke) == false) Spec.DeliveryPos.RelaxStroke = 3F;
            if (float.TryParse(fpSpread1.Sheets[0].Cells[28, 8].Text, out Spec.DeliveryPos.MinStroke) == false) Spec.DeliveryPos.MinStroke = 3F;
            if (float.TryParse(fpSpread1.Sheets[0].Cells[29, 8].Text, out Spec.DeliveryPos.MaxStroke) == false) Spec.DeliveryPos.MaxStroke = 3F;

            Spec.SWVer = fpSpread1.Sheets[0].Cells[33, 8].Text;
            return;
        }
        public void InitSpec()
        {
            Spec.ModelName = string.Empty;

            Spec.Current.Recliner.Min = 0;
            Spec.Current.Recliner.Max = 0;

            Spec.Current.Relax.Min = 0;
            Spec.Current.Relax.Max = 0;
            Spec.Current.Height.Min = 0;
            Spec.Current.Height.Max = 0;
            Spec.Current.Legrest.Min = 0;
            Spec.Current.Legrest.Max = 0;
            Spec.Current.LegrestExt.Min = 0;
            Spec.Current.LegrestExt.Max = 0;

            Spec.MovingSpeed.ReclinerFwd.Min = 0;
            Spec.MovingSpeed.ReclinerFwd.Max = 0;
            Spec.MovingSpeed.ReclinerBwd.Min = 0;
            Spec.MovingSpeed.ReclinerBwd.Max = 0;
            Spec.MovingSpeed.Relax.Min = 0;
            Spec.MovingSpeed.Relax.Max = 0;
            Spec.MovingSpeed.RelaxReturn.Min = 0;
            Spec.MovingSpeed.RelaxReturn.Max = 0;
            Spec.MovingSpeed.HeightUp.Min = 0;
            Spec.MovingSpeed.HeightUp.Max = 0;
            Spec.MovingSpeed.HeightDn.Min = 0;
            Spec.MovingSpeed.HeightDn.Max = 0;

            Spec.MovingSpeed.Legrest.Min = 0;
            Spec.MovingSpeed.Legrest.Max = 0;
            Spec.MovingSpeed.LegrestReturn.Min = 0;
            Spec.MovingSpeed.LegrestReturn.Max = 0;


            Spec.MovingSpeed.LegrestExt.Min = 0;
            Spec.MovingSpeed.LegrestExt.Max = 0;
            Spec.MovingSpeed.LegrestExtReturn.Min = 0;
            Spec.MovingSpeed.LegrestExtReturn.Max = 0;

            Spec.Sound.StartMax = 0;
            Spec.Sound.RunMax = 0;
            Spec.Sound.RMSMode = false;

            Spec.Sound.LimteCount = 0;

            Spec.ReclinerLimitTime = 0;
            Spec.RelaxLimitTime = 0;
            Spec.LegrestLimitTime = 0;
            Spec.LegrestExtLimitTime = 0;
            Spec.HeightLimitTime = 0;

            //Spec.Can = fpSpread1.Sheets[0].Cells[8, 8].Text == "True" ? true : false;

            Spec.MovingStroke.Recliner = 0;
            Spec.MovingStroke.Relax = 0;
            Spec.MovingStroke.Height = 0;
            Spec.MovingStroke.Legrest = 0;
            Spec.MovingStroke.LegrestExt = 0;

            Spec.ReclinerTestTime = 0;
            Spec.RelaxTestTime = 0;
            Spec.HeightTestTime = 0;
            Spec.LegrestTestTime = 0;
            Spec.LegrestExtTestTime = 0;


            Spec.DeliveryPos.Recliner = 0;
            Spec.DeliveryPos.RelaxToRecliner = 0;
            Spec.DeliveryPos.Relax = 0;
            Spec.DeliveryPos.Legrest = 0;
            Spec.DeliveryPos.LegrestExt = 0;
            Spec.DeliveryPos.Height = 0;

            Spec.Sound.StartTime = 0;
            Spec.ReclinerFwdSoundCheckTime = 0;
            Spec.ReclinerBwdSoundCheckTime = 0;
            Spec.RelaxSoundCheckTime = 0;
            Spec.HeightSoundCheckTime = 0;
            Spec.LegrestSoundCheckTime = 0;
            Spec.LegrestExtSoundCheckTime = 0;
            //Spec.Sound.기도음구동음간격시간 = 0;

            Spec.ReclinerLimitCurr = 0;
            Spec.RelaxLimitCurr = 0;
            Spec.LegrestLimitCurr = 0;
            Spec.LegrestExtLimitCurr = 0;
            Spec.HeightLimitCurr = 0;

            Spec.InComingVolt = 13.6F;
            Spec.TestVolt = 13.6F;
            Spec.RelaxSwOnTime = 3F;

            Spec.DeliveryPos.PowerReclinerStroke = 3F;
            Spec.DeliveryPos.RelaxStroke = 3F;
            Spec.DeliveryPos.MinStroke = 3F;
            Spec.DeliveryPos.MaxStroke = 3F;
            Spec.SWVer = "";
            return;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ModelBoxChangeFlag == true) return;
            string s = comboBox1.SelectedItem.ToString();

            string sName = Program.SPEC_PATH.ToString() + "\\" + s + ".Spc";

            InitSpec();
            if (File.Exists(sName) == true) mControl.공용함수.OpenSpec(sName, ref Spec);
            mName = Spec.ModelName;
            DisplaySpec();
            return;
        }
    }
}
