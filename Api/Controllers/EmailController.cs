using Microsoft.AspNetCore.Mvc;
using Infraestructura.Services;
using System;
using System.Threading.Tasks;

namespace Api.Controllers
{
 [ApiController]
 [Route("api/[controller]")]
 public class EmailController : ControllerBase
 {
 private readonly IEmailService _emailService;

 public EmailController(IEmailService emailService)
 {
 _emailService = emailService;
 }

 [HttpPost("enviar")]
 public async Task<IActionResult> EnviarEmail([FromBody] EnviarEmailDto dto)
 {
 try
 {
 var resultado = await _emailService.SendEmailWithAttachmentAsync(
 dto.Destinatario,
 dto.Asunto,
 dto.Mensaje,
 dto.Imagen ?? ""
 );

 if (resultado)
 {
 return Ok(new { mensaje = "Email enviado exitosamente", success = true });
 }
 else
 {
 return BadRequest(new { mensaje = "Error al enviar el email" });
 }
 }
 catch (Exception ex)
 {
 return StatusCode(500, new { mensaje = $"Error: {ex.Message}" });
 }
 }
 }

 public class EnviarEmailDto
 {
 public string Destinatario { get; set; }
 public string Asunto { get; set; }
 public string Mensaje { get; set; }
 public string? Imagen { get; set; } // Base64
 }
}
