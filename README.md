# kiota-client-benchmarks
Kiota Client Benchmarks

This repository contains benchmark tests for the Kiota HTTP client library.

To run the benchmarks, use the following command:
```powershell
dotnet run -c Release --project .\EntityMgmt.Tests.Benchmarks\EntityMgmt.Tests.Benchmarks.csproj -- --artifacts ".\EntityMgmt.Tests.Benchmarks\BenchmarkDotNet.Artifacts"
```
The benchmarks will be executed in Release configuration, and the results will be saved in the specified artifacts directory.
