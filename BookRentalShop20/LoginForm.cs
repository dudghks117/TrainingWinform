using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework;
using MetroFramework.Forms;

namespace BookRentalShop20
{
    public partial class LoginForm : MetroForm
    {
        string strConnString = "Data Source=192.168.0.20;Initial Catalog = BookRentalshopDB; Persist Security Info=True;User ID = sa; Password=p@ssw0rd!";
        public LoginForm()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 캔슬버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            //Application.Exit();
            Environment.Exit(0);
        }
        /// <summary>
        /// 로그인 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnOk_Click(object sender, EventArgs e)
        {
            LoginProcess();
        }

        private void TxtUserID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13) //  엔터
            {
                TxtPassword.Focus();
            }
        }

        private void TxtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13) //  엔터
            {
                LoginProcess();
            }
        }
        /// <summary>
        /// 로그인 구현
        /// </summary>
        private void LoginProcess()
        {
           // throw new NotImplementedException();
           if(string.IsNullOrEmpty(TxtUserID.Text) || string.IsNullOrEmpty(TxtPassword.Text))
            {
                MetroMessageBox.Show(this, "아이디/패스워드를 입력하세요","오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string stringUserId = string.Empty;

            try
            {
                using (SqlConnection conn = new SqlConnection(strConnString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT * FROM userTbl" +
                                      " WHERE userID = @userID" +
                                      "   AND password = @userPassword";
                    SqlParameter parmUserId = new SqlParameter("@userID", SqlDbType.VarChar, 12);
                    parmUserId.Value = TxtUserID.Text;
                    cmd.Parameters.Add(parmUserId);

                    SqlParameter parmPassword = new SqlParameter("@userPassword", SqlDbType.VarChar, 20);
                    parmPassword.Value = TxtPassword.Text;
                    cmd.Parameters.Add(parmPassword);



                    SqlDataReader reader = cmd.ExecuteReader();
                    reader.Read();
                    stringUserId = reader["userID"] != null ? reader["userID"].ToString() : "";

                    if (stringUserId != "")
                    {
                        MetroMessageBox.Show(this, "접속성공", "로그인");
                        this.Close();
                    }
                    else
                    {
                        MetroMessageBox.Show(this, "접속실패", "로그인",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    MetroMessageBox.Show(this, "접속성공", "로그인");
                    Debug.WriteLine("On the Debug");

                }
            }
            catch (Exception ex)
            {

                MetroMessageBox.Show(this, $"Error : {ex.StackTrace}", "오류", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
        }
    }
}
