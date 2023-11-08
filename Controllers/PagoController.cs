using API.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers{


    [ApiController]
    [Route("[controller]")]
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PagoController : ControllerBase{


        private readonly DataContext contexto;

        public PagoController(DataContext contexto)
		{
			this.contexto = contexto;

		}


		
        //pagos por contrato

        [HttpGet("ObtenerPagosPorContrato")]
		public async Task<IActionResult> PagosPorContrato(int idContrato)
        {

			try
			{
				var usuarioActual = User.Identity.Name;

			var pagos = contexto.Pagos
								.Where(e => e.ContratoId == idContrato)
								.Include(e => e.Contrato);

			if (pagos == null){
				return NotFound("No se encontró un contrato válido para este inmueble y usuario.");
			}else{
				return Ok(pagos);
			}
			}
			catch (Exception ex)
			{
				return BadRequest("Error interno del servidor:"+ ex.Message);
			}
		}















    }
}