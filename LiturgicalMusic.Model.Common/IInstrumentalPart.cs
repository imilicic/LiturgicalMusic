using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiturgicalMusic.Model.Common
{
    public interface IInstrumentalPart
    {
        #region Properties

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        int Id { get; set; }

        /// <summary>
        /// Gets or sets instrumental part position in song.
        /// </summary>
        /// <value><c>prelude</c>, <c>interlude</c> or <c>coda</c></value>
        string Position { get; set; }

        /// <summary>
        /// Gets or sets type of instrumental part.
        /// </summary>
        /// <value><c>hymn</c> or <c>psalm</c></value>
        string Type { get; set; }

        /// <summary>
        /// Gets or sets code of instrumental part.
        /// </summary>
        /// <value>The code.</value>
        string Code { get; set; }

        /// <summary>
        /// Gets or sets template of instrumental part.
        /// </summary>
        /// <value>The template of instrumental part.</value>
        IList<bool> Template { get; set; }
        #endregion Properties
    }
}
