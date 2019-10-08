using Elevar.Domain.Specs;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("Elevar.Tests")]
namespace Elevar.Domain
{
    public interface IRepository<T>
    {
        Task<T> GetByIdAsync(int id);
        Task<IList<T>> GetAllAsync(Specification<T> specification); 
    }
}