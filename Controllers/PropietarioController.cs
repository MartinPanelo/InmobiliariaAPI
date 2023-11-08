using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using API.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers{



    [ApiController]
    [Route("[controller]")]
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PropietarioController : ControllerBase{


        private readonly DataContext contexto;
        private readonly IConfiguration config;

        public PropietarioController(DataContext contexto,IConfiguration config)
		{
			this.contexto = contexto;
            this.config = config;
		}

        [HttpPost("login")]
		[AllowAnonymous]
		public async Task<IActionResult> Login([FromForm] LoginView loginView)
		{
			try
			{
				string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
					password: loginView.Clave,
					salt: System.Text.Encoding.ASCII.GetBytes(config["Salt"]),
					prf: KeyDerivationPrf.HMACSHA1,
					iterationCount: 1000,
					numBytesRequested: 256 / 8));
				var p = await contexto.Propietarios.FirstOrDefaultAsync(x => x.Mail == loginView.Usuario);
				if (p == null || p.Password != hashed)
				{
					return BadRequest("Nombre de usuario y/o clave incorrecta");
				}
				else
				{
					var key = new SymmetricSecurityKey(
						System.Text.Encoding.ASCII.GetBytes(config["TokenAuthentication:SecretKey"]));
					var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
					var claims = new List<Claim>
					{
						new Claim(ClaimTypes.Name, p.Mail)
					};

					var token = new JwtSecurityToken(
						issuer: config["TokenAuthentication:Issuer"],
						audience: config["TokenAuthentication:Audience"],
						claims: claims,
						expires: DateTime.Now.AddMinutes(60),
						signingCredentials: credenciales
					);
					return Ok(new JwtSecurityTokenHandler().WriteToken(token));
				}
			}
			catch (Exception ex)
			{
				return BadRequest("Error :"+ ex +"\nSe produjo un error al tratar de procesar la solicitud");
			}
		}


		
        [HttpGet("ObtenerPerfil")]
		public async Task<ActionResult<Propietario>> GetPerfil()
		{
			try
			{
				var propietario = User.Identity.Name;			

				Propietario p = await contexto.Propietarios.SingleOrDefaultAsync(x => x.Mail == propietario);

				if (p == null)
				{
					// El perfil no se encontró en la base de datos
					return NotFound("Perfil no encontrado");
				}

				return p;
			}
			 catch (Exception ex)
			{
				return BadRequest("Se produjo un error al tratar de procesar la solicitud: " + ex.Message);
			}
		}


		[HttpPut("ActualizarPropietario")]
		public async Task<IActionResult> Put([FromBody] Propietario PEditado)
		{
			try
			{
				if (ModelState.IsValid)
				{

					var mailLogeado = User.Identity.Name;

					Propietario original = await contexto.Propietarios.SingleOrDefaultAsync(x => x.Mail == mailLogeado);

					if (!string.IsNullOrEmpty(PEditado.Password))
					{

						PEditado.Password = Convert.ToBase64String(KeyDerivation.Pbkdf2(
							password: PEditado.Password,
							salt: System.Text.Encoding.ASCII.GetBytes(config["Salt"]),
							prf: KeyDerivationPrf.HMACSHA1,
							iterationCount: 1000,
							numBytesRequested: 256 / 8));
						original.Password = PEditado.Password;
					}

					original.Nombre = PEditado.Nombre;
					original.Apellido = PEditado.Apellido;
					original.Dni = PEditado.Dni;
					original.Telefono = PEditado.Telefono;
					original.Mail = PEditado.Mail;
					
					
					contexto.Propietarios.Update(original);
					await contexto.SaveChangesAsync();
					return Ok(original);
				}
				return BadRequest();
			}
			catch (Exception ex)
			{
				return BadRequest("Se produjo un error al tratar de procesar la solicitud: " + ex.Message);
			}
		}







    }
}