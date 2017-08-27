using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
using Owin;
using System;
using System.IO;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Experiments.WebApiOwinHmac.Middlewares
{
    public static class IAppBuilderExtensions
    {
        public static void UseHmacAuthentication(this IAppBuilder self, string secret)
        {
            self.Use<HmacAuthenticationMiddleware>(new HmacAuthenticationOptions(secret));
        }
    }

    internal class HmacAuthenticationMiddleware : AuthenticationMiddleware<HmacAuthenticationOptions>
    {
        public HmacAuthenticationMiddleware(OwinMiddleware next, HmacAuthenticationOptions options)
            : base(next, options)
        {
        }

        protected override AuthenticationHandler<HmacAuthenticationOptions> CreateHandler()
        {
            return new HmacAuthenticationHandler();
        }
    }

    internal class HmacAuthenticationOptions : AuthenticationOptions
    {
        public string Secret { get; private set; }

        public HmacAuthenticationOptions(string secret) : base("CUSTOM")
        {
            Secret = secret;
        }
    }

    internal class HmacAuthenticationHandler : AuthenticationHandler<HmacAuthenticationOptions>
    {
        protected override Task<AuthenticationTicket> AuthenticateCoreAsync()
        {
            if (!Request.Headers.ContainsKey("hmac")) return Task.FromResult(new AuthenticationTicket(null, null));
            var submittedHmac = Request.Headers["hmac"];

            var hmac = new HMACSHA1(Encoding.UTF8.GetBytes(Options.Secret));
            var result = hmac.ComputeHash(Request.Body);
            if (Convert.ToBase64String(result) != submittedHmac)
            {
                return Task.FromResult(new AuthenticationTicket(null, null));
            }

            Request.Body.Seek(0, SeekOrigin.Begin); //Rewind for later reads

            var identity = new ClaimsIdentity(Options.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, "hmac"));
            identity.AddClaim(new Claim(ClaimTypes.Name, "hmac"));
            identity.AddClaim(new Claim(ClaimTypes.Role, "hmac"));

            return Task.FromResult(new AuthenticationTicket(identity, new AuthenticationProperties()));
        }
    }
}