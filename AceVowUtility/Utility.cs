using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AceVowUtility
{
    public class Utility
    {
        public string GetConnectionString()
        {
            string conStr = ConfigurationManager.ConnectionStrings["AceVowAdmin"].ConnectionString.ToString();
            return conStr;
        }
    }
}
