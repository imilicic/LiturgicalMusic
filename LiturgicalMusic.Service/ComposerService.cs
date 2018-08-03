using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiturgicalMusic.Service.Common;
using LiturgicalMusic.Model.Common;
using LiturgicalMusic.Repository.Common;
using LiturgicalMusic.Model;

namespace LiturgicalMusic.Service
{
    public class ComposerService : IComposerService
    {
        protected IComposerRepository Repository { get; private set; }

        public ComposerService(IComposerRepository repository)
        {
            this.Repository = repository;
        }

        public IComposer CreateComposer()
        {
            return new Composer();
        }

        public async Task<List<IComposer>> GetComposersAsync()
        {
            return await Repository.GetComposersAsync();
        }

        public async Task<IComposer> InsertComposerAsync(IComposer composer)
        {
            return await Repository.InsertComposerAsync(composer);
        }
    }
}
