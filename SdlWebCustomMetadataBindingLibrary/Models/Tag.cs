using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trisoft.InfoShare.Plugins.SDK.Extensions.MetadataBinding;

namespace SdlWebCustomMetadataBindingLibrary.Models
{
    /// <summary>
    /// Simplistic and minimal implementation of the <code>ITag</code> interface.
    /// </summary>
    public class Tag : ITag
    {
        /// <summary>
        /// The tag label (the user-readable value shown in UI)
        /// </summary>
        public string Label { get; set; }
        /// <summary>
        /// Annotated label (the user-readable value shown in UI with some extra HTML markup,
        /// allowing highlighting inside the auto-suggest list).
        /// </summary>
        public string AnnotatedLabel { get; set; }
        /// <summary>
        /// The tag description (visible in UI)
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// The id of the tag (comes from the external taxonomy)
        /// Should be a unique value that allows to unambiguously link the tag to the taxonomy
        /// </summary>
        public string Id { get; set; }


    }
}
