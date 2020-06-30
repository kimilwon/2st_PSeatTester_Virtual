using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Text;
using System.Windows.Forms;
//using __COMMONE;

namespace PSeatTester
{
    public partial class PasswordCheckForm : Form
    {
        bool Result;

        public PasswordCheckForm()
        {
            InitializeComponent();            
            return;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Result = false;
            this.Close();
            return;
        }

        public bool result
        {
            get { return Result; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Result = true;
            this.Close();
            return;
        }

        private void PasswordCheckForm_Load(object sender, EventArgs e)
        {
            textBox1.Focus();
            
            return;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {            
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            //EventArgs x;

            if (e.KeyChar == (char)ConsoleKey.Enter)
            {
                Result = true;
                
                this.Close();
            }
            return;
        }

        private void PasswordCheckForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = false;
            return;
        }        
    }
}
