using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;

namespace PSeatTester
{

    /*
    #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            //this.treeView1 = new System.Windows.Forms.TreeView();
            this.treeView1 = new LedLifeTester.BufferedTreeView();
            //this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.richTextBox1 = new BufferedRichTextBox();
            //this.listView1 = new System.Windows.Forms.ListView();
            this.listView1 = new BufferedListView();
        }
        위에처럼 생선언하면 깜빡임을 엄청 줄일수 있다.
        먼저 도구 상자에서 해당 컴포넌트를 배치 시키고 선언할것 그래야 편함
    */
    class BufferedTreeView : TreeView
    {
        protected override void OnHandleCreated(EventArgs e)
        {
            SendMessage(this.Handle, TVM_SETEXTENDEDSTYLE, (IntPtr)TVS_EX_DOUBLEBUFFER, (IntPtr)TVS_EX_DOUBLEBUFFER);
            base.OnHandleCreated(e);
        }
        // Pinvoke:
        private const int TVM_SETEXTENDEDSTYLE = 0x1100 + 44;
        private const int TVM_GETEXTENDEDSTYLE = 0x1100 + 45;
        private const int TVS_EX_DOUBLEBUFFER = 0x0004;
        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);
    }
    class BufferedRichTextBox : RichTextBox
    {
        protected override void OnHandleCreated(EventArgs e)
        {
            SendMessage(this.Handle, TVM_SETEXTENDEDSTYLE, (IntPtr)TVS_EX_DOUBLEBUFFER, (IntPtr)TVS_EX_DOUBLEBUFFER);
            base.OnHandleCreated(e);
        }
        // Pinvoke:
        private const int TVM_SETEXTENDEDSTYLE = 0x1100 + 44;
        private const int TVM_GETEXTENDEDSTYLE = 0x1100 + 45;
        private const int TVS_EX_DOUBLEBUFFER = 0x0004;
        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);
    }
    //class BufferedListView : ListView
    //{
    //    protected override void OnHandleCreated(EventArgs e)
    //    {
    //        SendMessage(this.Handle, TVM_SETEXTENDEDSTYLE, (IntPtr)TVS_EX_DOUBLEBUFFER, (IntPtr)TVS_EX_DOUBLEBUFFER);
    //        base.OnHandleCreated(e);
    //    }
    //    Pinvoke:
    //    private const int TVM_SETEXTENDEDSTYLE = 0x1100 + 44;
    //    private const int TVM_GETEXTENDEDSTYLE = 0x1100 + 45;
    //    private const int TVS_EX_DOUBLEBUFFER = 0x0004;
    //    [DllImport("user32.dll")]
    //    private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);
    //}
    class BufferedListView : ListView
    {
        bool scrollDown;
        int lastScroll;
        public Color GridLinesColor { get; set; }
        [DllImport("user32")]
        private static extern int GetScrollPos(IntPtr hwnd, int nBar);

        public BufferedListView()
        {
            GridLinesColor = Color.Red;
            DoubleBuffered = true;
            base.GridLines = false;//We should prevent the default drawing of gridlines.
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            SendMessage(this.Handle, TVM_SETEXTENDEDSTYLE, (IntPtr)TVS_EX_DOUBLEBUFFER, (IntPtr)TVS_EX_DOUBLEBUFFER);
            base.OnHandleCreated(e);
        }
        // Pinvoke:
        private const int TVM_SETEXTENDEDSTYLE = 0x1100 + 44;
        private const int TVM_GETEXTENDEDSTYLE = 0x1100 + 45;
        private const int TVS_EX_DOUBLEBUFFER = 0x0004;
        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);


        public event ScrollEventHandler Scroll;
        protected virtual void OnScroll(ScrollEventArgs e)
        {
            ScrollEventHandler handler = this.Scroll;
            if (handler != null) handler(this, e);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x20a)
            {
                //WM_MOUSEWHEEL = 0x20a
                scrollDown = (m.WParam.ToInt64() >> 16) < 0;
                OnScroll(new ScrollEventArgs((ScrollEventType)(m.WParam.ToInt32() & 0xffff), GetScrollPos(Handle, 0)));
            }
            if (m.Msg == 0x115)
            {
                //WM_VSCROLL = 0x115
                int n = (m.WParam.ToInt32() >> 16);
                scrollDown = n > lastScroll;
                lastScroll = n;
                OnScroll(new ScrollEventArgs((ScrollEventType)(m.WParam.ToInt32() & 0xffff), GetScrollPos(Handle, 0)));
            }

            if (m.Msg == 0xf)
            {
                //위에 그림을 그린 후 호출하지 않으면 화면이 깨지는 현상 발생
                //Horigental scrol
                OnScroll(new ScrollEventArgs((ScrollEventType)(m.WParam.ToInt32() & 0xffff), GetScrollPos(Handle, 0)));
            }

            Pen op = new Pen(BackColor);
            base.WndProc(ref m);
            if (m.Msg == 0xf && GridLines && Items.Count > 0 && View == View.Details)//WM_PAINT = 0xf
            {
                using (Graphics g = CreateGraphics())
                {
                    using (Pen p = new Pen(GridLinesColor))
                    {
                        int w = -GetScrollPos(Handle, 0);
                        int hw = w;
                        for (int i = 0; i < Columns.Count; i++)
                        {
                            w += Columns[i].Width;
                            g.DrawLine(p, new Point(w, 0), new Point(w, ClientSize.Height));
                        }
                        int a = Items[0].Bounds.Bottom - 1;
                        int b = Height - Items[0].Bounds.Y;
                        int c = Items[0].Bounds.Height;

                         
                        for (int i = scrollDown ? a + (b / c) * c : a; scrollDown ? i >= a : i < b; i += scrollDown ? -c : c)
                        {
                            ListViewItem Item = GetItemAt(1, i + (Items[0].Bounds.Height / 2));

                            //if ((0 < i) && (i < Items.Count))
                            if (Item != null)
                            {
                                //if (0 < Item.SubItems.Count)
                                //{
                                //    if (0 < Item.SubItems[0].Text.Length)
                                //        g.DrawLine(p, new Point(0, i), new Point(ClientSize.Width, i));
                                //    else g.DrawLine(p, new Point(Columns[0].Width, i), new Point(ClientSize.Width, i));
                                //}
                                //else
                                //{
                                //    g.DrawLine(p, new Point(Columns[0].Width, i), new Point(ClientSize.Width, i));
                                //}

                                //화면을 갱신하기 위해 라인을 백 컬러로 지운다.
                                g.DrawLine(op, new Point(0, i), new Point(ClientSize.Width, i));

                                if (0 < Item.SubItems[1].Text.Length)
                                {
                                    g.DrawLine(p, new Point(0, i), new Point(ClientSize.Width, i));
                                }

                                else
                                {
                                    int x = (Columns[0].Width + Columns[1].Width) + hw;
                                    if (x < 0) x = 0;
                                    g.DrawLine(p, new Point(x, i), new Point(ClientSize.Width, i));
                                }
                            }
                            else
                            {
                                g.DrawLine(p, new Point(0, i), new Point(ClientSize.Width, i));
                            }
                        }
                    }
                }                
            }
        }        
    }

    public class ComboBoxWithBorder : ComboBox
    {
        private Color _borderColor = Color.Black;
        private ButtonBorderStyle _borderStyle = ButtonBorderStyle.Solid;
        private static int WM_PAINT = 0x000F;
        
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == WM_PAINT)
            {
                Graphics g = Graphics.FromHwnd(Handle);
                Rectangle bounds = new Rectangle(0, 0, Width, Height);
                ControlPaint.DrawBorder(g, bounds, _borderColor, _borderStyle);
            }
        }

        [Category("Appearance")]
        public Color BorderColor
        {
            get { return _borderColor; }
            set
            {
                _borderColor = value;
                Invalidate(); // causes control to be redrawn 
            }
        }

        [Category("Appearance")]
        public ButtonBorderStyle BorderStyle
        {
            get { return _borderStyle; }
            set
            {
                _borderStyle = value;
                Invalidate();
            }
        }
    }
}
