using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trisoft.InfoShare.Plugins.SDK.Extensions.MetadataBinding;

namespace SdlWebCustomMetadataBindingLibrary.Models
{
    /// <summary>
    /// Simplistic and minimal implementation of the <code>IStructureTag</code> interface.
    /// Comparing to a tag, contains an additional information required for the tree mode.
    /// </summary>
    public class StructureTag : Tag, IStructureTag
    {
        /// <summary>
        /// Defines whether the tag is selectable
        /// </summary>
        public bool IsSelectable { get; set; }
    }
}
