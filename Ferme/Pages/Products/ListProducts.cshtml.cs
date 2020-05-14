using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Ferme.Areas.Identity.Pages.Products
{
    public class ListProductsModel : PageModel
    {
        public string Description { get; set; }
        public long?  Id { get; set; }
        public string Name { get; set; }
        public long? Price { get; set; }
        public long?  ProductCode { get; set; }
        public long? Stock { get; set; }
        public long? SupplierId { get; set; } 
    }
}
