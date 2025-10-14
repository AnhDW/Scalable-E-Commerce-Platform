using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.DTOs.Auth
{
    public class LoginResponseDto
    {
        public object User { get; set; } = null!;
        public string Token { get; set; } = string.Empty;
    }
}
