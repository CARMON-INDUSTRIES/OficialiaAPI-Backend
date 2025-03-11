using OficialiaCrudAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Respuesta
{
    [Key]
    public int IdRespuesta { get; set; }
    public string Mensaje { get; set; }
    public string DocumentoRespuesta { get; set; }
   
}
