using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repo.Identity.SeedingData
{
    public static class IdentityContextSeeding
    {
        public static async Task GetDataAsync(UserManager<AppUser> _signin)
        {
            var user2 = new AppUser()
            { 
                UserName = "Ahmed_3zam",   
                DisplayName = "ahmed azam",
                Email = "Ahmed.Thabetx@gmail.com",
                PhoneNumber = "01002625244",
            };
            await _signin.CreateAsync(user2,"P@ssw0rd");
        }
    }
}
