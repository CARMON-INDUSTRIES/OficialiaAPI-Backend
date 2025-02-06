using System.ComponentModel.DataAnnotations;

namespace OficialiaCrudAPI.Models
{
    public class Comunidades
    {
        [Key]
        public int IdComunidad { get; set; }
        public string NombreComunidad { get; set; }

        public ICollection<Correspondencias> Correspondencias { get; set; } = new List<Correspondencias>();
    }
}
