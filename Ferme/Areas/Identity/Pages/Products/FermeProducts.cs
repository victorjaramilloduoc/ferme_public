using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferme.Areas.Identity.Pages.Products
{
    public class FermeProducts
    {
        public string Description { get; set; }
        public long? Id { get; set; }
        public string Name { get; set; }
        public long? Price { get; set; }
        public long? ProductCode { get; set; }
        public long? Stock { get; set; }
        public long? SupplierId { get; set; }
    }
}
