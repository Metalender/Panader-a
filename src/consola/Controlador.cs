using modelos;
using sistema;
using System.Globalization;

namespace consola
{
    class Controlador
    {
        private View vista;
        private Gestor sistema;
        private Dictionary<string, Action> casosdeUso;
        private Dictionary<string, Action> gestionPedidos;
        private Dictionary<string, Action> gestionClientes;
        private Dictionary<string, Action> verClientes;
        private Dictionary<string, Action> gestionFinanzas;
        private Dictionary<string, Action> marcarPedidos;
        private Dictionary<string, Action> validarPedidos;



        public Controlador(Vista vista, Gestor logicaNegocio)
        {
            vista = vista;
            sistema = logicaNegocio;
            casosdeUso = new Dictionary<string, Action>()
            {
                {"Gestión de Pedidos",gestionPedidos},
                {"Gestion de Clientes",gestionClientes},
                {"Gestion de Finanzas",gestionFinanzas},
                {"Salir y Cerrar aplicación",salir}
            };
        }

        public void Run()
        {
            vista.LimpiarPantalla();
            var menu = casosdeUso.Keys.ToList<string>();

            while (true)
                try
                {
                    vista.LimpiarPantalla();
                    var key = vista.TryObtenerElementoDeLista("Panadería Biskôte", menu, "Selecciona una opción ");
                    vista.Mostrar("");
                    casosdeUso[key].Invoke();
                    vista.MostrarYReturn("Pulsa <Return> para continuar");
                }
                catch { return; }
        }
        public void salir()
        {
            var key = "fin";
            vista.Mostrar("Gracias\n\nHasta la próxima!!\n\n");

            casosdeUso[key].Invoke();
        }
        public void volverAtras() { }


        private void gestionPedidos()
        {
            gestionPedidos = new Dictionary<string, Action>()
            {
                {"Ver Pedidos",verPedidos},
                {"Marcar pedido como pagado",marcarPedidoDiaSiguiente},
                {"Validar pedido para el dia siguiente",validarPedidoDiaSiguiente},
                {"Añadir Pedido",aniadirPedido},
                {"Cambiar Pedido",cambiarPedido},
                {"Borrar Pedido",borrarPedidio},
                {"Volver atras",volverAtras}
            };
            var menuPedidos = gestionPedidos.Keys.ToList<string>();
            try
            {
                vista.LimpiarPantalla();
                var key = vista.TryObtenerElementoDeLista("Gestión de Pedidos", menuPedidos, "Selecciona una opción ");
                vista.Mostrar("");
                gestionPedidos[key].Invoke();

            }
            catch { return; }
        }
        private void verPedidos()
        {
            foreach (Pedido i in sistema.misPedidos)
            {
                vista.Mostrar(i.ToString());
                foreach (PanesPedido j in i.listaDePan)
                {
                    vista.Mostrar("          " + j.ToString());

                }
            }
            vista.Mostrar("\n");
        }
        public void marcarPedidoDiaSiguiente()
        {
            marcarPedidos = new Dictionary<string, Action>()
            {
                {"Marcar un pedido del dia como pagado",marcarPedidoPagado},
                {"Marcar todos los pedidos del dia como pagados",marcarAPagadoTodos},
                {"Volver atras",volverAtras}
            };
            var menuMarcar = marcarPedidos.Keys.ToList<String>();
            try
            {
                vista.LimpiarPantalla();
                var key = vista.TryObtenerElementoDeLista("Opciones para Pedido", menuMarcar, "Selecciona una opción ");
                vista.Mostrar("");
                marcarPedidos[key].Invoke();

            }
            catch { return; }
        }

        public void marcarPedidoPagado()
        {
            Pedido nuevo = vista.TryObtenerElementoDeLista("Lista de Pedidos", sistema.misPedidos, "Seleciona una pedido");
            if (nuevo.estado.ToString().Equals(estadoPedido.pagado.ToString()))
            {
                vista.Mostrar("\nEste pedido ya esta pagado!!\nPorfavor, selecciona otro", ConsoleColor.Red);
            }
            else
            {
                nuevo.estado = estadoPedido.pagado;
                vista.Mostrar("\n\nPedido actualizado.\n", ConsoleColor.DarkYellow);
                sistema.actualizarMisPedidosConPedidoActualizado();
            }

        }
        public void marcarAPagadoTodos()
        {
            sistema.marcarAPagadoTodos();
            vista.Mostrar("\n\nPedidos actualizados. \n", ConsoleColor.DarkYellow);
        }

        public void validarPedidoDiaSiguiente()
        {
            validarPedidos = new Dictionary<string, Action>()
            {
                {"Validar un pedido para el dia siguiente",cambiarFechaPedido},
                {"Validar todos los pedidos para el dia siguiente",cambiarFechasPedidos},
                {"Volver atras",volverAtras}
            };
            var menuValidar = validarPedidos.Keys.ToList<String>();
            try
            {
                vista.LimpiarPantalla();
                var key = vista.TryObtenerElementoDeLista("Opciones para Pedido", menuValidar, "Selecciona una opción ");
                vista.Mostrar("");
                validarPedidos[key].Invoke();

            }
            catch { return; }
        }
        public void cambiarFechaPedido()
        {
            Pedido nuevo = vista.TryObtenerElementoDeLista("Lista de Pedidos", sistema.misPedidos, "Seleciona una pedido");
            if (nuevo.fecha.ToShortDateString().Equals(sistema.undiaMas(DateTime.Today).ToShortDateString()))
            {
                vista.Mostrar("\nEste pedido ya es para mañana!!\nPorfavor, selecciona otro", ConsoleColor.Red);
            }
            else
            {
                nuevo.fecha = sistema.undiaMas(DateTime.Today);
                sistema.actualizarMisPedidosConPedidoActualizado();
                vista.Mostrar("\n\nPedido actualizado para el dia siguiente.\n", ConsoleColor.DarkYellow);
            }


        }
        public void cambiarFechasPedidos()
        {
            sistema.cambiarFechasPedidos();
            vista.Mostrar("\n\nPedidos actualizados para el dia siguiente.\n", ConsoleColor.DarkYellow);
        }

        private void aniadirPedido()
        {
            try
            {
                var dniCli = vista.TryObtenerDatoDeTipo<string>("Introduzca dni");
                if (sistema.misClientes.Find(cliente => dniCli.Equals(cliente.dni)) == null)
                {
                    vista.Mostrar("\nEl dni no figura en el sistema\nPorfavor pruebe denuevo\nSi no esta registrado el dni, registre a nuevo cliente.", ConsoleColor.Red);
                }
                else
                {
                    if (sistema.clienteTienePedidoHabitual(dniCli))
                    {
                        vista.Mostrar("\nYa hay un pedido de tipo Habitual registrado con este dni.\nSi quiere continuar, el pedido deberá ser de tipo ocasional o si desea modificar el existente, acceda a cambiar pedido.\n", ConsoleColor.Red);
                        var salir = vista.TryObtenerDatoDeTipo<string>("Quiere continuar con un pedido de tipo Ocasional?? ( S/N )");
                        if (salir.Equals("s", StringComparison.InvariantCultureIgnoreCase))
                        {
                            Dictionary<Pan, int> panParaLista = new Dictionary<Pan, int>();
                            Pan panNuevo;
                            int cantidad;
                            string fuera = "";
                            while (true)
                            {
                                vista.LimpiarPantalla();
                                try
                                {
                                    panNuevo = vista.TryObtenerElementoDeLista("Tipos de Pan", sistema.misProductos, "Seleciona un Pan");
                                    cantidad = vista.TryObtenerDatoDeTipo<int>("Introduzca cantidad de unidades del pan seleccionado");
                                    panParaLista.Add(panNuevo, cantidad);
                                }
                                catch { vista.Mostrar("\nYa se ha introducido datos para este tipo de pan\n", ConsoleColor.Red); }
                                fuera = vista.TryObtenerDatoDeTipo<string>("Has terminado?? ( S/N )");
                                if (fuera.Equals("s", StringComparison.InvariantCultureIgnoreCase))
                                    break;
                            }
                            var ID = Guid.NewGuid();
                            var fecha = sistema.undiaMas(DateTime.Today);
                            var precio = sistema.calcularPrecioPedido(panParaLista);
                            var estado = estadoPedido.pendiente;
                            var tipoPedido = tipoDePedido.Ocasional;
                            Pedido nuevo = new Pedido
                            (
                                ID: ID,
                                dniCliente: dniCli,
                                fecha: fecha.Date,
                                precioPedido: precio,
                                estado: estado,
                                tipoPedido: tipoPedido
                            );
                            sistema.nuevoPedido(nuevo, panParaLista);
                            vista.Mostrar("\n\nNuevo pedido registrado.\n", ConsoleColor.DarkYellow);
                        }
                        else { volverAtras(); }
                    }
                    else
                    {
                        Dictionary<Pan, int> panParaLista = new Dictionary<Pan, int>();
                        Pan panNuevo;
                        int cantidad;
                        string fuera = "";
                        while (true)
                        {
                            vista.LimpiarPantalla();
                            try
                            {
                                panNuevo = vista.TryObtenerElementoDeLista("Tipos de Pan", sistema.misProductos, "Seleciona un Pan");
                                cantidad = vista.TryObtenerDatoDeTipo<int>("Introduzca cantidad de unidades del pan seleccionado");
                                panParaLista.Add(panNuevo, cantidad);
                            }
                            catch { vista.Mostrar("\nYa se ha introducido datos para este tipo de pan\n", ConsoleColor.Red); }
                            fuera = vista.TryObtenerDatoDeTipo<string>("Has terminado?? ( S/N )");
                            if (fuera.Equals("s", StringComparison.InvariantCultureIgnoreCase))
                                break;
                        }
                        var tipoPedido = vista.TryObtenerDatoDeTipo<tipoDePedido>("\nQue tipo de pedido quieres: Habitual ó Ocasional\n");
                        if (tipoPedido == tipoDePedido.Habitual && sistema.clienteTienePedidoHabitual(dniCli))
                        {
                            vista.Mostrar("\nYa tienes un pedido Habitual!!!\nSi deseas otro pedido ha de ser Ocasional\n", ConsoleColor.Red);
                        }
                        else
                        {

                            var ID = Guid.NewGuid();
                            var fecha = sistema.undiaMas(DateTime.Today);
                            var precio = sistema.calcularPrecioPedido(panParaLista);
                            var estado = estadoPedido.pendiente;
                            Pedido nuevo = new Pedido
                            (
                                ID: ID,
                                dniCliente: dniCli,
                                fecha: fecha.Date,
                                precioPedido: precio,
                                estado: estado,
                                tipoPedido: tipoPedido
                            );
                            sistema.nuevoPedido(nuevo, panParaLista);
                            vista.Mostrar("\n\nNuevo pedido registrado.\n", ConsoleColor.DarkYellow);
                        }

                    }
                }

            }
            catch { return; }
        }
        private void cambiarPedido()
        {
            try
            {
  
                Pedido nuevo = vista.TryObtenerElementoDeLista<Pedido>("Lista de Pedidos", sistema.misPedidos, "Selecciona un pedido que quieras cambiar");
                Dictionary<Pan, int> panParaLista = new Dictionary<Pan, int>();
                Pan panNuevo;
                int cantidad;
                string fuera = "";
                while (true)
                {
                    vista.LimpiarPantalla();
                    try
                    {
                        panNuevo = vista.TryObtenerElementoDeLista("Tipos de Pan", sistema.misProductos, "Seleciona un Pan");
                        cantidad = vista.TryObtenerDatoDeTipo<int>("Introduzca cantidad de unidades del pan seleccionado");
                        panParaLista.Add(panNuevo, cantidad);
                    }
                    catch { vista.Mostrar("\nYa se ha introducido datos para este tipo de pan\n", ConsoleColor.Red); }
                    fuera = vista.TryObtenerDatoDeTipo<string>("Has terminado?? ( S/N )");
                    if (fuera.Equals("s", StringComparison.InvariantCultureIgnoreCase))
                        break;
                }
                var tipoPedido = vista.TryObtenerDatoDeTipo<tipoDePedido>("\nQue tipo de pedido quieres: Habitual ó Ocasional\n");
                if (tipoPedido == tipoDePedido.Habitual && sistema.clienteTienePedidoHabitual(nuevo.dniCliente))
                {
                    vista.Mostrar("\nYa tienes un pedido Habitual!!!\nSi deseas otro pedido ha de ser Ocasional\n", ConsoleColor.Red);
                }
                else
                {

                    var ID = Guid.NewGuid();
                    var fecha = sistema.undiaMas(DateTime.Today);
                    var precio = sistema.calcularPrecioPedido(panParaLista);
                    var estado = estadoPedido.pendiente;
                    Pedido otro = new Pedido
                        (
                            ID: ID,
                            dniCliente: nuevo.dniCliente,
                            fecha: fecha.Date,
                            precioPedido: precio,
                            estado: estado,
                            tipoPedido: tipoPedido
                        );

                    sistema.nuevoPedido(otro, panParaLista);
                    sistema.borrarPedido(nuevo);

                    vista.Mostrar("\n\nPedido actualizado\n", ConsoleColor.DarkYellow);
                }
            }
            catch { return; }
        }
        private void borrarPedidio()
        {
            Pedido nuevo = vista.TryObtenerElementoDeLista<Pedido>("Pedidos registrados", sistema.misPedidos, "Selecciona un pedido");
            sistema.borrarPedido(nuevo);
            vista.Mostrar("\n\nPedido borrado\n", ConsoleColor.DarkYellow);
        }




        private void gestionClientes()
        {
            gestionClientes = new Dictionary<string, Action>()
            {
                {"Añadir Cliente nuevo",aniadirCliente},
                {"Borrar Cliente",borrarCliente},
                {"Ver Cliente",verCliente},
                {"Volver atras",volverAtras}
            };
            var menuClientes = gestionClientes.Keys.ToList<string>();
            try
            {
                vista.LimpiarPantalla();
                var key = vista.TryObtenerElementoDeLista("Gestión de Clientes", menuClientes, "Selecciona una opción ");
                vista.Mostrar("");
                gestionClientes[key].Invoke();
            }
            catch { return; }
        }
        public void aniadirCliente()
        {
            try
            {
                while(true){
                var nombre = vista.TryObtenerDatoDeTipo<string>("Nombre del cliente");
                var apellido = vista.TryObtenerDatoDeTipo<string>("Apellido del cliente");
                var dni = vista.TryObtenerDatoDeTipo<string>("DNI del cliente");
                var telefono = vista.TryObtenerDatoDeTipo<string>("Telefono del cliente");
                var pueblo = vista.TryObtenerDatoDeTipo<string>("Nombre del pueblo");
                if(nombre.Equals("")||apellido.Equals("")||dni.Equals("")||telefono.Equals("")||pueblo.Equals(""))
                {
                    vista.Mostrar("\nNo puedes dejar ningun campo sin rellenar\nPorfavor vuelve a intentarlo.\n",ConsoleColor.Red);
                }else
                {
                    Cliente nuevo = new Cliente
                (
                    nombre: nombre,
                    apellido: apellido,
                    dni: dni,
                    telefono: telefono,
                    pueblo: pueblo
                );   
                    sistema.nuevoCliente(nuevo);                 
                    break;
                }
                }    
                
            }
            catch { return; }
            finally
            {
                vista.Mostrar("Nuevo Cliente añadido.\nYa puede hacer su pedido.\n\n",ConsoleColor.DarkYellow);
            }
        }
        public void borrarCliente()
        {
            Cliente c;
            c = vista.TryObtenerElementoDeLista("Clientes registrados", sistema.misClientes, "Selecciona un cliente para borrar sus datos");
            sistema.borrarCliente(c);

        }
        public void verCliente()
        {
            verClientes = new Dictionary<string, Action>()
            {
                {"Ver datos de clientes",verDatosPorCliente},
                {"Ver clientes con precio del pedido diario",verDeudasDiariasPorCliente},
                {"Ver clientes con deudas pendientes",verTotalDeudasPorCliente},
                {"Ver pedidos por cliente",verPedidosPorClientes},
                {"Volver atras",volverAtras}
            };
            var menuClientes2 = verClientes.Keys.ToList<String>();
            try
            {
                vista.LimpiarPantalla();
                var key = vista.TryObtenerElementoDeLista("Clientes registrados", menuClientes2, "Selecciona una opción");
                vista.Mostrar("");
                verClientes[key].Invoke();

            }
            catch { return; }

        }
        public void verDatosPorCliente()
        {
            vista.MostrarListaEnumerada<Cliente>("Lista de Clientes", sistema.misClientes);

        }
        public void verDeudasDiariasPorCliente()
        {
            List<string> lista = new List<string>();
            foreach (Cliente i in sistema.misClientes)
            {
                if (sistema.pedidoDeCliente(i) != null && sistema.pedidoDeCliente(i).estado == estadoPedido.pendiente)
                {
                    lista.Add(i.verClientesConPedido() + " - Deuda a pagar: " + sistema.pedidoDeCliente(i).precioPedido + " \u20AC ");
                }
                else if (sistema.pedidoDeCliente(i) != null)
                {
                    lista.Add(i.verClientesConPedido() + " - Deuda a pagar: 0  \u20AC");
                }

            }
            vista.MostrarListaEnumerada<string>("Lista de Clientes con sus Deudas diarias", lista);

        }
        public void verTotalDeudasPorCliente()
        {
            List<string> lista = new List<string>();
            foreach (Cliente i in sistema.misClientes)
            {
                if (sistema.pedidoDeCliente(i) != null && sistema.pedidoDeCliente(i).estado == estadoPedido.pendiente)
                {
                    lista.Add(i.verClientesConPedido() + " - Deuda a pagar: " + (sistema.asignarDeudaSPorCliente2(i) + sistema.pedidoDeCliente(i).precioPedido) + " \u20AC ");/**/
                }
                else if (sistema.pedidoDeCliente(i) != null && sistema.pedidoDeCliente(i).estado == estadoPedido.pagado)
                {
                    lista.Add(i.verClientesConPedido() + " - Deuda a pagar: " + sistema.asignarDeudaSPorCliente2(i) + " \u20AC");
                }
                else if (sistema.pedidoDeCliente(i) != null)
                {
                    lista.Add(i.verClientesConPedido() + " - Deuda a pagar: 0 \u20AC");
                }

            }
            vista.MostrarListaEnumerada<string>("Lista de Clientes con sus Deudas", lista);

        }

        public void verPedidosPorClientes()
        {
            Cliente nuevo;
            nuevo = vista.TryObtenerElementoDeLista("Clientes", sistema.misClientes, "Selecciona un cliente");
            vista.Mostrar("\nCliente", ConsoleColor.DarkYellow);
            vista.Mostrar(nuevo.ToString());
            foreach (Pedido i in sistema.pedidosDeCliente(nuevo))
            {
                vista.Mostrar("\nPedido: ", ConsoleColor.DarkYellow);
                vista.Mostrar(i.stringParaVerCliente() + "\n");
                vista.Mostrar("Lista de panes\n", ConsoleColor.DarkYellow);

                foreach (PanesPedido j in i.listaDePan)
                {
                    vista.Mostrar(j.ToString() + "\n");
                }
                vista.Mostrar("----------------------------------------------\n");

            }

        }


        private void gestionFinanzas()
        {
            gestionFinanzas = new Dictionary<string, Action>()
            {
                {"Liquidar deudas por cliente",liquidarDeudasPorCliente},
                {"Ver deudas diarias por cliente",verDeudasDiariasPorCliente},
                {"Ver deudas pendientes Totales por cliente",verTotalDeudasPorCliente},
                {"Volver atras",volverAtras}

            };
            var menuFinanzas = gestionFinanzas.Keys.ToList<String>();
            try
            {
                vista.LimpiarPantalla();
                var key = vista.TryObtenerElementoDeLista("Gestion de Finanzas", menuFinanzas, "Selecciona una opción");
                vista.Mostrar("");
                gestionFinanzas[key].Invoke();

            }
            catch { return; }
        }
        public void liquidarDeudasPorCliente()
        {
            Decimal dineroAPagar = 0;
            Cliente nuevo = vista.TryObtenerElementoDeLista<Cliente>("Clientes de la Panaderia", sistema.misClientes, "Selecciona un cliente");

            if (nuevo.deudasPendientes > 0)
            {
                dineroAPagar = nuevo.deudasPendientes;
                if (sistema.pedidoDeCliente(nuevo).estado == estadoPedido.pendiente)
                {
                    var esto = vista.TryObtenerDatoDeTipo<string>("\nQuieres añadir el pedido de hoy a la suma del dinero pendiente?? S/N\n");
                    if (esto.Equals("s", StringComparison.InvariantCultureIgnoreCase))
                    {
                        dineroAPagar += sistema.pedidoDeCliente(nuevo).precioPedido;
                        sistema.pedidoDeCliente(nuevo).estado = estadoPedido.pagado;

                    }

                }
                vista.Mostrar("\nTotal a pagar " + dineroAPagar.ToString(CultureInfo.InvariantCulture) + "\n", ConsoleColor.DarkYellow);
                sistema.borrarDeudas(nuevo);
                sistema.actualizarMisDeudasConPedidoActualizado();
                sistema.actualizarMisPedidosConPedidoActualizado();
                vista.Mostrar("Deudas liquidadas.\nGracias.");

            }
            else
            {
                vista.Mostrar("\nEste cliente no tiene deudas pendientes\nVe a gestion de clientes/ver clientes/ver clientes con deudas pendientes,\no Ve a gestion finanzas/Ver deudas pendientes Totales por cliente\nPara ver quienes tienen deudas pendientes.\nGracias\n", ConsoleColor.Red);
            }


        }

    }

}