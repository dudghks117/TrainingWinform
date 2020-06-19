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
    public partial class DivForm : MetroForm
    {
        string strConnString = "Data Source=192.168.0.20;Initial Catalog = BookRentalshopDB; Persist Security Info=True;User ID = sa; Password=p@ssw0rd!";
        string mode = "";
        public DivForm()
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
            using (SqlConnection conn = new SqlConnection(strConnString))
            {
                conn.Open();    //  DB열기
                string strQuery = "SELECT Division, Names FROM divtbl"; //  검색할 쿼리문
               // SqlCommand cmd = new SqlCommand(strQuery, conn);
                SqlDataAdapter dataAdapter = new SqlDataAdapter(strQuery, conn);    //  플러그에 쿼리문이랑 데이터 연결할꺼
                DataSet ds = new DataSet(); //  데이터 통을 만듬
                dataAdapter.Fill(ds, "divtbl");     //  데이터 통을 채움 divtbl이라는 이름으로

                GrdDivTbl.DataSource = ds;      //  GrdDivtbl에 부어넣음
                GrdDivTbl.DataMember = "divtbl";
            }
        }

        private void GrdDivTbl_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex > -1)
            {
                DataGridViewRow data = GrdDivTbl.Rows[e.RowIndex];  //  Rows중에 한 행을 선택했으므로 DataGridViewRow이다.
                TxtDivision.Text = data.Cells[0].Value.ToString();    //  Cells는 한 행이기 때문에 Division, Names가 들어가있음
                TxtNames.Text = data.Cells[1].Value.ToString();
                TxtDivision.ReadOnly = true;
                TxtDivision.BackColor = Color.Black;

                mode = "UPDATE";    //수정은 UPDATE
            }
        }

        private void BtnNew_Click(object sender, EventArgs e)
        {
            mode = "INSERT"; //신규는 INSERT
            if (String.IsNullOrEmpty(TxtDivision.Text) || String.IsNullOrEmpty(TxtNames.Text))
            {
                MetroMessageBox.Show(this, "빈값은 저장할 수 없습니다", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            SaveProcess();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if(String.IsNullOrEmpty(TxtDivision.Text) || String.IsNullOrEmpty(TxtNames.Text))
            {
                MetroMessageBox.Show(this, "빈값은 저장할 수 없습니다", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            SaveProcess();
        }

        private void ClearTextControls()
        {
            TxtDivision.Text = TxtNames.Text = "";
            TxtDivision.ReadOnly = false;
            TxtDivision.BackColor = Color.White;
            TxtDivision.Focus();
        }

        private void SaveProcess()
        {
            using (SqlConnection conn = new SqlConnection(strConnString))
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
                    cmd.CommandText = "UPDATE dbo.divtbl" +
                                      "   SET Names = @Names" +
                                      " WHERE Division = @Division";
                }
                else if (mode == "INSERT")
                {
                    cmd.CommandText = "INSERT INTO dbo.divtbl(Division, Names) " +
                                      " VALUES (@Division, @Names) ";
                }
                SqlParameter parmNames = new SqlParameter("@Names", SqlDbType.NVarChar, 45);
                parmNames.Value = TxtNames.Text;
                cmd.Parameters.Add(parmNames);

                SqlParameter parmDivision = new SqlParameter("@Division", SqlDbType.Char, 4);
                parmDivision.Value = TxtDivision.Text;
                cmd.Parameters.Add(parmDivision);

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
            if (String.IsNullOrEmpty(TxtDivision.Text) || String.IsNullOrEmpty(TxtNames.Text))
            {
                MetroMessageBox.Show(this, "빈값은 삭제할 수 없습니다", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DeleteProcess();
        }

        private void DeleteProcess()
        {
            using (SqlConnection conn = new SqlConnection(strConnString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "DELETE FROM dbo.divtbl" +
                                  " WHERE Division = @Division";

                SqlParameter parmDivision = new SqlParameter("@Division", SqlDbType.Char, 4);
                parmDivision.Value = TxtDivision.Text;
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
