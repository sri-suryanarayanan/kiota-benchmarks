using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Aveva.Platform.EntityMgmt.Client.Api.Models;
using Aveva.Platform.EntityMgmt.Tests.Benchmarks.Helpers;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Order;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using Microsoft.Kiota.Serialization.Json;

namespace Aveva.Platform.EntityMgmt.Tests.Benchmarks.Serialization;

/// <summary>
/// Benchmarks comparing HttpRequestMessage creation: Manual vs Kiota request builders.
/// This tests ONLY the request building infrastructure, not the full HTTP workflow.
/// </summary>
[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.Default)]
[RankColumn]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class HttpRequestMessageCreationBenchmarks
{
    private const string BaseUrl = "https://api.example.com";
    private Entity _entity = null!;
    private BulkEntities _bulkEntities = null!;
    private JsonSerializerOptions _jsonOptions = null!;
    private HttpClientRequestAdapter _requestAdapter = null!;

    [GlobalSetup]
    public void Setup()
    {
        // Setup JSON options
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            WriteIndented = false,
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
        };

        // Setup Kiota request adapter
        ApiClientBuilder.RegisterDefaultSerializer<JsonSerializationWriterFactory>();
        var authProvider = new AnonymousAuthenticationProvider();
        _requestAdapter = new HttpClientRequestAdapter(authProvider)
        {
            BaseUrl = BaseUrl
        };

        // Create test entities
        _entity = new EntityBuilder()
            .FullyPopulated()
            .BuildEntityRequest();

        _bulkEntities = new BulkEntities
        {
            Items = [.. Enumerable.Range(0, 10)
                .Select(i => new EntityBuilder()
                    .WithId($"bulk-entity-{i}")
                    .WithName($"Bulk Entity {i}")
                    .BuildEntityRequest())],
        };
    }

    #region Single Entity POST

    [Benchmark(Description = "POST Entity - Manual HttpRequestMessage", Baseline = true)]
    [BenchmarkCategory("POST-Single")]
    public HttpRequestMessage CreatePostRequest_Manual()
    {
        var json = JsonSerializer.Serialize(_entity, _jsonOptions);
        var request = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}/api/entities")
        {
            Content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json),
        };
        return request;
    }

    [Benchmark(Description = "POST Entity - Kiota Request Builder")]
    [BenchmarkCategory("POST-Single")]
    public async Task<HttpRequestMessage> CreatePostRequest_Kiota()
    {
        // Kiota's request builder infrastructure
        var requestInfo = new RequestInformation
        {
            HttpMethod = Method.POST,
            UrlTemplate = $"{BaseUrl}/api/entities",
        };
        requestInfo.SetContentFromParsable(_requestAdapter, "application/json", _entity);

        return (await _requestAdapter.ConvertToNativeRequestAsync<HttpRequestMessage>(requestInfo))!;
    }

    #endregion

    #region Bulk Entity POST

    [Benchmark(Description = "POST Bulk - Manual HttpRequestMessage", Baseline = true)]
    [BenchmarkCategory("POST-Bulk")]
    public HttpRequestMessage CreateBulkPostRequest_Manual()
    {
        var json = JsonSerializer.Serialize(_bulkEntities, _jsonOptions);
        var request = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}/api/entities/bulk/create")
        {
            Content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json),
        };
        return request;
    }

    [Benchmark(Description = "POST Bulk - Kiota Request Builder")]
    [BenchmarkCategory("POST-Bulk")]
    public async Task<HttpRequestMessage> CreateBulkPostRequest_Kiota()
    {
        var requestInfo = new RequestInformation
        {
            HttpMethod = Method.POST,
            UrlTemplate = $"{BaseUrl}/api/entities/bulk/create",
        };
        requestInfo.SetContentFromParsable(_requestAdapter, "application/json", _bulkEntities);

        return (await _requestAdapter.ConvertToNativeRequestAsync<HttpRequestMessage>(requestInfo))!;
    }

    #endregion

    #region PUT Entity

    [Benchmark(Description = "PUT Entity - Manual HttpRequestMessage", Baseline = true)]
    [BenchmarkCategory("PUT")]
    public HttpRequestMessage CreatePutRequest_Manual()
    {
        var json = JsonSerializer.Serialize(_entity, _jsonOptions);
        var request = new HttpRequestMessage(HttpMethod.Put, $"{BaseUrl}/api/entities/{_entity.Id}")
        {
            Content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json),
        };
        return request;
    }

    [Benchmark(Description = "PUT Entity - Kiota Request Builder")]
    [BenchmarkCategory("PUT")]
    public async Task<HttpRequestMessage> CreatePutRequest_Kiota()
    {
        var requestInfo = new RequestInformation
        {
            HttpMethod = Method.PUT,
            UrlTemplate = $"{BaseUrl}/api/entities/{_entity.Id}",
        };
        requestInfo.SetContentFromParsable(_requestAdapter, "application/json", _entity);

        return (await _requestAdapter.ConvertToNativeRequestAsync<HttpRequestMessage>(requestInfo))!;
    }

    #endregion

    #region GET Entity

    [Benchmark(Description = "GET Entity - Manual HttpRequestMessage", Baseline = true)]
    [BenchmarkCategory("GET")]
    public HttpRequestMessage CreateGetRequest_Manual()
    {
        return new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}/api/entities/{_entity.Id}");
    }

    [Benchmark(Description = "GET Entity - Kiota Request Builder")]
    [BenchmarkCategory("GET")]
    public async Task<HttpRequestMessage> CreateGetRequest_Kiota()
    {
        var requestInfo = new RequestInformation
        {
            HttpMethod = Method.GET,
            UrlTemplate = $"{BaseUrl}/api/entities/{_entity.Id}",
        };

        return (await _requestAdapter.ConvertToNativeRequestAsync<HttpRequestMessage>(requestInfo))!;
    }

    #endregion

    #region With Headers

    [Benchmark(Description = "POST with Headers - Manual", Baseline = true)]
    [BenchmarkCategory("Headers")]
    public HttpRequestMessage CreatePostWithHeaders_Manual()
    {
        var json = JsonSerializer.Serialize(_entity, _jsonOptions);
        var request = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}/api/entities")
        {
            Content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json),
        };
        request.Headers.Add("X-Custom-Header", "BenchmarkValue");
        request.Headers.Add("DataSource", "TestSource");
        return request;
    }

    [Benchmark(Description = "POST with Headers - Kiota")]
    [BenchmarkCategory("Headers")]
    public async Task<HttpRequestMessage> CreatePostWithHeaders_Kiota()
    {
        var requestInfo = new RequestInformation
        {
            HttpMethod = Method.POST,
            UrlTemplate = $"{BaseUrl}/api/entities",
        };
        requestInfo.SetContentFromParsable(_requestAdapter, "application/json", _entity);
        requestInfo.Headers.Add("X-Custom-Header", "BenchmarkValue");
        requestInfo.Headers.Add("DataSource", "TestSource");

        return (await _requestAdapter.ConvertToNativeRequestAsync<HttpRequestMessage>(requestInfo))!;
    }

    #endregion

    [GlobalCleanup]
    public void Cleanup()
    {
        _requestAdapter?.Dispose();
    }
}
