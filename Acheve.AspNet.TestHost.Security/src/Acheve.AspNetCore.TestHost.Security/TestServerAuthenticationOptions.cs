using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace Acheve.AspNetCore.TestHost.Security
{
    public class TestServerAuthenticationOptions : AuthenticationSchemeOptions
    {
        public IEnumerable<Claim> CommonClaims { get; set; } = new Claim[0];

        public string NameClaimType { get; set; } = ClaimTypes.Name;

        public string RoleClaimType { get; set; } = ClaimTypes.Role;
    }
}