﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiturgicalMusic.Common
{
    public class Paging : IPaging
    {
        #region Properties

        /// <summary>
        /// Gets or sets current page number.
        /// </summary>
        /// <value>The page number.</value>
        public int PageNumber { get; set; }

        /// <summary>
        /// Gets or sets page size.
        /// </summary>
        /// <value>The page size.</value>
        public int PageSize { get; set; }
        #endregion Properties
    }
}
