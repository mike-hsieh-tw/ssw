using System.Web.Http;
using WorkScheduleSystem.Utilities.Attributes.Filters;

namespace WorkScheduleSystem.Controllers
{
    public class JwtTokenController : ApiController
    {        
        [JwtAuth]
        public void Get(){}
    }
}
