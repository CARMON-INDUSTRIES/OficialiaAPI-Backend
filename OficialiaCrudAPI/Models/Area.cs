using System.ComponentModel.DataAnnotations;

namespace OficialiaCrudAPI.Models
{
    public class Area
    {
        [Key]
        public int IdArea { get; set; }
        public string Nombre { get; set; }

        // Relación inversa: un área puede tener muchas correspondencias
        public ICollection<Correspondencias> Correspondencias { get; set; } = new List<Correspondencias>();
    }
}
