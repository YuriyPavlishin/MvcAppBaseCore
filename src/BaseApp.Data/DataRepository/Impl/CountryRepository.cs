using System.Collections.Generic;
using System.Linq;
using BaseApp.Data.DataContext.Entities;
using BaseApp.Data.Extensions;
using BaseApp.Data.Infrastructure;
using BaseApp.Data.Models;

namespace BaseApp.Data.DataRepository.Impl
{
    public class CountryRepository : RepositoryBase, ICountryRepository
    {
        public CountryRepository(DataContextProvider context) : base(context)
        {
        }

        public List<Country> GetCountries()
        {
            return Context.Set<Country>().OrderBy(m => m.Ordinal).ThenBy(m => m.Name).ToList();
        }

        public List<Country> GetCountries(string search, PagingSortingInfo pagingSorting)
        {
            var query = Context.Set<Country>().AsQueryable();
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                query = query.Where(x => x.Name.Contains(search));
            }

            return query
                .PagingSorting(pagingSorting).ToList();
        }

        public List<State> GetStates(int? countryId)
        {
            var q = Context.Set<State>().AsEnumerable();
            if (countryId != null)
            {
                q = q.Where(m => m.CountryId == countryId);
            }
            return q.ToList();
        }
    }
}
