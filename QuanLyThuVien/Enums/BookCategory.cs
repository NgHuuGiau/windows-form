using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyThuVien.Enums
{
    public enum BookCategory
    {
        Mathematics,
        Physics,
        Chemistry,
        Biology,
        History,
        Geography,
        English,
        Literature
    }

    public static class BookCategoryExtensions
    {
        public static string GetDisplayName(this BookCategory category)
        {
            switch (category)
            {
                case BookCategory.Mathematics:
                    return "Mathematics";
                case BookCategory.Physics:
                    return "Physics";
                case BookCategory.Chemistry:
                    return "Chemistry";
                case BookCategory.Biology:
                    return "Biology";
                case BookCategory.History:
                    return "History";
                case BookCategory.Geography:
                    return "Geography";
                case BookCategory.English:
                    return "English";
                case BookCategory.Literature:
                    return "Literature";
                default:
                    return category.ToString();
            }
        }
    }
}
