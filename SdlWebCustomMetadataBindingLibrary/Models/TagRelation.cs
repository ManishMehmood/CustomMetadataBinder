using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trisoft.InfoShare.Plugins.SDK.Extensions.MetadataBinding;

namespace SdlWebCustomMetadataBindingLibrary.Models
{
    /// <summary>
    /// Simplistic and minimal implementation of the <code>ITagRelation</code> interface.
    /// Allows to recreate the complete tree on the client side.
    /// </summary>
    public class TagRelation : ITagRelation
    {
        /// <summary>
        /// The id of the parent. 
        /// When not set, the relation is interpreted as pointing to the root node(s).
        /// </summary>
        public string FromId { get; set; }
        /// <summary>
        /// The id of the child.
        /// When FromId is empty, contains id of the root node(s).
        /// </summary>
        public string ToId { get; set; }
    }
}
