using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiturgicalMusic.Model.Common;

namespace LiturgicalMusic.Model
{
    public class Code: ICode
    {
        #region Properties

        /// <summary>
        /// Gets or sets voice code.
        /// </summary>
        /// <value>The voice code.</value>
        public string Voice { get; set; }

        /// <summary>
        /// Gets or sets a note to which voice is relative to.
        /// </summary>
        /// <value>The relative note.</value>
        public string VoiceRelative { get; set; }
        #endregion Properties
    }
}
