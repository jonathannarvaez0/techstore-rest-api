namespace Application.Database
{
    public class DatabaseConnection
    {
        public string ConnectionString { get; }

        public DatabaseConnection(IConfiguration config)
        {
            this.ConnectionString = config.GetConnectionString("App");
        }
    }
}
