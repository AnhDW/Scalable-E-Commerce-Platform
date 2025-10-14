using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.DTOs.Auth
{
    public class LoginRequestDto
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
    }
}
