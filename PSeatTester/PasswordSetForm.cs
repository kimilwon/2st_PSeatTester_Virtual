using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Text;
using System.Windows.Forms;
//using INI_IO;
//using __COMMONE;

namespace PSeatTester
{
    public partial class PasswordSetForm : Form
    {        
        public PasswordSetForm()
        {
            InitializeComponent();            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();            
            return;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string oldpass;
            string newpass;

            oldpass = textBox1.Text;
            newpass = textBox2.Text;
            /*
            if (oldpass == Pass.GetPassword)
            {
                Pass.SetPassword = newpass;
                Pass.SavePassword();
            }
            else
            {
                DialogResult Result;
                
                Result = MessageBox.Show("사용하던 암호 입력이 틀렸습니다.\n다시 입력하시겠습니까?", "경고", MessageBoxButtons.YesNo);
                if (Result == DialogResult.Yes) return;
            }
            */
            if (oldpass == newpass)
            {
                this.Close();
            }
            else
            {
                MessageBox.Show("암력한 암호가 일치하지 않습니다.\n다시 입력하십시오.", "경고");
            }            
            return;
        }

        private void PasswordSetForm_Load(object sender, EventArgs e)
        {
            textBox1.Focus();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar== (char)ConsoleKey.Enter) textBox2.Focus();
            return;
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)ConsoleKey.Enter)
            {
                string oldpass;
                string newpass;

                oldpass = textBox1.Text;
                newpass = textBox2.Text;

                this.Close();
            }
            return;
        }

        private void PasswordSetForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = false;
            return;
        }
    }
}
