using Microsoft.EntityFrameworkCore;
using OficialiaCrudAPI.Data;
using OficialiaCrudAPI.DTO;
using OficialiaCrudAPI.Models;

namespace OficialiaCrudAPI.Services
{
    public class CorrespondenciaService : ICorrespondenciaService
    {
        private readonly AppDataDbContext _context;

        public CorrespondenciaService(AppDataDbContext context)
        {
            _context = context;
        }

        public async Task<List<CorrespondenciaDto>> ObtenerCorrespondencias()
        {
            return await _context.Correspondencia
                .Select(c => new CorrespondenciaDto
                {
                    Folio = c.Folio,
                    Fecha = c.Fecha,
                    Dependencia = c.Dependencia,
                    Asunto = c.Asunto,
                    Remitente = c.Remitente,
                    Destinatario = c.Destinatario
                })
                .ToListAsync();
        }

        public async Task<bool> RegistrarCorrespondencia(CorrespondenciaDto correspondenciaDto)
        {
            // Verificar si todas las áreas existen
            foreach (var areaId in correspondenciaDto.Area)
            {
                var areaExiste = await _context.Area.AnyAsync(a => a.IdArea == areaId);
                if (!areaExiste)
                {
                    throw new Exception($"El área con id {areaId} no existe.");
                }
            }

            // Crear una nueva correspondencia para cada área
            foreach (var areaId in correspondenciaDto.Area)
            {
                var nuevaCorrespondencia = new Correspondencias
                {
                    Folio = correspondenciaDto.Folio,
                    Fecha = correspondenciaDto.Fecha,
                    Dependencia = correspondenciaDto.Dependencia,
                    Asunto = correspondenciaDto.Asunto,
                    Remitente = correspondenciaDto.Remitente,
                    Destinatario = correspondenciaDto.Destinatario,
                    Comunidad = correspondenciaDto.Comunidad,
                    CargoRemitente = correspondenciaDto.CargoRemitente,
                    CargoDestinatario = correspondenciaDto.CargoDestinatario,
                    Area = areaId,  // Asignamos el área correspondiente de la lista
                    Documento = correspondenciaDto.Documento,
                    Status = correspondenciaDto.Status,
                    Importancia = correspondenciaDto.Importancia
                };

                _context.Correspondencia.Add(nuevaCorrespondencia);
            }

            // Guardamos los cambios al contexto
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> EliminarCorrespondencia(int folio)
        {
            var correspondencia = await _context.Correspondencia
                .FirstOrDefaultAsync(c => c.Folio == folio);

            if (correspondencia == null)
            {
                return false;
            }

            _context.Correspondencia.Remove(correspondencia);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EditarCorrespondencia(CorrespondenciaDto correspondenciaDto)
        {
            var correspondencia = await _context.Correspondencia
                .FirstOrDefaultAsync(c => c.Folio == correspondenciaDto.Folio);
            if (correspondencia == null)
            {
                return false;
            }

            correspondencia.Fecha = correspondenciaDto.Fecha;
            correspondencia.Dependencia = correspondenciaDto.Dependencia;
            correspondencia.Asunto = correspondenciaDto.Asunto;
            correspondencia.Remitente = correspondenciaDto.Remitente;
            correspondencia.Destinatario = correspondenciaDto.Destinatario;
            correspondencia.Comunidad = correspondenciaDto.Comunidad;
            correspondencia.CargoRemitente = correspondenciaDto.CargoRemitente;
            correspondencia.CargoDestinatario = correspondenciaDto.CargoDestinatario;
            correspondencia.Area = correspondenciaDto.Area.FirstOrDefault(); // Se asigna solo el primer valor si se cambia a un único campo
            correspondencia.Documento = correspondenciaDto.Documento;
            correspondencia.Status = correspondenciaDto.Status;
            correspondencia.Importancia = correspondenciaDto.Importancia;

            await _context.SaveChangesAsync();
            return true;
        }

    }
}
