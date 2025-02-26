using System.ComponentModel.DataAnnotations;

namespace OficialiaCrudAPI.Models
{
    public class FormularioAreaDestino
    {
        [Key]
        public int Id { get; set; }
        public string CorrespondenciaId { get; set; }
        public int AreaId { get; set; }

        public Correspondencias Correspondencia { get; set; }
        public Area Area { get; set; }
    }
}
