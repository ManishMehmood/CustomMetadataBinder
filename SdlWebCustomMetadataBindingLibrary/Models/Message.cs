using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trisoft.InfoShare.Plugins.SDK;
using Trisoft.InfoShare.Plugins.SDK.MessageRelatedInfo;

namespace SdlWebCustomMetadataBindingLibrary.Models
{
    /// <summary>
    /// Simplistic and minimal implementation of the <code>IMessage</code> interface.
    /// Allows to return some feedback to teh client side.
    /// </summary>
    public class Message : IMessage
    {
        /// <summary>
        /// Readable description in English.
        /// Serves as a fallback when the message is not found in the resource file with server errors.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// The message level
        /// </summary>
        public MessageLevel Level { get; set; }
        /// <summary>
        /// The error number
        /// </summary>
        public int Number { get; set; }
        /// <summary>
        /// Additional error parameters
        /// </summary>
        public IEnumerable<IMessageParam> Parameters { get; set; }
        /// <summary>
        /// The resource functional area reference. Links the message to the translatable resource
        /// </summary>
        public string ResouceLib { get; set; }
        /// <summary>
        /// The resource id. Links the message to the translatable resource
        /// </summary>
        public string ResourceId { get; set; }

        /// <summary>
        /// RelatedInfoIshObject - added by Damian Jewett, June 2019 to successfully compile
        /// </summary>
        public IshObject RelatedInfoIshObject { get; set; }

        /// <summary>
        /// RelatedInfoIshObject - added by Damian Jewett, June 2019 to successfully compile
        /// </summary>
        public RelatedInfo RelatedInfo { get; set; }
    }
}
