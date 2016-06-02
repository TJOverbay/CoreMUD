namespace CoreMUD.Interfaces
{
    public interface IHaveIdentity
    {
        string Id { get; }
    }

    public interface IHaveIdentity<T> : IHaveIdentity
    {
        new T Id { get; }
    }
}
