﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiturgicalMusic.Common
{
    public class Options : IOptions
    {
        #region Properties
        /// <summary>
        /// Gets or sets whether to include stanzas
        /// </summary>
        /// <value><c>true</c> if you want to include stanzas, otherwise <c>false</c></value>
        public bool Stanzas { get; set; }

        /// <summary>
        /// Gets or sets whether to include composer
        /// </summary>
        /// <value><c>true</c> if you want to include composer, otherwise <c>false</c></value>
        public bool Composer { get; set; }

        /// <summary>
        /// Gets or sets whether to include arranger
        /// </summary>
        /// <value><c>true</c> if you want to include arranger, otherwise <c>false</c></value>
        public bool Arranger { get; set; }

        /// <summary>
        /// Gets or sets whether to include instrumental parts
        /// </summary>
        /// <value><c>true</c> if you want to include instrumental parts, otherwise <c>false</c></value>
        public bool InstrumentalParts { get; set; }

        /// <summary>
        /// Gets or sets whether to include theme categories
        /// </summary>
        /// <value><c>true</c> if you want to include theme categories, otherwise <c>false</c></value>
        public bool ThemeCategories { get; set; }

        /// <summary>
        /// Gets or sets whether to include liturgy categories
        /// </summary>
        /// <value><c>true</c> if you want to include liturgy categories, otherwise <c>false</c></value>
        public bool LiturgyCategories { get; set; }
        #endregion Properties
    }
}
