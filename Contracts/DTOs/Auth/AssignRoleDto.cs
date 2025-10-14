using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.DTOs.Auth
{
    public class AssignRoleDto
    {
        public string UserName { get; set; }
        public string RoleName { get; set; }
    }
}
