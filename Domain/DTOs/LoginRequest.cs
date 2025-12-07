using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class LoginRequest
    {
        public required string userNameOrEmail { get; set; } = null!;
        public required string Password { get; set; } = null!;
    }
}
