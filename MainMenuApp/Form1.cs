﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MainMenuApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void MnuNewFile_Click(object sender, EventArgs e)
        {
            textBox1.Text += MnuNewFile.Text + Environment.NewLine;
            toolStripStatusLabel1.Text = MnuNewFile.Text;
            //실제 새파일 로직 집어넣어야함
        }

        private void 열기OToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Text += 열기OToolStripMenuItem.Text + Environment.NewLine;
            // 실제 열기 로직 넣어야함
        }

        private void 저장SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Text += 저장SToolStripMenuItem.Text + Environment.NewLine;
            MessageBox.Show("저장했습니다.");

        }

        private void 종료XToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void 프로그램정보AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox about = new AboutBox();
            about.ShowDialog();
        }

        private void textBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {
                contextMenuStrip1.Show();
            }
        }

        private void 삭제DToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_MouseMove(object sender, MouseEventArgs e)
        {
            LblMouseLocation.Text = $"(X, Y) = ({e.X}, {e.Y})";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            toolStripComboBox1.Items.Add("python");
            toolStripComboBox1.Items.Add("C");
            toolStripComboBox1.Items.Add("C++");
            toolStripComboBox1.Items.Add("C#");
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip1.Show(e.Location);
            }
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            button1.Focus();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            MnuNewFile_Click(sender, e);
        }
    }
}
