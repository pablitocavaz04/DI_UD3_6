using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DI_UD3_6
{
    public class VentaEventArgs : EventArgs
    {
        public string Producto { get; set; }
        public decimal Precio { get; set; }
    }

    public class RegistroVentas
    {
        public event EventHandler<VentaEventArgs> VentaRealizada;

        public void ProcesarVenta(string producto, decimal precio)
        {
            Console.WriteLine("Procesando venta...");

            OnVentaRealizada(new VentaEventArgs { Producto = producto, Precio = precio });
        }

        protected virtual void OnVentaRealizada(VentaEventArgs e)
        {
            VentaRealizada?.Invoke(this, e);
        }
    }

    public class ServicioRegistro
    {
        public void RegistrarVenta(object sender, VentaEventArgs e)
        {
            Console.WriteLine($"Registro de Venta: Producto - {e.Producto}, Precio - ${e.Precio}");
        }
    }

    public class ServicioNotificacion
    {
        public void EnviarNotificacionVenta(object sender, VentaEventArgs e)
        {
            Console.WriteLine($"Notificación: La venta del producto '{e.Producto}' por ${e.Precio} ha sido realizada exitosamente.");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            RegistroVentas registroVentas = new RegistroVentas();
            ServicioRegistro servicioRegistro = new ServicioRegistro();
            ServicioNotificacion servicioNotificacion = new ServicioNotificacion();

            registroVentas.VentaRealizada += servicioRegistro.RegistrarVenta;
            registroVentas.VentaRealizada += servicioNotificacion.EnviarNotificacionVenta;

            registroVentas.ProcesarVenta("Laptop", 1200.99m);
            registroVentas.ProcesarVenta("Smartphone", 799.50m);

            Console.ReadLine();
        }
    }
}
