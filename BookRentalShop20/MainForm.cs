using System.Windows.Forms;
using MetroFramework;
using MetroFramework.Forms;

namespace BookRentalShop20
{
    public partial class MainForm : MetroForm
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, System.EventArgs e)
        {
            LoginForm loginForm = new LoginForm();
            loginForm.ShowDialog();
        }

        private void MainForm_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            if(MetroMessageBox.Show(this, "정말 종료하시겠습니까?", "종료", MessageBoxButtons.YesNo, MessageBoxIcon.Question)== DialogResult.Yes)
            {
                foreach(Form TheForm in this.MdiChildren)
                {
                    TheForm.Close();
                }
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }

        }

        private void InitChildForm(Form form, string strFormTitle)
        {
            form.Text = strFormTitle;
            form.Dock = DockStyle.Fill;
            form.MdiParent = this;
            form.WindowState = FormWindowState.Maximized;
            form.Show();
        }

        private void MnuItemDivMng_Click(object sender, System.EventArgs e)
        {
            DivForm form = new DivForm();
            InitChildForm(form, "구분코드관리");

        }


        private void MnuUserDiv_Click(object sender, System.EventArgs e)
        {
            UserForm form = new UserForm();
            InitChildForm(form, "사용자관리");
        }

        private void 회원관리MToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            memberForm form = new memberForm();
            InitChildForm(form, "회원관리");

        }

        private void MainForm_Activated(object sender, System.EventArgs e)
        {
            LblUserID.Text = Commons.LOGINUSERID;
        }

        private void 책관리BToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            BooksForm form = new BooksForm();
            InitChildForm(form, "책 관리");
        }

        private void 대여관리RToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            RentalForm form = new RentalForm();
            InitChildForm(form, "대여 관리");
        }
    }
}
