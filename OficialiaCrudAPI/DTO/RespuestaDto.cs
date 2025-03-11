namespace OficialiaCrudAPI.DTO
{
    public class RespuestaDto
    {
        public int IdRespuesta { get; set; }
        public string Mensaje { get; set; }
        public string? DocumentoRespuesta { get; set; }
        public int? RespuestaCorrecta { get; set; }
    }
}
