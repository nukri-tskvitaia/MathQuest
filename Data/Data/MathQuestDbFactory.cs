using Data.Data.InitialDataFactory;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Data.Data
{
    public class MathQuestDbFactory
    {
        private readonly AbstractDataFactory factory;

        public MathQuestDbFactory(AbstractDataFactory factory)
        {
            this.factory = factory;
        }
        /* // For testing
        public MathQuestDbContext CreateContext()
        {
            var context = new MathQuestDbContext(CreateOptions(), factory);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            return context;
        } */

        public static DbContextOptions<MathQuestDbContext> CreateOptions()
        {
            return new DbContextOptionsBuilder<MathQuestDbContext>()
                .UseSqlServer(CreateConnectionString()).Options;
        }

        private static string CreateConnectionString()
        {
            var projectDir = Directory.GetParent(Environment.CurrentDirectory)!.Parent!.Parent!.Parent!.Parent!.FullName;
            var dbPath = Path.Combine(projectDir, "Data\\DB\\Test", "MathQuest.mdf");

            var conString = new SqlConnectionStringBuilder 
            { 
                DataSource = "(localdb)\\mssqllocaldb",
                AttachDBFilename = dbPath,
                IntegratedSecurity = true

            }.ConnectionString;
            
            return conString;
        }
    }
}
