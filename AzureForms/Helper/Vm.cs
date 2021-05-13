using Microsoft.Azure.Management.Compute.Fluent;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AzureForms.Helper
{
    public class Vm
    {
        public static IAzure azure;
        public static IVirtualMachine[] allVms;
        public static string subscriptionId;
        public static string clientId;
        public static string tenantId;
        public static string clientKey;
        public static string bearerToken;
        public static string scaleSet;
        public static string resourceGroup;
        public static HttpClient client;
       
        /// <summary>
        /// Initialize values for use with sdk
        /// </summary>
        /// <param name="subscription"></param>
        /// <param name="tenant"></param>
        /// <param name="client"></param>
        /// <param name="key"></param>
        public static void InitializeValues(string subscription, string tenant, string client, string key)
        {
            subscriptionId = subscription;
            tenantId= tenant;
            clientId = client;
            clientKey= key;
            var creds = new AzureCredentialsFactory()
                 .FromServicePrincipal(clientId, clientKey, tenantId, AzureEnvironment.AzureGlobalCloud);
            azure = Microsoft.Azure.Management.Fluent.Azure.Authenticate(creds).WithSubscription(subscriptionId);
            allVms = azure.VirtualMachines.ListAsync().GetAwaiter().GetResult().ToArray();
        }
        /// <summary>
        /// initialize parameters for the api
        /// </summary>
        /// <param name="subscription"></param>
        /// <param name="tenant"></param>
        /// <param name="clId"></param>
        /// <param name="key"></param>
        /// <param name="scale"></param>
        /// <param name="resource"></param>
        public static void InitializeApiParams(string subscription, string tenant, string clId, string key,string scale,string resource)
        {
            subscriptionId = subscription;
            tenantId = tenant;
            clientId = clId;
            clientKey = key;
            scaleSet = scale;
            resourceGroup = resource;
            bearerToken = GetBearerToken().GetAwaiter().GetResult();
            client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);
        }
        /// <summary>
        /// get the bearer token for adding to authorization header
        /// </summary>
        /// <returns></returns>
        private async static Task<string> GetBearerToken()
        {
            try
            {
                string authContextURL = "https://login.windows.net/" + tenantId;
                var authenticationContext = new Microsoft.IdentityModel.Clients.ActiveDirectory.AuthenticationContext(authContextURL);
                var cred = new Microsoft.IdentityModel.Clients.ActiveDirectory.ClientCredential(clientId, clientKey);
                var result = await authenticationContext.AcquireTokenAsync("https://management.azure.com/", cred);
                if (result == null)
                {
                    throw new InvalidOperationException("Failed to obtain the JWT token");
                }
                string token = result.AccessToken;
                return token;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
