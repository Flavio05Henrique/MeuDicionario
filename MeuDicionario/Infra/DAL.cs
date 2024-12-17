using Microsoft.EntityFrameworkCore;

namespace MeuDicionario.Infra
{
    public abstract class DAL<T> where T : class
    {
        private readonly DbContext _context;

        public DAL(DbContext context)
        {
            _context = context;
        }

        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public IEnumerable<T> GetSome(int skip, int take)
        {
            return _context.Set<T>().ToList().Skip(skip).Take(take);
        }

        public void Add(T item)
        {
            _context.Set<T>().Add(item);
            _context.SaveChanges();
        }

        public void Remove(T item)
        {
            _context.Set<T>().Remove(item);
            _context.SaveChanges();
        }

        public void Update(T item)
        {
            _context.Set<T>().Update(item);
            _context.SaveChanges();
        }

        public T? FindBy(Func<T, bool> func)
        {
            return _context.Set<T>().FirstOrDefault(func);
        }

        public bool Has(Func<T, bool> func)
        {
            return _context.Set<T>().Any(func);
        }
    }
}
