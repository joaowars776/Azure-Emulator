using Azure.Database.Manager.Database.Session_Details.Interfaces;

namespace Azure.Database
{
    public sealed class DatabaseManager
    {
        private readonly string _connectionStr;
        private readonly string _typer;

        public DatabaseManager(string connectionStr, string connType)
        {
            _connectionStr = connectionStr;
            _typer = connType;
        }

        public IQueryAdapter GetQueryReactor()
        {
            IDatabaseClient databaseClient = new DatabaseConnection(this._connectionStr, this._typer);
            databaseClient.Connect();
            databaseClient.Prepare();
            return databaseClient.GetQueryReactor();
        }

        public void Destroy() { }
    }
}