using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AceVowEntities
{
    public class Login
    {
        private string userName = string.Empty;
        private string password = string.Empty;
        private Int32 _Loginid = 0;

        public string UserName { get => userName; set => userName = value; }
        public string Password { get => password; set => password = value; }
        public Int32 Loginid { get => _Loginid; set => _Loginid = value; }
    }
}
