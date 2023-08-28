namespace OsDsII.DAL
{
    public interface IUnitOfWork
    {
        public Task SaveChangesAsync();
    }
}