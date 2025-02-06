using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OficialiaCrudAPI.Models
{
    public class Correspondencias  
    {
        [Key]
        public int Id { get; set; }

        public int Folio { get; set; }

        public DateTime Fecha { get; set; }

        public string Dependencia { get; set; }

        public string Remitente { get; set; }

        public string CargoRemitente { get; set; }

        public string Asunto { get; set; }

        public string Destinatario { get; set; }

        public string CargoDestinatario { get; set; }

        public string Documento { get; set; }


        [ForeignKey("AreaNavigation")]
        public int Area { get; set; }

        public Area AreaNavigation { get; set; }

        [ForeignKey("ComunidadNavigation")]
        public int Comunidad { get; set; }

        public Comunidades ComunidadNavigation { get; set; }
    }
}
