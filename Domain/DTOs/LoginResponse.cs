using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class LoginResponse
    {
        public string? Token { get; set; }
        public DateTime? Expiration { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public IEnumerable<string>? Roles { get; set; }
    }
}
