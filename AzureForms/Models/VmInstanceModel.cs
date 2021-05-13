using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AzureForms.Models
{
    public partial class VmInstanceModel
    {
        [JsonProperty("placementGroupId")]
        public Guid PlacementGroupId { get; set; }

        [JsonProperty("platformFaultDomain")]
        public long PlatformFaultDomain { get; set; }

        [JsonProperty("computerName")]
        public string ComputerName { get; set; }

        [JsonProperty("osName")]
        public string OsName { get; set; }

        [JsonProperty("osVersion")]
        public string OsVersion { get; set; }

        [JsonProperty("vmAgent")]
        public VmAgent VmAgent { get; set; }

        [JsonProperty("disks")]
        public Disk[] Disks { get; set; }

        [JsonProperty("bootDiagnostics")]
        public BootDiagnostics BootDiagnostics { get; set; }

        [JsonProperty("hyperVGeneration")]
        public string HyperVGeneration { get; set; }

        [JsonProperty("statuses")]
        public Status[] Statuses { get; set; }
    }

    public partial class BootDiagnostics
    {
    }

    public partial class Disk
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("statuses")]
        public Status[] Statuses { get; set; }
    }

    public partial class Status
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("level")]
        public string Level { get; set; }

        [JsonProperty("displayStatus")]
        public string DisplayStatus { get; set; }

        [JsonProperty("time", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? Time { get; set; }

        [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }
    }

    public partial class VmAgent
    {
        [JsonProperty("vmAgentVersion")]
        public string VmAgentVersion { get; set; }

        [JsonProperty("statuses")]
        public Status[] Statuses { get; set; }
    }
}
