﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiturgicalMusic.Model.Common;

namespace LiturgicalMusic.Model
{
    public class Song : ISong
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public IList<bool> Template { get; set; }
        public string Type { get; set; }
        public string Code { get; set; }
        public string Source { get; set; }
        public string OtherInformations { get; set; }
        public IList<IStanza> Stanzas { get; set; }
        public IComposer Composer { get; set; }
        public IComposer Arranger { get; set; }
        public IList<IInstrumentalPart> InstrumentalParts { get; set; }
        public IList<int> ThemeCategories { get; set; }
        public IList<int> LiturgyCategories { get; set; }
    }
}
