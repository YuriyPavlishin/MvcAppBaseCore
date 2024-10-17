using System.Collections.Generic;
using System.Threading.Tasks;
using BaseApp.Data.DataContext.Entities;
using BaseApp.Data.Models;

namespace BaseApp.Data.DataRepository
{
    public interface ICountryRepository
    {
        Task<List<Country>> GetCountriesAsync(string search, PagingSortingInfo pagingSorting);
        List<State> GetStates(int? countryId);
    }
}
