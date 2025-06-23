using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GizmoGrid._01.Services.EmailService;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    private readonly IEmailService _emailService;

    public TestController(IEmailService emailService)
    {
        _emailService = emailService;
    }

    [AllowAnonymous]
    [HttpPost("send-test-email")]
    public async Task<IActionResult> SendTestEmail([FromQuery] string toEmail, [FromQuery] string subject, [FromQuery] string body)
    {
        if (string.IsNullOrWhiteSpace(toEmail) || string.IsNullOrWhiteSpace(subject) || string.IsNullOrWhiteSpace(body))
        {
            return BadRequest("All fields (toEmail, subject, body) are required.");
        }

        try
        {
            await _emailService.SendAsync(toEmail, subject, body);
            return Ok($"Email sent to {toEmail}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Failed to send email: {ex.Message}");
        }
    }
}