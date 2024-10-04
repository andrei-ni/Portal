using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Portal.Data;
using System.Security.Claims;

namespace WinAuth.Middleware
{
    public class ClaimsTransformer : IClaimsTransformation
    {
        private readonly DataContext _context;

        public ClaimsTransformer(DataContext context)
        {
                _context = context;
        }

        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal )
        {   

            var CurrentUser= (ClaimsIdentity)principal.Identity;

            if (CurrentUser != null)
            {
                var CurrentUserName = CurrentUser.Name.Replace("\\", "");

                var AppUser = _context.AppUsers.Where(x => x.AdIdentity /*username*/ == CurrentUserName).Include(x => x.Role).ThenInclude(c => c.Claims).FirstOrDefault();

                if (AppUser != null)
                {

                    foreach (var cl in AppUser.Role.Claims)
                    {

                        var NewClaim = new Claim(ClaimTypes.GroupSid, cl.Claim);
                        CurrentUser.AddClaim(NewClaim);
                    }
                }



            }

            return Task.FromResult(principal);
        }

 
    }
}
