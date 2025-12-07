using Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using Presentation.Serveces;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(AuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        if (loginRequest == null)
            return BadRequest(new { Message = "Invalid login request."});

        var loginResponse = await _authService.LoginAsync(loginRequest);

        if (loginResponse == null)
        {
            _logger.LogWarning("Failed login attempt for {User}", loginRequest.userNameOrEmail);
            return Unauthorized(new { Message = "Invalid username/email or password." });
        }

        _logger.LogInformation("User {User} logged in successfully.", loginRequest.userNameOrEmail);
        return Ok(loginResponse);
    }


    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
    {
        if (registerRequest == null)
        {
            _logger.LogWarning("Received null registration request.");
            return BadRequest(new { Message = "Invalid registration request." });
        }

        var result = await _authService.RegisterAsync(registerRequest);

        if (result)
        {
            _logger.LogInformation("User registration successful for {UserName}", registerRequest.UserName);
            return Ok(new { Message = "User registered successfully." });
        }
        else
        {
            _logger.LogWarning("User registration failed for {UserName}", registerRequest.UserName);
            return Conflict(new { Message = "User already exists or registration failed." });
        }
    }


}
