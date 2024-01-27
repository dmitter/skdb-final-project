using OutsourceTracker.Acceptance.Tests.Fixture;

namespace OutsourceTracker.Acceptance.Tests.Import;

[CollectionDefinition(nameof(ImportFeature))]
public class ImportFeature : ICollectionFixture<EmptyTestAppBuilder>;