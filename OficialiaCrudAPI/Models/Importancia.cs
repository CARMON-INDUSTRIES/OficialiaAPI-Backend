using System.ComponentModel.DataAnnotations;

namespace OficialiaCrudAPI.Models
{
    public class Importancia
    {
        [Key]
        public int IdImportancia { get; set; }
        public string Nivel { get; set; }

        public ICollection<Correspondencias> Correspondencias { get; set; } = new List<Correspondencias>();

    }
}
