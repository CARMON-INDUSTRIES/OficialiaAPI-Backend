public class CorrespondenciaDto
{
    public int Id { get; set; }
    public int Folio { get; set; }
    public DateTime Fecha { get; set; }
    public string? Dependencia { get; set; }
    public string Asunto { get; set; }
    public string Remitente { get; set; }
    public string Destinatario { get; set; }
    public int Comunidad { get; set; }
    public string CargoRemitente { get; set; }
    public string CargoDestinatario { get; set; }
    public List<int> Area { get; set; }
    public string? AreaDescripcion { get; set; } 
    public string Documento { get; set; }
    public int Status { get; set; }
    public string? StatusDescripcion { get; set; }
    public int Importancia { get; set; }
    public string? ImportanciaDescripcion { get; set; }
    public string? ComunidadDescripcion { get; set; }
}
