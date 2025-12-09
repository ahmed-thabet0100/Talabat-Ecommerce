using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Core.Repo.Contarct
{
    public interface IAuthService
    {
        Task<string> CreateToken(AppUser appUser, UserManager<AppUser> user);
    }
}
