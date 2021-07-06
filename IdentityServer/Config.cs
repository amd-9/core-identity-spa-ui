using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace IdentityServer
{
    public static class Config
    {
        public static List<TestUser> Users
        {
            get
            {
                var address = new
                {
                    street_address = "One Hacker Way",
                    locality = "Heidelberg",
                    postal_code = 123456,
                    country = "Russia"
                };


                return new List<TestUser>
                {
                    new TestUser
                    {
                        SubjectId = "818727",
                        Username = "developer",
                        Password = "developer",
                        Claims =
                        {
                            new Claim(JwtClaimTypes.Name, value: "Identity Developer"),
                            new Claim(JwtClaimTypes.GivenName, value: "Developer"),
                            new Claim(JwtClaimTypes.FamilyName, value: "Developer"),
                            new Claim(JwtClaimTypes.Email, value: "developer@identity.com"),
                            new Claim(JwtClaimTypes.EmailVerified, value: ClaimValueTypes.Boolean),
                            new Claim(JwtClaimTypes.Role, value: "admin"),
                            new Claim(JwtClaimTypes.WebSite, value: "http://developer.com"),
                            new Claim(JwtClaimTypes.Address, value: JsonSerializer.Serialize(address),
                                valueType: IdentityServerConstants.ClaimValueTypes.Json)
                        }
                    },
                    new TestUser
                    {
                        SubjectId = "88421113",
                        Username = "user",
                        Password = "user",
                        Claims =
                        {
                            new Claim(JwtClaimTypes.Name, value: "Identity User"),
                            new Claim(JwtClaimTypes.GivenName, value: "User"),
                            new Claim(JwtClaimTypes.FamilyName, value: "User"),
                            new Claim(JwtClaimTypes.Email, value: "User@identity.com"),
                            new Claim(JwtClaimTypes.EmailVerified, value: ClaimValueTypes.Boolean),
                            new Claim(JwtClaimTypes.Role, value: "User"),
                            new Claim(JwtClaimTypes.WebSite, value: "http://User.com"),
                            new Claim(JwtClaimTypes.Address, value: JsonSerializer.Serialize(address),
                                valueType: IdentityServerConstants.ClaimValueTypes.Json)
                        }
                    },
                };
            }
        }

        public static IEnumerable<IdentityResource> IdentityResources =>
            new[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource
                {
                    Name = "role",
                    UserClaims = new List<string> {"role"}
                }
            };

        public static IEnumerable<ApiScope> ApiScopes => new[]
        {
            new ApiScope("weatherapi.read"),
            new ApiScope("weatherapi.write"),
        };

        public static IEnumerable<ApiResource> ApiResources => new[]
        {
            new ApiResource("weatherapi")
            {
                Scopes = new List<string> { "weatherapi.read", "weatherapi.write"},
                ApiSecrets = new List<Secret> {new Secret("ScopeSecret".Sha256())},
                UserClaims = new List<string> {"role"}
            }
        };

        public static IEnumerable<Client> Clients
        {
            get
            {
                return new List<Client>
                {
                    // machine to machine (m2m) client
                    new Client
                    {
                        ClientId = "m2m.client",
                        ClientName = "Client Credentials Client",

                        AllowedGrantTypes =  GrantTypes.ClientCredentials,
                        ClientSecrets = { new Secret("SuperPassword".Sha256())},

                        AllowedScopes = {"weatherapi.read", "weatherapi.write"}
                    },
                    new Client
                    {
                        ClientId = "interactive",
                        ClientSecrets = { new Secret("SuperPassword".Sha256())},

                        AllowedGrantTypes =  GrantTypes.Code,

                        RedirectUris = {"https://localhost:5444/signin-oidc"},
                        FrontChannelLogoutUri = "https://localhost:5444/signout-oidc",
                        PostLogoutRedirectUris = {"http://localhost:5444/signout-callback-oidc"},                        

                        AllowOfflineAccess = true,
                        AllowedScopes = {"openid","profile","weatherapi.read"}
                    }
                };
            }
        }
    }
}