using HRMSAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entites.Model
{
    public class User:BaseEntity
    {

        public string Username { get; set; }
        public string PasswordHash { get; set; }

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}
