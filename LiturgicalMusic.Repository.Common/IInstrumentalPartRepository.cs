using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiturgicalMusic.Model.Common;

namespace LiturgicalMusic.Repository.Common
{
    public interface IInstrumentalPartRepository
    {
        Task DeleteAsync(int instrumentalPartId);
        Task<List<IInstrumentalPart>> GetBySongAsync(int songId);
        Task<IInstrumentalPart> InsertAsync(IInstrumentalPart instrumentalPart, int songId);
        Task<IInstrumentalPart> UpdateAsync(IInstrumentalPart instrumentalPart);
    }
}
