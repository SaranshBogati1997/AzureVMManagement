namespace AzureForms.Models
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class IpModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("etag")]
        public string Etag { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("zones")]
        [JsonConverter(typeof(DecodeArrayConverter))]
        public long[] Zones { get; set; }

        [JsonProperty("properties")]
        public Properties Properties { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("sku")]
        public Sku Sku { get; set; }
    }

    public partial class Properties
    {
        [JsonProperty("provisioningState")]
        public string ProvisioningState { get; set; }

        [JsonProperty("resourceGuid")]
        public Guid ResourceGuid { get; set; }

        [JsonProperty("ipAddress")]
        public string IpAddress { get; set; }

        [JsonProperty("publicIPAddressVersion")]
        public string PublicIpAddressVersion { get; set; }

        [JsonProperty("publicIPAllocationMethod")]
        public string PublicIpAllocationMethod { get; set; }

        [JsonProperty("idleTimeoutInMinutes")]
        public long IdleTimeoutInMinutes { get; set; }

        [JsonProperty("ipTags")]
        public object[] IpTags { get; set; }
    }

}
