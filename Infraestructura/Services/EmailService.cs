using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.IO;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace Infraestructura.Services
{
 public interface IEmailService
 {
 Task<bool> SendEmailWithAttachmentAsync(string destinatario, string asunto, string mensaje, string imagenBase64);
 }

 public class EmailService : IEmailService
 {
 private readonly IConfiguration _configuration;

 public EmailService(IConfiguration configuration)
 {
 _configuration = configuration;
 }

 public async Task<bool> SendEmailWithAttachmentAsync(
 string destinatario, 
 string asunto, 
 string mensaje, 
 string imagenBase64)
 {
 try
 {
 var email = _configuration["Email:Address"];
 var password = _configuration["Email:Password"];

 var message = new MimeMessage();
 message.From.Add(new MailboxAddress("SPA Premium", email));
 message.To.Add(new MailboxAddress("", destinatario));
 message.Subject = asunto;

 var builder = new BodyBuilder();
 builder.TextBody = mensaje;

 if (!string.IsNullOrEmpty(imagenBase64))
 {
 try
 {
 var base64Data = imagenBase64.Contains(",") 
 ? imagenBase64.Split(",")[1] 
 : imagenBase64;

 var imageData = Convert.FromBase64String(base64Data);
 builder.Attachments.Add("tarjeta-qr.png", imageData, ContentType.Parse("image/png"));
 }
 catch (Exception ex)
 {
 Console.WriteLine($"Error al procesar imagen: {ex.Message}");
 }
 }

 message.Body = builder.ToMessageBody();

 using (var client = new SmtpClient())
 {
 await client.ConnectAsync("smtp.gmail.com",587, SecureSocketOptions.StartTls);
 await client.AuthenticateAsync(email, password);
 await client.SendAsync(message);
 await client.DisconnectAsync(true);
 }

 return true;
 }
 catch (Exception ex)
 {
 Console.WriteLine($"Error al enviar email: {ex.Message}");
 return false;
 }
 }
 }
}
