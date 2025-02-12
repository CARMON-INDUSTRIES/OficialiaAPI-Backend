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
                    Fecha = c.Fecha.ToString("dd/MM/yyyy"),
                    Dependencia = c.Dependencia,
                    Asunto = c.Asunto,
                    Remitente = c.Remitente,
                    Destinatario = c.Destinatario
                })
                .ToListAsync();
        }

        public async Task<bool> RegistrarCorrespondencia(CorrespondenciaDto correspondenciaDto)
        {
            // Verificar si el idArea existe
            var areaExiste = await _context.Area.AnyAsync(a => a.IdArea == correspondenciaDto.Area);
            if (!areaExiste)
            {
                throw new Exception($"El área con id {correspondenciaDto.Area} no existe.");
            }

            var nuevaCorrespondencia = new Correspondencias
            {
                Folio = correspondenciaDto.Folio,
                Fecha = DateTime.ParseExact(correspondenciaDto.Fecha, "dd/MM/yyyy", null),
                Dependencia = correspondenciaDto.Dependencia,
                Asunto = correspondenciaDto.Asunto,
                Remitente = correspondenciaDto.Remitente,
                Destinatario = correspondenciaDto.Destinatario,
                Comunidad = correspondenciaDto.Comunidad,
                CargoRemitente = correspondenciaDto.CargoRemitente,
                CargoDestinatario = correspondenciaDto.CargoDestinatario,
                Area = correspondenciaDto.Area,  
                Documento = correspondenciaDto.Documento,
                Status = correspondenciaDto.Status,
                Importancia = correspondenciaDto.Importancia
            };

            _context.Correspondencia.Add(nuevaCorrespondencia);
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

            correspondencia.Fecha = DateTime.ParseExact(correspondenciaDto.Fecha, "dd/MM/yyyy", null);
            correspondencia.Dependencia = correspondenciaDto.Dependencia;
            correspondencia.Asunto = correspondenciaDto.Asunto;
            correspondencia.Remitente = correspondenciaDto.Remitente;
            correspondencia.Destinatario = correspondenciaDto.Destinatario;
            correspondencia.Comunidad = correspondenciaDto.Comunidad;
            correspondencia.CargoRemitente = correspondenciaDto.CargoRemitente;
            correspondencia.CargoDestinatario = correspondenciaDto.CargoDestinatario;
            correspondencia.Area = correspondenciaDto.Area;
            correspondencia.Documento = correspondenciaDto.Documento;
            correspondencia.Status = correspondenciaDto.Status;
            correspondencia.Importancia = correspondenciaDto.Importancia;
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
