using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;

namespace Identity.API.Configuration
{
    public class Config
    {
        public static IEnumerable<ApiResource> GetApis()
        {
            return new List<ApiResource>
            {
                new ApiResource("hotel", "Hotel Service"),
                new ApiResource("api1", "Test API 1 Service")
            };
        }

        public static IEnumerable<IdentityResource> GetResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }


        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "hotelswaggerui",
                    ClientName = "Hotel Swagger UI",
                    AllowedGrantTypes = GrantTypes.Code,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    //AllowAccessTokensViaBrowser = true,
                    
                    //RequireConsent = false,
                    AllowOfflineAccess = true,
                    
                    
                    // where to redirect to after login
                    RedirectUris = { "http://localhost:9500/signin-oidc" },

                    // where to redirect to after logout
                    PostLogoutRedirectUris = { "http://localhost:9500/signout-callback-oidc" },
                    AccessTokenLifetime = 7200,
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "hotel"
                    },
                    
                },
                new Client
                {
                    ClientId = "client",
                    
                    //** It will use client id/secret to authenticate
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    //** scope client can access
                    AllowedScopes = 
                    { 
                        "api1", "hotel"
                    }
                },
                new Client
                {
                    ClientId = "mvc",
                    ClientName = "MVC Client",
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    AllowAccessTokensViaBrowser = true,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    
                    RequireConsent = false,
                    AllowOfflineAccess = true,
                    
                    // where to redirect to after login
                    RedirectUris = { "http://localhost:9500/signin-oidc" },

                    // where to redirect to after logout
                    PostLogoutRedirectUris = { "http://localhost:9500/signout-callback-oidc" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "hotel"
                    },
                    
                }
        };
  }
}