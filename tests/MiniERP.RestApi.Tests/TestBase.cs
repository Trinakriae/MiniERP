namespace MiniERP.RestApi.Tests
{
    public abstract class TestBase : IAsyncLifetime
    {
        protected readonly CustomWebApplicationFactory<Program> Factory;
        protected readonly HttpClient Client;

        protected TestBase()
        {
            Factory = new CustomWebApplicationFactory<Program>();
            Client = Factory.CreateClient();
        }

        public Task InitializeAsync()
        {
            // Reset DBs and seed fresh data before each test
            TestDataManager.ResetAll(Factory.Services);
            return Task.CompletedTask;
        }

        public Task DisposeAsync()
        {
            // Nothing to clean up per test
            return Task.CompletedTask;
        }
    }
}