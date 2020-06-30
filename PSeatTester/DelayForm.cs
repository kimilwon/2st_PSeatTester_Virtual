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
    public partial class DelayForm : Form
    {
        //Timer timer1 = null;
        System.Threading.Timer timer = null;
        int Target = 0;
        public DelayForm()
        {
            InitializeComponent();
        }

        public DelayForm(int Delay)
        {
            InitializeComponent();
            Target = Delay;
        }
        
        delegate void TimerEventFiredDelegate();

        void Callback(object status)
        {
            // UI 에서 사용할 경우는 Cross-Thread 문제가 발생하므로 Invoke 또는 BeginInvoke 를 사용해서 마샬링을 통한 호출을 처리하여야 한다.
            BeginInvoke(new TimerEventFiredDelegate(Work));
        }

        private void Work()
        {
            // UI 처리 작업...
            timer.Dispose();
            this.Close();            
            return;
        }

        private void DelayForm_Load(object sender, EventArgs e)
        {
            //timer1 = new Timer();
            //timer1.Tick += new EventHandler(Timer_Tick);
            //timer1.Interval = Target;
            //timer1.Enabled = true;
            timer = new System.Threading.Timer(Callback, null, Target, Target);// dueTime 값을 0으로 설정할 경우 바로 함수가 한번 호출되게 되서 상태에 따라 값을 0 또는 Interval 만큼 주어야 한다.
            //timer.Change(0, Target);    // dueTime 은 Timer 가 시작되기 전 대기 시간 (ms)
        }

        //private void Timer_Tick(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        timer1.Enabled = false;
        //        this.Close();
        //    }
        //    catch
        //    {

        //    }
        //    finally
        //    {
        //    }
        //    return;
        //}
    }
}
