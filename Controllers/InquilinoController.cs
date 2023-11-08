using API.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers{


    [ApiController]
    [Route("[controller]")]
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class InquilinoController : ControllerBase{


        private readonly DataContext contexto;
        private readonly IConfiguration config;

        public InquilinoController(DataContext contexto,IConfiguration config)
		{
			this.contexto = contexto;
            this.config = config;
		}


		[HttpGet("ObtenerInquilinoPorInmueble")]
		public async Task<IActionResult> InquilinoPorInmueble(int idInmueble)
        {

			try
			{
				var usuarioActual = User.Identity.Name;

				var inquilino = contexto.Contratos.Include(e => e.Inmueble)
                                            .Include(e => e.Inquilino)
                                            .Include(e => e.Inmueble.Propietario)
                                            .Where(e => e.Inmueble.Propietario.Mail == usuarioActual && e.Inmueble.IdInmueble == idInmueble)
											.Select(e => e.Inquilino )
                                            .FirstOrDefault();


				return  Ok(inquilino);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}


    }
}