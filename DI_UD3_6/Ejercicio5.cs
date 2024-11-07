using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DI_UD3_6
{
    public class EnergiaEventArgs : EventArgs
    {
        public double ConsumoActual { get; set; }
        public double Umbral { get; set; }
    }

    public class MonitorEnergia
    {
        public event EventHandler<EnergiaEventArgs> ConsumoExcesivoDetectado;

        private readonly double umbralConsumo;

        public MonitorEnergia(double umbral)
        {
            umbralConsumo = umbral;
        }

        public void RegistrarConsumo(double consumoActual)
        {
            Console.WriteLine($"Consumo actual registrado: {consumoActual} kW");

            if (consumoActual > umbralConsumo)
                OnConsumoExcesivoDetectado(new EnergiaEventArgs
                {
                    ConsumoActual = consumoActual,
                    Umbral = umbralConsumo
                });
            }
            else
            {
                Console.WriteLine("Consumo dentro del umbral permitido.");
            }
        }

        protected virtual void OnConsumoExcesivoDetectado(EnergiaEventArgs e)
        {
            ConsumoExcesivoDetectado?.Invoke(this, e);
        }
    }

    public class ServicioNotificacion
    {
        public void EnviarAdvertencia(object sender, EnergiaEventArgs e)
        {
            Console.WriteLine($"[ADVERTENCIA] Consumo excesivo detectado: {e.ConsumoActual} kW (Umbral: {e.Umbral} kW). ¡Por favor, reduzca el consumo!");
        }
    }

    public class ServicioAjusteAutomatizado
    {
        public void AjustarConsumo(object sender, EnergiaEventArgs e)
        {
            Console.WriteLine("[AJUSTE AUTOMÁTICO] Reducción del consumo activada. Ajustando dispositivos para optimizar el uso de energía.");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            MonitorEnergia monitorEnergia = new MonitorEnergia(500);

            ServicioNotificacion servicioNotificacion = new ServicioNotificacion();
            ServicioAjusteAutomatizado servicioAjuste = new ServicioAjusteAutomatizado();

            monitorEnergia.ConsumoExcesivoDetectado += servicioNotificacion.EnviarAdvertencia;
            monitorEnergia.ConsumoExcesivoDetectado += servicioAjuste.AjustarConsumo;

            monitorEnergia.RegistrarConsumo(450);
            monitorEnergia.RegistrarConsumo(550); 

            Console.ReadLine();
        }
    }
}
