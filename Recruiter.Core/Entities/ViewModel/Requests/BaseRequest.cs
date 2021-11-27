using Newtonsoft.Json;
using System;

namespace Recruiter.Core.Entities.ViewModel.Requests
{
    public class BaseRequest
    {
        public virtual Guid? currentUserId { get; set; }
        public virtual string CurrentAccessToken { get; set; }
        public virtual string CurrentDeviceToken { get; set; }
        public virtual string CurrentClientId { get; set; }
        public virtual TokenInfo TokenInfo { get; set; }
    }

    public class TokenInfo
    {
        [JsonProperty(PropertyName = "aud")]
        public string Aud { get; set; }
        [JsonProperty(PropertyName = "iss")]
        public string Iss { get; set; }
        [JsonProperty(PropertyName = "exp")]
        public string Exp { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "preferred_username")]
        public string email { get; set; }
    }
}
