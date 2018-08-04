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

        public IComposer Create()
        {
            return new Composer();
        }

        public async Task<List<IComposer>> GetAsync()
        {
            return await Repository.GetAsync();
        }

        public async Task<IComposer> GetByIdAsync(int composerId)
        {
            return await Repository.GetByIdAsync(composerId);
        }

        public async Task<IComposer> InsertAsync(IComposer composer)
        {
            return await Repository.InsertAsync(composer);
        }
    }
}
