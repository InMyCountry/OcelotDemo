using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerDemo
{
    public sealed class Config
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
             {
                 new ApiResource("OcelotApiA", "OcelotApiA API"),
                 new ApiResource("OcelotApiB", "OcelotApiB API")
             };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
             {
                 new Client
                 {
                     ClientId = "OcelotApiAClient",
                     AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                     ClientSecrets =
                     {
                         new Secret("ServiceAClient".Sha256())
                     },
                     AllowedScopes = new List<string> {"OcelotApiA"},
                     AccessTokenLifetime = 60 * 60 * 1
                 },
                 new Client
                 {
                     ClientId = "OcelotApiBClient",
                     AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                     ClientSecrets =
                     {
                         new Secret("ServiceBClient".Sha256())
                     },
                     AllowedScopes = new List<string> {"OcelotApiB"},
                     AccessTokenLifetime = 60 * 60 * 1
                 }
             };
        }

        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
             {
                 new TestUser
                 {
                     Username = "test",
                     Password = "123456",
                     SubjectId = "1"
                 }
             };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>();
        }
    }
}
