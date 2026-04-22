using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyThuVien.Enums
{
    public enum LiquidationReason
    {
        Lost,
        Damaged,
        LostByUser
    }

    public static class LiquidationReasonExtensions
    {
        public static string GetDisplayName(this LiquidationReason reason)
        {
            return reason switch
            {
                LiquidationReason.Lost => "Lost",
                LiquidationReason.Damaged => "Damaged",
                LiquidationReason.LostByUser => "LostByUser",
                _ => reason.ToString()
            };
        }
    }
}
