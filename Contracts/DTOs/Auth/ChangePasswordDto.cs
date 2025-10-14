using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.DTOs.Auth
{
    public class ChangePasswordDto
    {
        public required string CurrentPassword { get; set; }
        public required string NewPassword { get; set; }
    }
}
