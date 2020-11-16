using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestfulQr.ViewModels
{
    /// <summary>
    /// Result of creating an file
    /// </summary>
    public class CreateFileResult
    {
        private CreateFileResult() { }

        /// <summary>
        /// Whether the operation was successful
        /// </summary>
        public bool Succeeded { get; set; }

        /// <summary>
        /// Any errors that occured during the operation
        /// </summary>
        public List<string> Errors { get; set; }

        /// <summary>
        /// The name of the created file
        /// </summary>
        public string Filename { get; set; }

        public static CreateFileResult Success(string filename)
        {
            return new CreateFileResult
            {
                Succeeded = true,
                Filename = filename
            };
        }

        public static CreateFileResult Failed(string message)
        {
            return new CreateFileResult
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
