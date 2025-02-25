namespace OficialiaCrudAPI.Models
{
    public class FormularioAreaDestino
    {
        public int Id { get; set; }
        public int CorrespondenciaId { get; set; }
        public int AreaId { get; set; }

        public Correspondencias Correspondencia { get; set; }
        public Area Area { get; set; }
    }
}
