using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiturgicalMusic.Model.Common;
using System.Linq.Expressions;

namespace LiturgicalMusic.Repository.Common
{
    public interface IStanzaRepository
    {
        Task DeleteAsync(int stanzaId);
        Task<List<IStanza>> GetBySongAsync(int songId);
        Task<IStanza> InsertAsync(IStanza stanza, int songId);
        Task<IStanza> UpdateAsync(IStanza stanza);
    }
}
