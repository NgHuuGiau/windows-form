namespace QuanLyThuVien
{
    partial class FrmSearchBook
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtMaSach = new System.Windows.Forms.TextBox();
            this.btnTimKiem = new System.Windows.Forms.Button();
            this.grpBoLoc = new System.Windows.Forms.GroupBox();
            this.cboTheLoaiFilter = new System.Windows.Forms.ComboBox();
            this.lblTheLoaiFilterLabel = new System.Windows.Forms.Label();
            this.lblTinhTrangLabel = new System.Windows.Forms.Label();
            this.btnDangMuon = new System.Windows.Forms.Button();
            this.btnSanSang = new System.Windows.Forms.Button();
            this.btnTatCa = new System.Windows.Forms.Button();
            this.dgvBooks = new System.Windows.Forms.DataGridView();
            this.lblMaSach = new System.Windows.Forms.Label();
            this.grpBoLoc.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBooks)).BeginInit();
            this.SuspendLayout();
            // 
            // txtMaSach
            // 
            this.txtMaSach.Location = new System.Drawing.Point(16, 47);
            this.txtMaSach.Margin = new System.Windows.Forms.Padding(4);
            this.txtMaSach.Name = "txtMaSach";
            this.txtMaSach.Size = new System.Drawing.Size(285, 22);
            this.txtMaSach.TabIndex = 0;
            // 
            // btnTimKiem
            // 
            this.btnTimKiem.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTimKiem.Location = new System.Drawing.Point(16, 79);
            this.btnTimKiem.Margin = new System.Windows.Forms.Padding(4);
            this.btnTimKiem.Name = "btnTimKiem";
            this.btnTimKiem.Size = new System.Drawing.Size(287, 33);
            this.btnTimKiem.TabIndex = 1;
            this.btnTimKiem.Text = "Tìm kiếm";
            this.btnTimKiem.UseVisualStyleBackColor = true;
            // 
            // grpBoLoc
            // 
            this.grpBoLoc.Controls.Add(this.cboTheLoaiFilter);
            this.grpBoLoc.Controls.Add(this.lblTheLoaiFilterLabel);
            this.grpBoLoc.Controls.Add(this.lblTinhTrangLabel);
            this.grpBoLoc.Controls.Add(this.btnDangMuon);
            this.grpBoLoc.Controls.Add(this.btnSanSang);
            this.grpBoLoc.Controls.Add(this.btnTatCa);
            this.grpBoLoc.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpBoLoc.Location = new System.Drawing.Point(16, 145);
            this.grpBoLoc.Margin = new System.Windows.Forms.Padding(4);
            this.grpBoLoc.Name = "grpBoLoc";
            this.grpBoLoc.Padding = new System.Windows.Forms.Padding(4);
            this.grpBoLoc.Size = new System.Drawing.Size(287, 270);
            this.grpBoLoc.TabIndex = 2;
            this.grpBoLoc.TabStop = false;
            this.grpBoLoc.Text = "Bộ Lọc";
            // 
            // cboTheLoaiFilter
            // 
            this.cboTheLoaiFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTheLoaiFilter.FormattingEnabled = true;
            this.cboTheLoaiFilter.Location = new System.Drawing.Point(8, 65);
            this.cboTheLoaiFilter.Margin = new System.Windows.Forms.Padding(4);
            this.cboTheLoaiFilter.Name = "cboTheLoaiFilter";
            this.cboTheLoaiFilter.Size = new System.Drawing.Size(265, 32);
            this.cboTheLoaiFilter.TabIndex = 4;
            // 
            // lblTheLoaiFilterLabel
            // 
            this.lblTheLoaiFilterLabel.AutoSize = true;
            this.lblTheLoaiFilterLabel.Location = new System.Drawing.Point(8, 38);
            this.lblTheLoaiFilterLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTheLoaiFilterLabel.Name = "lblTheLoaiFilterLabel";
            this.lblTheLoaiFilterLabel.Size = new System.Drawing.Size(82, 24);
            this.lblTheLoaiFilterLabel.TabIndex = 8;
            this.lblTheLoaiFilterLabel.Text = "Thể loại";
            // 
            // lblTinhTrangLabel
            // 
            this.lblTinhTrangLabel.AutoSize = true;
            this.lblTinhTrangLabel.Location = new System.Drawing.Point(8, 102);
            this.lblTinhTrangLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTinhTrangLabel.Name = "lblTinhTrangLabel";
            this.lblTinhTrangLabel.Size = new System.Drawing.Size(102, 24);
            this.lblTinhTrangLabel.TabIndex = 7;
            this.lblTinhTrangLabel.Text = "Tình trạng";
            // 
            // btnDangMuon
            // 
            this.btnDangMuon.Location = new System.Drawing.Point(8, 218);
            this.btnDangMuon.Margin = new System.Windows.Forms.Padding(4);
            this.btnDangMuon.Name = "btnDangMuon";
            this.btnDangMuon.Size = new System.Drawing.Size(267, 33);
            this.btnDangMuon.TabIndex = 6;
            this.btnDangMuon.Text = "Đang mượn";
            this.btnDangMuon.UseVisualStyleBackColor = true;
            // 
            // btnSanSang
            // 
            this.btnSanSang.Location = new System.Drawing.Point(8, 174);
            this.btnSanSang.Margin = new System.Windows.Forms.Padding(4);
            this.btnSanSang.Name = "btnSanSang";
            this.btnSanSang.Size = new System.Drawing.Size(267, 33);
            this.btnSanSang.TabIndex = 5;
            this.btnSanSang.Text = "Sẵn sàng";
            this.btnSanSang.UseVisualStyleBackColor = true;
            // 
            // btnTatCa
            // 
            this.btnTatCa.Location = new System.Drawing.Point(8, 129);
            this.btnTatCa.Margin = new System.Windows.Forms.Padding(4);
            this.btnTatCa.Name = "btnTatCa";
            this.btnTatCa.Size = new System.Drawing.Size(267, 33);
            this.btnTatCa.TabIndex = 4;
            this.btnTatCa.Text = "Tất cả";
            this.btnTatCa.UseVisualStyleBackColor = true;
            // 
            // dgvBooks
            // 
            this.dgvBooks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBooks.Location = new System.Drawing.Point(323, 15);
            this.dgvBooks.Margin = new System.Windows.Forms.Padding(4);
            this.dgvBooks.Name = "dgvBooks";
            this.dgvBooks.RowHeadersWidth = 51;
            this.dgvBooks.Size = new System.Drawing.Size(773, 538);
            this.dgvBooks.TabIndex = 3;
            // 
            // lblMaSach
            // 
            this.lblMaSach.AutoSize = true;
            this.lblMaSach.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMaSach.Location = new System.Drawing.Point(11, 15);
            this.lblMaSach.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMaSach.Name = "lblMaSach";
            this.lblMaSach.Size = new System.Drawing.Size(82, 24);
            this.lblMaSach.TabIndex = 4;
            this.lblMaSach.Text = "Mã sách";
            // 
            // FrmSearchBook
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1112, 567);
            this.Controls.Add(this.lblMaSach);
            this.Controls.Add(this.dgvBooks);
            this.Controls.Add(this.grpBoLoc);
            this.Controls.Add(this.btnTimKiem);
            this.Controls.Add(this.txtMaSach);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FrmSearchBook";
            this.Text = "Tra Cứu Sách";
            this.grpBoLoc.ResumeLayout(false);
            this.grpBoLoc.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBooks)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox txtMaSach;
        public System.Windows.Forms.Button btnTimKiem;
        public System.Windows.Forms.GroupBox grpBoLoc;
        public System.Windows.Forms.Button btnDangMuon;
        public System.Windows.Forms.Button btnSanSang;
        public System.Windows.Forms.Button btnTatCa;
        public System.Windows.Forms.Label lblTinhTrangLabel;
        public System.Windows.Forms.Label lblTheLoaiFilterLabel;
        public System.Windows.Forms.ComboBox cboTheLoaiFilter;
        public System.Windows.Forms.DataGridView dgvBooks;
        public System.Windows.Forms.Label lblMaSach;
    }
}