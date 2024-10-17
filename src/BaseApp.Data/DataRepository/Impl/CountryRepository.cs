using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseApp.Data.DataContext.Entities;
using BaseApp.Data.Extensions;
using BaseApp.Data.Infrastructure;
using BaseApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BaseApp.Data.DataRepository.Impl
{
    public class CountryRepository(DataContextProvider context) : RepositoryBase(context), ICountryRepository
    {
        public async Task<List<Country>> GetCountriesAsync(string search, PagingSortingInfo pagingSorting)
        {
            var query = Context.Set<Country>().AsQueryable();
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                query = query.Where(x => x.Name.Contains(search));
            }

            return await query.PagingSorting(pagingSorting).ToListAsync();
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
