using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace CenimaManagmentSystem.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<EntityEntry<T>> AddAsync(T entity);

         Task<IEnumerable<T>> GetListAsync(
            Expression<Func<T, bool>>? filter = null,        //  عشان اخلي الفلتر من اليوزر
            Expression<Func<T, object>>[]? includes = null,  // سيم بس ب لست و السينتاكس من الانستركشنز لما بقف علي كلمه انكلود
            bool isTraked = true

            );

        ////////////////////////////////////////////// نفس الفانكشن بس الي فوق لو عايز ارجع اكتر من حاجه
        ///////////////////دي لو عايز ارجع حاجه واحده
         Task<T> GetOneAsync(             // make it get one and remove IEmurable bec of its return
          Expression<Func<T, bool>>? filter = null,        //  عشان اخلي الفلتر من اليوزر
          Expression<Func<T, object>>[]? includes = null,  // سيم بس ب لست و السينتاكس من الانستركشنز لما بقف علي كلمه انكلود
          bool isTraked = true

          );




        IQueryable<T> Query(    // دي فانكشن فيها الكود المتكرر و بعمله كولينج في  جيت وان و جيت ليست
          Expression<Func<T, bool>>? filter = null,        //  عشان اخلي الفلتر من اليوزر
          Expression<Func<T, object>>[]? includes = null,  // سيم بس ب لست و السينتاكس من الانستركشنز لما بقف علي كلمه انكلود
          bool isTraked = true

          );




        void Update(T entity);
        void Delete(T entity);

        Task<int> CommitAsync();
    }
}
