using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pruebatecnica.Models;

namespace Pruebatecnica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampoEnFormulariosController : ControllerBase
    {
        private readonly prueba_tecnicaContext _context;

        public CampoEnFormulariosController(prueba_tecnicaContext context)
        {
            _context = context;
        }

        // GET: api/CampoEnFormularios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampoEnFormulario>>> GetCampoEnFormularios()
        {
          if (_context.CampoEnFormularios == null)
          {
              return NotFound();
          }
            return await _context.CampoEnFormularios.ToListAsync();
        }

        // GET: api/CampoEnFormularios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CampoEnFormulario>> GetCampoEnFormulario(int id)
        {
          if (_context.CampoEnFormularios == null)
          {
              return NotFound();
          }
            var campoEnFormulario = await _context.CampoEnFormularios.FindAsync(id);

            if (campoEnFormulario == null)
            {
                return NotFound();
            }

            return campoEnFormulario;
        }

        // PUT: api/CampoEnFormularios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCampoEnFormulario(int id, CampoEnFormulario campoEnFormulario)
        {
            if (id != campoEnFormulario.Id)
            {
                return BadRequest();
            }

            _context.Entry(campoEnFormulario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CampoEnFormularioExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/CampoEnFormularios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CampoEnFormulario>> PostCampoEnFormulario(CampoEnFormulario campoEnFormulario)
        {
          if (_context.CampoEnFormularios == null)
          {
              return Problem("Entity set 'prueba_tecnicaContext.CampoEnFormularios'  is null.");
          }
            _context.CampoEnFormularios.Add(campoEnFormulario);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CampoEnFormularioExists(campoEnFormulario.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCampoEnFormulario", new { id = campoEnFormulario.Id }, campoEnFormulario);
        }

        [HttpPost("llenar/{link}")]
        public async Task<ActionResult> LlenarFormulario(string link, [FromBody] List<Respuesta> respuestas)
        {
            // Buscar el formulario por el enlace proporcionado
            var formulario = await _context.Formularios.FirstOrDefaultAsync(f => f.LinkFormulario == link);

            // Si el formulario no existe, devolver un error 404
            if (formulario == null)
            {
                return NotFound("No se encontró el formulario con el enlace proporcionado.");
            }

            // Obtener los campos relacionados al formulario
            var campos = await _context.Campos.Where(c => c.LinkFormulario == link).ToListAsync();

            // Agregar las respuestas a la base de datos
            foreach (var respuesta in respuestas)
            {
                // Buscar el campo correspondiente a la respuesta
                var campo = campos.FirstOrDefault(c => c.NombreCampo == respuesta.campo);

                // Si el campo existe, agregar la respuesta a la base de datos
                if (campo != null)
                {
                    var campoEnFormulario = new CampoEnFormulario
                    {
                        LinkFormulario = link,
                        IdCampo = campo.Id,
                        Valor = respuesta.respuesta
                    };

                    // Agregar el campo en el formulario al contexto
                    _context.CampoEnFormularios.Add(campoEnFormulario);
                }
            }

            // Guardar los cambios en la base de datos
            await _context.SaveChangesAsync();

            // Devolver un mensaje de éxito
            return Ok("Encuesta llenada exitosamente.");
        }



        // DELETE: api/CampoEnFormularios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCampoEnFormulario(int id)
        {
            if (_context.CampoEnFormularios == null)
            {
                return NotFound();
            }
            var campoEnFormulario = await _context.CampoEnFormularios.FindAsync(id);
            if (campoEnFormulario == null)
            {
                return NotFound();
            }

            _context.CampoEnFormularios.Remove(campoEnFormulario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CampoEnFormularioExists(int id)
        {
            return (_context.CampoEnFormularios?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
