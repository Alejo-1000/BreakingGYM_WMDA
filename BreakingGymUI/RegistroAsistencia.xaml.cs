using System;
using System.Collections.Generic;
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
using MahApps.Metro.Controls;
using BreakinGymBL;
using BreakingGymEN;

namespace BreakingGymUI
{
    /// <summary>
    /// Lógica de interacción para RegistroAsistencia.xaml
    /// </summary>
    public partial class RegistroAsistencia : MetroWindow
    {
        RegistroAsistenciaBL _mostrarAsistencia = new RegistroAsistenciaBL();
        RegistroAsistenciaEN _AsistenciaEN = new RegistroAsistenciaEN();
        public RegistroAsistencia()
        {
            InitializeComponent();
            CargarGrid();
        }
        public void CargarGrid()
        {
            DgAsistencia.ItemsSource = _mostrarAsistencia.MostrarAsistencia();
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            // Validar cliente seleccionado
            if (txtCliente.Text == null || !byte.TryParse(txtCliente.Text.ToString(), out byte idCliente))
            {
                MessageBox.Show("Por favor, seleccione un cliente", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Validar fecha (usando DatePicker)
            if (!DtFecha.SelectedDate.HasValue)
            {
                MessageBox.Show("Por favor, ingrese una fecha válida.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            DateTime fechaAsistencia = DtFecha.SelectedDate.Value;

            // Crear objeto
            var asis = new RegistroAsistenciaEN()
            {
                IdCliente = idCliente,
                FechaAsistencia = fechaAsistencia
            };

            // Guardar inscripción
            _mostrarAsistencia.GuardarInscripcion(asis);

            MessageBox.Show("Asistencia registrada correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

            CargarGrid();
        }

        private void DgAsistencia_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DgAsistencia.SelectedItem is RegistroAsistenciaEN filaSeleccionada)
            {
                txtId.Text = filaSeleccionada.Id.ToString();
                txtCliente.Text = filaSeleccionada.IdCliente.ToString();
            }
        }
       
        private void BtnModificar_Click(object sender, RoutedEventArgs e)
        {
            // Validar que se haya seleccionado una fila
            if (DgAsistencia.SelectedItem is RegistroAsistenciaEN filaSeleccionada)
            {
                // Validar cliente seleccionado
                if (txtCliente.Text == null || !byte.TryParse(txtCliente.Text.ToString(), out byte idCliente))
                {
                    MessageBox.Show("Por favor, seleccione un cliente", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                // Validar fecha (usando DatePicker)
                if (!DtFecha.SelectedDate.HasValue)
                {
                    MessageBox.Show("Por favor, ingrese una fecha válida.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                DateTime fechaAsistencia = DtFecha.SelectedDate.Value;
                // Crear objeto
                var asis = new RegistroAsistenciaEN()
                {
                    Id = filaSeleccionada.Id,
                    IdCliente = idCliente,
                    FechaAsistencia = fechaAsistencia
                };
                // Modificar asistencia
                _mostrarAsistencia.ModificarAsistencia(asis);
                MessageBox.Show("Asistencia modificada correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                CargarGrid();
            }
            else
            {
                MessageBox.Show("Por favor, seleccione una asistencia para modificar.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
