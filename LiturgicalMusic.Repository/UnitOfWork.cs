using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiturgicalMusic.Repository.Common;
using LiturgicalMusic.DAL;
using AutoMapper;

namespace LiturgicalMusic.Repository
{
    public class UnitOfWork : IDisposable
    {
        private MusicContext _context;
        private IMapper _mapper;
        private bool disposed = false;

        private IComposerRepository _composerRepository;
        private IInstrumentalPartRepository _instrumentalPartRepository;
        private ISongRepository _songRepository;
        private IStanzaRepository _stanzaRepository;

        public UnitOfWork(MusicContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public IComposerRepository ComposerRepository
        {
            get
            {
                if (this._composerRepository == null)
                {
                    this._composerRepository = new ComposerRepository(_context, _mapper);
                }
                return this._composerRepository;
            }
        }

        public IInstrumentalPartRepository InstrumentalPartRepository
        {
            get
            {
                if (this._instrumentalPartRepository == null)
                {
                    this._instrumentalPartRepository = new InstrumentalPartRepository(_context, _mapper);
                }
                return this._instrumentalPartRepository;
            }
        }

        public ISongRepository SongRepository
        {
            get
            {
                if (this._songRepository == null)
                {
                    this._songRepository = new SongRepository(_context, _mapper);
                }
                return this._songRepository;
            }
        }

        public IStanzaRepository StanzaRepository
        {
            get
            {
                if (this._stanzaRepository == null)
                {
                    this._stanzaRepository = new StanzaRepository(_context, _mapper);
                }
                return this._stanzaRepository;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }

            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
