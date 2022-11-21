using ElectricalCarStoreApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ElectricalCarStoreApi.Context
{
    public class CarDb : DbContext
    {
        public CarDb(DbContextOptions<CarDb> options)
            : base(options) { }

        public DbSet<Car> Cars => Set<Car>();
    }
}
