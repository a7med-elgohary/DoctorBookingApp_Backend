using Domain.DTOs;
using Domain.Entities;
using Infrastructure.Repo;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Presentation.Serveces
{
    public class AuthService
    {
        protected readonly IUnitOfWork _unitOfWork;
        private ILogger<AuthService> _logger;
        private readonly IConfiguration _config;

        public AuthService(IUnitOfWork unitOfWork,ILogger<AuthService> logger, JwtSettings jwtSettings, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _config = configuration;
        }

        // Sgin Up Method
        public async Task<bool> RegisterAsync(RegisterRequest registerRequest)
        {
            if (registerRequest == null)
                return false;

            try
            {
                // تحقق إذا كان المستخدم موجود مسبقًا
                var existingUser = await _unitOfWork.Users
                    .FindAsync(u => u.UserName == registerRequest.UserName || u.Email == registerRequest.Email);

                if (existingUser.Any())
                    return false;


                // تأمين كلمة المرور عن طريق Hash
                var hashedPassword = HashPassword(registerRequest.Password);

                var user = new ApplicationUser
                {
                    UserName = registerRequest.UserName,
                    Email = registerRequest.Email,
                    Password = hashedPassword
                };

                await _unitOfWork.Users.AddAsync(user);
                await _unitOfWork.SaveChangesAsync(); // حفظ التغييرات

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while registering user.");
                return false;
            }
        }


        //login Method

        public async Task<LoginResponse?> LoginAsync(LoginRequest loginRequest)
        {
            if (loginRequest == null)
                return null;

            // جلب المستخدم
            var users = await _unitOfWork.Users
                .FindAsync(u => u.UserName == loginRequest.userNameOrEmail || u.Email == loginRequest.userNameOrEmail);
            var user = users.FirstOrDefault();

            if (user == null)
                return null;

            // تحقق من كلمة المرور
            if (!VerifyPassword(loginRequest.Password, user.Password))
                return null;

            // جلب الـ Roles لو عندك نظام أدوار
            //var roles = await _unitOfWork.UserRoles.GetRolesByUserIdAsync(user.Id); // افترض عندك method زي دي

            // توليد JWT Token
            var token = GenerateJwtToken(user);

            return new LoginResponse
            {
                Token = token,
                Expiration = DateTime.UtcNow.AddHours(2),
                UserName = user.UserName,
                Email = user.Email,
                //Roles = roles
            };
        }



        private string GenerateJwtToken(ApplicationUser user, IEnumerable<string?>? roles = null)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"] ?? throw new Exception("JWT Key is missing!"));


            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("system", "MyERPSystem") 
            };

            //foreach (var role in roles)
            //{
            //    claims.Add(new Claim(ClaimTypes.Role, role));
            //}

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(2),
                Issuer = _config["Jwt:Issuer"],
                Audience = _config["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }



        private bool VerifyPassword(string inputPassword, string storedHashedPassword)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(inputPassword);
            var hash = sha256.ComputeHash(bytes);
            var hashedInput = Convert.ToBase64String(hash);
            return hashedInput == storedHashedPassword;
        }
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }



    }
}
