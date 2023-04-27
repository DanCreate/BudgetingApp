using BudgetingApp.Models;
using System.Security.Claims;

namespace BudgetingApp.Services
{

    public interface IServiceUsers 
    {

        int GetUserID();
    }
    public class ServiceUsers: IServiceUsers
    {
        private readonly HttpContext httpContext;

        public ServiceUsers(IHttpContextAccessor httpContextAccessor)
        {
            httpContext = httpContextAccessor.HttpContext;
        }
        public int GetUserID()
        {
            if (httpContext.User.Identity.IsAuthenticated)
            {
                var idclaim = httpContext.User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault();
                var id = int.Parse(idclaim.Value);
                    return id;
               

            }
            else
            {
                throw new ApplicationException("User has not been authenticated");

            }

            
        }
    }
}
