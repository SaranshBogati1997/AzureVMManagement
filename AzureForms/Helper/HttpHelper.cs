using Azure.Identity;
using Microsoft.Azure.Management.Compute.Fluent;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.Azure.Management.Fluent.Azure;
using static Microsoft.Azure.Management.ResourceManager.Fluent.Core.RestClient;

namespace AzureForms.Helper
{
    public class HttpHelper
    {
        public const string BASE_URI = "https://management.azure.com/";

        /// <summary>
        /// Lists all the vms available in the scaleset
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetVmList()
        {
            string url = BASE_URI + GetVmListURL(Vm.subscriptionId, Vm.resourceGroup, Vm.scaleSet);
            var _client = Vm.client;
            try
            {
                var x = _client.GetAsync(url).GetAwaiter().GetResult();
                string response = x.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// gets the instance information
        /// </summary>
        /// <param name="instanceId"></param>
        /// <returns></returns>
        public async Task<string> GetVmInstance(string instanceId)
        {
            string url = BASE_URI + GetVmURL(Vm.subscriptionId, Vm.resourceGroup, Vm.scaleSet,instanceId);
            var _client = Vm.client;
            try
            {
                var x = _client.GetAsync(url).GetAwaiter().GetResult();
                string response = x.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Deallocates(release the resource) the instance
        /// </summary>
        /// <param name="instanceId"></param>
        /// <returns></returns>
        public async Task<string> DeallocateComputer(string instanceId)
        {
            string url = BASE_URI + GetDeallocateURL(Vm.subscriptionId, Vm.resourceGroup, Vm.scaleSet,instanceId);
            var _client = Vm.client;
            try
            {
                var x = _client.PostAsync(url,null).GetAwaiter().GetResult();
                string response = x.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// starts the machine 
        /// </summary>
        /// <param name="instanceId"></param>
        /// <returns></returns>
        public async Task<string> StartInstance(string instanceId)
        {
            string url = BASE_URI + GetStartURL(Vm.subscriptionId, Vm.resourceGroup, Vm.scaleSet,instanceId);
            var _client = Vm.client;
            try
            {
                var x = _client.PostAsync(url,null).GetAwaiter().GetResult();
                string response = x.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// gets the ip address for the ipName
        /// </summary>
        /// <param name="publicIpAddress"></param>
        /// <returns></returns>
        public async Task<string> GetIp(string publicIpAddress)
        {
            string url = BASE_URI + GetIpURL(Vm.subscriptionId, Vm.resourceGroup,publicIpAddress);
            var _client = Vm.client;
            try
            {
                var x = _client.GetAsync(url).GetAwaiter().GetResult();
                string response = x.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        string GetVmListURL(string subscriptionId, string resourceGroup, string scaleSet)
        {
            return string.Format("/subscriptions/{0}/resourceGroups/{1}/providers/Microsoft.Compute/virtualMachineScaleSets/{2}/virtualMachines?api-version=2020-12-01", subscriptionId, resourceGroup, scaleSet);
        }
        string GetDeallocateURL(string subscriptionId, string resourceGroup, string scaleSet, string instanceId)
        {
            return string.Format("/subscriptions/{0}/resourceGroups/{1}/providers/Microsoft.Compute/virtualMachineScaleSets/{2}/virtualmachines/{3}/deallocate?api-version=2020-12-01", subscriptionId, resourceGroup, scaleSet, instanceId);
        }
        string GetStartURL(string subscriptionId, string resourceGroup, string scaleSet, string instanceId)
        {
            return string.Format("/subscriptions/{0}/resourceGroups/{1}/providers/Microsoft.Compute/virtualMachineScaleSets/{2}/virtualmachines/{3}/start?api-version=2020-12-01", subscriptionId, resourceGroup, scaleSet, instanceId);
        }
        string GetVmURL(string subscriptionId, string resourceGroup, string scaleSet, string instanceId)
        {
            return string.Format("/subscriptions/{0}/resourceGroups/{1}/providers/Microsoft.Compute/virtualMachineScaleSets/{2}/virtualmachines/{3}/instanceView?api-version=2020-12-01", subscriptionId, resourceGroup, scaleSet, instanceId);
        }
        string GetIpURL(string subscriptionId,string resourceGroup,string publicIpName)
        {
            return string.Format("/subscriptions/{0}/resourceGroups/{1}/providers/Microsoft.Network/publicIPAddresses/{2}?api-version=2020-11-01", subscriptionId, resourceGroup, publicIpName);
        }
    }
}
