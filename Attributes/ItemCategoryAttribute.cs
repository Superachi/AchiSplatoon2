using System;

namespace AchiSplatoon2.Attributes
{
    internal class ItemCategoryAttribute : Attribute
    {
        public string CategoryName { get; }
        public string DirectorySuffix { get; }

        public ItemCategoryAttribute(string name, string directorySuffix)
        {
            CategoryName = name;
            DirectorySuffix = directorySuffix;
        }

        public string PluralizeCategory()
        {
            if (CategoryName == "Brush")
            {
                return "Brushes";
            }

            return CategoryName + "s";
        }
    }
}
