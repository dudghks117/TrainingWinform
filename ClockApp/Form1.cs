using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClockApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            label1.Text = now.ToString("HH:mm:ss");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MessageBox.Show("폼 로드시 발생");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            MessageBox.Show("폼 클로즈시 발생");
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            MessageBox.Show("폼 활성화시 발생");
        }

        private void Form1_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("진짜 닫을래?", "d경고d",
                MessageBoxButtons.YesNo,MessageBoxIcon.Error) == DialogResult.Yes)
            {
                e.Cancel = false;   //  클로징 이벤트를 취소 안시켜서 종료
            }
            else
            {
                e.Cancel = true;    //   클로징 이벤트를 취소시켜서 종료안됨
            }
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {

        }
    }
}
