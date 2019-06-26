using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trisoft.InfoShare.Plugins.SDK;
using Trisoft.InfoShare.Plugins.SDK.Extensions.MetadataBinding;


namespace SdlWebCustomMetadataBindingLibrary.Models
{
    /// <summary>
    /// Simplistic and minimal implementation of the <code>ValidateResult</code> interface.
    /// </summary>
    public class ValidateResult : IValidateResult
    {
        /// <summary>
        /// Collection of validation messages
        /// </summary>
        public IDictionary<string, IMessage> Messages { get; set; }
    }
}
