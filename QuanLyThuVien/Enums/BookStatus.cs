using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyThuVien.Enums
{
    public enum BookStatus
    {
        SanSang,
        DangMuon,
        Hong,
        Mat
    }

    public static class BookStatusExtensions
    {
        public static string ToDisplayString(this BookStatus status)
        {
            switch (status)
            {
                case BookStatus.SanSang: return "Sẵn sàng";
                case BookStatus.DangMuon: return "Đang mượn";
                case BookStatus.Hong: return "Hỏng";
                case BookStatus.Mat: return "Mất";
                default: return status.ToString();
            }
        }

        public static BookStatus FromDbString(string dbValue)
        {
            if (string.IsNullOrWhiteSpace(dbValue)) return BookStatus.SanSang;
            
            var trimmed = dbValue.Trim().ToLower();
            switch (trimmed)
            {
                case "sẵn sàng":
                case "san sang":
                case "ok":
                    return BookStatus.SanSang;
                case "đang mượn":
                case "dang muon":
                case "muon":
                    return BookStatus.DangMuon;
                case "hỏng":
                case "hong":
                    return BookStatus.Hong;
                case "mất":
                case "mat":
                    return BookStatus.Mat;
                default:
                    return BookStatus.SanSang;
            }
        }
    }
}
