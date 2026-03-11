
BenchmarkDotNet v0.15.8, Windows 11 (10.0.22631.6649/23H2/2023Update/SunValley3)
Intel Core Ultra 7 165H 3.80GHz, 1 CPU, 22 logical and 16 physical cores
.NET SDK 10.0.101
  [Host]     : .NET 8.0.24 (8.0.24, 8.0.2426.7010), X64 RyuJIT x86-64-v3
  Job-CNUJVU : .NET 8.0.24 (8.0.24, 8.0.2426.7010), X64 RyuJIT x86-64-v3

InvocationCount=1  UnrollFactor=1  

 Method                                   | Mean        | Error      | StdDev     | Median      | Ratio        | RatioSD | Rank | Allocated | Alloc Ratio |
----------------------------------------- |------------:|-----------:|-----------:|------------:|-------------:|--------:|-----:|----------:|------------:|
 'Bulk Entities (10) - System.Text.Json'  |   118.06 μs |   3.783 μs |  10.855 μs |   114.90 μs |     baseline |         |    1 |  11.51 KB |             |
 'Bulk Entities (10) - Kiota'             |   346.84 μs |  16.776 μs |  49.465 μs |   351.05 μs | 2.96x slower |   0.49x |    2 |  69.42 KB |  6.03x more |
                                          |             |            |            |             |              |         |      |           |             |
 'Bulk Entities (100) - System.Text.Json' |   819.46 μs |  19.824 μs |  56.559 μs |   808.10 μs |     baseline |         |    1 |  82.23 KB |             |
 'Bulk Entities (100) - Kiota'            | 3,106.34 μs | 223.744 μs | 656.203 μs | 2,831.50 μs | 3.81x slower |   0.84x |    2 | 682.08 KB |  8.29x more |
                                          |             |            |            |             |              |         |      |           |             |
 'Complex Entity - System.Text.Json'      |   182.94 μs |   3.281 μs |   3.069 μs |   182.80 μs |     baseline |         |    1 |  19.23 KB |             |
 'Complex Entity - Kiota'                 |   672.09 μs |  41.078 μs | 120.475 μs |   670.30 μs | 3.67x slower |   0.66x |    2 |  125.1 KB |  6.51x more |
                                          |             |            |            |             |              |         |      |           |             |
 'EntityResponse - System.Text.Json'      |   190.91 μs |   2.457 μs |   1.919 μs |   191.85 μs |     baseline |         |    1 |   19.3 KB |             |
 'EntityResponse - Kiota'                 |   606.70 μs |  36.618 μs | 107.393 μs |   557.40 μs | 3.18x slower |   0.56x |    2 | 126.28 KB |  6.54x more |
                                          |             |            |            |             |              |         |      |           |             |
 'Simple Entity - System.Text.Json'       |    45.79 μs |   2.474 μs |   7.255 μs |    48.20 μs |     baseline |         |    1 |   3.52 KB |             |
 'Simple Entity - Kiota'                  |    69.67 μs |   3.832 μs |  11.240 μs |    64.40 μs | 1.56x slower |   0.35x |    2 |   7.55 KB |  2.15x more |
