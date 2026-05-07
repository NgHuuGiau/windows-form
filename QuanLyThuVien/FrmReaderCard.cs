using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLyThuVien.Models;
using QuanLyThuVien.Helpers;
using System.Data.SqlClient;

namespace QuanLyThuVien
{
    public partial class FrmReaderCard : Form
    {
        public FrmReaderCard()
        {
            InitializeComponent();
            LoadReaderTypes();
            
            this.btnAdd.Click += BtnAdd_Click;
            this.btnCancel.Click += BtnCancel_Click;

            SetupGridColumnsTheDocGia();
            LoadDataTheDocGia();
        }

        private void SetupGridColumnsTheDocGia()
        {
            dgvReader.Columns.Clear();
            dgvReader.AutoGenerateColumns = false;
            dgvReader.Columns.Add("IDDocGia", "Mã độc giả");
            dgvReader.Columns.Add("HoTen", "Họ tên");
            dgvReader.Columns.Add("NgaySinh", "Ngày sinh");
            dgvReader.Columns.Add("DiaChi", "Địa chỉ");
            dgvReader.Columns.Add("Email", "Email");
            dgvReader.Columns.Add("NgayLap", "Ngày lập");
            dgvReader.Columns.Add("LoaiDocGia", "Loại độc giả");
            dgvReader.Columns.Add("TienNo", "Tiền nợ");

            dgvReader.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvReader.MultiSelect = false;
        }

        private void LoadDataTheDocGia()
        {
            try
            {
                string query = "SELECT IDDocGia, HoTen, NgaySinh, DiaChi, Email, NgayLap, LoaiDocGia, TienNo FROM TheDocGia";
                var dt = DatabaseHelper.ExecuteQuery(query);
                dgvReader.Rows.Clear();
                foreach (DataRow r in dt.Rows)
                {
                    dgvReader.Rows.Add(r["IDDocGia"].ToString().Trim(),
                        r["HoTen"].ToString().Trim(),
                        Convert.ToDateTime(r["NgaySinh"]).ToString("dd/MM/yyyy"),
                        r["DiaChi"].ToString().Trim(), 
                        r["Email"].ToString().Trim(),
                        Convert.ToDateTime(r["NgayLap"]).ToString("dd/MM/yyyy"),
                        r["LoaiDocGia"].ToString().Trim(),
                        r["TienNo"].ToString().Trim()
                        );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadReaderTypes()
        {
            var allowedTypes = new[] { "Whitelist", "Graylist" };
            cboReaderType.DataSource = allowedTypes;
            cboReaderType.SelectedIndex = 0;
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show("Vui lòng nhập họ tên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra tuổi từ 18 đến 55
            int age = DateTime.Now.Year - dtpBirthDate.Value.Year;
            if (dtpBirthDate.Value > DateTime.Now.AddYears(-age)) age--;
            if (age < 18 || age > 55)
            {
                MessageBox.Show("Tuổi độc giả phải từ 18 đến 55!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string readerID = "DG" + DateTime.Now.ToString("yyyyMMddHHmmss");
            
            string query = "INSERT INTO TheDocGia (IDDocGia, HoTen, NgaySinh, DiaChi, Email, NgayLap, LoaiDocGia, TienNo) " +
                           "VALUES (@ID, @Name, @DOB, @Address, @Email, @RegDate, @Type, @Debt)";
            
            SqlParameter[] parameters = {
                new SqlParameter("@ID", readerID),
                new SqlParameter("@Name", txtFullName.Text),
                new SqlParameter("@DOB", dtpBirthDate.Value),
                new SqlParameter("@Address", txtAddress.Text),
                new SqlParameter("@Email", txtEmail.Text),
                new SqlParameter("@RegDate", DateTime.Now),
                new SqlParameter("@Type", cboReaderType.SelectedItem.ToString()),
                new SqlParameter("@Debt", SqlDbType.Decimal) { Value = 0m }
            };

            try
            {
                DatabaseHelper.ExecuteNonQuery(query, parameters);
                MessageBox.Show($"Lập thẻ độc giả thành công! Mã độc giả: {readerID}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            SetupGridColumnsTheDocGia();
            LoadDataTheDocGia();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
