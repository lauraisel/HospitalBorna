namespace Hospital.Repositories
{
    public class RepositoryFactory
    {
        private readonly AppDbContext _context;

        public RepositoryFactory(AppDbContext context)
        {
            _context = context;
        }

        public IRepository<T> CreateRepository<T>() where T : class
        {
            return new Repository<T>(_context);
        }
    }

}
