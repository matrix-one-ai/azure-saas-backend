using Icon.EntityFrameworkCore;

namespace Icon.Migrations.Seed.Host
{
    public class InitialHostDbBuilder
    {
        private readonly IconDbContext _context;

        public InitialHostDbBuilder(IconDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            new DefaultEditionCreator(_context).Create();
            new DefaultLanguagesCreator(_context).Create();
            new HostRoleAndUserCreator(_context).Create();
            new DefaultSettingsCreator(_context).Create();

            _context.SaveChanges();
        }
    }
}
