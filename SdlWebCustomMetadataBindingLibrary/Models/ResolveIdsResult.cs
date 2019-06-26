using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trisoft.InfoShare.Plugins.SDK.Extensions.MetadataBinding;

namespace SdlWebCustomMetadataBindingLibrary.Models
{
    /// <summary>
    /// Simplistic and minimal implementation of the <code>IResolveIdsResult</code> interface.
    /// </summary>
    public class ResolveIdsResult : IResolveIdsResult
    {
        /// <summary>
        /// Collection of tags
        /// </summary>
        public IEnumerable<ITag> Tags { get; set; }
    }
}
