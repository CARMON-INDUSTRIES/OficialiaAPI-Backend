using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OficialiaCrudAPI.Models
{
    public class FormularioAreaDestino
    {
        [Key]
        public int Id { get; set; }

        public int CorrespondenciaId { get; set; }

        [ForeignKey("CorrespondenciaId")]
        public Correspondencias Correspondencia { get; set; }

        public int AreaId { get; set; }

        [ForeignKey("AreaId")]
        public Area Area { get; set; }
    }
}