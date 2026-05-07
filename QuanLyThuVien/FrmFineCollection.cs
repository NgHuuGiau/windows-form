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
    public partial class FrmFineCollection : Form
    {
        public FrmFineCollection()
        {
            InitializeComponent();
            
            this.btnSearchReader.Click += BtnSearchReader_Click;
            this.btnConfirm.Click += BtnConfirm_Click;
            this.btnCancel.Click += BtnCancel_Click;
            this.txtAmountCollected.TextChanged += TxtAmountCollected_TextChanged;
            
            txtCurrentFine.ReadOnly = true;
            txtRemainingFine.ReadOnly = true;
        }

        private void BtnSearchReader_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtReaderID.Text))
            {
                MessageBox.Show("Vui lòng nhập mã độc giả!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = "SELECT HoTen, Email, TienNo FROM TheDocGia WHERE IDDocGia = @ID";
            SqlParameter[] parameters = { new SqlParameter("@ID", txtReaderID.Text) };
            
            try
            {
                DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);
                if (dt.Rows.Count > 0)
                {
                    tenDocGia.Text = dt.Rows[0]["HoTen"].ToString();
                    email.Text = dt.Rows[0]["Email"].ToString();
                    decimal currentFine = Convert.ToDecimal(dt.Rows[0]["TienNo"]);
                    txtCurrentFine.Text = currentFine.ToString();
                    txtAmountCollected.Text = "0";
                    txtRemainingFine.Text = currentFine.ToString();
                    
                    LoadBorrowingHistory(txtReaderID.Text);
                }
                else
                {
                    MessageBox.Show("Không tìm thấy độc giả!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    tenDocGia.Text = "";
                    email.Text = "";
                    txtCurrentFine.Text = "0";
                    txtRemainingFine.Text = "0";
                    dataGridView1.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm độc giả: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void LoadBorrowingHistory(string readerID)
        {
            string query = @"SELECT CT.IDChiTietMuon, S.TenSach, CT.NgayMuon, CT.HanTra, CT.NgayTra, CT.TienPhat, CT.TinhTrangTra
                           FROM ChiTietMuon CT
                           JOIN PhieuMuon PM ON CT.IDPhieuMuon = PM.IDPhieuMuon
                           JOIN CaTheSach CTS ON CT.IDCaTheSach = CTS.IDCaTheSach
                           JOIN ThongTinSach S ON CTS.IDSach = S.IDSach
                           WHERE PM.IDNguoiMuon = @ReaderID
                           ORDER BY CT.NgayMuon DESC";
            
            SqlParameter[] parameters = { new SqlParameter("@ReaderID", readerID) };
            
            try
            {
                DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải lịch sử mượn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TxtAmountCollected_TextChanged(object sender, EventArgs e)
        {
            if (decimal.TryParse(txtCurrentFine.Text, out decimal current) && 
                decimal.TryParse(txtAmountCollected.Text, out decimal collected))
            {
                txtRemainingFine.Text = (current - collected).ToString();
            }
        }

        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtReaderID.Text))
            {
                MessageBox.Show("Vui lòng chọn độc giả!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtAmountCollected.Text, out decimal collected) || collected <= 0)
            {
                MessageBox.Show("Số tiền thu không hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            decimal currentDebt = decimal.Parse(txtCurrentFine.Text);
            if (collected > currentDebt)
            {
                MessageBox.Show("Số tiền thu không được vượt quá số nợ hiện tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (!decimal.TryParse(txtRemainingFine.Text, out decimal remaining))
                {
                    MessageBox.Show("Số tiền không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                string query = "UPDATE TheDocGia SET TienNo = @Remaining WHERE IDDocGia = @ID";
                SqlParameter[] parameters = {
                    new SqlParameter("@Remaining", remaining),
                    new SqlParameter("@ID", txtReaderID.Text)
                };
                
                DatabaseHelper.ExecuteNonQuery(query, parameters);
                
                MessageBox.Show($"Đã thu {collected:N0} VNĐ từ độc giả {txtReaderID.Text}. Số nợ còn lại: {remaining:N0} VNĐ", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật tiền nợ: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
