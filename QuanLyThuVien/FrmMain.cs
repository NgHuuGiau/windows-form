using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyThuVien
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void DangNhapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmLogin frmLogin = new FrmLogin();
            frmLogin.MdiParent = this;
            frmLogin.Show();
        }

        private void HSNhanVien_Click(object sender, EventArgs e)
        {
            FrmStaff frmStaff = new FrmStaff();
            frmStaff.MdiParent = this;
            frmStaff.Show();
        }

        private void TheDocGia_Click(object sender, EventArgs e)
        {
            FrmReaderCard frmReaderCard = new FrmReaderCard();
            frmReaderCard.MdiParent = this;
            frmReaderCard.Show();
        }

        private void PhieuMuonSach_Click(object sender, EventArgs e)
        {
            FrmBorrow frmBorrow = new FrmBorrow();
            frmBorrow.MdiParent = this;
            frmBorrow.Show();
        }

        private void PhieuTraSach_Click(object sender, EventArgs e)
        {
            FrmReturn frmReturn = new FrmReturn();
            frmReturn.MdiParent = this;
            frmReturn.Show();
        }

        private void TiepNhanSachMoi_Click(object sender, EventArgs e)
        {
            FrmBookEntry frmBookEntry = new FrmBookEntry();
            frmBookEntry.MdiParent = this;
            frmBookEntry.Show();
        }

        private void TraCuuSach_Click(object sender, EventArgs e)
        {
            FrmSearchBook frmSearchBook = new FrmSearchBook();
            frmSearchBook.MdiParent = this;
            frmSearchBook.Show();
        }

        private void ThuTienPhat_Click(object sender, EventArgs e)
        {
            FrmFineCollection frmFineCollection = new FrmFineCollection();
            frmFineCollection.MdiParent = this;
            frmFineCollection.Show();
        }

        private void ThanhLy_Click(object sender, EventArgs e)
        {
            FrmLiquidation frmLiquidation = new FrmLiquidation();
            frmLiquidation.MdiParent = this;
            frmLiquidation.Show();
        }

        private void BaoCaoThongKe_Click(object sender, EventArgs e)
        {
            FrmReports frmReports = new FrmReports();   
            frmReports.MdiParent = this;
            frmReports.Show();
        }

        private void CaiDat_Click(object sender, EventArgs e)
        {
            FrmSettings frmSettings = new FrmSettings();    
            frmSettings.MdiParent = this;
            frmSettings.Show();
        }

        private void TroGiupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Quản Lý Thư Viện:\n\n\n" +
                "1. Nguyễn Đăng Khoa - 2311554033\n\n" +
                "2. Nguyễn Lê Khánh Hoàng - 2311552947\n\n" +
                "3. Đỗ Văn Hiệp - 2311553289\n\n" +
                "4. Nguyễn Lê Văn Dũng - 2311555475\n\n" +
                "5. Nguyễn Hữu Giàu - 2311553450", "Thông tin nhóm 3", MessageBoxButtons.OK);
        }
    }
}
