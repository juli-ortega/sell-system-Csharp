namespace Sell_System.AppWeb.Utilidades.Response
{
    public class GenericResponse<TObject>
    {
        public bool estado { get; set; }
        public string? mensaje { get; set; }
        public TObject? objeto { get; set; }
        public List<TObject>? listaObjeto { get; set; }
    }
}
