
using System.Globalization;

namespace modelos
{
    public class Cliente
    {
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string dni { get; set; }
        public string telefono { get; set; }
        public string pueblo { get; set; }
        public Decimal deudasPendientes { get; set; } = new();
        public Cliente() { }
        public Cliente(string nombre, string apellido, string dni, string telefono, string pueblo)
        {
            this.nombre = nombre;
            this.apellido = apellido;
            this.dni = dni;
            this.telefono = telefono;
            this.pueblo = pueblo;
        }
        public override string ToString() =>
        $"{nombre} {apellido} con DNI: {dni} y Tfno: {telefono} - Pueblo: {pueblo} ";

        public string verClientesConPedido() =>
        $"{nombre} {apellido} - Pueblo: {pueblo} ";
    }

    public enum tipoDePan
    {
        panIntegral,
        masaMadre,
        panSemillas,
        Hogaza,
        panCenteno

    }
    public class Pan
    {
        public tipoDePan tipo { get; set; }
        public Decimal precio { get; set; }
        public Pan() { }
        public Pan(tipoDePan tipo, Decimal precio)
        {
            this.tipo = tipo;
            this.precio = Math.Round(precio, 2);
        }
        public override string ToString() =>
        $"{tipo}  precio: {precio}\u20AC";
        public string ToCSV() =>
        $"{tipo},{precio.ToString(CultureInfo.InvariantCulture)}";
        public string ToPanesPedido() =>
        $"{tipo}";

    }
    public enum estadoPedido
    {
        pagado,
        pendiente
    }
    public enum tipoDePedido
    {
        Ocasional,
        Habitual

    }

    public class Pedido
    {
        public Guid ID { get; set; }
        public string dniCliente { get; set; }
        public DateTime fecha { get; set; }
        public Decimal precioPedido { get; set; }
        public estadoPedido estado { get; set; }
        public tipoDePedido tipoPedido { get; set;}
        public List<PanesPedido> listaDePan { get; set; } = new();

        public Pedido() { }

        public Pedido(Guid ID, string dniCliente, DateTime fecha, Decimal precioPedido, estadoPedido estado, tipoDePedido tipoPedido)
        {
            this.ID = ID;
            this.dniCliente = dniCliente;
            this.fecha = fecha;
            this.precioPedido = Math.Round(precioPedido, 2);
            this.estado = estado;
            this.tipoPedido = tipoPedido;
        }
        public override string ToString() =>
       $"----------------------------------------------------\nPedido: ({tipoPedido})\n\nDNI del cliente: {dniCliente} - Para el dia: {fecha.ToShortDateString()} \nTotal del pedido: {precioPedido} \u20AC - Estado del pedido: {estado}\n";

        public string stringParaVerCliente() =>
        $"Para el dia: {fecha.ToShortDateString()} \nTotal del pedido {precioPedido} \u20AC Estado del pedido: {estado}";

    }
    public class PanesPedido
    {
        public Guid ID { get; set; }
        public Pan pan { get; set; }
        public int cantidad { get; set; }

        public PanesPedido(Guid ID, Pan pan, int cantidad)
        {
            this.ID = ID;
            this.pan = pan;
            this.cantidad = cantidad;

        }
        public override string ToString() =>
        $"{pan.ToPanesPedido()} - {cantidad} unidades";
    }

    public class Deuda
    {
        public string dniCliente { get; set; }
        public DateTime fecha { get; set; }
        public Decimal importe { get; set; }

        public Deuda(string dniCliente, DateTime fecha, Decimal importe)
        {
            this.dniCliente = dniCliente;
            this.fecha = fecha;
            this.importe = importe;
        }
        public override string ToString() =>
        $"Cliente con dni: {dniCliente} a fecha de {fecha} debe {importe}";
    }

}
