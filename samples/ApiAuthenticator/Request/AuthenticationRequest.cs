using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiAuthenticator.Request
{
    public sealed class AuthenticationRequest
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
