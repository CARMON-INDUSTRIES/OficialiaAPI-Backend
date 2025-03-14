using Microsoft.EntityFrameworkCore;
using OficialiaCrudAPI.Data;
using OficialiaCrudAPI.DTO;
using OficialiaCrudAPI.Interfaces;
using OficialiaCrudAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OficialiaCrudAPI.Services
{
    public class CorrespondenciaService : ICorrespondenciaService
    {
        private readonly AppDataDbContext _context;
        private readonly IUsuarioService _usuarioService;

        public CorrespondenciaService(AppDataDbContext context, IUsuarioService usuarioService)
        {
            _context = context;
            _usuarioService = usuarioService;
        }

        public async Task<List<CorrespondenciaDto>> ObtenerCorrespondencias(string userId)
        {
            // Obtener el AreaId del usuario
            var usuarioArea = await _usuarioService.ObtenerUsuarioArea(userId);
            if (usuarioArea == null)
            {
                return new List<CorrespondenciaDto>(); // Usuario sin área asignada, retorna lista vacía
            }

            int areaId = usuarioArea.AreaId;

            // Filtrar correspondencias asignadas a esa área
            return await _context.Correspondencia
                .Where(c => c.Area == areaId) // Filtrar solo las del área del usuario
                .Include(c => c.AreaNavigation)
                .Include(c => c.ComunidadNavigation)
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
                    RespuestaCorrecta = c.RespuestaCorrecta,
                    FechaTerminacion = c.FechaTerminacion,
                    Respuesta = c.Respuesta,
                    Area = new List<int> { c.Area }, // Adaptado a la relación 1:1
                    AreaDescripcion = c.AreaNavigation.NombreArea,
                    StatusDescripcion = c.StatusNavigation.Estado,
                    ComunidadDescripcion = c.ComunidadNavigation.NombreComunidad,
                    ImportanciaDescripcion = c.ImportanciaNavigation.Nivel
                })
                .ToListAsync();
        }

        public async Task<List<FormularioAreaDestinoDto>> ObtenerAsignaciones()
        {
            return await _context.FormularioAreaDestino
                .Select(fa => new FormularioAreaDestinoDto
                {
                    Id = fa.Id,
                    CorrespondenciaId = fa.CorrespondenciaId,
                    AreaId = fa.AreaId
                })
                .ToListAsync();
        }

        public async Task<bool> RegistrarCorrespondencia(CorrespondenciaDto correspondenciaDto)
        {
            var areaExiste = await _context.Area.AnyAsync(a => correspondenciaDto.Area.Contains(a.IdArea));
            if (!areaExiste)
            {
                throw new Exception($"Una de las áreas proporcionadas no existe.");
            }

            var correspondencias = new List<Correspondencias>();

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
                    Documento = correspondenciaDto.Documento,
                    Status = correspondenciaDto.Status,
                    Importancia = correspondenciaDto.Importancia,
                    RespuestaCorrecta = correspondenciaDto.RespuestaCorrecta,
                    FechaTerminacion = correspondenciaDto.FechaTerminacion,
                    Respuesta = correspondenciaDto.Respuesta,
                    Area = areaId // Asignación del área correspondiente en cada ciclo
                };

                correspondencias.Add(nuevaCorrespondencia);
            }

            _context.Correspondencia.AddRange(correspondencias);
            return await _context.SaveChangesAsync() > 0;
        }


        public async Task<bool> EliminarCorrespondencia(int id)
        {
            var correspondencia = await _context.Correspondencia.FindAsync(id);
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
            var correspondencia = await _context.Correspondencia.FindAsync(correspondenciaDto.Id);
            if (correspondencia == null)
            {
                return false;
            }

            correspondencia.Folio = correspondenciaDto.Folio;
            correspondencia.Fecha = correspondenciaDto.Fecha;
            correspondencia.Dependencia = correspondenciaDto.Dependencia;
            correspondencia.Asunto = correspondenciaDto.Asunto;
            correspondencia.Remitente = correspondenciaDto.Remitente;
            correspondencia.Destinatario = correspondenciaDto.Destinatario;
            correspondencia.Comunidad = correspondenciaDto.Comunidad;
            correspondencia.CargoRemitente = correspondenciaDto.CargoRemitente;
            correspondencia.CargoDestinatario = correspondenciaDto.CargoDestinatario;
            correspondencia.Documento = correspondenciaDto.Documento;
            correspondencia.Status = correspondenciaDto.Status;
            correspondencia.Importancia = correspondenciaDto.Importancia;
            correspondencia.RespuestaCorrecta = correspondenciaDto.RespuestaCorrecta;
            correspondencia.FechaTerminacion = correspondenciaDto.FechaTerminacion;
            correspondencia.Respuesta = correspondenciaDto.Respuesta;
            correspondencia.Area = correspondenciaDto.Area.FirstOrDefault(); // Actualizar el área

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ActualizarEstadoCorrespondencia(int id, int nuevoStatus)
        {
            var correspondencia = await _context.Correspondencia.FindAsync(id);
            if (correspondencia == null)
            {
                return false; // No se encontró la correspondencia
            }

            correspondencia.Status = nuevoStatus;
            _context.Correspondencia.Update(correspondencia);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<CorrespondenciaDto>> ObtenerTodasLasCorrespondencias()
        {
            var correspondencias = await _context.Correspondencia
                .Include(c => c.AreaNavigation)
                .Include(c => c.ComunidadNavigation)
                .Include(c => c.ImportanciaNavigation)
                .Include(c => c.StatusNavigation)
                .ToListAsync();

            return correspondencias.Select(c => new CorrespondenciaDto
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
                RespuestaCorrecta = c.RespuestaCorrecta,
                FechaTerminacion = c.FechaTerminacion,
                Respuesta = c.Respuesta,
                Area = new List<int> { c.Area }, // Adaptado a la relación 1:1
                AreaDescripcion = c.AreaNavigation.NombreArea,
                StatusDescripcion = c.StatusNavigation.Estado,
                ComunidadDescripcion = c.ComunidadNavigation.NombreComunidad,
                ImportanciaDescripcion = c.ImportanciaNavigation.Nivel
            }).ToList();
        }


        public async Task<int> ObtenerNuevasCorrespondencias(DateTime ultimaFecha)
        {
            return await _context.Correspondencia
                .Where(c => c.Fecha > ultimaFecha)
                .CountAsync();
        }
    }
}
