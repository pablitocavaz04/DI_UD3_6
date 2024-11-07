using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DI_UD3_6
{
    public class BackupEventArgs : EventArgs
    {
        public string NombreArchivo { get; set; }
        public DateTime FechaBackup { get; set; }
    }

    public class GestorBackups
    {
        public event EventHandler<BackupEventArgs> BackupCompletado;

        public void RealizarBackup(string nombreArchivo)
        {
            Console.WriteLine($"Respaldo en proceso para el archivo: {nombreArchivo}");
            System.Threading.Thread.Sleep(2000);

            OnBackupCompletado(new BackupEventArgs
            {
                NombreArchivo = nombreArchivo,
                FechaBackup = DateTime.Now
            });
        }

        protected virtual void OnBackupCompletado(BackupEventArgs e)
        {
            BackupCompletado?.Invoke(this, e);
        }
    }

    public class ServicioNotificacion
    {
        public void EnviarNotificacion(object sender, BackupEventArgs e)
        {
            Console.WriteLine($"[NOTIFICACIÓN] El archivo '{e.NombreArchivo}' ha sido respaldado exitosamente el {e.FechaBackup}.");
        }
    }

    public class ServicioLog
    {
        public void RegistrarBackup(object sender, BackupEventArgs e)
        {
            Console.WriteLine($"[LOG] Respaldo realizado para '{e.NombreArchivo}' en la fecha {e.FechaBackup}.");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            GestorBackups gestorBackups = new GestorBackups();
            ServicioNotificacion servicioNotificacion = new ServicioNotificacion();
            ServicioLog servicioLog = new ServicioLog();

            gestorBackups.BackupCompletado += servicioNotificacion.EnviarNotificacion;
            gestorBackups.BackupCompletado += servicioLog.RegistrarBackup;

            gestorBackups.RealizarBackup("archivo_importante.txt");
            gestorBackups.RealizarBackup("documento_seguro.docx");

            Console.ReadLine();
        }
    }
}
