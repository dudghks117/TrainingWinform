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
    public partial class memberForm : MetroForm
    {
        string mode = "";
        public memberForm()
        {
            InitializeComponent();
        }

        private void DivForm_Load(object sender, EventArgs e)
        {
            UpdateData();   //  데이터그리드에 DB 데이터 로딩하기
        }



        private void UpdateData()
        {
            //throw new NotImplementedException();
            using (SqlConnection conn = new SqlConnection(Commons.CONSTRING))
            {
                conn.Open();    //  DB열기
                string strQuery = "SELECT Idx, Names, Levels, Addr, Mobile, Email " +
                                   " FROM dbo.membertbl "; //  검색할 쿼리문
                SqlDataAdapter dataAdapter = new SqlDataAdapter(strQuery, conn);    //  플러그에 쿼리문이랑 데이터 연결할꺼
                DataSet ds = new DataSet(); //  데이터 통을 만듬
                dataAdapter.Fill(ds, "membertbl");     //  데이터 통을 채움 divtbl이라는 이름으로

                GrdMemberTbl.DataSource = ds;      //  GrdDivtbl에 부어넣음
                GrdMemberTbl.DataMember = "membertbl";
            }
        }

        private void GrdDivTbl_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex > -1)
            {
                DataGridViewRow data = GrdMemberTbl.Rows[e.RowIndex];  //  Rows중에 한 행을 선택했으므로 DataGridViewRow이다.
                TxtIdx.Text = data.Cells[0].Value.ToString();    //  Cells는 한 행이기 때문에 Division, Names가 들어가있음
                TxtNames.Text = data.Cells[1].Value.ToString();
                CboLevels.SelectedIndex = CboLevels.FindString(data.Cells[2].Value.ToString());
                TxtAddr.Text = data.Cells[3].Value.ToString();
                TxtMobile.Text = data.Cells[4].Value.ToString();
                TxtEmail.Text = data.Cells[5].Value.ToString();

                TxtIdx.ReadOnly = true;
                TxtIdx.BackColor = Color.Black;

                mode = "UPDATE";    //수정은 UPDATE
            }
        }

        private void BtnNew_Click(object sender, EventArgs e)
        {
            mode = "INSERT"; //신규는 INSERT
            if (String.IsNullOrEmpty(TxtAddr.Text) || String.IsNullOrEmpty(TxtNames.Text) ||
                String.IsNullOrEmpty(TxtMobile.Text) || String.IsNullOrEmpty(TxtEmail.Text))
            {
                MetroMessageBox.Show(this, "빈값은 저장할 수 없습니다", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            SaveProcess();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            mode = "UPDATE";
            if(String.IsNullOrEmpty(TxtAddr.Text) || String.IsNullOrEmpty(TxtNames.Text) ||
                String.IsNullOrEmpty(TxtMobile.Text) || String.IsNullOrEmpty(TxtEmail.Text))
            {
                MetroMessageBox.Show(this, "빈값은 저장할 수 없습니다", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            SaveProcess();
        }

        private void ClearTextControls()
        {
            TxtMobile.Text = TxtEmail.Text = TxtAddr.Text = TxtIdx.Text = TxtNames.Text = "";
            TxtIdx.ReadOnly = true;
            CboLevels.SelectedIndex = -1;
            TxtIdx.BackColor = Color.Beige;
            TxtNames.Focus();
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
                    cmd.CommandText = "UPDATE dbo.membertbl " +
                                      "   SET Names = @Names " +
                                      "     , Levels = @Levels " +
                                      "     , Addr = @Addr " +
                                      "     , Mobile = @Mobile " +
                                      "     , Email = @Email " +
                                      " WHERE Idx = @Idx ";
                    SqlParameter parmIdx = new SqlParameter("@Idx", SqlDbType.Int, 100);
                    parmIdx.Value = TxtIdx.Text;
                    cmd.Parameters.Add(parmIdx);
                }
                else if (mode == "INSERT")
                {
                    cmd.CommandText = "INSERT INTO dbo.membertbl (Names, Levels, Addr, Mobile, Email) " +
                                      "     VALUES (@Names, @Levels, @Addr, @Mobile, @Email) ";
                }

                SqlParameter parmNames = new SqlParameter("@Names", SqlDbType.VarChar, 45);
                parmNames.Value = TxtNames.Text;
                cmd.Parameters.Add(parmNames);
                SqlParameter parmLevels = new SqlParameter("@Levels", SqlDbType.Char, 1);
                parmLevels.Value = CboLevels.SelectedItem;
                cmd.Parameters.Add(parmLevels);
                SqlParameter parmAddr = new SqlParameter("@Addr", SqlDbType.VarChar, 100);
                parmAddr.Value = TxtAddr.Text;
                cmd.Parameters.Add(parmAddr);
                SqlParameter parmMobile = new SqlParameter("@Mobile", SqlDbType.VarChar, 13);
                parmMobile.Value = TxtMobile.Text;
                cmd.Parameters.Add(parmMobile);
                SqlParameter parmEmail = new SqlParameter("@Email", SqlDbType.VarChar, 50);
                parmEmail.Value = TxtEmail.Text;
                cmd.Parameters.Add(parmEmail);

                cmd.ExecuteNonQuery();
            }
            UpdateData();
            ClearTextControls();
        }

        private void TxtDivision_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13)
            {
                BtnSave_Click(sender, new EventArgs());
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            mode = "DELETE";
            if (String.IsNullOrEmpty(TxtIdx.Text) || String.IsNullOrEmpty(TxtNames.Text))
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
                parmDivision.Value = TxtIdx.Text;
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

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
