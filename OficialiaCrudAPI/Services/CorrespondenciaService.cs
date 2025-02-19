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
                .Include(c => c.AreaNavigation)  
                .Include(c => c.ImportanciaNavigation)
                .Include(c => c.StatusNavigation)
                .Select(c => new CorrespondenciaDto
                {
                    Id = c.Id,
                    Folio = c.Folio,
                    Fecha = c.Fecha,
                    Dependencia = c.Dependencia,
                    Asunto = c.Asunto,
                    Remitente = c.Remitente,
                    Destinatario = c.Destinatario,
                    Comunidad = c.Comunidad,
                    CargoRemitente = c.CargoRemitente,
                    CargoDestinatario = c.CargoDestinatario,
                    Documento = c.Documento,
                    Status = c.Status,
                    Importancia = c.Importancia,
                    Area = new List<int> { c.Area },
                    AreaDescripcion = c.AreaNavigation.NombreArea,  
                    StatusDescripcion = c.StatusNavigation.Estado,
                    ComunidadDescripcion = c.ComunidadNavigation.NombreComunidad,
                    ImportanciaDescripcion = c.ImportanciaNavigation.Nivel
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
                    Id = correspondenciaDto.Id,
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

        public async Task<bool> EliminarCorrespondencia(int id)
        {
            var correspondencia = await _context.Correspondencia
                .FirstOrDefaultAsync(c => c.Id == id);

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
                .FirstOrDefaultAsync(c => c.Id == correspondenciaDto.Id);
            if (correspondencia == null)
            {
                return false;
            }

            correspondencia.Id = correspondenciaDto.Id;
            correspondencia.Folio = correspondenciaDto.Folio;
            correspondencia.Fecha = correspondenciaDto.Fecha;
            correspondencia.Dependencia = correspondenciaDto.Dependencia;
            correspondencia.Asunto = correspondenciaDto.Asunto;
            correspondencia.Remitente = correspondenciaDto.Remitente;
            correspondencia.Destinatario = correspondenciaDto.Destinatario;
            correspondencia.Comunidad = correspondenciaDto.Comunidad;
            correspondencia.CargoRemitente = correspondenciaDto.CargoRemitente;
            correspondencia.CargoDestinatario = correspondenciaDto.CargoDestinatario;
            correspondencia.Area = correspondenciaDto.Area.FirstOrDefault(); 
            correspondencia.Documento = correspondenciaDto.Documento;
            correspondencia.Status = correspondenciaDto.Status;
            correspondencia.Importancia = correspondenciaDto.Importancia;

            await _context.SaveChangesAsync();
            return true;
        }

    }
}
