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
using BreakingGymEN;
using BreakinGymBL;
using BreakingGymDAL;

namespace BreakingGymUI
{
    /// <summary>
    /// Lógica de interacción para Membresia.xaml
    /// </summary>
    public partial class Membresia : MetroWindow
    {
        MembresiaEN membresiaEN = new MembresiaEN();
        MembresiaBL _mostrarMembresia = new MembresiaBL();
        public Membresia()
        {
            InitializeComponent();
            CargarGrid();
        }
        public void CargarGrid()
        {
            DataMembresia.ItemsSource = _mostrarMembresia.MostrarMembresia();
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (cbxIdServicio.SelectedValue == null || !byte.TryParse(cbxIdServicio.SelectedValue.ToString(), out byte idServicio))
{
    MessageBox.Show("Seleccione un servicio válido.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    return;
}

// Validar y convertir Precio
string precioTexto = TxtPrecio.Text.Trim();

if (!int.TryParse(precioTexto, out int precio) || precio <= 0)
{
    MessageBox.Show("Ingrese un precio válido mayor a 0.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    return;
}

// Validar campos de texto
string nombre = TxtNombre.Text.Trim();
string duracion = TxtDuracion.Text.Trim();
string descripcion = TxtDescripcion.Text.Trim();

if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(duracion) || string.IsNullOrEmpty(descripcion))
{
    MessageBox.Show("Por favor, complete todos los campos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    return;
}

// Obtener lista de membresías existentes para validar duplicados
var listaMembresias = _mostrarMembresia.MostrarMembresia(); // Método que devuelve todas las membresías

// Validar duplicado: que no exista ya una membresía con el mismo nombre y mismo servicio
bool yaExiste = listaMembresias.Any(m =>
    m.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase) &&
    m.IdServicio == idServicio);

if (yaExiste)
{
    MessageBox.Show("Ya existe una membresía con ese nombre para el servicio seleccionado. No se puede duplicar.", 
                    "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
    return;
}

// Crear objeto de membresía
var membresia = new MembresiaEN
{
    IdServicio = idServicio,
    Nombre = nombre,
    Duracion = duracion,
    Precio = precio,
    Descripcion = descripcion
};

// Guardar membresía
_mostrarMembresia.GuardarMembresia(membresia);

// Limpiar campos
TxtNombre.Clear();
TxtDuracion.Clear();
TxtPrecio.Clear();
TxtDescripcion.Clear();

MessageBox.Show("Membresía guardada correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

// Recargar el grid o lista en WPF
CargarGrid();

        }

        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (!byte.TryParse(TxtId.Text.Trim(), out byte id) || id <= 0)
            {
                MessageBox.Show("Por favor, seleccione un Id válido.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var membre = new MembresiaEN
            {
                Id = id
            };

            // Confirmar antes de eliminar
            MessageBoxResult confirmResult = MessageBox.Show("¿Estás seguro que deseas eliminar esta membresía?",
                                                             "Confirmar eliminación",
                                                             MessageBoxButton.YesNo,
                                                             MessageBoxImage.Warning);

            if (confirmResult == MessageBoxResult.Yes)
            {
                _mostrarMembresia.EliminarMembresia(membre);  // ✅ Ahora usas el objeto correcto
                CargarGrid();
                TxtId.Clear();
                MessageBox.Show("Membresía eliminada correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        private void BtnModificar_Click(object sender, RoutedEventArgs e)
        {
            // Validar ID
            if (!byte.TryParse(TxtId.Text.Trim(), out byte id) || id <= 0)
            {
                MessageBox.Show("Por favor, ingrese un Id válido.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Validar servicio seleccionado
            if (cbxIdServicio.SelectedValue == null || !byte.TryParse(cbxIdServicio.SelectedValue.ToString(), out byte idServicio))
            {
                MessageBox.Show("Seleccione un servicio válido.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Validar precio
            if (!int.TryParse(TxtPrecio.Text.Trim(), out int precio) || precio <= 0)
            {
                MessageBox.Show("Ingrese un precio válido mayor a 0.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Validar campos de texto
            string nombre = TxtNombre.Text.Trim();
            string duracion = TxtDuracion.Text.Trim();
            string descripcion = TxtDescripcion.Text.Trim();

            if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(duracion) || string.IsNullOrEmpty(descripcion))
            {
                MessageBox.Show("Por favor, complete todos los campos de texto.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Confirmar modificación
            MessageBoxResult confirmResult = MessageBox.Show("¿Estás seguro que deseas modificar esta membresía?",
                                                             "Confirmar modificación",
                                                             MessageBoxButton.YesNo,
                                                             MessageBoxImage.Question);

            if (confirmResult == MessageBoxResult.No)
                return;

            // Crear objeto
            var membresia = new MembresiaEN
            {
                Id = id,
                IdServicio = idServicio,
                Nombre = nombre,
                Duracion = duracion,
                Precio = precio,
                Descripcion = descripcion
            };

            // Guardar cambios
            _mostrarMembresia.ModificarMembresia(membresia);

            // Limpiar
            TxtId.Clear();
            TxtNombre.Clear();
            TxtDuracion.Clear();
            TxtPrecio.Clear();
            TxtDescripcion.Clear();

            CargarGrid();

            MessageBox.Show("Membresía modificada correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

        }

        private void BtnRefrescar_Click(object sender, RoutedEventArgs e)
        {
            CargarGrid();
            TxtBuscarMembresia.Clear();
        }

        private void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {
                string nombre = TxtBuscarMembresia.Text;
                List<MembresiaEN> membresias = MembresiaBL.BuscarMembresia(nombre);
                DataMembresia.ItemsSource = membresias;

        }

        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
