using System.ComponentModel.DataAnnotations;

namespace OficialiaCrudAPI.Models
{
    public class Usuario
    {
        [Key]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public int Semester { get; set; }

        public string Address { get; set; }
    }
}
