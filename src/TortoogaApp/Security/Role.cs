using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;

namespace TortoogaApp.Security
{
    public class Role : IdentityRole<Guid>
    {
        public Role()
        {
            //base.Id = Guid.NewGuid();
        }

        public Role(string roleName) : this()
        {
            base.Name = roleName;
        }
    }
}