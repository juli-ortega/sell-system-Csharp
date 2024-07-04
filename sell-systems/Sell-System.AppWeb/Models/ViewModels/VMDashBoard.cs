namespace Sell_System.AppWeb.Models.ViewModels
{
    public class VMDashBoard
    {
        public int totalVentas{ get; set; }
        public string? totalIngresos { get; set; }
        public int totalProductos { get; set; }
        public int totalCategorias { get; set; }
        public List<VMVentasSemana> ventasUltimaSemana { get; set; }
        public List<VMProductosSemana> productosTopUltimaSemana { get; set; }
    }
}
