namespace MongoDbEntityFramework.Settings;

public class DbSettings
{
    public string ConnString { get; }
    public string DatabaseName { get; }

    public DbSettings(string connString, string databaseName)
    {
        ConnString = connString;
        DatabaseName = databaseName;
    }

}