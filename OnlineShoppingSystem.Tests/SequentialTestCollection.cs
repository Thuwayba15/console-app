using Xunit;

namespace OnlineShoppingSystem.Tests;

/// <summary>
/// Test collection to disable parallel execution for tests that use AppDataStore singleton
/// This prevents race conditions when multiple tests access the shared singleton
/// </summary>
[CollectionDefinition("Sequential", DisableParallelization = true)]
public class SequentialTestCollection
{
    // This class is just a marker for the collection
    // Tests in this collection will run sequentially, not in parallel
}
