using Contracts;
using Entities;

namespace Repository;

public class RepositoryManager : IRepositoryManager
{
    private ApplicationContext _repositoryContext;
    private IBookRepository _bookRepository;
    public RepositoryManager(ApplicationContext repositoryContext)
    {
        _repositoryContext = repositoryContext;
    }
    public IBookRepository Book
    {
        get
        {
            if(_bookRepository == null)
                _bookRepository = new BookRepository(_repositoryContext);
            return _bookRepository;
        }
    }
    public Task SaveAsync() => _repositoryContext.SaveChangesAsync();
}
