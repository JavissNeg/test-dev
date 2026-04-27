using Microsoft.AspNetCore.Mvc;
using TestDevBackJR.Application.DTOs;
using TestDevBackJR.Application.Interfaces;
using TestDevBackJR.Domain.Entities;

namespace TestDevBackJR.Controllers;

[ApiController]
[Route("logins")]
public class LoginsController(ILoginService loginService) : ControllerBase
{
    // GET /logins
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Login>>> Get()
    {
        try
        {
            var logins = await loginService.GetAll();
            return Ok(logins);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    // POST /logins
    [HttpPost]
    public async Task<ActionResult<Login>> Post(LoginDto dto)
    {
        try
        {
            var login = await loginService.Create(dto);
            return CreatedAtAction(nameof(Get), new { id = login.Id }, login);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    // PUT /logins/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, LoginDto dto)
    {
        try
        {
            var result = await loginService.Update(id, dto);
            if (!result)
                return NotFound(new { message = $"Login con ID {id} no encontrado" });

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    // DELETE /logins/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var result = await loginService.Delete(id);
            if (!result)
                return NotFound(new { message = $"Login con ID {id} no encontrado" });

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpGet("export/csv")]
    public async Task<IActionResult> ExportCsv()
    {
        try
        {
            var report = await loginService.GenerateUserReport();

            var csv = new System.Text.StringBuilder();
            csv.AppendLine("Username,Full Name,Area,Total Hours");

            foreach (var row in report)
            {
                csv.AppendLine($"\"{row.Username}\",\"{row.FullName}\",\"{row.Area}\",{row.TotalHours}");
            }

            var bytes = System.Text.Encoding.UTF8.GetBytes(csv.ToString());
            return File(bytes, "text/csv", "user_report.csv");
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }
}