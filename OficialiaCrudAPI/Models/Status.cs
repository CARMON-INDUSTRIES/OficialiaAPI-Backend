using System.ComponentModel.DataAnnotations;

namespace OficialiaCrudAPI.Models
{
    public class Status
    {
        [Key]
        public int IdStatus { get; set; }
        public string Estado { get; set; }

        public ICollection<Correspondencias> Correspondencias { get; set; } = new List<Correspondencias>();

    }
}
