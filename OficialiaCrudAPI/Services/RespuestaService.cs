using OficialiaCrudAPI.DTO;
using OficialiaCrudAPI.Interfaces;
using OficialiaCrudAPI.Models;
using OficialiaCrudAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace OficialiaCrudAPI.Services
{
    public class RespuestaService : IRespuesta
    {
        private readonly AppDataDbContext _context;

        public RespuestaService(AppDataDbContext context)
        {
            _context = context;
        }

        public async Task<List<RespuestaDto>> ObtenerRespuesta()
        {
            var respuestas = await _context.Respuesta
                .Select(r => new RespuestaDto
                {
                    IdRespuesta = r.IdRespuesta,
                    Mensaje = r.Mensaje,
                    DocumentoRespuesta = r.DocumentoRespuesta
                })
                .ToListAsync();

            return respuestas;
        }

        public async Task<RespuestaDto?> ObtenerRespuestaPorId(int id)
        {
            return await _context.Respuesta
                .Where(r => r.IdRespuesta == id)
                .Select(r => new RespuestaDto
                {
                    IdRespuesta = r.IdRespuesta,
                    Mensaje = r.Mensaje,
                    DocumentoRespuesta = r.DocumentoRespuesta,
                    
                })
                .FirstOrDefaultAsync();
        }


        public async Task<bool> CrearRespuesta(RespuestaDto respuestaDto)
        {
            var nuevaRespuesta = new Respuesta
            {
                Mensaje = respuestaDto.Mensaje,
                DocumentoRespuesta = respuestaDto.DocumentoRespuesta
            };

            _context.Respuesta.Add(nuevaRespuesta);
            await _context.SaveChangesAsync();

            // Si el DTO trae una correspondencia asociada, actualizarla
            if (respuestaDto.RespuestaCorrecta.HasValue)
            {
                var correspondencia = await _context.Correspondencia
                    .FirstOrDefaultAsync(c => c.Id == respuestaDto.RespuestaCorrecta);

                if (correspondencia != null)
                {
                    correspondencia.RespuestaCorrecta = nuevaRespuesta.IdRespuesta;
                    await _context.SaveChangesAsync();
                }
            }

            return true;
        }
    }
}
