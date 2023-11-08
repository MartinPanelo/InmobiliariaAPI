using API.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers{


    [ApiController]
    [Route("[controller]")]
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ContratoController : ControllerBase{


        private readonly DataContext contexto;


        public ContratoController(DataContext contexto)
		{
			this.contexto = contexto;

		}


		
        //contrato por inmueble

        [HttpGet("ObtenerContratoPorInmueble")]
		public async Task<IActionResult> ContratoPorInmueble(int idInmueble)
        {

			try
			{


				var usuarioActual = User.Identity.Name;

				var contrato = contexto.Contratos
					.Where(e => e.Inmueble.Propietario.Mail == usuarioActual && e.Inmueble.IdInmueble == idInmueble)
					.Include(e => e.Inmueble)
					.Include(e => e.Inquilino)
					.Include(e => e.Inmueble.Propietario)
					.FirstOrDefault();
				if (contrato == null)
				{
					return NotFound("No se encontró un contrato válido para este inmueble y usuario.");
				}

				return Ok(contrato);

			}
			catch (Exception ex)
			{
				return BadRequest("Se produjo un error al tratar de procesar la solicitud: " + ex.Message);
			}
		}
















    }
}