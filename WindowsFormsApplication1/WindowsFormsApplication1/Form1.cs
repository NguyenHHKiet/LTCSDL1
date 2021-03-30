using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;


namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SqlConnection conn;
        void KetNoi()
        {
            string chuoi = ConfigurationManager.ConnectionStrings["cnstr"].ConnectionString;
            conn = new SqlConnection(chuoi);
        }
        DataTable LayDSNhanVien()
        {
            string query = "Select * from Employees";
            SqlDataAdapter da = new SqlDataAdapter(query, conn);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds.Tables[0];
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            KetNoi();
            dGNhanVien.DataSource = LayDSNhanVien();
        }

        private void btThem_Click(object sender, EventArgs e)
        {
            try
            {
                string query = string.Format("set dateformat dmy; insert into Employees(LastName, FirstName , BirthDate, Address, HomePhone) values (N'{0}',N'{1}', '{2}', '{3}','{4}')", txtHoten.Text, txtHoten.Text, dtpNgaySinh.Value.ToShortDateString(), txtDiaChi.Text, txtDienThoai.Text);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();

                dGNhanVien.Columns.Clear();
                dGNhanVien.DataSource = LayDSNhanVien();
            }
            catch (Exception exx)
            {

                MessageBox.Show(exx.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }

        private void dGNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex <= dGNhanVien.Rows.Count - 1)
            {
                txtHoten.Text = dGNhanVien.Rows[e.RowIndex].Cells["LastName"].Value.ToString() + " " + dGNhanVien.Rows[e.RowIndex].Cells["FirstName"].Value.ToString();

                dtpNgaySinh.Text = dGNhanVien.Rows[e.RowIndex].Cells["BirthDate"].Value.ToString();

            }
        }

        private void btXoa_Click(object sender, EventArgs e)
        {
            string giaTri = "";
            giaTri = dGNhanVien.Rows[dGNhanVien.CurrentRow.Index].Cells[0].Value.ToString();
            try
            {
                string query = string.Format("delete Employees where EmployeeID={0}", giaTri);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();

                dGNhanVien.Columns.Clear();
                dGNhanVien.DataSource = LayDSNhanVien();
            }
            catch (Exception exx)
            {

                MessageBox.Show(exx.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
