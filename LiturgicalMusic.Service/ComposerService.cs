using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiturgicalMusic.Service.Common;
using LiturgicalMusic.Model.Common;
using LiturgicalMusic.Repository;
using LiturgicalMusic.Model;

namespace LiturgicalMusic.Service
{
    public class ComposerService : IComposerService
    {
        protected UnitOfWork UnitOfWork { get; private set; }

        public ComposerService(UnitOfWork unitOfWork)
        {
            this.UnitOfWork = unitOfWork;
        }

        public IComposer Create()
        {
            return new Composer();
        }

        public async Task<List<IComposer>> GetAsync()
        {
            return await UnitOfWork.ComposerRepository.GetAsync();
        }

        public async Task<IComposer> GetByIdAsync(int composerId)
        {
            return await UnitOfWork.ComposerRepository.GetByIdAsync(composerId);
        }

        public async Task<IComposer> InsertAsync(IComposer composer)
        {
            return await UnitOfWork.ComposerRepository.InsertAsync(composer);
        }
    }
}
