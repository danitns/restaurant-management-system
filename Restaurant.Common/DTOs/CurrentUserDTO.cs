using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Common.DTOs
{
    public class CurrentUserDTO
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public bool IsAuthenticated { get; set; }
        public string Role { get; set; }
        public string Phone { get; set; }
    }
}
