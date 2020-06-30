#define PROGRAM_RUNNING

using PSeatTester.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

public class InputBox : IDisposable
{
    public static DialogResult Show(string title, string promptText, ref string value)
    {
        return Show(title, promptText, ref value, null);
    }
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
        return;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {

        }
        return;
    }

    public static DialogResult Show(string title, string promptText, ref string value,
                                    InputBoxValidation validation)
    {
        Form form = new Form();
        Label label = new Label();
        TextBox textBox = new TextBox();
        Button buttonOk = new Button();
        Button buttonCancel = new Button();

        form.Text = title;
        label.Text = promptText;
        textBox.Text = value;

        buttonOk.Text = "OK";
        buttonCancel.Text = "Cancel";
        buttonOk.DialogResult = DialogResult.OK;
        buttonCancel.DialogResult = DialogResult.Cancel;

        label.SetBounds(9, 20, 372, 13);
        textBox.SetBounds(12, 36, 372, 20);
        buttonOk.SetBounds(228, 72, 75, 23);
        buttonCancel.SetBounds(309, 72, 75, 23);

        label.AutoSize = true;
        textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
        buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

        form.ClientSize = new Size(396, 107);
        form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
        form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
        form.FormBorderStyle = FormBorderStyle.FixedDialog;
        form.StartPosition = FormStartPosition.CenterScreen;
        form.MinimizeBox = false;
        form.MaximizeBox = false;
        form.TopMost = true;
        form.AcceptButton = buttonOk;
        form.CancelButton = buttonCancel;
        if (validation != null)
        {
            form.FormClosing += delegate (object sender, FormClosingEventArgs e)
            {
                if (form.DialogResult == DialogResult.OK)
                {
                    string errorText = validation(textBox.Text);
                    if (e.Cancel = (errorText != ""))
                    {
                        MessageBox.Show(form, errorText, "Validation Error",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                        textBox.Focus();
                    }
                }
            };
        }
        DialogResult dialogResult = form.ShowDialog();
        value = textBox.Text;
        return dialogResult;
    }
}
public delegate string InputBoxValidation(string errorMessage);


public class uMessageBox
{
    public static void Show(string title = "경고", string promptText = null)
    {
        xShow(title, promptText);
        return;
    }

    public static void xShow(string title, string promptText)
    {
        MessageBox.Show(caption: title, text: promptText, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Warning,defaultButton:MessageBoxDefaultButton.Button1,options:MessageBoxOptions.ServiceNotification);
    }
}


public class uxMessageBox : IDisposable
{
    static Button[] Buttonx;
    static Form form = new Form();
    
    public static DialogResult Show(string title, string promptText, string[] sButton, DialogResult[] Result)
    {
        DialogResult sResult = Show(title, promptText, sButton, Result, null);
        return sResult;
    }

    public static DialogResult Show(string title = "경고", string promptText = null)
    {
        DialogResult sResult = Show(title, promptText, null);
        return sResult;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
        return;
    }

    
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {

        }
        return;
    }

    public static DialogResult Show(string title, string promptText, string[] sButton, DialogResult[] Result,
                                    uxMessageBoxValidation validation)
    {
        Label label = new Label();
        Buttonx = new Button[sButton.Length];
        PictureBox picture = new PictureBox()
        {
            Parent = form,
            Image = PSeatTester.Properties.Resources.Danger_Shield,

        };
        picture.SetBounds(9, 10, 32, 32);
                

        int i = 0;
        int bSize;

        for (i = 0; i < Buttonx.Length; i++)
        {
            Buttonx[i] = new Button()
            {
                AutoSize = false
            };
        }

        i = 0;
        foreach (string s in sButton)
        {
            Buttonx[i].Text = s;
            //Buttonx[i].AutoSize = true;
            i++;
        }
        int Max = 0;
        foreach (string s in sButton)
        {
            label.Text = s;
            if (Max < label.Width) Max = label.Right;
        }

        bSize = Max + 10;

        form.Text = title;
        label.Text = promptText;


        int Size = 0;
        for (i = 0; i < sButton.Length; i++)
        {
            Size += bSize + 8;
        }

        //label.SetBounds(9, 20, 372, 13);
        label.SetBounds(50, 20, 372, 13);
        label.Parent = form;
        label.AutoSize = true;

        Max = Math.Max(300, label.Right + 10);
        Max = Math.Max(Size, label.Right + 10);


        form.ClientSize = new Size(396 + 50, 107);
        form.ClientSize = new Size(Math.Max(300, Max) + 50, form.ClientSize.Height);

        int Widgth = (form.Width / 2) - (Size / 2);
        int X = 0;

        X = Widgth;
        for (i = 0; i < sButton.Length; i++)
        {
            Buttonx[i].SetBounds(X, 72, bSize, 23);
            X += bSize + 8;
            Buttonx[i].Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            Buttonx[i].Parent = form;
            Buttonx[i].Visible = true;
            Buttonx[i].DialogResult = Result[i];
            Buttonx[i].Click += new EventHandler(ButtonClick);
        }

        form.FormBorderStyle = FormBorderStyle.FixedDialog;
        form.StartPosition = FormStartPosition.CenterScreen;
        form.MinimizeBox = false;
        form.MaximizeBox = false;
        form.ControlBox = false;
        form.TopMost = true;

        if (validation != null)
        {
            form.FormClosing += delegate (object sender, FormClosingEventArgs e)
            {
                if (form.DialogResult == DialogResult.OK)
                {
                }
                else if (form.DialogResult == DialogResult.No)
                {
                }
                else if (form.DialogResult == DialogResult.Cancel)
                {
                }
                form.Dispose();
                form = null;
            };
        }
        DialogResult dialogResult = form.ShowDialog();
        return dialogResult;
    }

    public static DialogResult Show(string title, string promptText, uxMessageBoxValidation validation)
    {
        Label label = new Label();
        Buttonx = new Button[1];
        Buttonx[0] = new Button() { AutoSize = false };
        PictureBox picture = new PictureBox()
        {
            Parent = form,
            Image = PSeatTester.Properties.Resources.Danger_Shield

        };
        picture.SetBounds(9, 10, 32, 32);

        System.Drawing.Graphics formGraphics = form.CreateGraphics();

        float FWidth = (form.Font.SizeInPoints / 72) * formGraphics.DpiX;
        int FHeight = form.Font.Height;

        Buttonx[0].Text = "확인";

        int bSize = (int)("확인".Length * FWidth);
        int Size = bSize + 50;

        form.Text = title;
        label.Text = promptText;

        //label.SetBounds(9, 20, 372, 13);
        label.SetBounds(50, 20, 372, 13);
        label.Parent = form;
        label.AutoSize = true;

        int Max = 0;
        Max = Math.Max(300, label.Right + 10);
        Max = Math.Max(Size, label.Right + 10);


        form.ClientSize = new Size(396 + 50, 107);
        form.ClientSize = new Size(Math.Max(300, Max) + 50, form.ClientSize.Height);

        int Widgth = (form.Width / 2) - (Size / 2);
        int X = 0;

        X = Widgth;
        //for (i = 0; i < sButton.Length; i++)
        //{
        //    Buttonx[i].SetBounds(X, 72, bSize, 23);
        //    X += bSize + 8;
        //    Buttonx[i].Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        //    Buttonx[i].Parent = form;
        //    Buttonx[i].Visible = true;
        //    Buttonx[i].DialogResult = Result[i];
        //    Buttonx[i].Click += new EventHandler(ButtonClick);
        //}

        Buttonx[0].SetBounds(X, 72, Size, 23);
        X += bSize + 8;
        Buttonx[0].Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        Buttonx[0].Parent = form;
        Buttonx[0].Visible = true;
        Buttonx[0].DialogResult = DialogResult.OK;
        Buttonx[0].Click += new EventHandler(ButtonClick);

        form.FormBorderStyle = FormBorderStyle.FixedDialog;
        form.StartPosition = FormStartPosition.CenterScreen;
        form.MinimizeBox = false;
        form.MaximizeBox = false;
        form.ControlBox = false;
        form.TopMost = true;

        if (validation != null)
        {
            form.FormClosing += delegate (object sender, FormClosingEventArgs e)
            {
                if (form.DialogResult == DialogResult.OK)
                {
                }
                else if (form.DialogResult == DialogResult.No)
                {
                }
                else if (form.DialogResult == DialogResult.Cancel)
                {
                }
                form.Dispose();
            };
        }
        DialogResult dialogResult = form.ShowDialog();
        return dialogResult;
    }

    ~uxMessageBox()
    {
        
    }

    private static void ButtonClick(object sender, EventArgs e)
    {
        form.Close();        
        return;
    }

    private void FormClosing(object sender, FormClosingEventArgs e)
    {
        form.Dispose();
        form = null;
    }
}
public delegate string uxMessageBoxValidation(string errorMessage);




namespace PSeatTester
{
    //public class InputBox : IDisposable
    //{
    //    public static DialogResult Show(string title, string promptText, ref string value)
    //    {
    //        return Show(title, promptText, ref value, null);
    //    }

    //    public static DialogResult Show(string title, string promptText, ref string value,
    //                                    InputBoxValidation validation)
    //    {
    //        Form form = new Form();
    //        Label label = new Label();
    //        TextBox textBox = new TextBox();
    //        Button buttonOk = new Button();
    //        Button buttonCancel = new Button();

    //        form.Text = title;
    //        label.Text = promptText;
    //        textBox.Text = value;

    //        buttonOk.Text = "OK";
    //        buttonCancel.Text = "Cancel";
    //        buttonOk.DialogResult = DialogResult.OK;
    //        buttonCancel.DialogResult = DialogResult.Cancel;

    //        label.SetBounds(9, 20, 372, 13);
    //        textBox.SetBounds(12, 36, 372, 20);
    //        buttonOk.SetBounds(228, 72, 75, 23);
    //        buttonCancel.SetBounds(309, 72, 75, 23);

    //        label.AutoSize = true;
    //        textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
    //        buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    //        buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

    //        form.ClientSize = new Size(396, 107);
    //        form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
    //        form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
    //        form.FormBorderStyle = FormBorderStyle.FixedDialog;
    //        form.StartPosition = FormStartPosition.CenterScreen;
    //        form.MinimizeBox = false;
    //        form.MaximizeBox = false;
    //        form.TopMost = true;
    //        form.AcceptButton = buttonOk;
    //        form.CancelButton = buttonCancel;
    //        if (validation != null)
    //        {
    //            form.FormClosing += delegate (object sender, FormClosingEventArgs e)
    //            {
    //                if (form.DialogResult == DialogResult.OK)
    //                {
    //                    string errorText = validation(textBox.Text);
    //                    if (e.Cancel = (errorText != ""))
    //                    {
    //                        MessageBox.Show(form, errorText, "Validation Error",
    //                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
    //                        textBox.Focus();
    //                    }
    //                }
    //            };
    //        }
    //        DialogResult dialogResult = form.ShowDialog();
    //        value = textBox.Text;
    //        return dialogResult;
    //    }

    //    public void Dispose()
    //    {
    //        Dispose(true);
    //        GC.SuppressFinalize(this);
    //        return;
    //    }

    //    protected virtual void Dispose(bool disposing)
    //    {
    //        if (disposing)
    //        {

    //        }
    //        return;
    //    }
    //}
    //public delegate string InputBoxValidation(string errorMessage);


    //public class uMessageBox : IDisposable
    //{
    //    static Button[] Buttonx;
    //    static Form form = new Form();

    //    public static DialogResult Show(string title, string promptText, string[] sButton, DialogResult[] Result)
    //    {
    //        DialogResult sResult = Show(title, promptText, sButton, Result, null);
    //        return sResult;
    //    }

    //    public static DialogResult Show(string title, string promptText, string[] sButton, DialogResult[] Result,
    //                                    uMessageBoxValidation validation)
    //    {
    //        Label label = new Label();
    //        Buttonx = new Button[sButton.Length];
    //        PictureBox picture = new PictureBox()
    //        {
    //            Parent = form,
    //            Image = PSeatTester.Properties.Resources.Browser,

    //        };
    //        picture.SetBounds(9, 10, 32, 32);

    //        int i = 0;
    //        int bSize;

    //        for (i = 0; i < Buttonx.Length; i++)
    //        {
    //            Buttonx[i] = new Button()
    //            {
    //                AutoSize = false
    //            };
    //        }

    //        i = 0;
    //        foreach (string s in sButton)
    //        {
    //            Buttonx[i].Text = s;
    //            //Buttonx[i].AutoSize = true;
    //            i++;
    //        }
    //        int Max = 0;
    //        foreach (string s in sButton)
    //        {
    //            label.Text = s;
    //            if (Max < label.Width) Max = label.Right;
    //        }

    //        bSize = Max + 10;

    //        form.Text = title;
    //        label.Text = promptText;


    //        int Size = 0;
    //        for (i = 0; i < sButton.Length; i++)
    //        {
    //            Size += bSize + 8;
    //        }

    //        //label.SetBounds(9, 20, 372, 13);
    //        label.SetBounds(50, 20, 372, 13);
    //        label.Parent = form;
    //        label.AutoSize = true;

    //        Max = Math.Max(300, label.Right + 10);
    //        Max = Math.Max(Size, label.Right + 10);


    //        form.ClientSize = new Size(396 + 50, 107);
    //        form.ClientSize = new Size(Math.Max(300, Max) + 50, form.ClientSize.Height);

    //        int Widgth = (form.Width / 2) - (Size / 2);
    //        int X = 0;

    //        X = Widgth;
    //        for (i = 0; i < sButton.Length; i++)
    //        {
    //            Buttonx[i].SetBounds(X, 72, bSize, 23);
    //            X += bSize + 8;
    //            Buttonx[i].Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    //            Buttonx[i].Parent = form;
    //            Buttonx[i].Visible = true;
    //            Buttonx[i].DialogResult = Result[i];
    //            Buttonx[i].Click += new EventHandler(ButtonClick);
    //        }

    //        form.FormBorderStyle = FormBorderStyle.FixedDialog;
    //        form.StartPosition = FormStartPosition.CenterScreen;
    //        form.MinimizeBox = false;
    //        form.MaximizeBox = false;
    //        form.ControlBox = false;
    //        form.TopMost = true;

    //        if (validation != null)
    //        {
    //            form.FormClosing += delegate (object sender, FormClosingEventArgs e)
    //            {
    //                if (form.DialogResult == DialogResult.OK)
    //                {
    //                }
    //                else if (form.DialogResult == DialogResult.No)
    //                {
    //                }
    //                else if (form.DialogResult == DialogResult.Cancel)
    //                {
    //                }
    //            };
    //        }
    //        DialogResult dialogResult = form.ShowDialog();
    //        return dialogResult;
    //    }

    //    ~uMessageBox()
    //    {
    //    }

    //    private static void ButtonClick(object sender, EventArgs e)
    //    {
    //        form.Close();
    //        return;
    //    }

    //    public void Dispose()
    //    {
    //        Dispose(true);
    //        GC.SuppressFinalize(this);
    //        return;
    //    }

    //    protected virtual void Dispose(bool disposing)
    //    {
    //        if (disposing)
    //        {

    //        }
    //        return;
    //    }
    //}
    //public delegate string uMessageBoxValidation(string errorMessage);

    public class COMMON_FUCTION : IDisposable
    {
        public Stopwatch STOP_WATCH = new Stopwatch();
        private List<string> FileList = new List<string>();
        private List<string> DirList = new List<string>();


        //        private IntPtr hWnd;
        //uint vncstyles;


        public COMMON_FUCTION()
        {
            STOP_WATCH.Start();
        }
        ~COMMON_FUCTION()
        {
            STOP_WATCH.Stop();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
            return;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {

            }
            return;
        }

        /// <summary>
        /// 화면을 갱신할 때 잔상이 남지 않도록 한다 
        /// 프로그램 스타트가 되면 화면을 갱신할때 잔상이 남을 만한 컴포너트를 이곳으로 보내 속성을 바꾼다.
        /// </summary>
        /// <param name="control"></param>
        public void SetDoubleBuffered(Control control)
        {
            // set instance non-public property with name "DoubleBuffered" to true
            typeof(Control).InvokeMember("DoubleBuffered", BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic, null, control, new object[] { true });
            return;
        }

        /*
         컴퓨터를 종료/재부팅/로그오프를 하는방법은 다음과 같습니다.

        기본적으로 Using System.Diagnostics을 사용하며,

        예제코드는 다음과 같습니다.

        종료

        Process.Start("shutdown.exe", "-s"); // 기본적으로 30초 후 종료

        Process.Start("shutdown.exe", "-s -t xx") // xx 초 후 종료



        재부팅

        Process.Start("shutdown.exe","-r"); // 종료과 유사하며 커멘드만 "-r"을 사용

        Process.Start("shutdown.exe","-r -t xx");



        로그오프

        Process.Start("shutdown.exe","-l"); // 위 코드와 유사하며 커멘드만 "-l"(숫자 1이 아닌 소문자'l')을 사용

        원하는 시간에 종료하길 원하신다면, 현재시간을 비교해 종료하는 방법이 있음.


        */

        // 모니터 On/Off 관련
        const int WM_SYSCOMMAND = 0x0112;
        const int SC_MONITORPOWER = 0xF170;
        const int MONITOR_ON = -1;
        const int MONITOR_OFF = 2;
        const int MONITOR_STANBY = 1;
        [System.Runtime.InteropServices.DllImport("User32")]
        private static extern int SendMessage(int hWnd, int hMsg, int wParam, int lParam);
        /// <summary>
        /// this.Handle 을 상수로 주어야 한다.;
        /// </summary>
        /// <param name="Handle"></param>
        public void MonitorOff(IntPtr Handle)
        {
            SendMessage(Handle.ToInt32(), WM_SYSCOMMAND, SC_MONITORPOWER, MONITOR_OFF);
            return;
        }
        /// <summary>
        /// this.Handle 을 상수로 주어야 한다.;
        /// </summary>
        /// <param name="Handle"></param>
        public void MonitorOn(IntPtr Handle)
        {
            SendMessage(Handle.ToInt32(), WM_SYSCOMMAND, SC_MONITORPOWER, MONITOR_ON);
            return;
        }

        public void ExternalProgramRun(string name) //외부 프로그램 실행
        {
            Process.Start(name);
            return;
        }

        public void ExternalProgramExit(string name) //외부 프로그램 종료
        {
            string exitname;

            if (name.IndexOf(".exe") < 0)
            {
                exitname = name;
            }
            else
            {
                exitname = name.Substring(0, name.IndexOf(".exe"));
            }
            foreach (Process process in Process.GetProcesses())
            {
                if (process.ProcessName.StartsWith(exitname))
                {
                    process.Kill();
                }
            }
        }

        /// <summary>
        /// 윈도우를 로그 오프를 한다.
        /// </summary>
        public void LogOff()
        {
            Process.Start("shutdown.exe", " -l"); // 위 코드와 유사하며 커멘드만 "-l"(숫자 1이 아닌 소문자'l')을 사용
            return;
        }
        /// <summary>
        /// 윈도우를 설정 시간후 재 부팅을 하다.
        /// </summary>
        /// <param name="Sec"></param>
        public void WindowRestartToDelay(int Sec)
        {
            string s;

            s = "shutdown.exe" + " -r -t " + Sec.ToString();
            Process.Start(s);
            return;
        }

        /// <summary>
        /// 윈도우를 30초 후 재부팅 한다.
        /// </summary>
        public void WindowRestartTo30Secconds()
        {
            Process.Start("shutdown.exe", " -r");
            return;
        }

        /// <summary>
        /// 윈도우를 바로 재부팅 한다.
        /// </summary>
        public void WindowRestartToRightNow()
        {
            Process.Start("shutdown.exe", " -r -f");
            return;
        }

        /// <summary>
        /// 윈도우 종료를 취소한다.
        /// </summary>
        public void WindowShutdownCancel()
        {
            Process.Start("shutdown.exe", " -a");
            return;
        }



        /// <summary>
        /// Sec 시간 후 종료한다.
        /// </summary>
        /// <param name="Sec"></param>
        public void WindowExitToDelay(int Sec)
        {
            string s;

            s = "shutdown.exe" + " -s -t " + Sec.ToString();
            Process.Start(s);
            return;
        }

        /// <summary>
        /// PC 종료(프로그램 정리후 종료)
        /// </summary>
        public void WindowExitTo30Secconds()
        {
            Process.Start("shutdown.exe", " -s");
            return;
        }

        /// <summary>
        /// PC 바로 종료
        /// </summary>
        public void WindowExit()
        {
            Process.Start("shutdown.exe", " -s -f");
            return;
        }


        /// <summary>
        /// 시작 프로그램을 레지스터리에 등록한다.
        /// </summary>
        public void StartProgramRegistrySet()
        {
            var rkApp1 = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            var sName = rkApp1.GetValue("name");

            if (sName != null)
            {
                if (sName.ToString() != System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName))
                {
                    var rkApp2 = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                    //rkApp2.SetValue("name", System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName));
                    rkApp2.SetValue("name", Process.GetCurrentProcess().MainModule.FileName);
                }
            }
            else
            {
                var rkApp2 = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                //rkApp2.SetValue("name", System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName));
                rkApp2.SetValue("name", Process.GetCurrentProcess().MainModule.FileName);
            }
            return;
        }

        /// <summary>
        /// 레지스터리에 등록된 시작 프로그래을 삭제한다.
        /// </summary>
        public void StartProgramRegistryDel()
        {
            var rkApp = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            rkApp.DeleteValue("name");
            return;
        }

        /// <summary>
        /// Property 에 변수를 만든다.
        /// Type 은 typeof(string), typeof(int) 와 같이 형을 만들수 있다.
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Defalut"></param>
        /// <param name="sType"></param>
        //public void SetProperty(string Name, Type sType)
        //{
        //    System.Configuration.SettingsProperty property = new System.Configuration.SettingsProperty(Name);

        //    //if(sType == typeof(string))
        //    //    property.DefaultValue = "";
        //    //else if (sType == typeof(int))
        //    //    property.DefaultValue = 0;
        //    //else if (sType == typeof(short))
        //    //    property.DefaultValue = 0;
        //    //else if (sType == typeof(long))
        //    //    property.DefaultValue = 0;

        //    property.IsReadOnly = false;
        //    property.PropertyType = sType;
        //    property.Provider = Settings.Default.Providers["LocalFileSettingsProvider"];
        //    property.Attributes.Add(typeof(System.Configuration.UserScopedSettingAttribute), new System.Configuration.UserScopedSettingAttribute());
        //    try 
        //    {
        //        Properties.Settings.Default.Properties.Add(property);
        //    }
        //    catch
        //    {
        //        Properties.Settings.Default.Properties.Remove(property.Name);
        //        Properties.Settings.Default.Properties.Add(property);
        //    }

        //    return;
        //}


        public List<string> GetFileList
        {
            get
            {
                return FileList;
            }
        }

        public List<string> GetDirList
        {
            get
            {
                return DirList;
            }
        }


        [StructLayout(LayoutKind.Explicit)]
        private struct union_r
        {
            [FieldOffset(0)]
            public ushort i;
            [FieldOffset(0)]
            public byte c1;
            [FieldOffset(1)]
            public byte c2;
        };

        public class FileSortMode
        {
            public const short LASTWRITE_ODERBY = 0; //마지막 저장 날짜 기준 내림차순
            public const short LASTWRITE_ORDERBYDESCENDING = 1;//마지막 저장 날짜 기준 오름차순
            public const short FILENAME_ODERBY = 2; //파일명 기준으로 내림차순
            public const short FILENAME_ORDERBYDESCENDIN = 3; //파일명 기준으로 오름 차순
            public const short CREATETIME_ODERBY = 2; //생성날짜 기준으로 내림차순
            public const short CREATETIME_ORDERBYDESCENDIN = 3; //생성날짜 기준으로 오름 차순
            public const short NORMAL = 4;
        }

        public void ReadFileList(string Path, string Ext, short Mode = FileSortMode.NORMAL)
        {
            try
            {
                DirectoryInfo DirInf = new DirectoryInfo(Path);
                if (DirInf.Exists == true) //폴더가 존제하면
                {
                    //FileInfo[] FileInf = DirInf.GetFiles("*.*");

                    if (Mode == FileSortMode.NORMAL)
                    {
                        FileInfo[] FileInf = DirInf.GetFiles(Ext);

                        if (FileInf.Length == 0)
                        {
                            //MessageBox.Show("파일이 존재하지 않습니다.");
                            uMessageBox.Show(title: "경고", promptText:"파일이 존재하지 않습니다.");
                        }
                        else
                        {
                            string s = "";
                            for (int i = 0; i < FileInf.Length; i++)
                            {
                                s += FileInf[i].Name.ToString() + Environment.NewLine;

                                FileList.Add(s);
                                s = "";
                            }
                        }
                    }
                    else
                    {
                        //

                        //방식 1
                        //var Files = new DirectoryInfo(FileDirectory)
                        //.GetFiles()
                        //.OrderBy(f => f.Name.Substring(f.Name.Length - 23, 19)
                        //.ToList();

                        //방식 2
                        //생성 날짜를 기준으로 정렬한다.
                        //내림차순
                        //var FileInf = DirInf.GetFiles(Ext).OrderBy(f => f.LastWriteTime).ToList();
                        //오름차순

                        FileList.Clear();
                        string s = "";
                        if (Mode == FileSortMode.LASTWRITE_ODERBY)//마지막 저장 날짜 기준 오름차순
                        {
                            var FileInf = DirInf.GetFiles(Ext).OrderBy(f => f.LastWriteTime).ToList();
                            foreach (FileInfo fs in FileInf)
                            {
                                s = "";
                                s = fs.Name.ToString() + Environment.NewLine;

                                FileList.Add(s);
                            }
                        }
                        else if (Mode == FileSortMode.LASTWRITE_ORDERBYDESCENDING)//마지막 저장 날짜 기준 내림차순
                        {
                            var FileInf = DirInf.GetFiles(Ext).OrderByDescending(f => f.LastWriteTime).ToList();
                            foreach (FileInfo fs in FileInf)
                            {
                                s = "";
                                s = fs.Name.ToString() + Environment.NewLine;

                                FileList.Add(s);
                            }
                        }
                        else if (Mode == FileSortMode.FILENAME_ODERBY) //파일명을 기준으로 내림차순
                        {
                            var FileInf = DirInf.GetFiles(Ext).OrderBy(f => f.Name).ToList();
                            foreach (FileInfo fs in FileInf)
                            {
                                s = "";
                                s = fs.Name.ToString() + Environment.NewLine;

                                FileList.Add(s);
                            }
                        }
                        else if (Mode == FileSortMode.FILENAME_ORDERBYDESCENDIN)//파일명을 기준으로 오름 차순
                        {
                            var FileInf = DirInf.GetFiles(Ext).OrderByDescending(f => f.Name).ToList();
                            foreach (FileInfo fs in FileInf)
                            {
                                s = "";
                                s = fs.Name.ToString() + Environment.NewLine;

                                FileList.Add(s);
                            }
                        }
                        else if (Mode == FileSortMode.FILENAME_ODERBY) //파일명을 기준으로 내림차순
                        {
                            var FileInf = DirInf.GetFiles(Ext).OrderBy(f => f.CreationTime).ToList();
                            foreach (FileInfo fs in FileInf)
                            {
                                s = "";
                                s = fs.Name.ToString() + Environment.NewLine;

                                FileList.Add(s);
                            }
                        }
                        else if (Mode == FileSortMode.FILENAME_ORDERBYDESCENDIN)//파일명을 기준으로 오름 차순
                        {
                            var FileInf = DirInf.GetFiles(Ext).OrderByDescending(f => f.CreationTime).ToList();
                            foreach (FileInfo fs in FileInf)
                            {
                                s = "";
                                s = fs.Name.ToString() + Environment.NewLine;

                                FileList.Add(s);
                            }
                        }
                    }
                }
                else
                {
                    FileList.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
                //uMessageBox.Show(promptText: ex.Message + "\n" + ex.StackTrace);
            }
        }
        public void ReadFileListNotExt(string Path, string Ext, short Mode = FileSortMode.NORMAL)
        {
            try
            {
                DirectoryInfo DirInf = new DirectoryInfo(Path);
                if (DirInf.Exists == true) //폴더가 존제하면
                {
                    //FileInfo[] FileInf = DirInf.GetFiles("*.*");

                    if (Mode == FileSortMode.NORMAL)
                    {
                        FileInfo[] FileInf = DirInf.GetFiles(Ext);

                        FileList.Clear();
                        if (FileInf.Length == 0)
                        {
                            //MessageBox.Show("파일이 존재하지 않습니다.");
                            uMessageBox.Show(promptText: "파일이 존재하지 않습니다.");
                        }
                        else
                        {
                            string s = "";
                            for (int i = 0; i < FileInf.Length; i++)
                            {
                                s += FileInf[i].Name.ToString() + Environment.NewLine;
                                string[] t = s.Split('.');
                                FileList.Add(t[0]);
                                s = "";
                            }
                        }
                    }
                    else
                    {
                        //

                        //방식 1
                        //var Files = new DirectoryInfo(FileDirectory)
                        //.GetFiles()
                        //.OrderBy(f => f.Name.Substring(f.Name.Length - 23, 19)
                        //.ToList();

                        //방식 2
                        //생성 날짜를 기준으로 정렬한다.
                        //내림차순
                        //var FileInf = DirInf.GetFiles(Ext).OrderBy(f => f.LastWriteTime).ToList();
                        //오름차순

                        FileList.Clear();
                        string s = "";
                        if (Mode == FileSortMode.LASTWRITE_ODERBY)//마지막 저장 날짜 기준 오름차순
                        {
                            var FileInf = DirInf.GetFiles(Ext).OrderBy(f => f.LastWriteTime).ToList();
                            foreach (FileInfo fs in FileInf)
                            {
                                s = "";
                                s = fs.Name.ToString() + Environment.NewLine;

                                string[] t = s.Split('.');
                                FileList.Add(t[0]);
                            }
                        }
                        else if (Mode == FileSortMode.LASTWRITE_ORDERBYDESCENDING)//마지막 저장 날짜 기준 내림차순
                        {
                            var FileInf = DirInf.GetFiles(Ext).OrderByDescending(f => f.LastWriteTime).ToList();
                            foreach (FileInfo fs in FileInf)
                            {
                                s = "";
                                s = fs.Name.ToString() + Environment.NewLine;

                                string[] t = s.Split('.');
                                FileList.Add(t[0]);
                            }
                        }
                        else if (Mode == FileSortMode.FILENAME_ODERBY) //파일명을 기준으로 내림차순
                        {
                            var FileInf = DirInf.GetFiles(Ext).OrderBy(f => f.Name).ToList();
                            foreach (FileInfo fs in FileInf)
                            {
                                s = "";
                                s = fs.Name.ToString() + Environment.NewLine;

                                string[] t = s.Split('.');
                                FileList.Add(t[0]);
                            }
                        }
                        else if (Mode == FileSortMode.FILENAME_ORDERBYDESCENDIN)//파일명을 기준으로 오름 차순
                        {
                            var FileInf = DirInf.GetFiles(Ext).OrderByDescending(f => f.Name).ToList();
                            foreach (FileInfo fs in FileInf)
                            {
                                s = "";
                                s = fs.Name.ToString() + Environment.NewLine;

                                string[] t = s.Split('.');
                                FileList.Add(t[0]);
                            }
                        }
                        else if (Mode == FileSortMode.FILENAME_ODERBY) //파일명을 기준으로 내림차순
                        {
                            var FileInf = DirInf.GetFiles(Ext).OrderBy(f => f.CreationTime).ToList();
                            foreach (FileInfo fs in FileInf)
                            {
                                s = "";
                                s = fs.Name.ToString() + Environment.NewLine;

                                string[] t = s.Split('.');
                                FileList.Add(t[0]);
                            }
                        }
                        else if (Mode == FileSortMode.FILENAME_ORDERBYDESCENDIN)//파일명을 기준으로 오름 차순
                        {
                            var FileInf = DirInf.GetFiles(Ext).OrderByDescending(f => f.CreationTime).ToList();
                            foreach (FileInfo fs in FileInf)
                            {
                                s = "";
                                s = fs.Name.ToString() + Environment.NewLine;

                                string[] t = s.Split('.');
                                FileList.Add(t[0]);
                            }
                        }
                    }
                }
                else
                {
                    FileList.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
                //uMessageBox.Show(promptText: ex.Message + "\n" + ex.StackTrace);
            }
        }

        public void ReadDirList(string Path)
        {
            try
            {
                DirectoryInfo DirInf = new DirectoryInfo(Path + "\\");
                if (DirInf.Exists == true) //폴더가 존제하면
                {
                    DirectoryInfo[] DirListData = DirInf.GetDirectories();

                    DirList.Clear();
                    if (DirListData.Length == 0)
                    {
                        //MessageBox.Show("폴더가 존재하지 않습니다.");
                        uMessageBox.Show(promptText: "파일이 존재하지 않습니다.");
                    }
                    else
                    {
                        string s = "";
                        for (int i = 0; i < DirListData.Length; i++)
                        {
                            s += DirListData[i].Name.ToString();
                            //string[] t = s.Split('.');
                            //DirList.Add(t[0]);
                            string[] t = s.Split('\r');
                            DirList.Add(t[0]);
                            s = "";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
                //uMessageBox.Show(promptText: ex.Message + "\n" + ex.StackTrace);
            }
        }

        

        public __Infor__ OpenInfor(string Name,ref __Infor__ Inf)
        {
            if (File.Exists(Name) == true)
            {
                TIniFile Ini = new TIniFile(Name);

            }
            else
            {
                
            }

            return Inf;
        }

        public void SaveSpec(string Name, __Spec__ Spec)
        {
            TIniFile Ini = new TIniFile(Name);

            Ini.WriteString("MODEL", "VALUE", Spec.ModelName);

            Ini.WriteFloat("CURRENT", "RECLINE MIN", Spec.Current.Recliner.Min);
            Ini.WriteFloat("CURRENT", "RECLINE MAX", Spec.Current.Recliner.Max);

            Ini.WriteFloat("CURRENT", "RELAX MIN", Spec.Current.Relax.Min);
            Ini.WriteFloat("CURRENT", "RELAX MAX", Spec.Current.Relax.Max);

            Ini.WriteFloat("CURRENT", "HEIGHT MIN", Spec.Current.Height.Min);
            Ini.WriteFloat("CURRENT", "HEIGHT MAX", Spec.Current.Height.Max);

            Ini.WriteFloat("CURRENT", "LEGREST MIN", Spec.Current.Legrest.Min);
            Ini.WriteFloat("CURRENT", "LEGREST MAX", Spec.Current.Legrest.Max);

            Ini.WriteFloat("CURRENT", "LEGRESTEXT MIN", Spec.Current.LegrestExt.Min);
            Ini.WriteFloat("CURRENT", "LEGRESTEXT MAX", Spec.Current.LegrestExt.Max);

            Ini.WriteFloat("MOVE SPEED", "RECLINEFWD MIN", Spec.MovingSpeed.ReclinerFwd.Min);
            Ini.WriteFloat("MOVE SPEED", "RECLINEFWD MAX", Spec.MovingSpeed.ReclinerFwd.Max);

            Ini.WriteFloat("MOVE SPEED", "RECLINEBWD MIN", Spec.MovingSpeed.ReclinerBwd.Min);
            Ini.WriteFloat("MOVE SPEED", "RECLINEBWD MAX", Spec.MovingSpeed.ReclinerBwd.Max);

            Ini.WriteFloat("MOVE SPEED", "RELAX MIN", Spec.MovingSpeed.Relax.Min);
            Ini.WriteFloat("MOVE SPEED", "RELAX MAX", Spec.MovingSpeed.Relax.Max);

            Ini.WriteFloat("MOVE SPEED", "RELAX RETURN MIN", Spec.MovingSpeed.RelaxReturn.Min);
            Ini.WriteFloat("MOVE SPEED", "RELAX RETURN MAX", Spec.MovingSpeed.RelaxReturn.Max);

            Ini.WriteFloat("MOVE SPEED", "HEIGHT UP MIN", Spec.MovingSpeed.HeightUp.Min);
            Ini.WriteFloat("MOVE SPEED", "HEIGHT UP MAX", Spec.MovingSpeed.HeightUp.Max);

            Ini.WriteFloat("MOVE SPEED", "HEIGHT DN MIN", Spec.MovingSpeed.HeightDn.Min);
            Ini.WriteFloat("MOVE SPEED", "HEIGHT DN MAX", Spec.MovingSpeed.HeightDn.Max);

            Ini.WriteFloat("MOVE SPEED", "LEGREST MIN", Spec.MovingSpeed.Legrest.Min);
            Ini.WriteFloat("MOVE SPEED", "LEGREST MAX", Spec.MovingSpeed.Legrest.Max);

            Ini.WriteFloat("MOVE SPEED", "LEGREST RETURN MIN", Spec.MovingSpeed.LegrestReturn.Min);
            Ini.WriteFloat("MOVE SPEED", "LEGREST RETURN MAX", Spec.MovingSpeed.LegrestReturn.Max);

            Ini.WriteFloat("MOVE SPEED", "LEGRESTEXT MIN", Spec.MovingSpeed.LegrestExt.Min);
            Ini.WriteFloat("MOVE SPEED", "LEGRESTEXT MAX", Spec.MovingSpeed.LegrestExt.Max);

            Ini.WriteFloat("MOVE SPEED", "LEGRESTEXT RETURN MIN", Spec.MovingSpeed.LegrestExtReturn.Min);
            Ini.WriteFloat("MOVE SPEED", "LEGRESTEXT RETURN MAX", Spec.MovingSpeed.LegrestExtReturn.Max);

            Ini.WriteFloat("SOUND", "START MAX", Spec.Sound.StartMax);
            Ini.WriteFloat("SOUND", "RUN MAX", Spec.Sound.RunMax);
            Ini.WriteBool("SOUDN", "RMS MODE", Spec.Sound.RMSMode);
            Ini.WriteInteger("SOUND", "LIMIT COUNT", Spec.Sound.LimteCount);
            Ini.WriteFloat("SOUND", "START TIME", Spec.Sound.StartTime);
            //Ini.WriteFloat("SOUND", "간격시간", Spec.Sound.기도음구동음간격시간);

            Ini.WriteFloat("SOUND", "CHECK TIME RECLINE FWD", Spec.ReclinerFwdSoundCheckTime);
            Ini.WriteFloat("SOUND", "CHECK TIME RECLINE BWD", Spec.ReclinerBwdSoundCheckTime);
            Ini.WriteFloat("SOUND", "CHECK TIME RELAX", Spec.RelaxSoundCheckTime);
            Ini.WriteFloat("SOUND", "CHECK TIME HEIGHT", Spec.HeightSoundCheckTime);
            Ini.WriteFloat("SOUND", "CHECK TIME LEGREST", Spec.LegrestSoundCheckTime);
            Ini.WriteFloat("SOUND", "CHECK TIME LEGRESTEXT", Spec.LegrestExtSoundCheckTime);

            Ini.WriteFloat("LIMIT TIME", "RECLINE", Spec.ReclinerLimitTime);
            Ini.WriteFloat("LIMIT TIME", "RELAX RECLINE", Spec.RelaxToReclineLimitTime);
            Ini.WriteFloat("LIMIT TIME", "RELAX", Spec.RelaxLimitTime);
            Ini.WriteFloat("LIMIT TIME", "LEGREST", Spec.LegrestLimitTime);
            Ini.WriteFloat("LIMIT TIME", "C EXT", Spec.LegrestExtLimitTime);
            Ini.WriteFloat("LIMIT TIME", "HEIGHT", Spec.HeightLimitTime);

            Ini.WriteFloat("LENGTH", "RECLINE", Spec.MovingStroke.Recliner);
            Ini.WriteFloat("LENGTH", "RELAX", Spec.MovingStroke.Relax);
            Ini.WriteFloat("LENGTH", "HEIGHT", Spec.MovingStroke.Height);
            Ini.WriteFloat("LENGTH", "LEGREST", Spec.MovingStroke.Legrest);
            Ini.WriteFloat("LENGTH", "LEGREST EXT", Spec.MovingStroke.LegrestExt);


            //Ini.WriteFloat("TEST TIME", "RECLINE", Spec.ReclinerTestTime);
            //Ini.WriteFloat("TEST TIME", "RELAX", Spec.RelaxTestTime);
            //Ini.WriteFloat("TEST TIME", "HEIGHT", Spec.HeightTestTime);
            //Ini.WriteFloat("TEST TIME", "LEGREST", Spec.LegrestTestTime);
            //Ini.WriteFloat("TEST TIME", "LEGREST EXT", Spec.LegrestExtTestTime);

            Ini.WriteInteger("납입위치", "RECLINE", Spec.DeliveryPos.Recliner);
            Ini.WriteInteger("납입위치", "RELAX RECLINE", Spec.DeliveryPos.RelaxToRecliner);
            Ini.WriteInteger("납입위치", "RELAX", Spec.DeliveryPos.Relax);
            Ini.WriteInteger("납입위치", "HEIGHT", Spec.DeliveryPos.Height);
            Ini.WriteInteger("납입위치", "LEGREST", Spec.DeliveryPos.Legrest);
            Ini.WriteInteger("납입위치", "LEGREST EXT", Spec.DeliveryPos.LegrestExt);

            Ini.WriteFloat("납입위치", "NOTRELAX STROKE", Spec.DeliveryPos.PowerReclinerStroke);
            Ini.WriteFloat("납입위치", "RELAX STROKE", Spec.DeliveryPos.RelaxStroke);
            Ini.WriteFloat("납입위치", "MIN STROKE", Spec.DeliveryPos.MinStroke);
            Ini.WriteFloat("납입위치", "MAX STROKE", Spec.DeliveryPos.MaxStroke);

            Ini.WriteFloat("LIMIT CURR", "RECLINE", Spec.ReclinerLimitCurr);
            Ini.WriteFloat("LIMIT CURR", "RELAX", Spec.RelaxLimitCurr);
            Ini.WriteFloat("LIMIT CURR", "HEIGHT", Spec.HeightLimitCurr);
            Ini.WriteFloat("LIMIT CURR", "LEGREST", Spec.LegrestLimitCurr);
            Ini.WriteFloat("LIMIT CURR", "LEGRESTEXT", Spec.LegrestExtLimitCurr);

            Ini.WriteFloat("INCOM VOLT", "VALUE", Spec.InComingVolt);
            Ini.WriteFloat("TEST VOLT", "VALUE", Spec.TestVolt);
            Ini.WriteFloat("RELAYX SW", "ON TIME", Spec.RelaxSwOnTime);

            Ini.WriteString("VERSION", "SW", Spec.SWVer);
            return;
        }

        public void OpenSpec(string Name, ref __Spec__ Spec)
        {
            TIniFile Ini = new TIniFile(Name);

            if (Ini.ReadString("MODEL", "VALUE", ref Spec.ModelName) == false) Spec.ModelName = "";

            if (Ini.ReadFloat("CURRENT", "RECLINE MIN", ref Spec.Current.Recliner.Min) == false) Spec.Current.Recliner.Min = 0;
            if (Ini.ReadFloat("CURRENT", "RECLINE MAX", ref Spec.Current.Recliner.Max) == false) Spec.Current.Recliner.Max = 0;

            if (Ini.ReadFloat("CURRENT", "RELAX MIN", ref Spec.Current.Relax.Min) == false) Spec.Current.Relax.Min = 0;
            if (Ini.ReadFloat("CURRENT", "RELAX MAX", ref Spec.Current.Relax.Max) == false) Spec.Current.Relax.Max = 0;

            if (Ini.ReadFloat("CURRENT", "HEIGHT MIN", ref Spec.Current.Height.Min) == false) Spec.Current.Height.Min = 0;
            if (Ini.ReadFloat("CURRENT", "HEIGHT MAX", ref Spec.Current.Height.Max) == false) Spec.Current.Height.Max = 0;

            if (Ini.ReadFloat("CURRENT", "LEGREST MIN", ref Spec.Current.Legrest.Min) == false) Spec.Current.Legrest.Min = 0;
            if (Ini.ReadFloat("CURRENT", "LEGREST MAX", ref Spec.Current.Legrest.Max) == false) Spec.Current.Legrest.Max = 0;

            if (Ini.ReadFloat("CURRENT", "LEGRESTEXT MIN", ref Spec.Current.LegrestExt.Min) == false) Spec.Current.LegrestExt.Min = 0;
            if (Ini.ReadFloat("CURRENT", "LEGRESTEXT MAX", ref Spec.Current.LegrestExt.Max) == false) Spec.Current.LegrestExt.Max = 0;

            if (Ini.ReadFloat("MOVE SPEED", "RECLINEFWD MIN", ref Spec.MovingSpeed.ReclinerFwd.Min) == false) Spec.MovingSpeed.ReclinerFwd.Min = 0;
            if (Ini.ReadFloat("MOVE SPEED", "RECLINEFWD MAX", ref Spec.MovingSpeed.ReclinerFwd.Max) == false) Spec.MovingSpeed.ReclinerFwd.Max = 0;

            if (Ini.ReadFloat("MOVE SPEED", "RECLINEBWD MIN", ref Spec.MovingSpeed.ReclinerBwd.Min) == false) Spec.MovingSpeed.ReclinerBwd.Min = 0;
            if (Ini.ReadFloat("MOVE SPEED", "RECLINEBWD MAX", ref Spec.MovingSpeed.ReclinerBwd.Max) == false) Spec.MovingSpeed.ReclinerBwd.Max = 0;

            if (Ini.ReadFloat("MOVE SPEED", "RELAX MIN", ref Spec.MovingSpeed.Relax.Min) == false) Spec.MovingSpeed.Relax.Min = 0;
            if (Ini.ReadFloat("MOVE SPEED", "RELAX MAX", ref Spec.MovingSpeed.Relax.Max) == false) Spec.MovingSpeed.Relax.Max = 0;

            if (Ini.ReadFloat("MOVE SPEED", "RELAX RETURN MIN", ref Spec.MovingSpeed.RelaxReturn.Min) == false) Spec.MovingSpeed.RelaxReturn.Min = 0;
            if (Ini.ReadFloat("MOVE SPEED", "RELAX RETURN MAX", ref Spec.MovingSpeed.RelaxReturn.Max) == false) Spec.MovingSpeed.RelaxReturn.Max = 0;

            if (Ini.ReadFloat("MOVE SPEED", "HEIGHT UP MIN", ref Spec.MovingSpeed.HeightUp.Min) == false) Spec.MovingSpeed.HeightUp.Min = 0;
            if (Ini.ReadFloat("MOVE SPEED", "HEIGHT UP MAX", ref Spec.MovingSpeed.HeightUp.Max) == false) Spec.MovingSpeed.HeightUp.Max = 0;

            if (Ini.ReadFloat("MOVE SPEED", "HEIGHT DN MIN", ref Spec.MovingSpeed.HeightDn.Min) == false) Spec.MovingSpeed.HeightDn.Min = 0;
            if (Ini.ReadFloat("MOVE SPEED", "HEIGHT DN MAX", ref Spec.MovingSpeed.HeightDn.Max) == false) Spec.MovingSpeed.HeightDn.Max = 0;

            if (Ini.ReadFloat("MOVE SPEED", "LEGREST MIN", ref Spec.MovingSpeed.Legrest.Min) == false) Spec.MovingSpeed.Legrest.Min = 0;
            if (Ini.ReadFloat("MOVE SPEED", "LEGREST MAX", ref Spec.MovingSpeed.Legrest.Max) == false) Spec.MovingSpeed.Legrest.Max = 0;

            if (Ini.ReadFloat("MOVE SPEED", "LEGREST RETURN MIN", ref Spec.MovingSpeed.LegrestReturn.Min) == false) Spec.MovingSpeed.LegrestReturn.Min = 0;
            if (Ini.ReadFloat("MOVE SPEED", "LEGREST RETURN MAX", ref Spec.MovingSpeed.LegrestReturn.Max) == false) Spec.MovingSpeed.LegrestReturn.Max = 0;

            if (Ini.ReadFloat("MOVE SPEED", "LEGRESTEXT MIN", ref Spec.MovingSpeed.LegrestExt.Min) == false) Spec.MovingSpeed.LegrestExt.Min = 0;
            if (Ini.ReadFloat("MOVE SPEED", "LEGRESTEXT MAX", ref Spec.MovingSpeed.LegrestExt.Max) == false) Spec.MovingSpeed.LegrestExt.Max = 0;

            if (Ini.ReadFloat("MOVE SPEED", "LEGRESTEXT RETURN MIN", ref Spec.MovingSpeed.LegrestExtReturn.Min) == false) Spec.MovingSpeed.LegrestExtReturn.Min = 0;
            if (Ini.ReadFloat("MOVE SPEED", "LEGRESTEXT RETURN MAX", ref Spec.MovingSpeed.LegrestExtReturn.Max) == false) Spec.MovingSpeed.LegrestExtReturn.Max = 0;

            if (Ini.ReadFloat("SOUND", "START MAX", ref Spec.Sound.StartMax) == false) Spec.Sound.StartMax = 0;
            if (Ini.ReadFloat("SOUND", "RUN MAX", ref Spec.Sound.RunMax) == false) Spec.Sound.RunMax = 0;
            if (Ini.ReadBool("SOUDN", "RMS MODE", ref Spec.Sound.RMSMode) == false) Spec.Sound.RMSMode = false;
            if (Ini.ReadInteger("SOUND", "LIMIT COUNT", ref Spec.Sound.LimteCount) == false) Spec.Sound.LimteCount = 0;
            if (Ini.ReadFloat("SOUND", "START TIME", ref Spec.Sound.StartTime) == false) Spec.Sound.StartTime = 0;
            //if (Ini.ReadFloat("SOUND", "간격시간", ref Spec.Sound.기도음구동음간격시간) == false) Spec.Sound.기도음구동음간격시간 = 0;

            if (Ini.ReadFloat("SOUND", "CHECK TIME RECLINE FWD", ref Spec.ReclinerFwdSoundCheckTime) == false) Spec.ReclinerFwdSoundCheckTime = 0;
            if (Ini.ReadFloat("SOUND", "CHECK TIME RECLINE BWD", ref Spec.ReclinerBwdSoundCheckTime) == false) Spec.ReclinerBwdSoundCheckTime = 0;
            if (Ini.ReadFloat("SOUND", "CHECK TIME RELAX", ref Spec.RelaxSoundCheckTime) == false) Spec.RelaxSoundCheckTime = 0;
            if (Ini.ReadFloat("SOUND", "CHECK TIME HEIGHT", ref Spec.HeightSoundCheckTime) == false) Spec.HeightSoundCheckTime = 0;
            if (Ini.ReadFloat("SOUND", "CHECK TIME LEGREST", ref Spec.LegrestSoundCheckTime) == false) Spec.LegrestSoundCheckTime = 0;
            if (Ini.ReadFloat("SOUND", "CHECK TIME LEGRESTEXT", ref Spec.LegrestExtSoundCheckTime) == false) Spec.LegrestExtSoundCheckTime = 0;

            if (Ini.ReadFloat("LIMIT TIME", "RECLINE", ref Spec.ReclinerLimitTime) == false) Spec.ReclinerLimitTime = 0;
            if (Ini.ReadFloat("LIMIT TIME", "RELAX RECLINE", ref Spec.RelaxToReclineLimitTime) == false) Spec.RelaxToReclineLimitTime = 0;
            if (Ini.ReadFloat("LIMIT TIME", "RELAX", ref Spec.RelaxLimitTime) == false) Spec.RelaxLimitTime = 0;
            if (Ini.ReadFloat("LIMIT TIME", "LEGREST", ref Spec.LegrestLimitTime) == false) Spec.LegrestLimitTime = 0;
            if (Ini.ReadFloat("LIMIT TIME", "LEGREST EXT", ref Spec.LegrestExtLimitTime) == false) Spec.LegrestExtLimitTime = 0;
            if (Ini.ReadFloat("LIMIT TIME", "HEIGHT", ref Spec.HeightLimitTime) == false) Spec.HeightLimitTime = 0;

            if (Ini.ReadFloat("LENGTH", "RECLINE", ref Spec.MovingStroke.Recliner) == false) Spec.MovingStroke.Recliner = 0;
            if (Ini.ReadFloat("LENGTH", "RELAX", ref Spec.MovingStroke.Relax) == false) Spec.MovingStroke.Relax = 0;
            if (Ini.ReadFloat("LENGTH", "HEIGHT", ref Spec.MovingStroke.Height) == false) Spec.MovingStroke.Height = 0;
            if (Ini.ReadFloat("LENGTH", "LEGREST", ref Spec.MovingStroke.Legrest) == false) Spec.MovingStroke.Legrest = 0;
            if (Ini.ReadFloat("LENGTH", "LEGREST EXT", ref Spec.MovingStroke.LegrestExt) == false) Spec.MovingStroke.LegrestExt = 0;

            if (Ini.ReadFloat("TEST TIME", "RECLINE", ref Spec.ReclinerTestTime) == false) Spec.ReclinerTestTime = 0;
            if (Ini.ReadFloat("TEST TIME", "RELAX", ref Spec.RelaxTestTime) == false) Spec.RelaxTestTime = 0;
            if (Ini.ReadFloat("TEST TIME", "HEIGHT", ref Spec.HeightTestTime) == false) Spec.HeightTestTime = 0;
            if (Ini.ReadFloat("TEST TIME", "LEGREST", ref Spec.LegrestTestTime) == false) Spec.LegrestTestTime = 0;
            if (Ini.ReadFloat("TEST TIME", "LEGREST EXT", ref Spec.LegrestExtTestTime) == false) Spec.LegrestExtTestTime = 0;

            if (Ini.ReadShort("납입위치", "RELAX RECLINE", ref Spec.DeliveryPos.RelaxToRecliner) == false) Spec.DeliveryPos.RelaxToRecliner = 0;
            if (Ini.ReadShort("납입위치", "RECLINE", ref Spec.DeliveryPos.Recliner) == false) Spec.DeliveryPos.Recliner = 0;
            if (Ini.ReadShort("납입위치", "RELAX", ref Spec.DeliveryPos.Relax) == false) Spec.DeliveryPos.Relax = 0;
            if (Ini.ReadShort("납입위치", "HEIGHT", ref Spec.DeliveryPos.Height) == false) Spec.DeliveryPos.Height = 0;
            if (Ini.ReadShort("납입위치", "LEGREST", ref Spec.DeliveryPos.Legrest) == false) Spec.DeliveryPos.Legrest = 0;
            if (Ini.ReadShort("납입위치", "LEGREST EXT", ref Spec.DeliveryPos.LegrestExt) == false) Spec.DeliveryPos.LegrestExt = 0;

            if (Ini.ReadFloat("납입위치", "NOTRELAX STROKE", ref Spec.DeliveryPos.PowerReclinerStroke) == false) Spec.DeliveryPos.PowerReclinerStroke = 0;
            if (Ini.ReadFloat("납입위치", "RELAX STROKE", ref Spec.DeliveryPos.RelaxStroke) == false) Spec.DeliveryPos.RelaxStroke = 0;
            if (Ini.ReadFloat("납입위치", "MIN STROKE", ref Spec.DeliveryPos.MinStroke) == false) Spec.DeliveryPos.MinStroke = 0;
            if (Ini.ReadFloat("납입위치", "MAX STROKE", ref Spec.DeliveryPos.MaxStroke) == false) Spec.DeliveryPos.MaxStroke = 0;

            if (Ini.ReadFloat("LIMIT CURR", "RECLINE", ref Spec.ReclinerLimitCurr) == false) Spec.ReclinerLimitCurr = 0;
            if (Ini.ReadFloat("LIMIT CURR", "RELAX", ref Spec.RelaxLimitCurr) == false) Spec.RelaxLimitCurr = 0;
            if (Ini.ReadFloat("LIMIT CURR", "HEIGHT", ref Spec.HeightLimitCurr) == false) Spec.HeightLimitCurr = 0;
            if (Ini.ReadFloat("LIMIT CURR", "LEGREST", ref Spec.LegrestLimitCurr) == false) Spec.LegrestLimitCurr = 0;
            if (Ini.ReadFloat("LIMIT CURR", "LEGRESTEXT", ref Spec.LegrestExtLimitCurr) == false) Spec.LegrestExtLimitCurr = 0;

            if (Ini.ReadFloat("INCOM VOLT", "VALUE", ref Spec.InComingVolt) == false) Spec.InComingVolt = 0;
            if (Ini.ReadFloat("TEST VOLT", "VALUE", ref Spec.TestVolt) == false) Spec.TestVolt = 0;
            if (Ini.ReadFloat("RELAYX SW", "ON TIME",ref Spec.RelaxSwOnTime) == false) Spec.RelaxSwOnTime = 0;

            if(Ini.ReadString("VERSION", "SW", ref Spec.SWVer) == false) Spec.SWVer = "";
            return;
        }


        public void SaveInfor(string Name, __Infor__ Inf,bool Flag = false)
        {
            TIniFile Ini = new TIniFile(Name);
            
            return;
        }

        
        public void SaveRunTime<__Struct__>(short Ch, __Struct__ Time)
        {
            try
            {
                string Name = null;

                FileStream DataStream = new FileStream(Name, FileMode.Append, FileAccess.Write);

                byte[] buffer = new byte[Marshal.SizeOf(typeof(__Struct__))];
                GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                Marshal.StructureToPtr(Time, handle.AddrOfPinnedObject(), false);
                DataStream.Write(buffer, 0, Marshal.SizeOf(typeof(__Struct__)));
                handle.Free();
                DataStream.Close();
            }
            catch //(Exception ex)
            {
                //MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
            }
        }


        

        public string[] GetComName(string[] List)
        {
            string[] Name = new string[List.Length];
            int x;

            for (int i = 0; i < Name.Length; i++) Name[i] = "";
            x = 0;
            foreach (string s in List)
            {
                Name[x] = "COM";
                Name[x] = Name[x] + GetNumber(s);
                x++;
            }

            return Name;
        }

        public string GetNumber(string Data)
        {
            string s = "";

            foreach (char c in Data)
            {
                if (isnumeric(c) == true) s = s + c;
            }
            return s;
        }

        public int GetToNumInt(string s, bool ReturnToChar = true, bool ToHex = true) //ReturnToChar == true 일때 숫자가 아니면 바로 리턴하고 false 이면 숫자값을 모두 읽어 온다.
        {
            int value;
            string sb;

            sb = GetString(s, ReturnToChar, ToHex);

            if (ToHex == true)
            {
                if (int.TryParse(sb, System.Globalization.NumberStyles.HexNumber, null, out value) == false)
                    return 0;
                else return value;
            }
            else
            {
                if (int.TryParse(sb, System.Globalization.NumberStyles.Number, null, out value) == false)
                    return 0;
                else return value;
            }
        }
                
        public string GetString(string data = "0", bool ReturnToChar = true, bool ToHexString = true)
        {
            int i;
            StringBuilder sb = new StringBuilder();

            i = 0;
            foreach (char c in data)
            {
                i++;
                // Check for numeric characters (hex in this case).  Add "." and "e" if float,
                // and remove letters.  Include initial space because it is harmless.
                if (ToHexString == true)
                {
                    if (((c >= '0') && (c <= '9')) || ((c >= 'A') && (c <= 'F')) || ((c >= 'a') && (c <= 'f')) || (c == ' ') || (c == '+') || (c == '-'))
                    {
                        sb.Append(c);
                    }
                    else
                    {
                        if (ReturnToChar == true)
                        {
                            if ((c == 'x') || (c == 'X'))
                            {
                                if (2 < i) break;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
                else
                {
                    if (((c >= '0') && (c <= '9')) || (c == '.') || (c == ' ') || (c == '+') || (c == '-'))
                    {
                        sb.Append(c);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return sb.ToString();
        }

        public void strcpy(out string target, char[] source)
        {
            target = "";

            foreach (char c in source)
            {
                if (c != 0x00)
                    target = target + c;
                else break;
            }
            //target = target1;
            return;
        }

        public void strcpy(out char[] target, char[] source)
        {
            int i;

            target = new char[source.Length];
            i = 0;
            foreach (char c in source)
            {
                //if (c != 0x00)
                target[i++] = c;
                //else break;
            }
            return;
        }

        public void strcpy(out char[] target, string source)
        {
            int i;

            target = new char[source.Length];
            i = 0;
            foreach (char c in source)
            {
                //if (c != 0x00)
                target[i++] = c;
                //else break;
            }
            return;
        }

        public void strcpy(out byte[] target, string source)
        {
            int i;

            target = new byte[source.Length];
            i = 0;
            foreach (char c in source)
            {
                if (c != 0x00)
                    target[i++] = (byte)c;
                else break;
            }
            return;
        }

        public void strncpy(out char[] target, string source, int Length)
        {
            int i;

            i = 0;
            target = new char[Length];
            foreach (char c in source)
            {
                if (c != 0x00)
                    target[i++] = (char)c;
                else break;
            }
            return;
        }

        public void strncpy(out char[] target, byte[] source, int Length)
        {
            int i;

            i = 0;
            target = new char[Length];
            foreach (char c in source)
            {
                if (c != 0x00)
                    target[i++] = (char)c;
                else break;
            }
            return;
        }


        public float GetToNumFloat(string s, bool ReturnToChar = true, bool ToHex = true) //ReturnToChar == true 일때 숫자가 아니면 바로 리턴하고 false 이면 숫자값을 모두 읽어 온다.
        {
            float value;
            string sb;

            sb = GetString(s, ReturnToChar, ToHex);

            if (ToHex == true)
            {
                if (float.TryParse(sb, System.Globalization.NumberStyles.HexNumber, null, out value) == false)
                    return 0;
                else return value;
            }
            else
            {
                if (float.TryParse(sb, System.Globalization.NumberStyles.Number, null, out value) == false)
                    return 0;
                else return value;
            }
        }

        public short GetToNumShort(string s, bool ReturnToChar = true, bool ToHex = true) //ReturnToChar == true 일때 숫자가 아니면 바로 리턴하고 false 이면 숫자값을 모두 읽어 온다.
        {
            short value;
            string sb;

            sb = GetString(s, ReturnToChar, ToHex);

            if (ToHex == true)
            {
                if (short.TryParse(sb, System.Globalization.NumberStyles.HexNumber, null, out value) == false)
                    return 0;
                else return value;
            }
            else
            {
                if (short.TryParse(sb, System.Globalization.NumberStyles.Number, null, out value) == false)
                    return 0;
                else return value;
            }
        }

        public long GetToNumLong(string s, bool ReturnToChar = true, bool ToHex = true) //ReturnToChar == true 일때 숫자가 아니면 바로 리턴하고 false 이면 숫자값을 모두 읽어 온다.
        {
            long value;
            string sb;

            sb = GetString(s, ReturnToChar, ToHex);

            if (ToHex == true)
            {
                if (long.TryParse(sb, System.Globalization.NumberStyles.HexNumber, null, out value) == false)
                    return 0;
                else return value;
            }
            else
            {
                if (long.TryParse(sb, System.Globalization.NumberStyles.Number, null, out value) == false)
                    return 0;
                else return value;
            }
        }

        public double GetToNumDouble(string s, bool ReturnToChar = true, bool ToHex = true) //ReturnToChar == true 일때 숫자가 아니면 바로 리턴하고 false 이면 숫자값을 모두 읽어 온다.
        {
            double value;
            string sb;

            sb = GetString(s, ReturnToChar, ToHex);

            if (ToHex == true)
            {
                if (double.TryParse(sb, System.Globalization.NumberStyles.HexNumber, null, out value) == false)
                    return 0;
                else return value;
            }
            else
            {
                if (double.TryParse(sb, System.Globalization.NumberStyles.Number, null, out value) == false)
                    return 0;
                else return value;
            }
        }

        public byte GetToNumByte(string s, bool ReturnToChar = true, bool ToHex = true) //ReturnToChar == true 일때 숫자가 아니면 바로 리턴하고 false 이면 숫자값을 모두 읽어 온다.
        {
            byte value;
            string sb;

            sb = GetString(s, ReturnToChar, ToHex);

            if (ToHex == true)
            {
                if (byte.TryParse(sb, System.Globalization.NumberStyles.HexNumber, null, out value) == false)
                    return 0x00;
                else return value;
            }
            else
            {
                if (byte.TryParse(sb, System.Globalization.NumberStyles.Number, null, out value) == false)
                    return 0x00;
                else return value;
            }
        }

        public void GetSystemDate(out string outTime)
        {
            outTime = DateTime.Now.ToString("yyyy.MM.dd");

            return;
        }

        //---------------------------------------------------------------------------


        public void GetSystemTime(out string outTime)
        {
            outTime = DateTime.Now.ToString("HH:mm:ss");
            return;
        }

        //---------------------------------------------------------------------------
        public void GetSystemDateTime(out string outTime)
        {
            outTime = "[" + DateTime.Now.ToString("yyyy.MM.dd") + "-" + DateTime.Now.ToString("HH:mm:ss") + "]";
            return;
        }

        //---------------------------------------------------------------------------
        public bool isnumeric(char s)
        {
            switch (s)
            {
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9': return true;
            }
            return false;
        }
        //---------------------------------------------------------------------------
        public int StringToHex(char[] Data)
        {
            int Value;
            //short i;
            //short Length;

            //Value = 0;

            //Length = (short)Data.Length;

            //for (i = 0; i < Length; i++)
            //{
            //    Value |= CharToHex(Data[i]) << (4 * ((Length - 1) - i));
            //}

            byte[] s = Encoding.UTF8.GetBytes(Data);
            string s2 = Encoding.UTF8.GetString(s).ToUpper();

            var sx = new String(s2.Where(Char.IsLetterOrDigit).ToArray());

            s2 = sx;
            //if (0 <= s2.IndexOf("0x")) s2 = s2.Remove(0, 2);
            if (0 <= s2.IndexOf("0X")) s2 = s2.Remove(0, 2);


            if (int.TryParse(s2, System.Globalization.NumberStyles.HexNumber, null, out Value) == false) Value = 0;
            return Value;
        }

        public int StringToHex(byte[] Data)
        {
            int Value;
            //short i;
            //short Length;

            //Value = 0;

            //Length = (short)Data.Length;

            //for (i = 0; i < Length; i++)
            //{
            //    Value |= CharToHex(Data[i]) << (4 * ((Length - 1) - i));
            //}

            string s2 = Encoding.UTF8.GetString(Data).ToUpper();

            //if (0 <= s2.IndexOf("0x")) s2 = s2.Remove(0, 2);

            var sx = new String(s2.Where(Char.IsLetterOrDigit).ToArray());
            s2 = sx;

            if (0 <= s2.IndexOf("0X")) s2 = s2.Remove(0, 2);
            
            if (int.TryParse(s2, System.Globalization.NumberStyles.HexNumber, null, out Value) == false) Value = 0;
            return Value;
        }
        //---------------------------------------------------------------------------
        public int StringToHex(string Data)
        {
            int Value;
            //short i;
            //short Length;

            //if(0 <= Data.IndexOf("0x"))
            //{
            //    Data = Data.Substring(2);
            //}
            //else if (0 <= Data.IndexOf("0X"))
            //{
            //    Data = Data.Substring(2);
            //}

            //Value = 0;

            //Length = (short)Data.Length;

            //i = 0;
            //foreach (char c in Data)
            //{
            //    Value |= CharToHex(c) << (4 * ((Length - 1) - i));
            //    i++;
            //}

            string s2 = Data.ToUpper();
            var sx = new String(s2.Where(Char.IsLetterOrDigit).ToArray());
            s2 = sx;
            //if (0 <= s2.IndexOf("0x")) s2 = s2.Remove(0, 2);
            if (0 <= s2.IndexOf("0X")) s2 = s2.Remove(0, 2);

            if (int.TryParse(s2, System.Globalization.NumberStyles.HexNumber, null, out Value) == false) Value = 0;
            return Value;
        }
        //---------------------------------------------------------------------------
        public short CharToHex(char c)
        {
            //    switch(c)
            //    {
            //        case '0' : return 0x00;
            //        case '1' : return 0x01;
            //        case '2' : return 0x02;
            //        case '3' : return 0x03;
            //        case '4' : return 0x04;
            //        case '5' : return 0x05;
            //        case '6' : return 0x06;
            //        case '7' : return 0x07;
            //        case '8' : return 0x08;
            //        case '9' : return 0x09;
            //        case 'A' : return 0x0a;
            //        case 'B' : return 0x0b;
            //        case 'C' : return 0x0c;
            //        case 'D' : return 0x0d;
            //        case 'E' : return 0x0e;
            //        case 'F' : return 0x0f;
            //        default  : return 0x00;
            //    }
            short Value = 0x00;

            if (('0' <= c) && (c <= '9'))
            {
                Value = (short)(c - '0');
            }
            else if (('A' <= c) && (c <= 'F'))
            {
                Value = (short)(c - 'A');
                Value += 10;
            }

            else if (('a' <= c) && (c <= 'f'))
            {
                Value = (short)(c - 'a');
                Value += 10;
            }
            return Value;
        }
        //------------------------------------------------------------------------------
        public string ToString(char[] Data)
        {
            string s;
            //short i;

            if (Data == null) return "";
            if (Data.Length == 0) return "";
            if (Data[0] == 0x00) return "";

            /*
            for (i = 0; i < Data.Length; i++)
            {
                if (Data[i] == 0x0000) break;
            }

            s = new string(Data).Substring(0, i);
            */
            s = "";
            foreach (char c in Data)
            {
                if (c != 0x00)
                    s = s + c;
                else break;
            }
            return s;
        }
        //------------------------------------------------------------------------------
        public string ReturnNumData(string Data)
        {
            string s;

            //if (Data.Length == 0) return "0";
            //s = "";
            //foreach (char c in Data)
            //{
            //    if (char.IsNumber(c))
            //    {
            //        s = s + c;
            //    }
            //    else
            //    {
            //        s = "0";
            //        return s;
            //    }
            //}
            //s = new string(Data).Substring(0, i);
            //s = Data.ToString().Substring(0,i);


            //using System.Text.RegularExpressions;

            //위와 같이 사용하시면 됩니다. "\D" 는 숫자가 아닌 문자열을 뜻하므로 숫자를 제외하고 다 없애라는 뜻이 됩니다.간단하지만 요긴한것 같습니다.
            string strTmp = Regex.Replace(Data, @"\D", "");
            int nTmp = int.Parse(strTmp);

            s = Convert.ToString(nTmp);

            //숫자만 가져오기
            //var onlyLetters = new String(Data.Where(Char.IsNumber).ToArray());
            return s;
        }
        //------------------------------------------------------------------------------
        public string ReturnStringData(string Data)
        {
            string s;

            //if (Data.Length == 0) return "0";
            //s = "";
            //foreach (char c in Data)
            //{
            //    if (char.IsNumber(c))
            //    {
            //        s = s + c;
            //    }
            //    else
            //    {
            //        s = "0";
            //        return s;
            //    }
            //}
            //s = new string(Data).Substring(0, i);
            //s = Data.ToString().Substring(0,i);


            //using System.Text.RegularExpressions;

            //위와 같이 사용하시면 됩니다. "\d" 는 문자가 아닌 숫자열을 뜻하므로 문자를 제외하고 다 없애라는 뜻이 됩니다.간단하지만 요긴한것 같습니다.
            string strTmp = Regex.Replace(Data, @"\d", "");
            int nTmp = int.Parse(strTmp);

            s = Convert.ToString(nTmp);

            //문자열만 축출
            //var onlyLetters = new String(Data.Where(Char.IsLetter).ToArray());
            //대문자만 축출
            //var onlyLetters = new String(Data.Where(c => Char.IsLetter(c) && Char.IsUpper(c)).ToArray());
                       

            return s;
        }
        //------------------------------------------------------------------------------

        /*
        public void SaveIOMap<__IOMap__>(string Name, __IOMap__ Data)
        {
            //string Name;            

            //Path = Application.StartupPath + "\\SYSTEM";
            //Name = Path + "\\IO.dat";
            //File.Delete(Name);

            FileStream DataStream = new FileStream(Name, FileMode.Create, FileAccess.Write);

            byte[] buffer = new byte[Marshal.SizeOf(typeof(__IOMap__))];
            GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            //Marshal.StructureToPtr((object)Data, handle.AddrOfPinnedObject(), false);
            Marshal.StructureToPtr(Data, handle.AddrOfPinnedObject(), false);
            DataStream.Write(buffer, 0, Marshal.SizeOf(typeof(__IOMap__)));
            handle.Free();
            DataStream.Close();
            return;
        }
        
        //------------------------------------------------------------------------------
        public __IOMap__ ReadIOMap<__IOMap__>(string Name, __IOMap__ Data)
        {
            //string Name;            
            int Length;
            FileStream DataStream;

            //Path = Application.StartupPath + "\\SYSTEM";
            //Name = Path + "\\IO.dat";

            byte[] buffer = new byte[Marshal.SizeOf(typeof(__IOMap__))];

            if (File.Exists(Name))
            {
                DataStream = new FileStream(Name, FileMode.Open, FileAccess.Read);
                Length = DataStream.Read(buffer, 0, Marshal.SizeOf(typeof(__IOMap__)));
                DataStream.Close();

                //if (Marshal.SizeOf(typeof(__IOMap__)) == Length) //나중에 함수가 추가 되더라도 데이타를 읽어 오기 위해 이 비교문을 삭제한다.
                {
                    GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                    Data = (__IOMap__)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(__IOMap__));
                    handle.Free();
                }
            }
            return Data;
        }
        */
        //------------------------------------------------------------------------------
        public void SaveData<__Struct__>(string Name, __Struct__ Data)
        {
            try
            {
                using (File.Open(Name, FileMode.Append)) { } //파일 엑세스가 가능한지 확인한다.

                if (File.Exists(Name) == false)
                {
                    using (var DataStream = new FileStream(Name, FileMode.Create, FileAccess.Write))
                    {
                        byte[] buffer = new byte[Marshal.SizeOf(typeof(__Struct__))];
                        GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                        //Marshal.StructureToPtr((object)Data, handle.AddrOfPinnedObject(), false);
                        Marshal.StructureToPtr(Data, handle.AddrOfPinnedObject(), false);
                        DataStream.Write(buffer, 0, Marshal.SizeOf(typeof(__Struct__)));
                        handle.Free();
                        DataStream.Close();
                    }
                }
                else
                {
                    using (var DataStream = new FileStream(Name, FileMode.Append, FileAccess.Write))
                    {
                        byte[] buffer = new byte[Marshal.SizeOf(typeof(__Struct__))];
                        GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                        //Marshal.StructureToPtr((object)Data, handle.AddrOfPinnedObject(), false);
                        Marshal.StructureToPtr(Data, handle.AddrOfPinnedObject(), false);
                        DataStream.Write(buffer, 0, Marshal.SizeOf(typeof(__Struct__)));
                        handle.Free();
                        DataStream.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
            }
        }
        //------------------------------------------------------------------------------
        public void SaveBuffer<__Struct__>(string Name, __Struct__ Data)
        {
            try
            {
                //using (File.Open(Name, FileMode.Append)) { } //파일 엑세스가 가능한지 확인한다.

                using (var DataStream = new FileStream(Name, FileMode.Create, FileAccess.Write))
                {
                    //FileStream DataStream = new FileStream(Name, FileMode.Create, FileAccess.Write);

                    byte[] buffer = new byte[Marshal.SizeOf(typeof(__Struct__))];
                    GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                    //Marshal.StructureToPtr((object)Data, handle.AddrOfPinnedObject(), false);
                    Marshal.StructureToPtr(Data, handle.AddrOfPinnedObject(), false);
                    DataStream.Write(buffer, 0, Marshal.SizeOf(typeof(__Struct__)));
                    handle.Free();
                    DataStream.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
                //uMessageBox.Show(promptText: ex.Message + "\n" + ex.StackTrace);
            }
        }
        //------------------------------------------------------------------------------
        public __Struct__ ReadBuffer<__Struct__>(string Name, __Struct__ Data)
        {
            int Length;
            //FileStream DataStream;

            //using (File.Open(Name, FileMode.Open)) { } //파일 엑세스가 가능한지 확인한다.

            byte[] buffer = new byte[Marshal.SizeOf(typeof(__Struct__))];

            if (File.Exists(Name))
            {
                using (var DataStream = new FileStream(Name, FileMode.Open, FileAccess.Read))
                {
                    //DataStream = new FileStream(Name, FileMode.Open, FileAccess.Read);
                    Length = DataStream.Read(buffer, 0, Marshal.SizeOf(typeof(__Struct__)));
                    DataStream.Close();

                    //if (Marshal.SizeOf(typeof(__Spec__)) == Length) //나중에 함수가 추가 되더라도 데이타를 읽어 오기 위해 이 비교문을 삭제한다.
                    {
                        GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                        Data = (__Struct__)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(__Struct__));
                        handle.Free();
                    }
                }
            }
            return Data;
        }


        //------------------------------------------------------------------------------
        /*
        public __Can__ OpenCan<__Can__>(string Name, __Can__ Data)
        {
            //string Path;
            int Length;
            FileStream DataStream;

            //Path = Application.StartupPath + "\\SYSTEM";

            //Name = Path + "\\Can.dat";

            byte[] buffer = new byte[Marshal.SizeOf(typeof(__Can__))];

            if (File.Exists(Name))
            {
                DataStream = new FileStream(Name, FileMode.Open, FileAccess.Read);
                Length = DataStream.Read(buffer, 0, Marshal.SizeOf(typeof(__Can__)));
                DataStream.Close();

                //if (Marshal.SizeOf(typeof(__Can__)) == Length) //나중에 함수가 추가 되더라도 데이타를 읽어 오기 위해 이 비교문을 삭제한다.
                {
                    GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                    Data = (__Can__)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(__Can__));
                    handle.Free();
                }
            }

            return Data;
        }
        //---------------------------------------------------------------------------
        public void SaveCan<__Can__>(string Name, __Can__ Data)
        {
            //string Name;            

            //Path = Application.StartupPath + "\\SYSTEM";
            //Name = Path + "\\Can.dat";
            //            File.Delete(Name);

            FileStream DataStream = new FileStream(Name, FileMode.Create, FileAccess.Write);

            byte[] buffer = new byte[Marshal.SizeOf(typeof(__Can__))];
            GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            //Marshal.StructureToPtr((object)Data, handle.AddrOfPinnedObject(), false);
            Marshal.StructureToPtr(Data, handle.AddrOfPinnedObject(), false);
            DataStream.Write(buffer, 0, Marshal.SizeOf(typeof(__Can__)));
            handle.Free();
            DataStream.Close();
            
            return;
        }



        public void SaveCount<__NgCount__>(string Name, __NgCount__ Data)
        {
            try
            {
                FileStream DataStream;

                if (File.Exists(Name) == false)
                    DataStream = new FileStream(Name, FileMode.Create, FileAccess.Write);
                else DataStream = new FileStream(Name, FileMode.Append, FileAccess.Write);
                byte[] buffer = new byte[Marshal.SizeOf(typeof(__NgCount__))];
                GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                //Marshal.StructureToPtr((object)Data, handle.AddrOfPinnedObject(), false);
                Marshal.StructureToPtr(Data, handle.AddrOfPinnedObject(), false);
                DataStream.Write(buffer, 0, Marshal.SizeOf(typeof(__NgCount__)));
                handle.Free();
                DataStream.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
            }
        }

        //------------------------------------------------------------------------------
        public __NgCount__ ReadCount<__NgCount__>(string Name, __NgCount__ Data)
        {
            int Length;
            FileStream DataStream;


            byte[] buffer = new byte[Marshal.SizeOf(typeof(__Infor__))];

            if (File.Exists(Name))
            {
                DataStream = new FileStream(Name, FileMode.Open, FileAccess.Read);
                Length = DataStream.Read(buffer, 0, Marshal.SizeOf(typeof(__NgCount__)));
                DataStream.Close();

                //if (Marshal.SizeOf(typeof(__NgCount__)) == Length) //나중에 함수가 추가 되더라도 데이타를 읽어 오기 위해 이 비교문을 삭제한다.
                {
                    GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                    Data = (__NgCount__)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(__NgCount__));
                    handle.Free();
                }
            }
            return Data;
        }
        //------------------------------------------------------------------------------

        public void SaveData<__TestData__>(string Name, __TestData__ Data)
        {
            try
            {
                FileStream DataStream;

                if (File.Exists(Name) == false)
                    DataStream = new FileStream(Name, FileMode.Create, FileAccess.Write);
                else DataStream = new FileStream(Name, FileMode.Append, FileAccess.Write);
                byte[] buffer = new byte[Marshal.SizeOf(typeof(__TestData__))];
                GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                //Marshal.StructureToPtr((object)Data, handle.AddrOfPinnedObject(), false);
                Marshal.StructureToPtr(Data, handle.AddrOfPinnedObject(), false);
                DataStream.Write(buffer, 0, Marshal.SizeOf(typeof(__TestData__)));
                handle.Free();
                DataStream.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
            }
            return;
        }
        //------------------------------------------------------------------------------
        public __TestData__ ReadData<__TestData__>(string Name, __TestData__ Data)
        {
            int Length;
            FileStream DataStream;


            byte[] buffer = new byte[Marshal.SizeOf(typeof(__TestData__))];

            if (File.Exists(Name))
            {
                DataStream = new FileStream(Name, FileMode.Open, FileAccess.Read);
                Length = DataStream.Read(buffer, 0, Marshal.SizeOf(typeof(__TestData__)));
                DataStream.Close();

                //if (Marshal.SizeOf(typeof(__TestData__)) == Length) //나중에 함수가 추가 되더라도 데이타를 읽어 오기 위해 이 비교문을 삭제한다.
                {
                    GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                    Data = (__TestData__)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(__TestData__));
                    handle.Free();
                }
            }
            return Data;
        }
        //------------------------------------------------------------------------------
        */
        //public __IOPort CheckLowInPos(short Pos)
        //{
        //    __IOPort Data = new __IOPort()
        //    {
        //        Pos = 0,
        //        Data = 0x00
        //    };

        //    //unsafe
        //    //{
        //    if (Pos == -1) return Data;
        //    //}

        //    if (Pos < 200)
        //    {
        //        //Data.Pos = (short)(MainForm.IOMap.In[Pos].Pos / 8);
        //        //Data.Data = (byte)(0x01 << (MainForm.IOMap.In[Pos].Pos % 8));
        //        Data.Pos = (short)(Pos / 8);
        //        Data.Data = (byte)(0x01 << (Pos % 8));
        //    }
        //    else
        //    {
        //        //Data.Pos = (short)(MainForm.IOMap.ExtIn[Pos - 200].Pos / 8);
        //        //Data.Data = (byte)(0x01 << (MainForm.IOMap.ExtIn[Pos - 200].Pos % 8));
        //        Data.Pos = (short)((Pos - 200) / 8);
        //        Data.Data = (byte)(0x01 << ((Pos - 200) % 8));
        //    }
        //    return Data;
        //}


        //public __IOPort CheckHighInPos(short Pos)
        //{
        //    __IOPort Data = new __IOPort()
        //    {
        //        Pos = 0,
        //        Data = 0x00
        //    };

        //    if (Pos == -1) return Data;

        //    //Data.Pos = (short)(MainForm.IOMap.HiIn[Pos].Pos / 8);
        //    //Data.Data = (byte)(0x01 << (MainForm.IOMap.HiIn[Pos].Pos % 8));
        //    Data.Pos = (short)(Pos / 8);
        //    Data.Data = (byte)(0x01 << (Pos % 8));
        //    return Data;
        //}




        //double ticks = sw.ElapsedTicks;
        //double seconds = ticks / Stopwatch.Frequency;
        //double milliseconds = (ticks / Stopwatch.Frequency) * 1000;
        //double nanoseconds = (ticks / Stopwatch.Frequency) * 1000000000;

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


        public void delay(long time)
        {
            long first;
            long last;

            first = timeGetTimems();
            last = timeGetTimems();
            do
            {
                last = timeGetTimems();
            } while ((last - first) < time);
            return;
        }

        //private DelayForm delayForm = null;
        public void timedelay2(long time)
        {
            //long first;
            //long last;

            //first = timeGetTimems();
            //last = timeGetTimems();
            //do
            //{
            //    Application.DoEvents();
            //    last = timeGetTimems();
            //} while ((last - first) < time);

            using (DelayForm delayForm = new DelayForm((int)time) { WindowState = FormWindowState.Minimized })
            {
                delayForm.FormClosing += delegate (object sender, FormClosingEventArgs e)
                {
                    e.Cancel = false;
                    delayForm.Dispose();
                    //delayForm = null;
                };

                delayForm.ShowDialog();

                //delayForm.Dispose();
                //delayForm = null;
            }
            return;
        }

        public void timedelay(long time)
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

        //private SecurityUnlockForm SecurityForm = null;


        public void nanotimedelay(long time)
        {
            long first;
            long last;

            first = timeGetNanoTimes();
            last = timeGetNanoTimes();
            do
            {
                Application.DoEvents();
                last = timeGetNanoTimes();
            } while ((last - first) < time);
            return;
        }
    }

    //------------------------------------------------------------------------------
    

    public class ConfigSetting
    {
        __Config__ xConfig = new __Config__();
        //{
        //    Lin = new __LinDevice__
        //    {
        //        Device = 0,
        //        Speed = 0
        //    },
        //    Can = new __CanDevice__
        //    {
        //        Channel = 0,
        //        Device = 0,
        //        ID = 0,
        //        Speed = 0
        //    },
        //    Client = new __TcpIP__
        //    {
        //        IP = string.Empty,
        //        Port = 0
        //    },
        //    NoiseMeter = new __Port__
        //    {
        //        Port = string.Empty,
        //        Speed = 2
        //    }
        //};

        //TIniFile Ini;// = new TIniFile(Name);                

        public ConfigSetting()
        {
            return;
        }

        public __Config__ ReadWriteConfig
        {
            set { xConfig = value; SaveConfig(); }

            get { OpenConfig(); return xConfig; }
        }

        private void OpenConfig()
        {
            //string Path;

            ////Settings 는 using PSeatTester.Properties 즉 Settings 를 사용

            //Path = Program.SYSTEM_PATH.ToString() + "\\Config.ini";

            //if (File.Exists(Path) == false) return;
            //Ini = new TIniFile(Path);

            //xConfig.Lin.Device = Settings.Default.LIN_DEVICE;
            //xConfig.Lin.Speed = Settings.Default.LIN_SPEED;

            xConfig.Can.Channel = Settings.Default.CAN_CHANNEL;
            xConfig.Can.Device = Settings.Default.CAN_DEVICE;
            xConfig.Can.ID = Settings.Default.CAN_ID;
            xConfig.Can.Speed = Settings.Default.CAN_SPEED;

            xConfig.Client.IP = Settings.Default.CLIENT_IP;
            xConfig.Client.Port = Settings.Default.CLIENT_PORT;
            xConfig.Server.IP = Settings.Default.SERVER_IP;
            xConfig.Server.Port = Settings.Default.SERVER_PORT;

            xConfig.PC.IP = Settings.Default.PC_IP;
            xConfig.PC.Port = Settings.Default.PC_PORT;
            xConfig.Board.IP = Settings.Default.BOARD_IP;
            xConfig.Board.Port = Settings.Default.BOARD_PORT;

            xConfig.NoiseMeter.Port = Settings.Default.NOISE_METER_PORT;
            xConfig.NoiseMeter.Speed = Settings.Default.NOISE_METER_SPEED;

            xConfig.Panel.Port = Settings.Default.PANEL_METER_PORT;
            xConfig.Panel.Speed = Settings.Default.PANEL_METER_SPEED;

            xConfig.Power.Port = Settings.Default.POWER_PORT;
            
            xConfig.Power.Speed = Settings.Default.POWER_SPEED;

            xConfig.BattID = Settings.Default.BATT_ID;
            xConfig.CurrID = Settings.Default.CURR_ID;
            xConfig.AutoConnection = Settings.Default.AUTO_CONNECTION;
            xConfig.PinConnectionDelay = Settings.Default.CONNECTIONDELAY;

            xConfig.Stroke.Port = Settings.Default.STROKE_PORT;
            xConfig.Stroke.Speed = Settings.Default.STROKE_SPEED;

            xConfig.KalmanSpeed = Settings.Default.KALMAN_SPEED;
            return;
        }

        private void SaveConfig()
        {
            //string Path = Program.SYSTEM_PATH.ToString() + "\\Config.ini";
            //TIniFile Ini = new TIniFile(Path);


            //Settings.Default.LIN_DEVICE = xConfig.Lin.Device;
            //Settings.Default.LIN_SPEED = xConfig.Lin.Speed;

            Settings.Default.CAN_CHANNEL = xConfig.Can.Channel;
            Settings.Default.CAN_DEVICE = xConfig.Can.Device;
            Settings.Default.CAN_ID = xConfig.Can.ID;
            Settings.Default.CAN_SPEED = xConfig.Can.Speed; 

            Settings.Default.CLIENT_IP = xConfig.Client.IP;
            Settings.Default.CLIENT_PORT = xConfig.Client.Port;
            
            Settings.Default.SERVER_IP = xConfig.Server.IP;
            Settings.Default.SERVER_PORT = xConfig.Server.Port;

            Settings.Default.PC_IP = xConfig.PC.IP;
            Settings.Default.PC_PORT = xConfig.PC.Port;
            Settings.Default.BOARD_IP = xConfig.Board.IP;
            Settings.Default.BOARD_PORT = xConfig.Board.Port;

            Settings.Default.NOISE_METER_PORT = xConfig.NoiseMeter.Port;
            Settings.Default.NOISE_METER_SPEED = xConfig.NoiseMeter.Speed;

            Settings.Default.PANEL_METER_PORT = xConfig.Panel.Port;
            Settings.Default.PANEL_METER_SPEED = xConfig.Panel.Speed;

            Settings.Default.POWER_PORT = xConfig.Power.Port;
            Settings.Default.POWER_SPEED = xConfig.Power.Speed;

            Settings.Default.BATT_ID = xConfig.BattID;
            Settings.Default.CURR_ID = xConfig.CurrID;

            Settings.Default.AUTO_CONNECTION = xConfig.AutoConnection;
            Settings.Default.CONNECTIONDELAY = xConfig.PinConnectionDelay;

            Settings.Default.STROKE_PORT = xConfig.Stroke.Port;
            Settings.Default.STROKE_SPEED = xConfig.Stroke.Speed;

            Settings.Default.KALMAN_SPEED = xConfig.KalmanSpeed;
            Settings.Default.Save();
            return;
        }

        ~ConfigSetting()
        {
        }
    }

    /*
    public class ELoadControl
    {
        private string SendString;
        private char[] SendBuf = new char[1000];
        //private bool ELoadCheckFlag; //extern 

        private char[] ELoad_R_Data = new char[200];//extern 
        //private string rxData;
        private short ELoad_R_Length;//extern 
        private bool ELoad_R_Flag;//extern 
        private const byte STX = 0x13;
        private const byte ETX = 0x11;
        private const byte END = 0x0A;


        private float Power = 0;
        private float Current = 0;
        private bool InputLoadOnOffFlag = false;
        private bool OutputLoadOnOffFlag = false;
        private short ReadMode = 0;
        private float LoadVoltValue = 0;
        private float LoadCurrentValue = 0;

        private SerialPort ELoadPort;

        public ELoadControl(SerialPort Ps)
        {
            //rxData = "";
            ELoadPort = Ps;
            //ELoadCheckFlag = false;
        }

        public SerialPort ELoadPortOpen
        {
            set
            {
                ELoadPort = value;
                //ELoadCheckFlag = false;
                //rxData = "";
            }
        }

        ~ELoadControl()
        {
        }

        public bool IsOpen
        {
            get
            {
                return ELoadPort.IsOpen;
            }
        }

        
        public void ELoadCheckMessage()
        {
            if (ELoadPort.IsOpen == true)
            {
                Array.Clear(SendBuf, 0, 1000);

                SendString = "*IDN?\n";
                Array.Copy(SendString.ToCharArray(), SendBuf, SendString.Length);
#if PROGRAM_RUNNING
                ELoadPort.Write(SendBuf, 0, SendString.Length);
#endif
            }

            return;
        }

        public char[] SetReceiveData
        {
            set
            {
                Array.Copy(value, 0, ELoad_R_Data, 0, value.Length);
            }        
        }
        public short SetReceiveLength
        {
            set
            {
                ELoad_R_Length = value;
            }
        }

        public bool SetReceiveFlag
        {
            set
            {
                ELoad_R_Flag = value;
            }
            get
            {
                return ELoad_R_Flag;
            }
        }

        

        public void ReceiveMsgCheck()
        {
            if (ELoad_R_Flag == false) return;

            string s = MainForm.ComF.ToString(ELoad_R_Data);

            if (0 <= s.IndexOf("PLZ664WA"))
            {
                //ELoadCheckFlag = true;
            }
            else
            {
                if (ELoad_R_Data[0] == STX)
                {
                    if ((ELoad_R_Data[ELoad_R_Length - 2] == END) && (ELoad_R_Data[ELoad_R_Length - 1] == ETX))
                    {
                        string[] sx = s.Split((char)STX);
                        if (0 < sx.Length)
                        {
                            string[] sx1 = sx[0].Split((char)END);
                            if (0 < sx1.Length)
                            {
                                
                                switch (ReadMode)
                                {
                                    case 1:
                                        if (float.TryParse(sx1[0], out Power) == false) Power = 0;
                                        break;
                                    case 2:
                                        if (float.TryParse(sx1[0], out Current) == false) Current = 0;
                                        break;
                                }
                            }
                        }
                    }
                }
            }
            ReadMode = 0;
            Array.Clear(ELoad_R_Data, 0, ELoad_R_Data.Length);
            ELoad_R_Flag = false;
            ELoad_R_Length = 0;
            return;
        }

        private void SetCurrent(float Current)
        {
            if (ELoadPort.IsOpen == true)
            {
                Array.Clear(SendBuf, 0, 1000);

                SendString = string.Format("SOURce:CURRent:LEVel:IMMediate:AMPLitude {0:0.00}\n", Current);
                Array.Copy(SendString.ToCharArray(), SendBuf, SendString.Length);
#if PROGRAM_RUNNING
                ELoadPort.Write(SendBuf, 0, SendString.Length);
#endif
            }
            return;
        }

        private void SetPower(float Volt)
        {
            if (ELoadPort.IsOpen == true)
            {
                Array.Clear(SendBuf, 0, 1000);

                SendString = string.Format("SOURce:POWer:LEVel:IMMediate:AMPLitude {0:0.00}\n", Volt);
                Array.Copy(SendString.ToCharArray(), SendBuf, SendString.Length);
#if PROGRAM_RUNNING
                ELoadPort.Write(SendBuf, 0, SendString.Length);
#endif
            }
            return;
        }
        
        public float LoadVoltage
        {
            set
            {
                LoadVoltValue = value;
                SetPower(value);
            }
            get
            {
                return LoadVoltValue;
            }
        }

        public float LoadCurrent
        {
            set
            {
                LoadCurrentValue = value;
                SetCurrent(value);
            }
            get
            {
                return LoadCurrentValue;
            }
        }


        public bool InputLoadOnOff
        {
            set
            {
                InputLoadOnOffFlag = value;
                SetInputLoadOn(value);
            }
            get
            {
                return InputLoadOnOffFlag;
            }
        }

        public bool OutputLoadOnOff
        {
            set
            {
                OutputLoadOnOffFlag = value;
                SetOutputLoadOn(value);
            }
            get
            {
                return OutputLoadOnOffFlag;
            }
        }

        private void SetInputLoadOn(bool Flag)
        {
            if (ELoadPort.IsOpen == true)
            {
                Array.Clear(SendBuf, 0, 1000);

                if (Flag == false)
                    SendString = "INPut:STATe:IMMEdiate 0\n";
                else SendString = "INPut:STATe:IMMEdiate 1\n";
                Array.Copy(SendString.ToCharArray(), SendBuf, SendString.Length);
#if PROGRAM_RUNNING
                ELoadPort.Write(SendBuf, 0, SendString.Length);
#endif
            }
            return;
        }

        private void SetOutputLoadOn(bool Flag)
        {
            if (ELoadPort.IsOpen == true)
            {
                Array.Clear(SendBuf, 0, 1000);

                if (Flag == false)
                    SendString = "OUTPut:STATe:IMMEdiate 0\n";
                else SendString = "OUTPut:STATe:IMMEdiate 1\n";
                Array.Copy(SendString.ToCharArray(), SendBuf, SendString.Length);
#if PROGRAM_RUNNING
                ELoadPort.Write(SendBuf, 0, SendString.Length);
#endif
            }
            return;
        }
        
        public void GetPower()
        {
            if (ELoadPort.IsOpen == true)
            {
                Array.Clear(SendBuf, 0, 1000);

                SendString = "SOURce:POWer:LEVel:IMMediate:AMPLitude?\n";
                
                Array.Copy(SendString.ToCharArray(), SendBuf, SendString.Length);
                ReadMode = 1;
#if PROGRAM_RUNNING
                ELoadPort.Write(SendBuf, 0, SendString.Length);
#endif
                return;
            }
        }

        public void GetCurrent()
        {
            if (ELoadPort.IsOpen == true)
            {
                Array.Clear(SendBuf, 0, 1000);

                SendString = "SOURce:CURRent:LEVel:IMMediate:AMPLitude?\n";

                Array.Copy(SendString.ToCharArray(), SendBuf, SendString.Length);
                ReadMode = 2;
#if PROGRAM_RUNNING
                ELoadPort.Write(SendBuf, 0, SendString.Length);
#endif
                return;
            }
        }
    }

    */
    public class PowerControl
    {
        //public string SendString;
        //public char[] SendBuf = new char[1000];
        public bool DCPowerCheckFlag; //extern 

        public char[] POWER_R_Data = new char[200];//extern 
        //private string rxData;
        public short Power_R_Length;//extern 
        private bool Power_R_Flag;//extern 
        private bool ExitFlag = false;
        private Timer pwTimer = new Timer()
        {
            Interval = 50
        };

        //long DCPowerCheckFirst = 0;
        //long DCPowerCheckLast = 0;


        public Stopwatch STOP_WATCH = new Stopwatch();

        private SerialPort PowerPort;

        public PowerControl(__Port__ Ps)
        {
            int Speed;

            Speed = 9600;
            //rxData = "";



            bool Flag = true;
            if (Ps.Port == "") Flag = false;
            if (Ps.Port == null) Flag = false;
            if (Ps.Port == string.Empty) Flag = false;

            if(Flag == true)
                PowerPort = new SerialPort(Ps.Port, Speed, Parity.None, 8, StopBits.One);
            else PowerPort = new SerialPort();

            if (Flag == false)
            {
                PowerPort.Parity = Parity.None;
                PowerPort.DataBits = 8;
                PowerPort.StopBits = StopBits.One;
            }
            PowerPort.ReadTimeout = 500;
            PowerPort.WriteTimeout = 500;
            PowerPort.ReadBufferSize = 4096;
            PowerPort.WriteBufferSize = 2048;
            PowerPort.DataReceived += new SerialDataReceivedEventHandler(PowerDataCatch);

            if (Flag == true)
            {
                string[] sName = System.IO.Ports.SerialPort.GetPortNames();

                if (sName.Contains(Ps.Port) == true)
                {
                    try
                    {
                        PowerPort.Open();
                    }
                    catch { }
                }
            }
            
            STOP_WATCH.Start();
            pwTimer.Tick += new EventHandler(pwTimer_Tick);
            pwTimer.Enabled = true;
        }

        public SerialPort PowerPortOpen
        {
            set
            {
                PowerPort = value;
                pwTimer.Tick += new EventHandler(pwTimer_Tick);
                pwTimer.Enabled = true;
                //rxData = "";
                STOP_WATCH.Start();
            }
        }

        ~PowerControl()
        {
            ExitFlag = true;
            STOP_WATCH.Stop();
        }

        public void Close()
        {
            if (PowerPort != null)
            {
                if (PowerPort.IsOpen == true) PowerPort.Close();
            }
            return;
        }

        private List<string> MsgList = new List<string>();

        private bool Question = false;
        private long QuestionFirst = 0;
        private long QuestionLast = 0;
        private bool MsgSendFlag = false;

        private void pwTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                pwTimer.Enabled = false;

                if (Question == false)
                {
                    if (0 < MsgList.Count)
                    {
                        string SendString = MsgList[0] + "\n";
                        char[] SendBuf = new char[1000];

                        if (0 <= SendString.IndexOf("?"))
                        {
                            Question = true;
                            QuestionFirst = timeGetTimems();
                            QuestionLast = timeGetTimems();
                        }
                        Array.Clear(SendBuf, 0, 1000);
                        Array.Copy(SendString.ToCharArray(), SendBuf, SendString.Length);
                        PowerPort.Write(SendBuf, 0, SendString.Length);
                        MsgList.RemoveAt(0);
                        QuestionFirst = timeGetTimems();
                        QuestionLast = timeGetTimems();
                    }
                    else
                    {
                        QuestionLast = timeGetTimems();
                        if (500 <= (QuestionLast - QuestionFirst))
                        {
                            if (MsgSendFlag == false)
                                PowerReadMessage();
                            else PowerOnOffReadMessage();
                            MsgSendFlag = !MsgSendFlag;
                        }
                    }
                }
                else
                {
                    QuestionLast = timeGetTimems();
                    if (1500 <= (QuestionLast - QuestionFirst))
                    {
                        //1.5초 동안 응답이 없으면 타임 종료로 다시 통신을 제게한다.
                        Question = false;
                    }
                }
            }
            catch
            {

            }
            finally
            {
                pwTimer.Enabled = !ExitFlag;
            }
            return;
        }

        public bool IsOpen
        {
            get
            {
                return PowerPort.IsOpen;
            }
        }

        public long timeGetTimems()
        {
            return STOP_WATCH.ElapsedMilliseconds;
        }

        public void POWER_PWSetting(float Volt)   // Power Setting
        {
            if (PowerPort.IsOpen == false) return;

#if PROGRAM_RUNNING
            if (PowerPort.IsOpen == true) MsgList.Add(string.Format("VOLT {0:00.00}", Volt));
#endif
            return;
        }
        //---------------------------------------------------------------------------
        public void POWER_CURRENTSetting(float Current)   // Current Setting
        {
            if (PowerPort.IsOpen == false) return;
#if PROGRAM_RUNNING
            if (PowerPort.IsOpen == true) MsgList.Add(string.Format("CURR {0:00.00}", Current));
#endif
            return;
        }
        //---------------------------------------------------------------------------

        public void POWER_PWON()   // Power Output ON
        {
            if (PowerPort.IsOpen == false) return;

#if PROGRAM_RUNNING
            if (PowerPort.IsOpen == true) MsgList.Add("Outp ON");
#endif
            return;
        }
        //---------------------------------------------------------------------------
        public void POWER_PWOFF()  // Power Output OFF
        {
            if (PowerPort.IsOpen == false) return;

#if PROGRAM_RUNNING
            if (PowerPort.IsOpen == true) MsgList.Add("Outp OFF");
#endif
            return;
        }
        //---------------------------------------------------------------------------
        public void CHANNELSELECT()   // Power Output ON
        {
            if (PowerPort.IsOpen == false) return;

#if PROGRAM_RUNNING
            if (PowerPort.IsOpen == true) MsgList.Add("INST OUT1");
#endif
            return;
        }
        //---------------------------------------------------------------------------
        public void timedelay(long time)
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
        //---------------------------------------------------------------------------

        public void DCPowerReset()
        {
#if PROGRAM_RUNNING
            DCPowerCheckFlag = false;
            if (PowerPort.IsOpen == true) MsgList.Add("*RST");
#endif
            return;
        }
        //---------------------------------------------------------------------------

        public string CheckPowerSerialNo()
        {
#if PROGRAM_RUNNING
            string s;

            DCPowerCheckFlag = false;
            if (PowerPort.IsOpen == true) MsgList.Add("*SN?");
            s = ReadMessage();
            return s;
#else
            return "";
#endif
        }

        private string ToString(char[] Data)
        {
            string s;
            short i;

            for (i = 0; i < 50; i++)
            {
                if (Data[i] == 0x0000) break;
            }

            s = new string(Data).Substring(0, i);

            return s;
        }
        //---------------------------------------------------------------------------

        public enum SendMsgMode
        {
            VOLT,
            ONOFF
        }

        private bool SendFlag = false;
        private SendMsgMode SendMessageMode = SendMsgMode.VOLT;

        public SendMsgMode GetSendMsg
        {
            get
            {
                return SendMessageMode;
            }
        }

        private float SetVolt = 0;
        private bool PowerOnOff = false;

        public float ReadVolt
        {
            get
            {
                if (PowerOnOff == true)
                    return SetVolt;
                else return 0;
            }
        }

        public bool isPowerOn
        {
            get { return PowerOnOff; }
        }

        public bool GetReadEnd
        {
            get { return !SendFlag; }
            set { SendFlag = !value; }
        }

        public void PowerReadMessage()
        {

#if PROGRAM_RUNNING
            if (PowerPort.IsOpen == true) MsgList.Add("VOLT?");
#endif
            SendFlag = true;
            SendMessageMode = SendMsgMode.VOLT;
            return;
        }

        public void PowerOnOffReadMessage()
        {


#if PROGRAM_RUNNING
            if (PowerPort.IsOpen == true) MsgList.Add("OUTP?");
#endif
            SendFlag = true;
            SendMessageMode = SendMsgMode.ONOFF;
            return;
        }



        void POWER_Read()
        {
            /*
                        try
                        {
#if PROGRAM_RUNNING
                        try
                        {
                            s = ReadMessage();                    
                            return;
                        }
                        else{
                            Data->Flag = false;
                            Data->ReadFlag = false;
                        }

                        catch(...)
                        {
                        }
#endif
                        }
                        finally
                        {
                        }
            */
            return;
        }

        public string ReadMessage()
        {
            //#if PROGRAM_RUNNING	        
            long First;
            long Last;
            bool Flag;
            string s;

            if (PowerPort.IsOpen == false) return "";
            s = "";
            try
            {
                try
                {
                    First = timeGetTimems();
                    Flag = PowerPort.IsOpen;

                    do
                    {
                        if (Flag == true)
                        {
                            if (Power_R_Flag == true)
                            {
                                s = POWER_R_Data.ToString();
                                Power_R_Flag = false;
                                Power_R_Length = 0;
                                Array.Clear(POWER_R_Data, 0, 200);
                                return s;
                            }
                        }
                        else break;
                        Application.DoEvents();
                        Last = timeGetTimems();
                    } while ((Last - First) < 1000);
                }
                catch (Exception Msg)
                {
                    MessageBox.Show(Msg.Message + "\n" + Msg.StackTrace);
                    //uMessageBox.Show(promptText: ex.Message + "\n" + ex.StackTrace);
                }
            }
            finally
            {

            }
            return s;
            //#else
            //            return "";
            //#endif
        }
        //---------------------------------------------------------------------------

        private void PowerDataCatch(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                timedelay(30);
                int Length = PowerPort.BytesToRead;
                byte[] buffer = new byte[Length + 10];

                PowerPort.Read(buffer, 0, Length);

                string[] s = Encoding.Default.GetString(buffer).Split('\n');

                if (Question == true) Question = false;
                if (SendMessageMode == SendMsgMode.VOLT)
                {
                    if (float.TryParse(s[0], out SetVolt) == false) { }
                }
                else
                {
                    PowerOnOff = s[0] == "1" ? true : false;
                }
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
    }
}

