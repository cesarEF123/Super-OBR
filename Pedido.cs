using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Super_OBR
{
    internal class Pedido
    {
        public int Id 
        { get; set; }
        public int UsuarioId 
        { get; set; }
        public DateTime Fecha 
        { get; set; }
        public string Estado 
        { get; set; }
        public decimal Total 
        { get; set; }
        public List<Producto> Productos 
        { get; set; }


    }
}
