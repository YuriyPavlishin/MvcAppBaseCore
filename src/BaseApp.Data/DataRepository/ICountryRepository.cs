using System.Collections.Generic;
using BaseApp.Data.DataContext.Entities;
using BaseApp.Data.Models;

namespace BaseApp.Data.DataRepository
{
    public interface ICountryRepository
    {
        List<Country> GetCountries();
        List<Country> GetCountries(string search, PagingSortingInfo pagingSorting);
        List<State> GetStates(int? countryId);
    }
}
