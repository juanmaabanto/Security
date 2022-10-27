namespace N5.Challenge.Services.Security.Domain.SeedWork
{
    public interface IRepository<T> where T : Entity
    {
        IUnitOfWork UnitOfWork { get; }
    }
}