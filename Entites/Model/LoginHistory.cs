using HRMSAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entites.Model
{
    public class LoginHistory:BaseEntity
    {

        public int UserId { get; set; }
        public User User { get; set; }

        public DateTime LoginTime { get; set; }
        public string IPAddress { get; set; }
    }
}
