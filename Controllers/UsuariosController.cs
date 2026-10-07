using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly AppDbContext _context;

    public UsuariosController(IConfiguration config, AppDbContext context)
    {
        _config = config;
        _context = context;
    }

    // GET: api/usuarios/saludo
    [HttpGet("saludo")]
    public IActionResult Saludo()
    {
        var saludo = _config["MensajeSaludo"];
        return Ok(new { Mensaje = saludo });
    }

    // GET: api/usuarios/env
    [HttpGet("env")]
    public IActionResult VerVariablesDeEntorno()
    {
        // 1. Leyendo desde IConfiguration (appsettings.json o Variables de Entorno)
        var configSaludo = _config["MensajeSaludo"];
        
        // 2. Leyendo directamente una Variable de Entorno del sistema
        var aspnetEnv = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        return Ok(new 
        { 
            Configuracion = configSaludo,
            EntornoSistema = aspnetEnv ?? "No definida"
        });
    }
    // GET: api/usuarios
    [HttpGet]
    public async Task<IActionResult> GetUsuarios()
    {
        var usuarios = await _context.Usuarios.ToListAsync();
        return Ok(usuarios);
    }

    // GET: api/usuarios/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUsuario(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);

        if (usuario == null)
        {
            return NotFound();
        }

        return Ok(usuario);
    }

    // POST: api/usuarios
    [HttpPost]
    public async Task<IActionResult> PostUsuario(Usuario usuario)
    {
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id }, usuario);
    }

    
}
