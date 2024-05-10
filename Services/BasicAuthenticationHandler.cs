//using Microsoft.AspNetCore.Authentication;
//using Microsoft.Extensions.Options;
//using Microsoft.Identity.Client;
//using System.Net.Http.Headers;
//using System.Security.Claims;
//using System.Text;
//using System.Text.Encodings.Web;
//using WebApplicationTodoList.Models;

//namespace WebApplicationTodoList.Services
//{
//    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationOptions>
//    {
//        private WebApiDemoContext _context;

//        public BasicAuthenticationHandler(
//            IOptionsMonitor<AuthenticationOptions> options,
//            ILoggerFactory logger,
//            UrlEncoder encoder,
//            WebApiDemoContext context) : base(options, logger, encoder)
//        {
//            _context = context;
//        }

//        protected async override Task<AuthenticateResult> HandleAuthenticateAsync()
//        {
//            var authHeader = AuthenticationHeaderValue.Parse(Request.Headers.Authorization.ToString());
//            byte[] decodedHeader = Convert.FromBase64String(authHeader.Parameter ?? "");

//            string[] auth = Encoding.UTF8.GetString(decodedHeader).Split(':', 2);

//            user = _context.Users.FirstOrDefault(u => u.Username == authHeader[0] && u.Password == authHeader[1]);

//            if (user == null) return AuthenticateResult.Fail("User not found");
//            else
//            {
//                new List<claim>
//                {
//                    new Claim(ClaimTypes.NameIdentifier, user?.UserId),
//                    new Claim(ClaimTypes.Name, user?.Username)
//                }
//                var identity = new ClaimsIdentity(claims);
//                var pricipal = new ClaimsPrincipal(identity);
//                var ticket = new AuthenticationTicket(pricipal, Scheme.Name);
//                return AuthenticateResult.Success(ticket);
//            }
//        }
//    }
//}
