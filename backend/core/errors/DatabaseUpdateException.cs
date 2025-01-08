namespace core.errors;

public class DatabaseUpdateException : Exception
{
    public DatabaseUpdateException(string message) : base(message)
    {
        
    }
    
    public DatabaseUpdateException() : base("Error updating database")
    {
        
    }
}
