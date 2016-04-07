using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacificBBExtranet.Web.Models
{
    public class User : IUser
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string Id
        {
            get { return ""; }
        }
    }
}
