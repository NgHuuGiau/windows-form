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
            return category switch
            {
                BookCategory.Mathematics => "Mathematics",
                BookCategory.Physics => "Physics",
                BookCategory.Chemistry => "Chemistry",
                BookCategory.Biology => "Biology",
                BookCategory.History => "History",
                BookCategory.Geography => "Geography",
                BookCategory.English => "English",
                BookCategory.Literature => "Literature",
                _ => category.ToString()
            };
        }
    }
}
