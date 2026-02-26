using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Aveva.Platform.EntityMgmt.Client.Api.Models;
using Aveva.Platform.EntityMgmt.Tests.Benchmarks.Helpers;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Order;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;

namespace Aveva.Platform.EntityMgmt.Tests.Benchmarks.Serialization;

/// <summary>
/// Benchmarks comparing entity deserialization performance between Kiota and System.Text.Json.
/// </summary>
[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.Default)]
[RankColumn]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
#pragma warning disable CA1001 // Disposable handled by benchmark's GlobalCleanup
public class EntityDeserializationBenchmarks
#pragma warning restore CA1001 // Types that own disposable fields should be disposable
{
    private MemoryStream _simpleEntityJsonStream = null!;
    private MemoryStream _complexEntityJsonStream = null!;
    private MemoryStream _bulkEntities10JsonStream = null!;
    private MemoryStream _bulkEntities100JsonStream = null!;

    private JsonSerializerOptions _systemTextJsonOptions = null!;

    [GlobalSetup]
    public void Setup()
    {
        // Setup Kiota deserializers (register globally)
        ApiClientBuilder.RegisterDefaultDeserializer<JsonParseNodeFactory>();

        _systemTextJsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) },
        };

        // Prepare JSON streams for deserialization
        var simpleEntity = new EntityBuilder()
            .WithId("simple-entity")
            .WithName("Simple Test Entity")
            .BuildEntityRequest();

        var simpleEntityBytes = JsonSerializer.SerializeToUtf8Bytes(simpleEntity, _systemTextJsonOptions);
        _simpleEntityJsonStream = new MemoryStream(simpleEntityBytes, writable: false);

        var complexEntity = new EntityBuilder()
            .FullyPopulated()
            .WithAllPropertyTypes()
            .BuildEntityRequest();

        var complexEntityBytes = JsonSerializer.SerializeToUtf8Bytes(complexEntity, _systemTextJsonOptions);
        _complexEntityJsonStream = new MemoryStream(complexEntityBytes, writable: false);

        var bulkEntities10 = new BulkEntities
        {
            Items = [.. Enumerable.Range(0, 10)
                .Select(i => new EntityBuilder()
                    .WithId($"bulk-entity-{i}")
                    .WithName($"Bulk Entity {i}")
                    .BuildEntityRequest())],
        };
        var bulkEntities10Bytes = JsonSerializer.SerializeToUtf8Bytes(bulkEntities10, _systemTextJsonOptions);
        _bulkEntities10JsonStream = new MemoryStream(bulkEntities10Bytes, writable: false);

        var bulkEntities100 = new BulkEntities
        {
            Items = [.. Enumerable.Range(0, 100)
                .Select(i => new EntityBuilder()
                    .WithId($"bulk-entity-{i}")
                    .WithName($"Bulk Entity {i}")
                    .BuildEntityRequest())],
        };
        var bulkEntities100Bytes = JsonSerializer.SerializeToUtf8Bytes(bulkEntities100, _systemTextJsonOptions);
        _bulkEntities100JsonStream = new MemoryStream(bulkEntities100Bytes, writable: false);
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _simpleEntityJsonStream?.Dispose();
        _complexEntityJsonStream?.Dispose();
        _bulkEntities10JsonStream?.Dispose();
        _bulkEntities100JsonStream?.Dispose();
    }

    [IterationSetup]
    public void ResetStreams()
    {
        // Reset stream positions before each benchmark iteration
        _simpleEntityJsonStream.Position = 0;
        _complexEntityJsonStream.Position = 0;
        _bulkEntities10JsonStream.Position = 0;
        _bulkEntities100JsonStream.Position = 0;
    }

    #region Simple Entity Benchmarks

    [Benchmark(Description = "Simple Entity - System.Text.Json", Baseline = true)]
    [BenchmarkCategory("Simple")]
    public async Task<Entity> DeserializeSimpleEntity_SystemTextJson()
    {
        return (await JsonSerializer.DeserializeAsync<Entity>(_simpleEntityJsonStream, _systemTextJsonOptions))!;
    }

    [Benchmark(Description = "Simple Entity - Kiota")]
    [BenchmarkCategory("Simple")]
    public async Task<Entity> DeserializeSimpleEntity_Kiota()
    {
        return (await KiotaJsonSerializer.DeserializeAsync(_simpleEntityJsonStream, Entity.CreateFromDiscriminatorValue))!;
    }

    #endregion

    #region Complex Entity Benchmarks

    [Benchmark(Description = "Complex Entity - System.Text.Json", Baseline = true)]
    [BenchmarkCategory("Complex")]
    public async Task<Entity> DeserializeComplexEntity_SystemTextJson()
    {
        return (await JsonSerializer.DeserializeAsync<Entity>(_complexEntityJsonStream, _systemTextJsonOptions))!;
    }

    [Benchmark(Description = "Complex Entity - Kiota")]
    [BenchmarkCategory("Complex")]
    public async Task<Entity> DeserializeComplexEntity_Kiota()
    {
        return (await KiotaJsonSerializer.DeserializeAsync(_complexEntityJsonStream, Entity.CreateFromDiscriminatorValue))!;
    }

    #endregion

    #region Bulk Entity Benchmarks

    [Benchmark(Description = "Bulk Entities (10) - System.Text.Json", Baseline = true)]
    [BenchmarkCategory("Bulk-10")]
    public async Task<BulkEntities> DeserializeBulkEntities10_SystemTextJson()
    {
        return (await JsonSerializer.DeserializeAsync<BulkEntities>(_bulkEntities10JsonStream, _systemTextJsonOptions))!;
    }

    [Benchmark(Description = "Bulk Entities (10) - Kiota")]
    [BenchmarkCategory("Bulk-10")]
    public async Task<BulkEntities> DeserializeBulkEntities10_Kiota()
    {
        return (await KiotaJsonSerializer.DeserializeAsync(_bulkEntities10JsonStream, BulkEntities.CreateFromDiscriminatorValue))!;
    }

    [Benchmark(Description = "Bulk Entities (100) - System.Text.Json", Baseline = true)]
    [BenchmarkCategory("Bulk-100")]
    public async Task<BulkEntities> DeserializeBulkEntities100_SystemTextJson()
    {
        return (await JsonSerializer.DeserializeAsync<BulkEntities>(_bulkEntities100JsonStream, _systemTextJsonOptions))!;
    }

    [Benchmark(Description = "Bulk Entities (100) - Kiota")]
    [BenchmarkCategory("Bulk-100")]
    public async Task<BulkEntities> DeserializeBulkEntities100_Kiota()
    {
        return (await KiotaJsonSerializer.DeserializeAsync(_bulkEntities100JsonStream, BulkEntities.CreateFromDiscriminatorValue))!;
    }

    #endregion

    #region Response Type Benchmarks

    [Benchmark(Description = "EntityResponse - System.Text.Json", Baseline = true)]
    [BenchmarkCategory("Response")]
    public async Task<EntityResponse> DeserializeEntityResponse_SystemTextJson()
    {
        return (await JsonSerializer.DeserializeAsync<EntityResponse>(_complexEntityJsonStream, _systemTextJsonOptions))!;
    }

    [Benchmark(Description = "EntityResponse - Kiota")]
    [BenchmarkCategory("Response")]
    public async Task<EntityResponse> DeserializeEntityResponse_Kiota()
    {
        return (await KiotaJsonSerializer.DeserializeAsync(_complexEntityJsonStream, EntityResponse.CreateFromDiscriminatorValue))!;
    }

    #endregion
}
