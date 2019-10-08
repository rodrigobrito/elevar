using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Elevar.Domain.Specs;

namespace Elevar.Tests.Domain.Specs.Utils
{
    public class SampleRepository : ReadOnlyCollection<string>
    {
        public SampleRepository()
            : base(new[] { "Rodrigo", "Elton", "Alison" })
        { }

        public IEnumerable<string> Find(Specification<string> specfication)
        {
            return this.AsQueryable().Where(specfication.ToExpression());
        }
    }
}
