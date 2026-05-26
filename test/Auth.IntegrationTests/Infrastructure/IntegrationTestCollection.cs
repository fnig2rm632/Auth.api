using Xunit;

namespace Auth.IntegrationTest.Infrastructure;

[CollectionDefinition("Integration Tests")]
public class IntegrationTestCollection : ICollectionFixture<TestInfrastructureFixture>;