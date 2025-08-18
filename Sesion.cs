using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Super_OBR
{
    internal class Sesion
    {
        public static Usuario UsuarioActual 
        { get; set; }
        public static Usuario Usuario 
        { get; set; } = null;
        public static List<Producto> Carrito = new List<Producto>();
        public static List<Pedido> Pedidos = new List<Pedido>();



    }
}
