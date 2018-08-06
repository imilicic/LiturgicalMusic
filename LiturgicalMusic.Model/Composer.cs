using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiturgicalMusic.Model.Common;

namespace LiturgicalMusic.Model
{
    public class Composer: IComposer
    {
        #region Properties

        /// <summary>
        ///  Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets composer name.
        /// </summary>
        /// <value>The composer name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets composer surname.
        /// </summary>
        /// <value>The composer surname.</value>
        public string Surname { get; set; }
        #endregion Properties
    }
}
