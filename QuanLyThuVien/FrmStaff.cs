using QuanLyThuVien.Helpers;
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

namespace QuanLyThuVien
{
    public partial class FrmStaff : Form
    {
        public FrmStaff()
        {
            InitializeComponent();
        }

        private void FrmStaff_Load(object sender, EventArgs e)
        {
            cboBangCap.SelectedIndex = 0;
            cboChucVu.SelectedIndex = 0;
            cboBoPhan.SelectedIndex = 0;
            LayHS_NhanVien();
        }

        private void LayHS_NhanVien()
        {
            try
            {
                string sql = "SELECT * FROM HoSoNhanVien";
                dataGridView1.DataSource = DatabaseHelper.ExecuteQuery(sql);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách nhân viên: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Thembtn_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra dữ liệu trống
                if (string.IsNullOrWhiteSpace(cboidnhanvien.Text) ||
                    string.IsNullOrWhiteSpace(cboHoTen.Text) ||
                    string.IsNullOrWhiteSpace(cboDiaChi.Text) ||
                    string.IsNullOrWhiteSpace(cboSDT.Text) ||
                    string.IsNullOrWhiteSpace(cboMatKhau.Text))
                {
                    MessageBox.Show("Vui lòng điền đầy đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Kiểm tra định dạng ngày tháng
                if (!DateTime.TryParse(date_NS.Text, out DateTime ngaySinh))
                {
                    MessageBox.Show("Ngày sinh không hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string sql = "INSERT INTO HoSoNhanVien (IDNhanVien, HoTen, NgaySinh, DiaChi, DienThoai, BangCap, BoPhan, ChucVu, MatKhau) " +
                             "VALUES (@ID, @HoTen, @NgaySinh, @DiaChi, @DienThoai, @BangCap, @BoPhan, @ChucVu, @MatKhau)";

                SqlParameter[] parameters = {
                    new SqlParameter("@ID", cboidnhanvien.Text.Trim()),
                    new SqlParameter("@HoTen", cboHoTen.Text.Trim()),
                    new SqlParameter("@NgaySinh", ngaySinh),
                    new SqlParameter("@DiaChi", cboDiaChi.Text.Trim()),
                    new SqlParameter("@DienThoai", cboSDT.Text.Trim()),
                    new SqlParameter("@BangCap", cboBangCap.SelectedItem?.ToString() ?? ""),
                    new SqlParameter("@BoPhan", cboBoPhan.SelectedItem?.ToString() ?? ""),
                    new SqlParameter("@ChucVu", cboChucVu.SelectedItem?.ToString() ?? ""),
                    new SqlParameter("@MatKhau", cboMatKhau.Text.Trim())
                };

                DatabaseHelper.ExecuteNonQuery(sql, parameters);
                MessageBox.Show("Thêm nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LayHS_NhanVien();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm nhân viên: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnxoa_Click_1(object sender, EventArgs e)
        {
            string idNhanVien = cboidnhanvien.Text.Trim();
            if (string.IsNullOrEmpty(idNhanVien))
            {
                MessageBox.Show("Vui lòng chọn nhân viên cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show($"Bạn có chắc chắn muốn xóa nhân viên {idNhanVien} không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                try
                {
                    string sql = "DELETE FROM HoSoNhanVien WHERE IDNhanVien = @ID";
                    DatabaseHelper.ExecuteNonQuery(sql, new SqlParameter[] { new SqlParameter("@ID", idNhanVien) });
                    MessageBox.Show("Xóa nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LayHS_NhanVien();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa nhân viên: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
