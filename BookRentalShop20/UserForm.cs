using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework;
using MetroFramework.Forms;

namespace BookRentalShop20
{
    public partial class UserForm : MetroForm
    {
        string mode = "";
        public UserForm()
        {
            InitializeComponent();
        }

        private void DivForm_Load(object sender, EventArgs e)
        {
            UpdateData();   //  데이터그리드에 DB 데이터 로딩하기
        }

        private void UpdateData()   //  사용자 데이터 가져오기
        {


            using (SqlConnection conn = new SqlConnection(Commons.CONSTRING))
            {
                conn.Open();
                string strQuery = "SELECT id, userID, password, lastLoginDt, loginIpAddr " +
                                  "  FROM dbo.userTbl"; //  검색할 쿼리문
                                                        // SqlCommand cmd = new SqlCommand(strQuery, conn);
                SqlDataAdapter dataAdapter = new SqlDataAdapter(strQuery, conn);
                DataSet ds = new DataSet();
                dataAdapter.Fill(ds, "userTbl");

                GrdUserTbl.DataSource = ds;
                GrdUserTbl.DataMember = "userTbl";
            }
            DataGridViewColumn column = GrdUserTbl.Columns[0];  //  id컬럼
            column.Width = 40;
            column.HeaderText = "순번";   
            column = GrdUserTbl.Columns[1];     //     userID컬럼
            column.Width = 80;
            column.HeaderText = "아이디";
            column = GrdUserTbl.Columns[2];     //      password컬럼
            column.Width = 100;
            column.HeaderText = "패스워드";
            column = GrdUserTbl.Columns[3];     //      최종접속시간
            column.Width = 120;
            column.HeaderText = "접속시간";
            column = GrdUserTbl.Columns[4];
            column.Width = 150;
            column.HeaderText = "접속아이피주소";
        }


        private void GrdDivTbl_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex > -1)
            {
                DataGridViewRow data = GrdUserTbl.Rows[e.RowIndex];  //  Rows중에 한 행을 선택했으므로 DataGridViewRow이다.
                TxtId.Text = data.Cells[0].Value.ToString();
                TxtUserID.Text = data.Cells[1].Value.ToString(); 
                TxtPassword.Text = data.Cells[2].Value.ToString();
            }
        }

        private void BtnNew_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(TxtUserID.Text) || String.IsNullOrEmpty(TxtPassword.Text))
            {
                MetroMessageBox.Show(this, "빈값은 저장할 수 없습니다", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            mode = "INSERT"; //신규는 INSERT
            SaveProcess();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if(String.IsNullOrEmpty(TxtUserID.Text) || String.IsNullOrEmpty(TxtPassword.Text))
            {
                MetroMessageBox.Show(this, "빈값은 저장할 수 없습니다", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            mode = "UPDATE";
            SaveProcess();
        }

        private void ClearTextControls()
        {
            TxtId.Text = TxtUserID.Text = TxtPassword.Text = "";
            TxtUserID.Focus();
        }

        private void SaveProcess()
        {
            using (SqlConnection conn = new SqlConnection(Commons.CONSTRING))
            {
                if(String.IsNullOrEmpty(mode))
                {
                    MetroMessageBox.Show(this, "신규버튼을 누르고 데이터를 저장하십시오", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }    
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;

                if (mode == "UPDATE")
                {
                    cmd.CommandText = "UPDATE dbo.userTbl " +
                                      "   SET userID = @userID, password = @password " +
                                      " WHERE id = @id ";
                }
                else if (mode == "INSERT")
                {
                    cmd.CommandText = "INSERT INTO dbo.userTbl ( userID, password ) " +
                                      " VALUES (@UserID, @Password) ";
                }


                SqlParameter parmUserID = new SqlParameter("@userID", SqlDbType.VarChar, 12);
                parmUserID.Value = TxtUserID.Text;
                cmd.Parameters.Add(parmUserID);

                SqlParameter parmPassword = new SqlParameter("@password", SqlDbType.VarChar, 20);
                parmPassword.Value = TxtPassword.Text;
                cmd.Parameters.Add(parmPassword);

                if (mode == "UPDATE")
                {
                    SqlParameter parmid = new SqlParameter("@id", SqlDbType.Int);
                    parmid.Value = TxtId.Text;
                    cmd.Parameters.Add(parmid);
                }




                cmd.ExecuteNonQuery();
            }
            UpdateData();
            ClearTextControls();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            mode = "DELETE";
            if (String.IsNullOrEmpty(TxtUserID.Text) || String.IsNullOrEmpty(TxtPassword.Text))
            {
                MetroMessageBox.Show(this, "빈값은 삭제할 수 없습니다", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DeleteProcess();
        }

        private void DeleteProcess()
        {
            using (SqlConnection conn = new SqlConnection(Commons.CONSTRING))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "DELETE FROM dbo.divtbl" +
                                  " WHERE Division = @Division";

                SqlParameter parmDivision = new SqlParameter("@Division", SqlDbType.Char, 4);
                parmDivision.Value = TxtUserID.Text;
                cmd.Parameters.Add(parmDivision);
                cmd.ExecuteNonQuery();
            }
            UpdateData();
            ClearTextControls();

        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
