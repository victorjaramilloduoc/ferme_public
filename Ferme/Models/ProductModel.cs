using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferme.Models
{   
    // Me permite transformar la clase, en este caso JSON
    [Serializable]
    public class ProductModel
    {
        public long CodigoProducto { get; set; }
        public string Nombre { get; set; }
        public string Imagen { get; set; }
        public string Descripcion { get; set; }
        public long Stock { get; set; }
        public long Precio { get; set; }
        public string MarcaProducto { get; set; }
    }
}
