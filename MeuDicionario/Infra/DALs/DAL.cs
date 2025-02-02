using Microsoft.EntityFrameworkCore;

namespace MeuDicionario.Infra.DALs
{
    public abstract class DAL<T> where T : class
    {
        private readonly DbContext _context;

        public DAL(DbContext context)
        {
            _context = context;
        }

        public IEnumerable<T> ListAll()
        {
            return _context.Set<T>().ToList();
        }

        public IEnumerable<T> ListOrderByFirstLast(int skip, int take)
        {
            return _context.Set<T>().Skip(skip).Take(take).ToList();
        }

        public IEnumerable<T> ListOrderByLastFirst(int skip, int take, Func<T, int> func)
        {
            return _context.Set<T>().OrderByDescending(func).Skip(skip).Take(take).ToList();
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

        public void RemoveRange(IEnumerable<T> list)
        {
            _context.Set<T>().RemoveRange(list);
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

        public IEnumerable<T> FindBySome(Func<T, bool> func)
        {
            return _context.Set<T>().Where(func).ToList();
        }

        public bool Has(Func<T, bool> func)
        {
            return _context.Set<T>().Any(func);
        }

        public bool HasOne()
        {
            return _context.Set<T>().Any();
        }

        public T GetFirst()
        {
            return _context.Set<T>().FirstOrDefault();
        }

        public T GetLast(Func<T, int> func)
        {
            return _context.Set<T>().OrderByDescending(func).FirstOrDefault();
        }

        public int GetCount()
        {
            return _context.Set<T>().Count();
        }

        public DbContext GetContex()
        {
            return _context;
        }
    }
}
