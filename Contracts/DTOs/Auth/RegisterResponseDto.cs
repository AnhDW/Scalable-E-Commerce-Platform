using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.DTOs.Auth
{
    public class RegisterResponseDto
    {
        public object User { get; set; } = null!;
        public bool Error { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
