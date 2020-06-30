using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PSeatTester
{
    public partial class IOMaping : Form
    {
        private MyInterface mControl = null;
        private PinMapStruct PinMap;


        public IOMaping()
        {
            InitializeComponent();
        }

        public IOMaping(MyInterface mControl,PinMapStruct PinMap)
        {
            InitializeComponent();
            this.mControl = mControl;
            this.PinMap = PinMap;
            DisplayPinMap(PinMap);
        }

        public PinMapStruct GetPinMap
        {
            get { return PinMap; }
        }

        private void imageButton2_Click(object sender, EventArgs e)
        {
            this.Close();
            return;
        }

        private void DisplayPinMap(PinMapStruct pMap)
        {
            //fpSpread1.Sheets[0].Cells[2, 3].Value = pMap.IMSSet.PinNo;
            //fpSpread1.Sheets[0].Cells[3, 3].Value = pMap.IMSSet.Mode;

            //fpSpread1.Sheets[0].Cells[4, 3].Value = pMap.IMSM1.PinNo;
            //fpSpread1.Sheets[0].Cells[5, 3].Value = pMap.IMSM1.Mode;

            //fpSpread1.Sheets[0].Cells[6, 3].Value = pMap.IMSM2.PinNo;
            //fpSpread1.Sheets[0].Cells[7, 3].Value = pMap.IMSM2.Mode;

            checkBox1.Checked = pMap.PSeat_직구동;
            checkBox2.Checked = pMap.WalkIn;

            fpSpread1.Sheets[0].Cells[8, 3].Value = pMap.SlideFWD.PinNo;
            fpSpread1.Sheets[0].Cells[9, 3].Value = pMap.SlideFWD.Mode;

            fpSpread1.Sheets[0].Cells[10, 3].Value = pMap.SlideBWD.PinNo;
            fpSpread1.Sheets[0].Cells[11, 3].Value = pMap.SlideBWD.Mode;

            fpSpread1.Sheets[0].Cells[12, 3].Value = pMap.ReclineFWD.PinNo;
            fpSpread1.Sheets[0].Cells[13, 3].Value = pMap.ReclineFWD.Mode;

            fpSpread1.Sheets[0].Cells[14, 3].Value = pMap.ReclineBWD.PinNo;
            fpSpread1.Sheets[0].Cells[15, 3].Value = pMap.ReclineBWD.Mode;

            fpSpread1.Sheets[0].Cells[16, 3].Value = pMap.TiltUp.PinNo;
            fpSpread1.Sheets[0].Cells[17, 3].Value = pMap.TiltUp.Mode;

            fpSpread1.Sheets[0].Cells[18, 3].Value = pMap.TiltDn.PinNo;
            fpSpread1.Sheets[0].Cells[19, 3].Value = pMap.TiltDn.Mode;

            fpSpread1.Sheets[0].Cells[20, 3].Value = pMap.HeightUp.PinNo;
            fpSpread1.Sheets[0].Cells[21, 3].Value = pMap.HeightUp.Mode;

            fpSpread1.Sheets[0].Cells[22, 3].Value = pMap.HeightDn.PinNo;
            fpSpread1.Sheets[0].Cells[23, 3].Value = pMap.HeightDn.Mode;

            fpSpread1.Sheets[0].Cells[24, 3].Value = pMap.LumberFwdBwd.Batt;
            fpSpread1.Sheets[0].Cells[25, 3].Value = pMap.LumberFwdBwd.Gnd;

            fpSpread1.Sheets[0].Cells[26, 3].Value = pMap.LumberUpDn.Batt;
            fpSpread1.Sheets[0].Cells[27, 3].Value = pMap.LumberUpDn.Gnd;

            fpSpread1.Sheets[0].Cells[28, 3].Value = pMap.Power.Batt1.Batt;
            fpSpread1.Sheets[0].Cells[29, 3].Value = pMap.Power.Batt1.Gnd;

            fpSpread1.Sheets[0].Cells[30, 3].Value = pMap.Power.Batt2.Batt;
            fpSpread1.Sheets[0].Cells[31, 3].Value = pMap.Power.Batt2.Gnd;
            return;
        }

        
        private void MovePinMap(ref PinMapStruct PMap)
        {
            //PMap.IMSSet.PinNo = (int)fpSpread1.Sheets[0].Cells[2, 3].Value;
            //PMap.IMSSet.Mode = (int)fpSpread1.Sheets[0].Cells[3, 3].Value;

            //PMap.IMSM1.PinNo = (int)fpSpread1.Sheets[0].Cells[4, 3].Value;
            //PMap.IMSM1.Mode = (int)fpSpread1.Sheets[0].Cells[5, 3].Value;

            //PMap.IMSM2.PinNo = (int)fpSpread1.Sheets[0].Cells[6, 3].Value;
            //PMap.IMSM2.Mode = (int)fpSpread1.Sheets[0].Cells[7, 3].Value;
            PMap.PSeat_직구동 = checkBox1.Checked;
            PMap.WalkIn = checkBox2.Checked;

            PMap.SlideFWD.PinNo = (int)fpSpread1.Sheets[0].Cells[8, 3].Value;
            PMap.SlideFWD.Mode = (int)fpSpread1.Sheets[0].Cells[9, 3].Value;

            PMap.SlideBWD.PinNo = (int)fpSpread1.Sheets[0].Cells[10, 3].Value;
            PMap.SlideBWD.Mode = (int)fpSpread1.Sheets[0].Cells[11, 3].Value;

            PMap.ReclineFWD.PinNo = (int)fpSpread1.Sheets[0].Cells[12, 3].Value;
            PMap.ReclineFWD.Mode = (int)fpSpread1.Sheets[0].Cells[13, 3].Value;

            PMap.ReclineBWD.PinNo = (int)fpSpread1.Sheets[0].Cells[14, 3].Value;
            PMap.ReclineBWD.Mode = (int)fpSpread1.Sheets[0].Cells[15, 3].Value;


            PMap.TiltUp.PinNo = (int)fpSpread1.Sheets[0].Cells[16, 3].Value;
            PMap.TiltUp.Mode = (int)fpSpread1.Sheets[0].Cells[17, 3].Value;

            PMap.TiltDn.PinNo = (int)fpSpread1.Sheets[0].Cells[18, 3].Value;
            PMap.TiltDn.Mode = (int)fpSpread1.Sheets[0].Cells[19, 3].Value;

            PMap.HeightUp.PinNo = (int)fpSpread1.Sheets[0].Cells[20, 3].Value;
            PMap.HeightUp.Mode = (int)fpSpread1.Sheets[0].Cells[21, 3].Value;

            PMap.HeightDn.PinNo = (int)fpSpread1.Sheets[0].Cells[22, 3].Value;
            PMap.HeightDn.Mode = (int)fpSpread1.Sheets[0].Cells[23, 3].Value;

            PMap.LumberFwdBwd.Batt = (int)fpSpread1.Sheets[0].Cells[24, 3].Value;
            PMap.LumberFwdBwd.Gnd = (int)fpSpread1.Sheets[0].Cells[25, 3].Value;

            PMap.LumberUpDn.Batt = (int)fpSpread1.Sheets[0].Cells[26, 3].Value;
            PMap.LumberUpDn.Gnd = (int)fpSpread1.Sheets[0].Cells[27, 3].Value;

            PMap.Power.Batt1.Batt = (int)fpSpread1.Sheets[0].Cells[28, 3].Value;
            PMap.Power.Batt1.Gnd = (int)fpSpread1.Sheets[0].Cells[29, 3].Value;

            PMap.Power.Batt2.Batt = (int)fpSpread1.Sheets[0].Cells[30, 3].Value;
            PMap.Power.Batt2.Gnd = (int)fpSpread1.Sheets[0].Cells[31, 3].Value;
            return;
        }

        private void imageButton3_Click(object sender, EventArgs e)
        {
            //복사
            PinMapStruct pMap = mControl.GetPinMap;

            MovePinMap(ref pMap);
            mControl.GetPinMap = pMap;
            return;
        }

        private void imageButton1_Click(object sender, EventArgs e)
        {
            //저장
            MovePinMap(ref PinMap);
            return;
        }

        private void imageButton4_Click(object sender, EventArgs e)
        {
            //붙여넣기
            PinMapStruct pMap = mControl.GetPinMap;

            DisplayPinMap(pMap);
            return;
        }
    }
}
