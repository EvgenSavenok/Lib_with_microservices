namespace Contracts;

public interface IRepositoryManager
{
    IBookRepository Book { get; }
    Task SaveAsync();
}
