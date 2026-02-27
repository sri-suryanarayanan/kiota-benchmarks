
BenchmarkDotNet v0.15.8, Windows 11 (10.0.22631.6649/23H2/2023Update/SunValley3)
Intel Core Ultra 7 165H 3.80GHz, 1 CPU, 22 logical and 16 physical cores
.NET SDK 10.0.101
  [Host]     : .NET 8.0.24 (8.0.24, 8.0.2426.7010), X64 RyuJIT x86-64-v3
  DefaultJob : .NET 8.0.24 (8.0.24, 8.0.2426.7010), X64 RyuJIT x86-64-v3


 Method                         | Mean       | Error      | StdDev      | Median     | Ratio        | RatioSD | Rank | Gen0   | Gen1   | Allocated | Alloc Ratio |
------------------------------- |-----------:|-----------:|------------:|-----------:|-------------:|--------:|-----:|-------:|-------:|----------:|------------:|
 'Bulk(10) - System.Text.Json'  |  13.288 μs |  0.4030 μs |   1.1563 μs |  12.928 μs |     baseline |         |    1 | 0.3357 |      - |    4288 B |             |
 'Bulk(10) - Kiota (Standard)'  |  84.668 μs |  2.1729 μs |   6.3728 μs |  84.267 μs | 6.42x slower |   0.71x |    2 | 0.7324 |      - |    9645 B |  2.25x more |
                                |            |            |             |            |              |         |      |        |        |           |             |
 'Bulk(100) - System.Text.Json' | 141.920 μs |  4.6595 μs |  13.6654 μs | 141.340 μs |     baseline |         |    1 | 4.1504 | 0.2441 |   52628 B |             |
 'Bulk(100) - Kiota (Standard)' | 881.756 μs | 37.1787 μs | 107.8622 μs | 867.242 μs | 6.27x slower |   0.97x |    2 | 3.9063 |      - |   91048 B |  1.73x more |
                                |            |            |             |            |              |         |      |        |        |           |             |
 'Complex - System.Text.Json'   |  23.532 μs |  0.7238 μs |   2.1341 μs |  23.494 μs |     baseline |         |    1 | 0.7019 |      - |    8913 B |             |
 'Complex - Kiota (Standard)'   | 205.775 μs |  7.4091 μs |  21.1386 μs | 202.055 μs | 8.81x slower |   1.19x |    2 | 0.9766 |      - |   20715 B |  2.32x more |
                                |            |            |             |            |              |         |      |        |        |           |             |
 'Simple - System.Text.Json'    |   1.613 μs |  0.0445 μs |   0.1313 μs |   1.588 μs |     baseline |         |    1 | 0.0687 |      - |     880 B |             |
 'Simple - Kiota (Standard)'    |   9.653 μs |  0.4275 μs |   1.2197 μs |   9.435 μs | 6.02x slower |   0.90x |    2 | 0.0916 |      - |    1433 B |  1.63x more |
