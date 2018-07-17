using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiturgicalMusic.Service.Common;
using LiturgicalMusic.Model.Common;
using LiturgicalMusic.Repository.Common;

namespace LiturgicalMusic.Service
{
    public class ComposerService : IComposerService
    {
        protected IComposerRepository Repository { get; private set; }

        public ComposerService(IComposerRepository repository)
        {
            this.Repository = repository;
        }

        public IComposer CreateComposer(IComposer composer)
        {
            return Repository.CreateComposer(composer);
        }

        public List<IComposer> GetAllComposers()
        {
            return Repository.GetAllComposers();
        }
    }
}
