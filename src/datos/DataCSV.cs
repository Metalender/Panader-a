
using modelos;
using System.Globalization;

namespace datos
{
    public class ClientesCSV : IData<Cliente>
    {
        string _fileClientes = "../../RepositoriosCSV/clientes.csv";

        public void guardar(List<Cliente> misClientes)
        {
            List<string> data = new() { };
            misClientes.ForEach(Cliente =>
            {
                var str = $"{Cliente.nombre},{Cliente.apellido},{Cliente.dni},{Cliente.telefono},{Cliente.pueblo}";
                data.Add(str);
            });
            File.WriteAllLines(_fileClientes, data);

        }

        public List<Cliente> leer()
        {
            List<Cliente> misClientes = new();
            var data = File.ReadAllLines(_fileClientes).Where(row => row.Length > 0).ToList();
            data.ForEach(row =>
            {
                var campos = row.Split(",");
                Cliente cliente = new Cliente
                (
                    nombre: campos[0],
                    apellido: campos[1],
                    dni: campos[2],
                    telefono: campos[3],
                    pueblo: campos[4]
                );
                misClientes.Add(cliente);
            });

            return misClientes;

        }

    }

    public class PedidosCSV : IData<Pedido>
    {
        string _filePedidos = "../../RepositoriosCSV/pedidos.csv";

        public void guardar(List<Pedido> misPedidos)
        {
            List<string> data = new() { };
            misPedidos.ForEach(Pedido =>
            {
                var str = $"{Pedido.ID},{Pedido.dniCliente},{Pedido.fecha.ToShortDateString()},{Pedido.precioPedido.ToString(CultureInfo.InvariantCulture)},{Pedido.estado},{Pedido.tipoPedido}";
                data.Add(str);
            });
            File.WriteAllLines(_filePedidos, data);

        }

        public List<Pedido> leer()
        {
            List<Pedido> misPedidos = new();
            var data = File.ReadAllLines(_filePedidos).Where(row => row.Length > 0).ToList();
            data.ForEach(row =>
            {
                var campos = row.Split(",");
                Pedido pedido = new Pedido
                (
                    ID: Guid.Parse(campos[0]),
                    dniCliente: campos[1],
                    fecha: DateTime.Parse(campos[2]),
                    precioPedido: Decimal.Parse(campos[3], CultureInfo.InvariantCulture),
                    estado: (estadoPedido)Enum.Parse((typeof(estadoPedido)), campos[4]),
                    tipoPedido: (tipoDePedido)Enum.Parse((typeof(tipoDePedido)), campos[5])
                );
                misPedidos.Add(pedido);
            });

            return misPedidos;

        }
    }


    public class PanesPedidosCSV : IData<PanesPedido>
    {
        string _filePanesPedidos = "../../RepositoriosCSV/panesPedidos.csv";

        public void guardar(List<PanesPedido> misPanesPorPedido)
        {
            List<string> data = new() { };
            misPanesPorPedido.ForEach(PanesPedido =>
            {
                var str = $"{PanesPedido.ID.ToString()},{PanesPedido.pan.ToCSV()},{PanesPedido.cantidad.ToString()}";
                data.Add(str);
            });
            File.WriteAllLines(_filePanesPedidos, data);

        }

        public List<PanesPedido> leer()
        {
            List<PanesPedido> misPanesPorPedido = new();
            var data = File.ReadAllLines(_filePanesPedidos).Where(row => row.Length > 0).ToList();
            data.ForEach(row =>
            {
                var campos = row.Split(",");
                var panesPedido = new PanesPedido
                (
                    ID: Guid.Parse(campos[0]),
                    pan: new Pan((tipoDePan)Enum.Parse((typeof(tipoDePan)), campos[1]), Decimal.Parse(campos[2], CultureInfo.InvariantCulture)),
                    cantidad: int.Parse(campos[3])
                );
                misPanesPorPedido.Add(panesPedido);
            });

            return misPanesPorPedido;

        }

    }

    public class DeudasCSV : IData<Deuda>
    {
        string _fileDeudas = "../../RepositoriosCSV/deudas.csv";

        public void guardar(List<Deuda> misDeudas)
        {
            List<string> data = new() { };
            misDeudas.ForEach(deuda =>
            {
                var str = $"{deuda.dniCliente},{deuda.fecha.ToShortDateString()},{deuda.importe.ToString(CultureInfo.InvariantCulture)}";
                data.Add(str);
            });
            File.WriteAllLines(_fileDeudas, data);

        }

        public List<Deuda> leer()
        {
            List<Deuda> misDeudas = new();
            var data = File.ReadAllLines(_fileDeudas).Where(row => row.Length > 0).ToList();
            data.ForEach(row =>
            {
                var campos = row.Split(",");
                var deuda = new Deuda
                (
                    dniCliente: campos[0],
                    fecha: DateTime.Parse(campos[1]),
                    importe: Decimal.Parse(campos[2], CultureInfo.InvariantCulture)
                );
                misDeudas.Add(deuda);
            });
            return misDeudas;
        }

    }

}
