namespace shared;

public class DbInitializer : IDbInitializer
{
    public void Initialize(Context context)
    {
        context.Database.EnsureCreated();
        context.SaveChanges();
    }
}