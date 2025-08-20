using BreakingGymDAL;
using BreakingGymEN;
using BreakinGymBL;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace BreakingGymUI
{
    /// <summary>
    /// Lógica de interacción para RegistroAsistencia.xaml
    /// </summary>
    public partial class RegistroAsistencia : MetroWindow
    {

        private RegistroAsistenciaBL _asistenciaBL = new RegistroAsistenciaBL();
        private SerialPort _puerto;

        public RegistroAsistencia()
        {
            InitializeComponent();
            CargarAsistencia();

            // Configura el puerto COM (ver en el Administrador de Dispositivos cuál es)
            _puerto = new SerialPort("COM3", 9600); // ⚠️ Cambia "COM3" si tu Arduino usa otro
            _puerto.DataReceived += Puerto_DataReceived;
            _puerto.Open();
        }

        public void CargarAsistencia()
        {
            List<RegistroAsistenciaEN> lista = _asistenciaBL.MostrarAsistencia();
            DgAsistencia.ItemsSource = lista;
        }

        private void BtnRefrescar_Click(object sender, RoutedEventArgs e)
        {
            CargarAsistencia();
        }

        private void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {
            if (DtBuscar.SelectedDate.HasValue)
            {
                DateTime fechaAsistencia = DtBuscar.SelectedDate.Value;
                List<RegistroAsistenciaEN> lista = _asistenciaBL.BuscarAsistencia(fechaAsistencia);
                DgAsistencia.ItemsSource = lista;
            }
            else
            {
                MessageBox.Show("Seleccione una fecha para buscar.");
            }
        }

        private void Puerto_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string uid = _puerto.ReadLine().Trim();
                Dispatcher.Invoke(() => RegistrarAsistencia(uid));
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(() =>
                {
                    MessageBox.Show("Error leyendo COM: " + ex.Message);
                });
            }
        }

        private void RegistrarAsistencia(string uid)
        {
            try
            {
                using (SqlConnection conn = (SqlConnection)ComunBD.ObtenerConexion(ComunBD.TipoBD.SqlServer))
                {
                    // Abre la conexión si aún no está abierta
                    if (conn.State != System.Data.ConnectionState.Open)
                        conn.Open();

                    // Buscar cliente con esa tarjeta
                    using (SqlCommand cmd = new SqlCommand(
                        "SELECT IdCliente FROM Cliente WHERE TarjetaRFID = @uid", conn))
                    {
                        cmd.Parameters.AddWithValue("@uid", uid);

                        object result = cmd.ExecuteScalar();

                        if (result != null)
                        {
                            int idCliente = Convert.ToInt32(result);

                            // Insertar asistencia
                            using (SqlCommand cmdInsert = new SqlCommand(
                                "INSERT INTO RegistroAsistencia(IdCliente, FechaAsistencia, HoraEntrada) " +
                                "VALUES(@idCliente, GETDATE(), CONVERT(TIME, GETDATE()))", conn))
                            {
                                cmdInsert.Parameters.AddWithValue("@idCliente", idCliente);
                                cmdInsert.ExecuteNonQuery();
                            }

                            Dispatcher.Invoke(() =>
                            {
                                MessageBox.Show($"✅ Asistencia registrada para Cliente {idCliente}");
                                CargarAsistencia(); // refresca la grilla en UI
                            });
                        }
                        else
                        {
                            Dispatcher.Invoke(() =>
                            {
                                MessageBox.Show("⚠️ Tarjeta no registrada en el sistema");
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(() =>
                {
                    MessageBox.Show("❌ Error al registrar asistencia: " + ex.Message);
                });
            }
        }

        private void DgAsistencia_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
