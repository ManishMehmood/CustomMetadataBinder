using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trisoft.InfoShare.Plugins.SDK;
using Trisoft.InfoShare.Plugins.SDK.Extensions.MetadataBinding;

namespace SdlWebCustomMetadataBindingLibrary.Models
{
    /// <summary>
    /// Simplistic and minimal implementation of the <code>IRetrieveTagStructureResult</code> interface.
    /// </summary>
    public class RetrieveTagStructureResult : IRetrieveTagStructureResult
    {
        /// <summary>
        /// Collection of tags
        /// </summary>
        public IEnumerable<Trisoft.InfoShare.Plugins.SDK.Extensions.MetadataBinding.IStructureTag> Tags { get; set; }
        /// <summary>
        /// Collection of relations
        /// </summary>
        public IEnumerable<ITagRelation> Relations { get; set; }
        /// <summary>
        /// Collection of validation messages
        /// </summary>
        public IEnumerable<IMessage> Messages { get; set; }
    }
}
