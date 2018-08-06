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
        #region Fields
        /// <summary>
        /// Represents context.
        /// </summary>
        private MusicContext _context;

        /// <summary>
        /// Represents mapper.
        /// </summary>
        private IMapper _mapper;

        /// <summary>
        /// Whether this class if disposed or not.
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Represents composer repository.
        /// </summary>
        private IComposerRepository _composerRepository;

        /// <summary>
        /// Represents instrumental part repository.
        /// </summary>
        private IInstrumentalPartRepository _instrumentalPartRepository;

        /// <summary>
        /// Represents song repository.
        /// </summary>
        private ISongRepository _songRepository;

        /// <summary>
        /// Represents stanza repository.
        /// </summary>
        private IStanzaRepository _stanzaRepository;
        #endregion Fields

        #region Constructors
        /// <summary>
        /// Initializes new instance of <see cref="UnitOfWork"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="mapper">The mapper.</param>
        public UnitOfWork(MusicContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }
        #endregion Constructors

        #region Properties
        /// <summary>
        /// Gets composer repository.
        /// </summary>
        /// <value>The composer repository.</value>
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

        /// <summary>
        /// Gets instrumental part repository.
        /// </summary>
        /// <value>The instrumental part repository.</value>
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

        /// <summary>
        /// Gets song repository.
        /// </summary>
        /// <value>The song repository.</value>
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

        /// <summary>
        /// Gets stanza repository.
        /// </summary>
        /// <value>The stanza repository.</value>
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
        #endregion Properties

        #region Methods
        /// <summary>
        /// Disposes this class.
        /// </summary>
        /// <param name="disposing">Whether to dispose or not.</param>
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

        /// <summary>
        /// Runs Dispose method.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion Methods
    }
}
