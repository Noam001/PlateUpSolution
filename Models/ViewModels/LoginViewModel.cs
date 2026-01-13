using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class LoginViewModel/// מה שמתקבל
    {

        public string  ClientId { get; set; }
        public string Name { get; set; }
        public bool IsAdmin { get; set; }
    }

    public class LoginModel// מה ששולחים
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
    }

}

