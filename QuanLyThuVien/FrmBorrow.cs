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
using DocumentFormat.OpenXml.Drawing.Diagrams;

namespace QuanLyThuVien
{
    public partial class FrmBorrow : Form
    {
        double tienNo;
        private string _currentMasterId = null; // Tracks the current master ID for detail view

        public FrmBorrow()
        {
            InitializeComponent();
            
            // Attach DoubleClick event handler
            dgvBooks.DoubleClick += DataGridView1_DoubleClick;
            
            this.btnSearchReader.Click += BtnSearchReader_Click;
            this.btnBorrow.Click += BtnBorrow_Click;
            this.btnCancel.Click += BtnCancel_Click;
            this.btnBack.Click += BtnBack_Click;
            
            SetupGridColumnsThongTinSach();
            LoadThongTinSach();
            btnBack.Visible = false;
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            _currentMasterId = null;
            SetupGridColumnsThongTinSach();
            LoadThongTinSach();
            btnBack.Visible = false;
        }

        private void DataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (dgvBooks.SelectedRows.Count == 0 || _currentMasterId != null) return;
            
            var selectedMasterId = dgvBooks.SelectedRows[0].Cells[0].Value?.ToString()?.Trim();
            if (string.IsNullOrWhiteSpace(selectedMasterId)) return;
            
            _currentMasterId = selectedMasterId;
            
            SetupGridColumnsCaTheSach();
            LoadThongTinCaTheSach(_currentMasterId);
            btnBack.Visible = true;
        }

        private void LoadBooksFromDB(string searchQuery = "")
        {
            string query = @"SELECT CT.IDCaTheSach, S.IDSach, S.TenSach, S.TacGia, CT.TinhTrang 
                           FROM CaTheSach CT 
                           JOIN ThongTinSach S ON CT.IDSach = S.IDSach 
                           WHERE CT.TinhTrang = N'Sẵn sàng'";

            if (!string.IsNullOrEmpty(searchQuery))
            {
                query += " AND S.TenSach LIKE @SearchQuery";
            }
            SqlParameter[] parameters = { new SqlParameter("@SearchQuery", "%" + searchQuery + "%") };

            try
            {
                dgvBooks.DataSource = DatabaseHelper.ExecuteQuery(query, parameters);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách sách: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupGridColumnsThongTinSach()
        {
            _currentMasterId = null;

            dgvBooks.Columns.Clear();
            dgvBooks.AutoGenerateColumns = false;
            dgvBooks.Columns.Add("IDSach", "Mã sách");
            dgvBooks.Columns.Add("TenSach", "Tên sách");
            dgvBooks.Columns.Add("TacGia", "Tác giả");
            dgvBooks.Columns.Add("NamXuatBan", "Năm xuất bản");
            dgvBooks.Columns.Add("NhaXuatBan", "Nhà xuất bản");
            dgvBooks.Columns.Add("GiaBan", "Trị giá");
            dgvBooks.Columns.Add("GiaThue", "Giá thuê");
            dgvBooks.Columns.Add("IDDauSach", "Đầu sách");
            dgvBooks.Columns.Add("SoLuong", "Số lượng");
            dgvBooks.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBooks.MultiSelect = false;
        }

        private void SetupGridColumnsCaTheSach()
        {
            dgvBooks.Columns.Clear();
            dgvBooks.AutoGenerateColumns = false;
            dgvBooks.Columns.Add("IDCaTheSach", "Mã cá thể sách");
            dgvBooks.Columns.Add("IDSach", "Mã sách");
            dgvBooks.Columns.Add("NgayNhap", "Ngày nhập");
            dgvBooks.Columns.Add("TinhTrang", "Tình trạng");
            dgvBooks.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBooks.MultiSelect = true;
        }

        private void LoadThongTinSach(string whereClause = null)
        {
            try
            {
                string sql = "SELECT s.IDSach, s.TenSach, s.TacGia, s.NhaXuatBan, s.NamXuatBan, s.GiaBan, s.GiaThue, s.SoLuong, ISNULL(d.TenDauSach, s.IDDauSach) AS DauSach " +
                             "FROM ThongTinSach s " +
                             "LEFT JOIN DauSach d ON s.IDDauSach = d.IDDauSach";
                if (!string.IsNullOrWhiteSpace(whereClause))
                {
                    sql += " WHERE s.TenSach LIKE @search";
                }
                var parameters = new SqlParameter[] { new SqlParameter("@search", "%" + whereClause + "%") };

                var dt = DatabaseHelper.ExecuteQuery(sql, parameters);
                dgvBooks.Rows.Clear();
                foreach (DataRow r in dt.Rows)
                {
                    dgvBooks.Rows.Add(
                        r["IDSach"]?.ToString()?.Trim(),
                        r["TenSach"]?.ToString()?.Trim(),
                        r["TacGia"]?.ToString()?.Trim(),
                        r["NamXuatBan"],
                        r["NhaXuatBan"]?.ToString()?.Trim(),
                        r["GiaBan"],
                        r["GiaThue"],
                        r["DauSach"]?.ToString()?.Trim(),
                        r["SoLuong"]
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể truy vấn dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LoadThongTinCaTheSach(string idSach)
        {
            try
            {
                string sql = "SELECT c.IDCaTheSach, c.IDSach, c.NgayNhap, c.TinhTrang " +
                             "FROM CaTheSach c " +
                             "INNER JOIN ThongTinSach s ON c.IDSach = s.IDSach " +
                             "WHERE c.IDSach = @idSach";
                
                var parameters = new SqlParameter[] { new SqlParameter("@idSach", idSach) };
                var dt = DatabaseHelper.ExecuteQuery(sql, parameters);
                dgvBooks.Rows.Clear();
                foreach (DataRow r in dt.Rows)
                {
                    dgvBooks.Rows.Add(
                        r["IDCaTheSach"]?.ToString()?.Trim(),
                        r["IDSach"]?.ToString()?.Trim(),
                        r["NgayNhap"] != DBNull.Value ? Convert.ToDateTime(r["NgayNhap"]).ToString("dd/MM/yyyy") : "",
                        r["TinhTrang"]?.ToString()?.Trim()
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể truy vấn dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
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
                    tienNo = dt.Rows[0]["TienNo"] != DBNull.Value ? Convert.ToDouble(dt.Rows[0]["TienNo"]) : 0;
                }
                else
                {
                    MessageBox.Show("Không tìm thấy độc giả!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    tenDocGia.Text = "";
                    email.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm độc giả: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnBorrow_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtReaderID.Text))
            {
                MessageBox.Show("Vui lòng chọn độc giả!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvBooks.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn ít nhất một quyển sách!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string readerID = txtReaderID.Text.Trim();

                // Kiểm tra loại độc giả (Chặn Blacklist và Graylist)
                string queryType = "SELECT LoaiDocGia FROM TheDocGia WHERE IDDocGia = @ID";
                object typeObj = DatabaseHelper.ExecuteScalar(queryType, new SqlParameter[] { new SqlParameter("@ID", readerID) });
                if (typeObj == null)
                {
                    MessageBox.Show("Không tìm thấy thông tin độc giả!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                string loaiDocGia = typeObj.ToString().Trim();
                if (loaiDocGia == "Blacklist")
                {
                    MessageBox.Show("Độc giả đang nằm trong danh sách đen (Blacklist) và không thể mượn sách!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                if (loaiDocGia == "Graylist" && tienNo > 0)
                {
                    MessageBox.Show("Độc giả đang trong danh sách cảnh báo (Graylist). Vui lòng thanh toán nợ trước khi mượn sách!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Kiểm tra nợ chưa trả
                string queryDebt = "SELECT TienNo FROM TheDocGia WHERE IDDocGia = @ID";
                object debtObj = DatabaseHelper.ExecuteScalar(queryDebt, new SqlParameter[] { new SqlParameter("@ID", readerID) });
                if (debtObj != null && Convert.ToDecimal(debtObj) > 0)
                {
                    MessageBox.Show($"Độc giả có nợ {Convert.ToDecimal(debtObj):N0} VNĐ. Vui lòng thanh toán trước khi mượn sách!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // QĐ2: Kiểm tra thời hạn thẻ (6 tháng)
                string queryCheckCard = "SELECT NgayLap FROM TheDocGia WHERE IDDocGia = @ID";
                object regDateObj = DatabaseHelper.ExecuteScalar(queryCheckCard, new SqlParameter[] { new SqlParameter("@ID", readerID) });
                if (regDateObj == null)
                {
                    MessageBox.Show("Không tìm thấy thông tin độc giả!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                DateTime regDate = Convert.ToDateTime(regDateObj);
                if (DateTime.Now > regDate.AddMonths(6))
                {
                    MessageBox.Show("Thẻ độc giả đã hết hạn (giá trị 6 tháng)!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // QĐ5: Kiểm tra số lượng sách đang mượn (Max 5)
                string queryCount = "SELECT COUNT(*) FROM ChiTietMuon CT JOIN PhieuMuon PM ON CT.IDPhieuMuon = PM.IDPhieuMuon " +
                                    "WHERE PM.IDNguoiMuon = @ReaderID AND CT.NgayTra IS NULL";
                int currentlyBorrowed = Convert.ToInt32(DatabaseHelper.ExecuteScalar(queryCount, new SqlParameter[] { new SqlParameter("@ReaderID", readerID) }));
                int newBooks = dgvBooks.SelectedRows.Count;

                if (currentlyBorrowed + newBooks > 5)
                {
                    MessageBox.Show($"Mỗi độc giả chỉ được mượn tối đa 5 quyển sách. Hiện tại độc giả đang mượn {currentlyBorrowed} quyển.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Sử dụng Transaction để tránh race condition
                string phieuMuonID = "PM" + DateTime.Now.ToString("yyyyMMddHHmmss");
                
                DatabaseHelper.ExecuteWithTransaction(cmd =>
                {
                    // 1. Insert into PhieuMuon
                    cmd.CommandText = "INSERT INTO PhieuMuon (IDPhieuMuon, IDNguoiMuon) VALUES (@ID, @ReaderID)";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@ID", phieuMuonID);
                    cmd.Parameters.AddWithValue("@ReaderID", readerID);
                    cmd.ExecuteNonQuery();

                    // 2. Insert into ChiTietMuon for each selected book
                    foreach (DataGridViewRow row in dgvBooks.SelectedRows)
                    {
                        string caTheSachID = row.Cells["IDCaTheSach"].Value.ToString();
                        
                        // Kiểm tra sách có sẵn không trong transaction
                        cmd.CommandText = "SELECT TinhTrang FROM CaTheSach WITH (UPDLOCK) WHERE IDCaTheSach = @CaTheSachID";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@CaTheSachID", caTheSachID);
                        object tinhTrangObj = cmd.ExecuteScalar();
                        
                        if (tinhTrangObj == null || tinhTrangObj.ToString().Trim() != "Sẵn sàng")
                        {
                            throw new Exception($"Sách {caTheSachID} không còn sẵn sàng!");
                        }

                        string ctID = DatabaseHelper.GenerateUniqueID("CT");
                        cmd.CommandText = @"INSERT INTO ChiTietMuon (IDChiTietMuon, IDPhieuMuon, IDCaTheSach, NgayMuon, HanTra, TinhTrangTra) 
                                          VALUES (@CTID, @PMID, @CaTheSachID, @NgayMuon, @HanTra, N'Đang mượn')";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@CTID", ctID);
                        cmd.Parameters.AddWithValue("@PMID", phieuMuonID);
                        cmd.Parameters.AddWithValue("@CaTheSachID", caTheSachID);
                        cmd.Parameters.AddWithValue("@NgayMuon", dtpBorrowDate.Value);
                        cmd.Parameters.AddWithValue("@HanTra", dtpBorrowDate.Value.AddDays(4));
                        cmd.ExecuteNonQuery();

                        // Update CaTheSach status
                        cmd.CommandText = "UPDATE CaTheSach SET TinhTrang = N'Đang mượn' WHERE IDCaTheSach = @CaTheSachID";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@CaTheSachID", caTheSachID);
                        cmd.ExecuteNonQuery();
                    }
                });

                UpdateBookCount(); // Update the book count after successful transaction
                SetupGridColumnsThongTinSach();
                MessageBox.Show($"Lập phiếu mượn thành công!\nMã phiếu: {phieuMuonID}\nHạn trả: {dtpBorrowDate.Value.AddDays(4):dd/MM/yyyy}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadThongTinSach();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lập phiếu mượn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvBooks_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void UpdateBookCount()
        {
            try
            {
                string query = @"
                    UPDATE ThongTinSach
                    SET SoLuong = (
                        SELECT COUNT(*)
                        FROM CaTheSach
                        WHERE CaTheSach.IDSach = ThongTinSach.IDSach
                          AND (CaTheSach.TinhTrang = N'Sẵn sàng' OR CaTheSach.TinhTrang = N'Đang mượn')
                    )
                    WHERE EXISTS (
                        SELECT 1
                        FROM CaTheSach
                        WHERE CaTheSach.IDSach = ThongTinSach.IDSach
                    );";

                DatabaseHelper.ExecuteNonQuery(query, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating book count: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string searchQuery = searchBox.Text;
            SetupGridColumnsThongTinSach();
            LoadThongTinSach(searchQuery);
        }
    }
}
