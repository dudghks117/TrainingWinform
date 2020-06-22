using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using MetroFramework;
using MetroFramework.Forms;

namespace BookRentalShop20
{
    public partial class BooksForm : MetroForm
    {
        string mode = "";
        public BooksForm()
        {
            InitializeComponent();
        }

        private void DivForm_Load(object sender, EventArgs e)
        {
            DtpReleaseDate.CustomFormat = "yyyy-MM-dd";
            DtpReleaseDate.Format = DateTimePickerFormat.Custom;
            UpdateData();   //  데이터그리드에 DB 데이터 로딩하기
            UpdateCboDivision();
        }

        private void UpdateCboDivision()
        {
            using(SqlConnection conn = new SqlConnection(Commons.CONSTRING))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "SELECT Division, Names FROM divtbl ";
                SqlDataReader reader = cmd.ExecuteReader();
                Dictionary<string, string> temps = new Dictionary<string, string>();
                while (reader.Read())
                {
                    temps.Add(reader[0].ToString(), reader[1].ToString());
                }
                CboDivision.DataSource = new BindingSource(temps, null);
                CboDivision.DisplayMember = "Value";
                CboDivision.ValueMember = "Key";
                CboDivision.SelectedIndex = -1;
            }
        }

        private void UpdateData()
        {
            //throw new NotImplementedException();
            using (SqlConnection conn = new SqlConnection(Commons.CONSTRING))
            {
                conn.Open();    //  DB열기
                string strQuery = "SELECT b.Idx, b.Author, b.division, " +
                                  "       d.Names AS 'DivNames'," +
                                  "       b.Names, b.ReleaseDate, b.ISBN," +
                                  "       REPLACE(CONVERT(VARCHAR, CONVERT(MONEY, b.price), 1),'.00','') AS 'PRICE' " +
                                  "  FROM dbo.bookstbl AS b " +
                                  " INNER JOIN dbo.divtbl AS d ON b.Division = d.Division"; //  검색할 쿼리문
                SqlDataAdapter dataAdapter = new SqlDataAdapter(strQuery, conn);    //  플러그에 쿼리문이랑 데이터 연결할꺼
                DataSet ds = new DataSet(); //  데이터 통을 만듬
                dataAdapter.Fill(ds, "bookstbl");     //  데이터 통을 채움 divtbl이라는 이름으로

                GrdBooksTbl.DataSource = ds;      //  GrdDivtbl에 부어넣음
                GrdBooksTbl.DataMember = "bookstbl";
            }
            DataGridViewColumn column = GrdBooksTbl.Columns[2];  //  id컬럼
            column.Visible = false;
        }

        private void GrdDivTbl_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex > -1)
            {
                DataGridViewRow data = GrdBooksTbl.Rows[e.RowIndex];  //  Rows중에 한 행을 선택했으므로 DataGridViewRow이다.
                TxtIdx.Text = data.Cells[0].Value.ToString();    
                TxtIdx.ReadOnly = true;
                TxtIdx.BackColor = Color.Beige;
                TxtAuthor.Text = data.Cells[1].Value.ToString();
                //CboDivision.SelectedIndex = CboDivision.FindString(data.Cells[3].Value.ToString());   //  글자로 findstring으로 인덱스 찾음
                CboDivision.SelectedValue = data.Cells[2].Value;    //  Value로 지정된 값을 선택했을때 display 되는게 정해져있어서 됨
                TxtNames.Text = data.Cells[4].Value.ToString();

                DtpReleaseDate.CustomFormat = "yyyy-MM-dd";
                DtpReleaseDate.Format = DateTimePickerFormat.Custom;

                DtpReleaseDate.Value = DateTime.Parse(data.Cells[5].Value.ToString());
                TxtISBN.Text = data.Cells[6].Value.ToString();
                TxtPrice.Text = data.Cells[7].Value.ToString();

                mode = "UPDATE";    //수정은 UPDATE
            }
        }

        private void BtnNew_Click(object sender, EventArgs e)
        {
            mode = "INSERT"; //신규는 INSERT
            if (String.IsNullOrEmpty(TxtNames.Text) || String.IsNullOrEmpty(TxtAuthor.Text) ||
                String.IsNullOrEmpty(TxtISBN.Text) || String.IsNullOrEmpty(TxtPrice.Text))
            {
                MetroMessageBox.Show(this, "빈값은 저장할 수 없습니다", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            SaveProcess();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            mode = "UPDATE";
            if(String.IsNullOrEmpty(TxtNames.Text) || String.IsNullOrEmpty(TxtAuthor.Text) ||
                String.IsNullOrEmpty(TxtISBN.Text) || String.IsNullOrEmpty(TxtPrice.Text))
            {
                MetroMessageBox.Show(this, "빈값은 저장할 수 없습니다", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            SaveProcess();
        }

        private void ClearTextControls()
        {
            TxtPrice.Text = TxtISBN.Text = TxtNames.Text = TxtIdx.Text = TxtAuthor.Text = "";
            TxtIdx.ReadOnly = true;
            CboDivision.SelectedIndex = -1;
            TxtIdx.BackColor = Color.Beige;
            TxtAuthor.Focus();
            DtpReleaseDate.CustomFormat = " ";
            DtpReleaseDate.Format = DateTimePickerFormat.Custom;
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
                    cmd.CommandText = "UPDATE dbo.bookstbl " +
                                      "   SET Author = @Author, " +
                                      "       Division = @Division, " +
                                      "       Names = @Names, " +
                                      "       ReleaseDate = @ReleaseDate, " +
                                      "       ISBN = @ISBN, " +
                                      "       Price = @Price " +
                                      " WHERE Idx = @Idx";
                    SqlParameter parmIdx = new SqlParameter("@Idx", SqlDbType.Int, 100);
                    parmIdx.Value = TxtIdx.Text;
                    cmd.Parameters.Add(parmIdx);
                }
                else if (mode == "INSERT")
                {
                    cmd.CommandText = "INSERT INTO dbo.bookstbl (Author, Division, Names, ReleaseDate, ISBN, Price) " +
                                      "     VALUES (@Author, @Division, @Names, @ReleaseDate, @ISBN, @Price) ";
                }

                SqlParameter parmAuthor = new SqlParameter("@Author", SqlDbType.VarChar, 45);
                parmAuthor.Value = TxtAuthor.Text;
                cmd.Parameters.Add(parmAuthor);

                SqlParameter parmDivision = new SqlParameter("@Division", SqlDbType.Char, 4);
                parmDivision.Value = CboDivision.SelectedValue;
                cmd.Parameters.Add(parmDivision);

                SqlParameter parmNames = new SqlParameter("@Names", SqlDbType.VarChar, 100);
                parmNames.Value = TxtNames.Text;
                cmd.Parameters.Add(parmNames);

                SqlParameter parmReleaseDate = new SqlParameter("@ReleaseDate", SqlDbType.Date);
                parmReleaseDate.Value = DtpReleaseDate.Value;
                cmd.Parameters.Add(parmReleaseDate);

                SqlParameter parmISBN = new SqlParameter("@ISBN", SqlDbType.VarChar, 200);
                parmISBN.Value = TxtISBN.Text;
                cmd.Parameters.Add(parmISBN);

                SqlParameter parmPrice = new SqlParameter("@Price", SqlDbType.Decimal, 10);
                parmPrice.Value = TxtPrice.Text;
                cmd.Parameters.Add(parmPrice);


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
            if (String.IsNullOrEmpty(TxtIdx.Text) || String.IsNullOrEmpty(TxtAuthor.Text))
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

        private void DtpReleaseDate_ValueChanged(object sender, EventArgs e)
        {
            DtpReleaseDate.CustomFormat = "yyyy-MM-dd";
            DtpReleaseDate.Format = DateTimePickerFormat.Custom;
        }
    }
}
