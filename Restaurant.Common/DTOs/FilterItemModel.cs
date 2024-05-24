using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Common.DTOs
{
    public class FilterItemModel
    {
        public int CurrentPage { get; set; } = 0;

        public int ItemsOnPage { get; set; } = 6;
    }
}
