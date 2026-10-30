using CenimaManagmentSystem.Data;
using CenimaManagmentSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;


namespace CenimaManagmentSystem.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _Context; //= new ApplicationDbContext();

        ///////// generic Repository => 

        private DbSet<T> _dbSet;

        public Repository(ApplicationDbContext context)
        {
            _Context = context;
            _dbSet = _Context.Set<T>();
           
        }
        // => switch all (_Context.Categories) with   (_dbset) 
        // =>       and        (Category)      with   ( T )
        // =>       and        (Categories)    with   ( entities )
        // =>       and        (category)      with   ( entity )

        //////////////////////////////////////
        public async Task<EntityEntry<T>> AddAsync(T entity)
        {
            return await _dbSet.AddAsync(entity);
        }

        public async Task<IEnumerable<T>> GetListAsync(
            Expression<Func<T, bool>>? filter = null,        //  عشان اخلي الفلتر من اليوزر
            Expression<Func<T, object>>[]? includes = null,  // سيم بس ب لست و السينتاكس من الانستركشنز لما بقف علي كلمه انكلود
            bool isTraked = true

            )
        {
            var entities = Query(filter, includes, isTraked);

            return await entities.ToListAsync();
        }

        ////////////////////////////////////////////// نفس الفانكشن بس الي فوق لو عايز ارجع اكتر من حاجه
        ///////////////////دي لو عايز ارجع حاجه واحده
        public async Task<T> GetOneAsync(             // make it get one and remove IEmurable bec of its return
          Expression<Func<T, bool>>? filter = null,        //  عشان اخلي الفلتر من اليوزر
          Expression<Func<T, object>>[]? includes = null,  // سيم بس ب لست و السينتاكس من الانستركشنز لما بقف علي كلمه انكلود
          bool isTraked = true

          )
        {
            var entities = Query(filter, includes, isTraked);
            return await entities.FirstOrDefaultAsync(); 
        }




        public IQueryable<T> Query(    // دي فانكشن فيها الكود المتكرر و بعمله كولينج في  جيت وان و جيت ليست
          Expression<Func<T, bool>>? filter = null,        //  عشان اخلي الفلتر من اليوزر
          Expression<Func<T, object>>[]? includes = null,  // سيم بس ب لست و السينتاكس من الانستركشنز لما بقف علي كلمه انكلود
          bool isTraked = true

          )
        {
            var entities = _dbSet.AsQueryable();
            // Filter

            if (filter != null) // تحنبا للايرورز
            {
                entities = entities.Where(filter);
            }
            if (includes != null)
            {
                foreach (var include in includes)         // لوب عشان دي انكلود  (join)<=
                    entities = entities.Include(include);
            }

            if (!isTraked)
            {
                entities = entities.AsNoTracking();
            }
            return entities;
        }
       



        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }
        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task<int> CommitAsync()
        {
            try // عشلت ساعات السيف بيضرب ايرورز عشان ممكن يبعت نال لو مدخلتش بيانات صح
            {
                return await _Context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                return -1;
            }
        
        }




    }
}
//// ===>                      /////////////// بنروح بعد منخلص نمسك الكنترولرز نعدل القديم بالجديد 
 //// ===>                      /// no more _context 
  //// ===>                      /// Async + await 