using API.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers{


    [ApiController]
    [Route("[controller]")]
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class InmuebleController : ControllerBase{


        private readonly DataContext contexto;

        public InmuebleController(DataContext contexto)
		{
			this.contexto = contexto;
		}


		
        [HttpGet("ObtenerInmuebles")]
		public async Task<IActionResult> Obtener()
		{
			try
			{
				var usuarioActual = User.Identity.Name;
				var inmuebles = await contexto.Inmuebles
											  .Include(e => e.Propietario)
											  .Where(e => e.Propietario.Mail == usuarioActual)
											  .ToListAsync();

				if (inmuebles == null || !inmuebles.Any())
				{
					// Si no se encuentran inmuebles
					return NotFound("No se encontraron inmuebles para el usuario actual.");
				}

				return Ok(inmuebles);
			}
			catch (Exception ex)
			{
				// mensaje informativo en caso de error
				return BadRequest("Se produjo un error al procesar la solicitud."+ "\n"+ex.Message);
			}
		}

		[HttpPut("ActualizarInmueble")]
        public async Task<IActionResult> Actualizar([FromBody] Inmueble IEditado)
		{
			try
			{
				var usuarioActual = User.Identity.Name;

				// Verifico si el modelo recibido es válido
				if (!ModelState.IsValid)
				{
					return BadRequest(ModelState);
				}

				// Verifico si el inmueble pertenece al usuario actual
				var inmuebleExistente = await contexto.Inmuebles
					.Include(i => i.Propietario)
					.AsNoTracking()
					.FirstOrDefaultAsync(i => i.IdInmueble == IEditado.IdInmueble && i.Propietario.Mail == usuarioActual);

				if (inmuebleExistente != null)
				{
					// Actualizo el inmueble
					contexto.Inmuebles.Update(IEditado);
					await contexto.SaveChangesAsync();

					return Ok(IEditado);
				}
				else
				{
					return NotFound("No se pudo encontrar el inmueble o no tienes permiso para editarlo.");
				}
			}
			catch (Exception ex)
			{

				return BadRequest("Se produjo un error al procesar la solicitud.");
			}
		}



		[HttpGet("ObtenerAlquilados")]
		public async Task<IActionResult> ObtenerAlquilados()
		{
			try
			{
				var usuarioActual = User.Identity.Name;

				return Ok(contexto.Contratos.Include(e => e.Inmueble)
                                            .Include(e => e.Inquilino)
                                            .Include(e => e.Inmueble.Propietario)
                                            .Where(e => e.Inmueble.Propietario.Mail == usuarioActual)
											.Select(e => e.Inmueble ));
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPost("AgregarInmueble")]

		public async Task<IActionResult> Agregar([FromForm] Inmueble inmueble)
		{

			try
			{
				if (!ModelState.IsValid)
				{
					return BadRequest(ModelState);
				}

				var propietario = contexto.Propietarios.SingleOrDefault(e => e.Mail == User.Identity.Name);

				if (propietario == null)
				{
					return BadRequest("Propietario no encontrado.");
				}

						string DireccionfotosInmueble =("data/images/inmueble/");

						var nombreInmueble = Guid.NewGuid().ToString() + Path.GetExtension(inmueble.archivoFoto.FileName);

						string pathCompleto = DireccionfotosInmueble + nombreInmueble;
						inmueble.Imagen = pathCompleto;

						using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
						{
							inmueble.archivoFoto.CopyTo(stream);
						}
						contexto.Inmuebles.Add(inmueble);
						contexto.SaveChanges();
						return Ok(inmueble);
					
			
			}
			catch (Exception ex)
			{
				return BadRequest("Se produjo un error al procesar la solicitud." + "\n" + ex.Message);
			}
							
		}

		
		




        
    }
}
