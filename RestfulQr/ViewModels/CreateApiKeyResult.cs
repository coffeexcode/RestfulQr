using RestfulQr.Entities;
using System.Collections.Generic;

namespace RestfulQr.ViewModels
{
    /// <summary>
    /// Result of creating an api key
    /// </summary>
    public class CreateApiKeyResult
    {
        private CreateApiKeyResult() { }

        /// <summary>
        /// Whether the operation was successful
        /// </summary>
        public bool Succeeded { get; set; }

        /// <summary>
        /// Any errors that occured during the operation
        /// </summary>
        public List<string> Errors { get; set; }

        /// <summary>
        /// The api key entity that was created
        /// <see cref="ApiKey"/>
        /// </summary>
        public ApiKey ApiKey { get; set; }

        public static CreateApiKeyResult Success(ApiKey apiKey)
        {
            return new CreateApiKeyResult
            {
                Succeeded = true,
                ApiKey = apiKey
            };
        }

        public static CreateApiKeyResult Failed(string message)
        {
            return new CreateApiKeyResult
            {
                Succeeded = false,
                Errors = new List<string>
                {
                    message
                }
            };
        }
    }
}
