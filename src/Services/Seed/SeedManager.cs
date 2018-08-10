using System.Collections.Generic;
using System.Linq;
using CallGate.Data;
using CallGate.Seeders;

namespace CallGate.Services.Seed
{
    public class SeedManager : ISeedManager
    {
        private readonly IRethinkDbDelegateBus _rethinkDbDelegateBus;
        private readonly IDatabaseManager _databaseManager;
        private readonly IEnumerable<ISeeder> _seeders;

        public SeedManager(
            IRethinkDbDelegateBus rethinkDbDelegateBus,
            IDatabaseManager databaseManager,
            IEnumerable<ISeeder> seeders)
        {
            _rethinkDbDelegateBus = rethinkDbDelegateBus;
            _databaseManager = databaseManager;
            _seeders = seeders;
        }

        public void Seed()
        {
            var seeders = _seeders.ToList();
            seeders.Sort();

            foreach (var seeder in seeders)
            {
                seeder.Seed();
                _databaseManager.Commit();
                _rethinkDbDelegateBus.Commit();
            }
        }
    }
}
