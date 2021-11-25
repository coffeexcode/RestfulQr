using RestfulQr.Domain;

namespace RestfulQr.Api.Core.Providers
{
    /// <summary>
    /// Scoped class to faciliate easy api key extraction. This field is
    /// </summary>
    public class ApiKeyProvider
    {
        /// <summary>
        /// The Api key that was provided. This will only be set to an api
        /// key that exists in the backing store
        /// </summary>
        public ApiKey ApiKey { get; set; }
    }
}