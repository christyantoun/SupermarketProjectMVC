using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SupermarketProjectMVC.Models
{
    public class CategoryListModel
    {
        public List<Item> Item { get; set; }
        public SelectList Category { get; set; }
        public string ItemCategory { get; set; }
        public string SearchString { get; set; }

    }
}
