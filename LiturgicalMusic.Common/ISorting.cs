using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiturgicalMusic.Common
{
    public interface ISorting
    {
        #region Properties
        /// <summary>
        /// Gets or sets name of property to sort by.
        /// </summary>
        string SortBy { get; set; }

        /// <summary>
        /// Whether to sort ascending or descending.
        /// </summary>
        /// <value><c>true</c> if you want to sort ascending, <c>false</c> otherwise</value>
        bool SortAscending { get; set; }
        #endregion Properties
    }
}
