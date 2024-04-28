using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp2.Model.UserDto
{
    public class UserUpdateDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string SelfIntro { get; set; }
    }
}
