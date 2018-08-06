using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiturgicalMusic.Model.Common
{
    public interface IStanza
    {
        #region Properties

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        int Id { get; set; }

        /// <summary>
        /// Gets or sets stanza number.
        /// </summary>
        /// <value>The number.</value>
        int Number { get; set; }

        /// <summary>
        /// Gets or sets stanza text.
        /// </summary>
        /// <value>The text.</value>
        string Text { get; set; }
        #endregion Properties
    }
}
