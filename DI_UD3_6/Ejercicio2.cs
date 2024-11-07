using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DI_UD3_6
{
    public class TemperaturaEventArgs : EventArgs
    {
        public double TemperaturaActual { get; set; }
        public double Umbral { get; set; }
    }

    public class ControlTemperatura
    {
        public event EventHandler<TemperaturaEventArgs> TemperaturaAlta;

        private double umbralTemperatura;

        public ControlTemperatura(double umbral)
        {
            umbralTemperatura = umbral;
        }

        public void MonitorearTemperatura(double temperaturaActual)
        {
            Console.WriteLine($"Temperatura actual: {temperaturaActual}°C");

            if (temperaturaActual > umbralTemperatura)
            {
                OnTemperaturaAlta(new TemperaturaEventArgs
                {
                    TemperaturaActual = temperaturaActual,
                    Umbral = umbralTemperatura
                });
            }
        }

        protected virtual void OnTemperaturaAlta(TemperaturaEventArgs e)
        {
            TemperaturaAlta?.Invoke(this, e);
        }
    }

    public class ServicioAlerta
    {
        public void EnviarAlerta(object sender, TemperaturaEventArgs e)
        {
            Console.WriteLine($"[ALERTA] La temperatura actual ({e.TemperaturaActual}°C) ha superado el umbral de {e.Umbral}°C.");
        }
    }

    public class ServicioRegistroTemperatura
    {
        public void RegistrarTemperatura(object sender, TemperaturaEventArgs e)
        {
            Console.WriteLine($"[REGISTRO] Registro de alta temperatura: {e.TemperaturaActual}°C (Umbral: {e.Umbral}°C).");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            double umbral = 30.0;
            ControlTemperatura controlTemperatura = new ControlTemperatura(umbral);

            ServicioAlerta servicioAlerta = new ServicioAlerta();
            ServicioRegistroTemperatura servicioRegistro = new ServicioRegistroTemperatura();
            controlTemperatura.TemperaturaAlta += servicioAlerta.EnviarAlerta;
            controlTemperatura.TemperaturaAlta += servicioRegistro.RegistrarTemperatura;

            double[] temperaturas = { 25.0, 28.5, 31.2, 29.8, 32.0 };

            foreach (double temp in temperaturas)
            {
                controlTemperatura.MonitorearTemperatura(temp);
            }

            Console.ReadLine();
        }
    }
}
