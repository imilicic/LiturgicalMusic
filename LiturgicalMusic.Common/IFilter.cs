using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiturgicalMusic.Common
{
    public interface IFilter
    {
        #region Properties
        /// <summary>
        /// Gets or sets title.
        /// </summary>
        /// <value>The title.</value>
        string Title { get; set; }
        #endregion Properties
    }
}
