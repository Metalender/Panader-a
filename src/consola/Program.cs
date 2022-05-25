using datos;
using sistema;
using consola;




var RePoC = new ClientesCSV();
var RePoP = new PedidosCSV();
var RepoPanPedido = new PanesPedidosCSV();
var RepoDe = new DeudasCSV();
var view = new Vista();
var sistema = new Gestor(RePoC, RePoP, RepoPanPedido, RepoDe);
var controlador = new Controlador(view, sistema);
controlador.Run();

