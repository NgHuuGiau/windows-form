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
using QuanLyThuVien.Models;
using QuanLyThuVien.Helpers;
using System.Data.SqlClient;

namespace QuanLyThuVien
{
    public partial class FrmBookEntry : Form
    {
        public event EventHandler BookSaved;
        private string _editingId = null;
        public FrmBookEntry()
        {
            InitializeComponent();
            Load += FrmBookEntry_Load;
            btnLuu.Click += btnLuu_Click;
            btnDong.Click += btnDong_Click;
        }

        private void FrmBookEntry_Load(object sender, EventArgs e)
        {
            EnsureDatabase();
            EnsureDauSachData();
            LoadCategories();
            SetupGridColumns();
            LoadData();
        }

        private void EnsureDatabase()
        {
            try
            {
                // Thêm cột GiaThue nếu chưa có
                DatabaseHelper.ExecuteNonQuery(@"
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('ThongTinSach') AND name = 'GiaThue')
                BEGIN
                    ALTER TABLE ThongTinSach ADD GiaThue money NOT NULL DEFAULT ((0))
                END", null);

                // Xóa constraint năm xuất bản sai
                DatabaseHelper.ExecuteNonQuery(@"
                IF EXISTS (SELECT * FROM sys.check_constraints WHERE name = 'CK_Sach_NamXuatBan')
                BEGIN
                    ALTER TABLE ThongTinSach DROP CONSTRAINT CK_Sach_NamXuatBan
                END", null);

                // Sửa IDTheLoai cho phép NULL
                DatabaseHelper.ExecuteNonQuery(@"
                IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('DauSach') AND name = 'IDTheLoai' AND is_nullable = 0)
                BEGIN
                    ALTER TABLE DauSach ALTER COLUMN IDTheLoai nchar(10) NULL
                END", null);

                // Bỏ FK DauSach -> TheLoai nếu gây lỗi
                DatabaseHelper.ExecuteNonQuery(@"
                IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_DauSach_TheLoai')
                BEGIN
                    ALTER TABLE DauSach DROP CONSTRAINT FK_DauSach_TheLoai
                END", null);

                // Thêm cột NgayNhap nếu chưa có
                DatabaseHelper.ExecuteNonQuery(@"
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('ThongTinSach') AND name = 'NgayNhap')
                BEGIN
                    ALTER TABLE ThongTinSach ADD NgayNhap datetime NULL
                END", null);

                // Thêm cột TinhTrang nếu chưa có
                DatabaseHelper.ExecuteNonQuery(@"
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('ThongTinSach') AND name = 'TinhTrang')
                BEGIN
                    ALTER TABLE ThongTinSach ADD TinhTrang nvarchar(50) NULL
                END", null);
            }
            catch { }
        }

        private void EnsureDauSachData()
        {
            try
            {
                string[] names = { "Mathematics", "Physics", "Chemistry", "Biology", "History", "Geography", "English", "Literature" };
                for (int i = 0; i < names.Length; i++)
                {
                    string name = names[i];
                    string newId = "DS" + (i + 1).ToString("D2");
                    
                    // Kiểm tra xem môn này đã tồn tại chưa (theo tên hoặc ID mới)
                    var dtCheck = DatabaseHelper.ExecuteQuery("SELECT IDDauSach FROM DauSach WHERE TenDauSach = @name OR IDDauSach = @id", 
                        new[] { new SqlParameter("@name", name), new SqlParameter("@id", newId) });
                    
                    if (dtCheck.Rows.Count > 0)
                    {
                        string oldId = dtCheck.Rows[0]["IDDauSach"].ToString().Trim();
                        if (oldId != newId)
                        {
                            // Cập nhật ID cũ thành ID mới (cần tắt constraint nếu có hoặc cập nhật cả bảng liên quan)
                            // Ở đây ta đơn giản là cập nhật IDDauSach trong cả 2 bảng
                            DatabaseHelper.ExecuteNonQuery("UPDATE ThongTinSach SET IDDauSach = @newId WHERE IDDauSach = @oldId",
                                new[] { new SqlParameter("@newId", newId), new SqlParameter("@oldId", oldId) });
                            DatabaseHelper.ExecuteNonQuery("UPDATE DauSach SET IDDauSach = @newId WHERE IDDauSach = @oldId",
                                new[] { new SqlParameter("@newId", newId), new SqlParameter("@oldId", oldId) });
                        }
                    }
                    else
                    {
                        // Thêm mới nếu chưa có
                        DatabaseHelper.ExecuteNonQuery(
                            "INSERT INTO DauSach (IDDauSach, IDTheLoai, TenDauSach) VALUES (@id, NULL, @name)",
                            new[] { new SqlParameter("@id", newId), new SqlParameter("@name", name) });
                    }
                }
            }
            catch { }
        }

        private void LoadCategories()
        {
            try
            {
                cboTheLoai.Items.Clear();
                var dt = DatabaseHelper.ExecuteQuery("SELECT IDDauSach, TenDauSach FROM DauSach", null);

                foreach (DataRow r in dt.Rows)
                {
                    cboTheLoai.Items.Add(new CategoryItem(r["IDDauSach"]?.ToString().Trim(), r["TenDauSach"]?.ToString().Trim()));
                }

                if (cboTheLoai.Items.Count > 0) 
                    cboTheLoai.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load danh mục: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private class CategoryItem
        {
            public string Id { get; }
            public string Name { get; }
            public CategoryItem(string id, string name) { Id = id; Name = name; }
            public override string ToString() => Name ?? "";
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            txtTenSach.Text = string.Empty;
            txtTacGia.Text = string.Empty;
            txtNhaXuatBan.Text = string.Empty;
            txtNamXuatBan.Text = string.Empty;
            txtGiaBan.Text = string.Empty;
            txtGiaThue.Text = string.Empty;
            dtpNgayNhap.Value = DateTime.Now;
            if (cboTheLoai.Items.Count > 0) cboTheLoai.SelectedIndex = 0;
            _editingId = null;
            btnLuu.Text = "Lưu";
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenSach.Text))
            {
                MessageBox.Show("Vui lòng nhập tên sách.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenSach.Focus();
                return;
            }

            if (!int.TryParse(txtNamXuatBan.Text.Trim(), out int namXB) || namXB < 1000 || namXB > DateTime.Now.Year)
            {
                MessageBox.Show("Năm xuất bản không hợp lệ.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNamXuatBan.Focus();
                return;
            }

            // QĐ3: Chỉ nhận các sách xuất bản trong vòng 8 năm
            if (DateTime.Now.Year - namXB > 8)
            {
                MessageBox.Show("Chỉ nhận các sách xuất bản trong vòng 8 năm gần đây!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNamXuatBan.Focus();
                return;
            }

            if (!decimal.TryParse(txtGiaBan.Text.Trim(), out decimal giaBan) || giaBan < 0)
            {
                MessageBox.Show("Giá bán (trị giá) không hợp lệ.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtGiaBan.Focus();
                return;
            }

            decimal giaThue = 0;
            if (!string.IsNullOrWhiteSpace(txtGiaThue.Text.Trim()))
            {
                if (!decimal.TryParse(txtGiaThue.Text.Trim(), out giaThue) || giaThue < 0)
                {
                    MessageBox.Show("Giá thuê không hợp lệ.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtGiaThue.Focus();
                    return;
                }
            }

            try
            {
                var book = new Book
                {
                    TenSach = txtTenSach.Text.Trim(),
                    TacGia = txtTacGia.Text.Trim(),
                    NhaXuatBan = txtNhaXuatBan.Text.Trim(),
                    NgayNhap = dtpNgayNhap.Value,
                    NamXuatBan = namXB,
                    GiaBan = giaBan,
                    IDDauSach = (cboTheLoai.SelectedItem is CategoryItem cat) ? cat.Id : null
                };

                if (!string.IsNullOrEmpty(_editingId))
                {
                    string updateSql = "UPDATE ThongTinSach SET TenSach=@TenSach, TacGia=@TacGia, NhaXuatBan=@NhaXuatBan, NamXuatBan=@NamXuatBan, NgayNhap=@NgayNhap, GiaBan=@GiaBan, GiaThue=@GiaThue, IDDauSach=@IDDauSach WHERE IDSach=@id";
                    var updateParams = new SqlParameter[]
                    {
                        new SqlParameter("@TenSach", book.TenSach),
                        new SqlParameter("@TacGia", book.TacGia),
                        new SqlParameter("@NhaXuatBan", book.NhaXuatBan),
                        new SqlParameter("@NamXuatBan", book.NamXuatBan),
                        new SqlParameter("@NgayNhap", book.NgayNhap.Date),
                        new SqlParameter("@GiaBan", book.GiaBan),
                        new SqlParameter("@GiaThue", giaThue),
                        new SqlParameter("@IDDauSach", (object)book.IDDauSach ?? DBNull.Value),
                        new SqlParameter("@id", _editingId)
                    };

                    DatabaseHelper.ExecuteNonQuery(updateSql, updateParams);
                    MessageBox.Show("Cập nhật sách thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RefreshAllForms();
                    BookSaved?.Invoke(this, EventArgs.Empty);
                    this.Close();
                }
                else
                {
                    var newId = GenerateNewBookId(book.IDDauSach);
                    string sql = "INSERT INTO ThongTinSach (IDSach, TenSach, TacGia, NhaXuatBan, NamXuatBan, NgayNhap, GiaBan, GiaThue, IDDauSach, TinhTrang, SoLuong) " +
                                 "VALUES (@IDSach, @TenSach, @TacGia, @NhaXuatBan, @NamXuatBan, @NgayNhap, @GiaBan, @GiaThue, @IDDauSach, N'Sẵn sàng', 1);";

                    var parameters = new SqlParameter[]
                    {
                        new SqlParameter("@IDSach", newId),
                        new SqlParameter("@TenSach", book.TenSach),
                        new SqlParameter("@TacGia", book.TacGia),
                        new SqlParameter("@NhaXuatBan", book.NhaXuatBan),
                        new SqlParameter("@NamXuatBan", (object)book.NamXuatBan),
                        new SqlParameter("@NgayNhap", (object)book.NgayNhap.Date),
                        new SqlParameter("@GiaBan", (object)book.GiaBan),
                        new SqlParameter("@GiaThue", giaThue),
                        new SqlParameter("@IDDauSach", (object)book.IDDauSach ?? DBNull.Value)
                    };

                    DatabaseHelper.ExecuteNonQuery(sql, parameters);
                    MessageBox.Show($"Thêm sách thành công. Mã sách: {newId}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RefreshAllForms();
                    BookSaved?.Invoke(this, EventArgs.Empty);

                    var result = MessageBox.Show("Bạn có muốn tiếp tục nhập thêm sách mới?", "Tiếp tục?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        btnDong_Click(null, null);
                    }
                    else
                    {
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu dữ liệu: " + ex.Message, "Lỗi DB", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupGridColumns()
        {
            dgvBooks.Columns.Clear();
            dgvBooks.AutoGenerateColumns = false;
            dgvBooks.Columns.Add("IDSach", "Mã sách");
            dgvBooks.Columns.Add("TenSach", "Tên sách");
            dgvBooks.Columns.Add("TacGia", "Tác giả");
            dgvBooks.Columns.Add("NhaXuatBan", "Nhà xuất bản");
            dgvBooks.Columns.Add("NamXuatBan", "Năm XB");
            dgvBooks.Columns.Add("GiaBan", "Trị giá");
            dgvBooks.Columns.Add("GiaThue", "Giá thuê");
            dgvBooks.Columns.Add("TheLoai", "Thể loại");
            dgvBooks.Columns.Add("TinhTrang", "Tình trạng");
            
            dgvBooks.Columns["IDSach"].DataPropertyName = "IDSach";
            dgvBooks.Columns["TenSach"].DataPropertyName = "TenSach";
            dgvBooks.Columns["TacGia"].DataPropertyName = "TacGia";
            dgvBooks.Columns["NhaXuatBan"].DataPropertyName = "NhaXuatBan";
            dgvBooks.Columns["NamXuatBan"].DataPropertyName = "NamXuatBan";
            dgvBooks.Columns["GiaBan"].DataPropertyName = "GiaBan";
            dgvBooks.Columns["GiaThue"].DataPropertyName = "GiaThue";
            dgvBooks.Columns["TheLoai"].DataPropertyName = "TheLoai";
            dgvBooks.Columns["TinhTrang"].DataPropertyName = "TinhTrang";

            dgvBooks.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvBooks.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBooks.MultiSelect = false;
            dgvBooks.ReadOnly = true;
            dgvBooks.DoubleClick += (s, ev) => {
                if (dgvBooks.SelectedRows.Count > 0) {
                    var id = dgvBooks.SelectedRows[0].Cells[0].Value?.ToString();
                    OpenForEdit(id);
                }
            };
        }

        private void LoadData()
        {
            try
            {
                string sql = @"SELECT TOP 20 s.IDSach, s.TenSach, s.TacGia, s.NhaXuatBan, s.NamXuatBan, s.GiaBan, s.GiaThue, 
                             d.TenDauSach AS TheLoai, s.NgayNhap, s.TinhTrang 
                             FROM ThongTinSach s 
                             LEFT JOIN DauSach d ON s.IDDauSach = d.IDDauSach 
                             ORDER BY s.NgayNhap DESC, s.IDSach DESC";
                var dt = DatabaseHelper.ExecuteQuery(sql);
                
                // Cập nhật tổng số lượng sách
                var dtCount = DatabaseHelper.ExecuteQuery("SELECT COUNT(*) FROM ThongTinSach", null);
                if (dtCount.Rows.Count > 0)
                {
                    lblTotalBooksValue.Text = dtCount.Rows[0][0].ToString();
                }

                dgvBooks.Rows.Clear();
                foreach (DataRow r in dt.Rows)
                {
                    string status = GetStatusText(r["TinhTrang"]?.ToString());
                    dgvBooks.Rows.Add(
                        r["IDSach"]?.ToString()?.Trim(),
                        r["TenSach"]?.ToString()?.Trim(),
                        r["TacGia"]?.ToString()?.Trim(),
                        r["NhaXuatBan"]?.ToString()?.Trim(),
                        r["NamXuatBan"],
                        r["GiaBan"],
                        r["GiaThue"],
                        r["TheLoai"]?.ToString()?.Trim(),
                        status);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi load grid: " + ex.Message);
            }
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
                case "san sang":
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
                case "lost by user":
                case "lostbyuser":
                    return "Mất";
                case "người dùng làm mất":
                    return "Mất";
                default:
                    return status;
            }
        }

        private void RefreshAllForms()
        {
            LoadData(); // Refresh current grid
            foreach (Form frm in Application.OpenForms)
            {
                if (frm is FrmSearchBook searchForm)
                {
                    searchForm.RefreshData();
                }
                if (frm is FrmLiquidation liqForm)
                {
                    liqForm.RefreshData();
                }
            }
        }

        private string GenerateNewBookId(string dauSachId)
        {
            if (string.IsNullOrWhiteSpace(dauSachId)) return "S" + DateTime.Now.Ticks.ToString().Substring(10);

            try
            {
                // Format: DS01_01, DS01_02...
                string prefix = dauSachId + "_";
                var dt = DatabaseHelper.ExecuteQuery("SELECT IDSach FROM ThongTinSach WHERE IDSach LIKE @prefix", 
                    new[] { new SqlParameter("@prefix", prefix + "%") });
                
                int maxSeq = 0;
                foreach (DataRow row in dt.Rows)
                {
                    string idStr = row["IDSach"]?.ToString().Trim();
                    if (!string.IsNullOrWhiteSpace(idStr) && idStr.StartsWith(prefix))
                    {
                        string seqPart = idStr.Substring(prefix.Length);
                        if (int.TryParse(seqPart, out int seq))
                        {
                            if (seq > maxSeq) maxSeq = seq;
                        }
                    }
                }

                return prefix + (maxSeq + 1).ToString("D2");
            }
            catch { }
            return dauSachId + "_01";
        }

        public void OpenForEdit(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return;
            try
            {
                var p = new SqlParameter[] { new SqlParameter("@id", id) };
                var dt = DatabaseHelper.ExecuteQuery("SELECT TOP 1 * FROM ThongTinSach WHERE IDSach = @id", p);
                if (dt.Rows.Count == 0) return;
                var r = dt.Rows[0];
                _editingId = r["IDSach"]?.ToString().Trim();
                txtTenSach.Text = r["TenSach"]?.ToString().Trim();
                txtTacGia.Text = r["TacGia"]?.ToString().Trim();
                txtNhaXuatBan.Text = r["NhaXuatBan"]?.ToString().Trim();
                txtNamXuatBan.Text = r["NamXuatBan"]?.ToString().Trim();
                if (DateTime.TryParse(r["NgayNhap"]?.ToString(), out var dn)) dtpNgayNhap.Value = dn;
                txtGiaBan.Text = r["GiaBan"]?.ToString().Trim();
                txtGiaThue.Text = r.Table.Columns.Contains("GiaThue") ? r["GiaThue"]?.ToString().Trim() : "0";
                var cat = r.Table.Columns.Contains("IDDauSach") ? r["IDDauSach"]?.ToString().Trim() : null;
                if (!string.IsNullOrWhiteSpace(cat))
                {
                    for (int i = 0; i < cboTheLoai.Items.Count; i++)
                    {
                        if (cboTheLoai.Items[i] is CategoryItem item && item.Id == cat)
                        {
                            cboTheLoai.SelectedIndex = i;
                            break;
                        }
                    }
                }
                btnLuu.Text = "Cập nhật";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể nạp thông tin sách: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
