namespace N5.Challenge.Services.Security.Domain.SeedWork
{
    public abstract class Entity
    {
        int _id;

        public virtual int Id
        {
            get
            {
                return _id;
            }
            protected set
            {
                _id = value;
            }
        }

        public bool IsTransient()
        {
            return this.Id == default(Int32);
        }
    }
}