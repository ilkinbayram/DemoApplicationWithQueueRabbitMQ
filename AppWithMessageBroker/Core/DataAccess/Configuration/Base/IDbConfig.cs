namespace Core.DataAccess.Configuration.Base
{
    public interface IDbConfig
    {
        string DATABASE_NAME { get; set; }
        string CONNECTION_STRING { get; set; }
    }
}
