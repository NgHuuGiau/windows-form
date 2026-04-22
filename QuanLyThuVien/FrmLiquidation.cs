using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLyThuVien.Enums;
using QuanLyThuVien.Helpers;
using System.Data.SqlClient;

namespace QuanLyThuVien
{
    public partial class FrmLiquidation : Form
    {
        public FrmLiquidation()
        {
            InitializeComponent();
            Load += FrmLiquidation_Load;
            btnTimKiem.Click += Timkiem_Click;
            btnXacNhan.Click += XacNhan_Click;
            btnDong.Click += Dong_Click;
        }

        private void FrmLiquidation_Load(object sender, EventArgs e)
        {
            cboLyDo.Items.Clear();
            foreach (var c in Enum.GetValues(typeof(LiquidationReason))) cboLyDo.Items.Add(((LiquidationReason)c).GetDisplayName());
            if (cboLyDo.Items.Count > 0) cboLyDo.SelectedIndex = 0;

            dgvBooks.Columns.Clear();
            dgvBooks.AutoGenerateColumns = false;
            dgvBooks.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBooks.MultiSelect = false;
            dgvBooks.Columns.Add("IDSach", "Mã sách");
            dgvBooks.Columns.Add("TenSach", "Tên sách");
            dgvBooks.Columns.Add("TacGia", "Tác giả");
            dgvBooks.Columns.Add("TinhTrang", "Tình trạng");
            dgvBooks.DoubleClick += DataGridView1_DoubleClick;

            LoadData();
        }

        public void LoadData(string whereClause = null, SqlParameter[] parameters = null)
        {
            try
            {
                string sql = "SELECT IDSach, TenSach, TacGia, TinhTrang FROM ThongTinSach";

                if (!string.IsNullOrWhiteSpace(whereClause))
                {
                    sql += " WHERE " + whereClause;
                }

                var dt = DatabaseHelper.ExecuteQuery(sql, parameters);
                dgvBooks.Rows.Clear();
                foreach (DataRow r in dt.Rows)
                {
                    var statusObj = r["TinhTrang"];
                    string statusText = statusObj?.ToString()?.Trim() ?? "";
                    if (!string.IsNullOrWhiteSpace(statusText))
                    {
                        statusText = GetStatusText(statusText);
                    }
                    dgvBooks.Rows.Add(
                        r["IDSach"]?.ToString()?.Trim(),
                        r["TenSach"]?.ToString()?.Trim(),
                        r["TacGia"]?.ToString()?.Trim(),
                        statusText);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể truy vấn dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void RefreshData()
        {
            LoadData();
        }

        private string GetStatusText(string status)
        {
            if (string.IsNullOrWhiteSpace(status)) return status;
            var normalized = status.Trim().ToLower();
            switch (normalized)
            {
                case "ok":
                case "sẵn sàng":
                case "ready":
                    return "Sẵn sàng";
                case "đang mượn":
                case "muon":
                case "borrowed":
                case "dang muon":
                    return "Đang mượn";
                case "hỏng":
                case "hong":
                case "damaged":
                    return "Hỏng";
                case "mất":
                case "mat":
                case "lost":
                    return "Mất";
                default:
                    return status;
            }
        }

        public void SelectAndHighlight(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return;
            txtMaSach.Text = id;
            var p = new SqlParameter[] { new SqlParameter("@id", id) };
            LoadData("IDSach = @id", p);
            foreach (DataGridViewRow row in dgvBooks.Rows)
            {
                if (row.Cells.Count > 0 && row.Cells[0].Value != null && row.Cells[0].Value.ToString().Trim() == id)
                {
                    row.Selected = true;
                    dgvBooks.CurrentCell = row.Cells[0];
                    break;
                }
            }
        }

        private void DataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (dgvBooks.SelectedRows.Count > 0)
            {
                var id = dgvBooks.SelectedRows[0].Cells[0].Value?.ToString()?.Trim();
                if (!string.IsNullOrWhiteSpace(id)) txtMaSach.Text = id;
            }
        }

        private void Timkiem_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtMaSach.Text))
            {
                var p = new SqlParameter[] { new SqlParameter("@id", txtMaSach.Text.Trim()) };
                LoadData("IDSach = @id", p);
            }
            else
            {
                LoadData();
            }
        }

        private void XacNhan_Click(object sender, EventArgs e)
        {
            if (dgvBooks.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn sách từ danh sách để thanh lý.", "Chú ý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var id = dgvBooks.SelectedRows[0].Cells[0].Value?.ToString()?.Trim();
            if (string.IsNullOrWhiteSpace(id)) return;

            var result = MessageBox.Show($"Bạn có chắc chắn muốn thanh lý sách mã {id}?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes) return;

            try
            {
                var lydo = cboLyDo.SelectedItem?.ToString();
                var tien = (int)nudTienPhat.Value;

                try
                {
                    string insertSql = "INSERT INTO ThanhLySach (IDSach, NgayThanhLy, LyDoThanhLy) VALUES (@id, @ngay, @lydo)";
                    var insParams = new SqlParameter[] {
                        new SqlParameter("@id", id),
                        new SqlParameter("@ngay", DateTime.Now.Date),
                        new SqlParameter("@lydo", lydo ?? (object)DBNull.Value)
                    };
                    DatabaseHelper.ExecuteNonQuery(insertSql, insParams);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi lưu phiếu thanh lý: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string newStatus = "Hỏng";
                if (lydo == "Mất" || lydo == "Người mất") newStatus = "Mất";

                string updateSql = "UPDATE ThongTinSach SET TinhTrang = @t WHERE IDSach = @id";
                var p = new SqlParameter[] {
                    new SqlParameter("@t", newStatus),
                    new SqlParameter("@id", id)
                };
                DatabaseHelper.ExecuteNonQuery(updateSql, p);

                MessageBox.Show("Đã thanh lý sách thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Refresh lại danh sách sau khi thanh lý
                LoadData();

                // Refresh form tra cứu sách nếu đang mở
                foreach (Form frm in Application.OpenForms)
                {
                    if (frm is FrmSearchBook searchForm)
                    {
                        searchForm.RefreshData();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thanh lý sách: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Dong_Click(object sender, EventArgs e)
        {
            txtMaSach.Text = string.Empty;
            nudTienPhat.Value = nudTienPhat.Minimum;
            if (cboLyDo.Items.Count > 0) cboLyDo.SelectedIndex = 0;
            LoadData();
        }
    }
}
