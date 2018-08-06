using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiturgicalMusic.Model.Common
{
    public interface IComposer
    {
        #region Properties

        /// <summary>
        ///  Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        int Id { get; set; }

        /// <summary>
        /// Gets or sets composer name.
        /// </summary>
        /// <value>The composer name.</value>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets composer surname.
        /// </summary>
        /// <value>The composer surname.</value>
        string Surname { get; set; }
        #endregion Properties
    }
}
