
BenchmarkDotNet v0.15.8, Windows 11 (10.0.22631.6649/23H2/2023Update/SunValley3)
Intel Core Ultra 7 165H 3.80GHz, 1 CPU, 22 logical and 16 physical cores
.NET SDK 10.0.101
  [Host]     : .NET 8.0.24 (8.0.24, 8.0.2426.7010), X64 RyuJIT x86-64-v3
  Job-CNUJVU : .NET 8.0.24 (8.0.24, 8.0.2426.7010), X64 RyuJIT x86-64-v3

InvocationCount=1  UnrollFactor=1  

 Method                                   | Mean        | Error      | StdDev      | Median      | Ratio        | RatioSD | Rank | Allocated  | Alloc Ratio |
----------------------------------------- |------------:|-----------:|------------:|------------:|-------------:|--------:|-----:|-----------:|------------:|
 'Bulk Entities (10) - System.Text.Json'  |   148.65 μs |  13.197 μs |    38.08 μs |   133.65 μs |     baseline |         |    1 |   27.53 KB |             |
 'Bulk Entities (10) - Kiota'             |   870.35 μs | 119.410 μs |   352.08 μs |   685.80 μs | 6.19x slower |   2.88x |    2 |   144.8 KB |  5.26x more |
                                          |             |            |             |             |              |         |      |            |             |
 'Bulk Entities (100) - System.Text.Json' |   883.03 μs |  21.072 μs |    57.68 μs |   881.50 μs |     baseline |         |    1 |   98.26 KB |             |
 'Bulk Entities (100) - Kiota'            | 7,781.75 μs | 934.236 μs | 2,754.62 μs | 6,689.65 μs | 8.85x slower |   3.17x |    2 | 1313.26 KB | 13.37x more |
                                          |             |            |             |             |              |         |      |            |             |
 'Complex Entity - System.Text.Json'      |   225.16 μs |   8.049 μs |    22.03 μs |   219.70 μs |     baseline |         |    1 |   35.25 KB |             |
 'Complex Entity - Kiota'                 | 1,431.62 μs |  81.931 μs |   236.39 μs | 1,351.00 μs | 6.41x slower |   1.19x |    2 |  286.88 KB |  8.14x more |
                                          |             |            |             |             |              |         |      |            |             |
 'EntityResponse - System.Text.Json'      |   232.79 μs |  13.263 μs |    38.27 μs |   221.35 μs |     baseline |         |    1 |   35.33 KB |             |
 'EntityResponse - Kiota'                 | 1,985.90 μs | 291.010 μs |   858.05 μs | 1,491.80 μs | 8.74x slower |   4.03x |    2 |  288.06 KB |  8.15x more |
                                          |             |            |             |             |              |         |      |            |             |
 'Simple Entity - System.Text.Json'       |    56.37 μs |   4.039 μs |    11.59 μs |    53.40 μs |     baseline |         |    1 |   19.54 KB |             |
 'Simple Entity - Kiota'                  |   431.22 μs |  42.171 μs |   122.35 μs |   473.70 μs | 7.95x slower |   2.74x |    2 |   23.98 KB |  1.23x more |
