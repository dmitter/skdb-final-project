namespace OutsourceTracker.Acceptance.Tests.Fixture;

public class TestAppBuilder : EmptyTestAppBuilder
{
    protected override EmptyDatabaseInitializer CreateDatabaseInitializer()
    {
        return new DatabaseInitializer(ConnectionString);
    }
}