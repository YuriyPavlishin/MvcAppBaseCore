using System;

namespace BaseApp.Data.DataContext.Interfaces
{
    public interface IDeletable
    {
        DateTime? DeletedDate { get; set; }
    }
}
