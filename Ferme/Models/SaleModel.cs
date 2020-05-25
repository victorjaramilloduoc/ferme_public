using System.Collections.Generic;

namespace Ferme.Models
{
    public class SaleModel
    {
        public int DocumentType { get; set; }
        public List<ProductModel> Cart { get; set; }
    }
}