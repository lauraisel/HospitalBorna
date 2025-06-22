namespace Hospital.Infrastructure
{
    public class LazyResolver<T> : Lazy<T>
    {
        public LazyResolver(IServiceProvider provider)
            : base(() => provider.GetRequiredService<T>())
        {
        }
    }
}
