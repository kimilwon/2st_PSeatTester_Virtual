using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace PSeatTester
{
    static class Program
    {
        public static DirectoryInfo MAIN_PATH;
        public static DirectoryInfo SPEC_PATH;
        public static DirectoryInfo IMAGE_PATH;
        public static DirectoryInfo DATA_PATH;
        public static DirectoryInfo SYSTEM_PATH;
        public static DirectoryInfo INFOR_PATH;
        public static DirectoryInfo LOG_PATH;
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // GUID 대신 사용자 임의대로 뮤텍스 이름 사용  
            string mtxName = "LED 내구 검사기";

            //------------ 방법 1
            /*
            Mutex mtx = new Mutex(true, mtxName);

            // 1초 동안 뮤텍스를 획득하려 대기  
            TimeSpan tsWait = new TimeSpan(0, 0, 1);
            bool success = mtx.WaitOne(tsWait);

            // 실패하면 프로그램 종료  
            if (!success)
            {
                MessageBox.Show("이미실행중입니다.");
                return;

            }
            */
            //------------ 방법 2

            bool bnew;
            Mutex mutex = new Mutex(true, mtxName, out bnew);

            if (bnew == true)
            {
                string XPath;

                //if (this.DesignMode == true) return; //디자인 창 안뜨는것 해결함수라 한다.   
                XPath = Application.StartupPath;
                MAIN_PATH = new DirectoryInfo(XPath);
                if (MAIN_PATH.Exists == false) MAIN_PATH.Create();

                XPath = Application.StartupPath + "\\SPEC";
                SPEC_PATH = new DirectoryInfo(XPath);
                if (SPEC_PATH.Exists == false) SPEC_PATH.Create();

                XPath = Application.StartupPath + "\\IMAGE";
                IMAGE_PATH = new DirectoryInfo(XPath);
                if (IMAGE_PATH.Exists == false) IMAGE_PATH.Create();

                XPath = Application.StartupPath + "\\SYSTEM";
                SYSTEM_PATH = new DirectoryInfo(XPath);
                if (SYSTEM_PATH.Exists == false) SYSTEM_PATH.Create();

                XPath = Application.StartupPath + "\\LOG DATA";
                LOG_PATH = new DirectoryInfo(XPath);
                if (LOG_PATH.Exists == false) LOG_PATH.Create();

                bool Flag = false;
                XPath = Application.StartupPath;
                DriveInfo[] allDrives = DriveInfo.GetDrives();
                foreach (DriveInfo d in allDrives)
                {
                    if (d.IsReady == true)
                    {
                        if (0 <= XPath.IndexOf(d.Name))
                        {
                            XPath = d.Name + "DB";
                            Flag = true;
                            break;
                        }
                    }
                }
                if (Flag == false) XPath = Application.StartupPath + "\\DATA";
                DATA_PATH = new DirectoryInfo(XPath);
                if (DATA_PATH.Exists == false) DATA_PATH.Create();


                XPath = Application.StartupPath + "\\INFOR";
                INFOR_PATH = new DirectoryInfo(XPath);
                if (INFOR_PATH.Exists == false) INFOR_PATH.Create();


                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
            else
            {
                MessageBox.Show("이미실행중입니다.");
            }
            return;
        }
    }
}
