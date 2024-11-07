using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DI_UD3_6
{
    public class IntrusoEventArgs : EventArgs
    {
        public string NombreSensor { get; set; }
        public DateTime HoraDeteccion { get; set; }
    }

    public class SensorMonitoreo
    {
        public event EventHandler<IntrusoEventArgs> AlertaIntruso;

        private readonly TimeSpan horarioInicio = new TimeSpan(8, 0, 0);
        private readonly TimeSpan horarioFin = new TimeSpan(20, 0, 0);

        public void VerificarSensor(string nombreSensor)
        {
            DateTime horaActual = DateTime.Now;
            TimeSpan horaActualTimeSpan = horaActual.TimeOfDay;

            if (horaActualTimeSpan < horarioInicio || horaActualTimeSpan > horarioFin)
            {
                OnAlertaIntruso(new IntrusoEventArgs
                {
                    NombreSensor = nombreSensor,
                    HoraDeteccion = horaActual
                });
            }
            else
            {
                Console.WriteLine($"Sensor {nombreSensor}: Detección dentro del horario permitido.");
            }
        }

        protected virtual void OnAlertaIntruso(IntrusoEventArgs e)
        {
            AlertaIntruso?.Invoke(this, e);
        }
    }

    public class ServicioAlarma
    {
        public void ActivarAlarma(object sender, IntrusoEventArgs e)
        {
            Console.WriteLine($"[ALERTA] ¡Intruso detectado en {e.NombreSensor} a las {e.HoraDeteccion}! Activando alarma.");
        }
    }
    public class ServicioRegistroIncidencias
    {
        public void RegistrarIncidencia(object sender, IntrusoEventArgs e)
        {
            Console.WriteLine($"[REGISTRO] Incidencia registrada: Intruso en {e.NombreSensor} a las {e.HoraDeteccion}.");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            SensorMonitoreo sensorMonitoreo = new SensorMonitoreo();
            ServicioAlarma servicioAlarma = new ServicioAlarma();
            ServicioRegistroIncidencias servicioRegistro = new ServicioRegistroIncidencias();

            sensorMonitoreo.AlertaIntruso += servicioAlarma.ActivarAlarma;
            sensorMonitoreo.AlertaIntruso += servicioRegistro.RegistrarIncidencia;

            sensorMonitoreo.VerificarSensor("Puerta Principal");
            sensorMonitoreo.VerificarSensor("Ventana Sala");

            Console.ReadLine();
        }
    }
}
