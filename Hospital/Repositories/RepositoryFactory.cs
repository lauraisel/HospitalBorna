namespace Hospital.Repositories
{
    public class RepositoryFactory
    {
        private readonly Lazy<AppDbContext> _lazyContext;

        public RepositoryFactory(Lazy<AppDbContext> lazyContext)
        {
            _lazyContext = lazyContext;
        }

        public IRepository<T> CreateRepository<T>() where T : class
        {
            return new Repository<T>(_lazyContext.Value);
        }
    }

}
