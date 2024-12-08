using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Model
{
    public class ConnectionContext : DbContext
    {
        private IConfiguration _configuration;

        public ConnectionContext(IConfiguration configuration, DbContextOptions options) : base(options)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var typedatabase = _configuration["Typedatabase"];
            var connectionString = _configuration.GetConnectionString(typedatabase);

            if (typedatabase == "MySql")
            {
                optionsBuilder.UseMySQL(connectionString);
            }
        }
    }
}
