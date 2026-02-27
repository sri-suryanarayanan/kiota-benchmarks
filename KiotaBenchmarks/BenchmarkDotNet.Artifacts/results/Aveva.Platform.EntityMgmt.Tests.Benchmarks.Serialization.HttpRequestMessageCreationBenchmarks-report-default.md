
BenchmarkDotNet v0.15.8, Windows 11 (10.0.22631.6649/23H2/2023Update/SunValley3)
Intel Core Ultra 7 165H 3.80GHz, 1 CPU, 22 logical and 16 physical cores
.NET SDK 10.0.101
  [Host]     : .NET 8.0.24 (8.0.24, 8.0.2426.7010), X64 RyuJIT x86-64-v3
  DefaultJob : .NET 8.0.24 (8.0.24, 8.0.2426.7010), X64 RyuJIT x86-64-v3


 Method                                    | Mean        | Error     | StdDev     | Median      | Ratio         | RatioSD | Rank | Gen0   | Gen1   | Allocated | Alloc Ratio |
------------------------------------------ |------------:|----------:|-----------:|------------:|--------------:|--------:|-----:|-------:|-------:|----------:|------------:|
 'GET Entity - Manual HttpRequestMessage'  |   0.1816 μs | 0.0037 μs |  0.0102 μs |   0.1808 μs |      baseline |         |    1 | 0.0234 |      - |     296 B |             |
 'GET Entity - Kiota Request Builder'      |   1.0269 μs | 0.0204 μs |  0.0448 μs |   1.0303 μs |  5.67x slower |   0.40x |    2 | 0.2060 |      - |    2592 B |  8.76x more |
                                           |             |           |            |             |               |         |      |        |        |           |             |
 'POST with Headers - Manual'              |  15.8933 μs | 0.3173 μs |  0.8523 μs |  15.7802 μs |      baseline |         |    1 | 1.2512 | 0.0305 |   15881 B |             |
 'POST with Headers - Kiota'               | 161.4790 μs | 5.6298 μs | 16.2432 μs | 158.4351 μs | 10.19x slower |   1.15x |    2 | 1.4648 | 0.4883 |   18741 B |  1.18x more |
                                           |             |           |            |             |               |         |      |        |        |           |             |
 'POST Bulk - Manual HttpRequestMessage'   |  13.8935 μs | 0.3130 μs |  0.9129 μs |  13.8136 μs |      baseline |         |    1 | 0.7477 |      - |    9392 B |             |
 'POST Bulk - Kiota Request Builder'       |  91.6721 μs | 3.1325 μs |  9.1871 μs |  89.9408 μs |  6.63x slower |   0.78x |    2 | 0.9766 | 0.2441 |   12703 B |  1.35x more |
                                           |             |           |            |             |               |         |      |        |        |           |             |
 'POST Entity - Manual HttpRequestMessage' |  16.4899 μs | 0.5880 μs |  1.7246 μs |  16.0825 μs |      baseline |         |    1 | 1.2207 | 0.0305 |   15681 B |             |
 'POST Entity - Kiota Request Builder'     | 156.9767 μs | 6.1920 μs | 18.2571 μs | 151.1070 μs |  9.62x slower |   1.46x |    2 | 0.9766 |      - |   17947 B |  1.14x more |
                                           |             |           |            |             |               |         |      |        |        |           |             |
 'PUT Entity - Manual HttpRequestMessage'  |  16.8810 μs | 0.4721 μs |  1.3845 μs |  16.7612 μs |      baseline |         |    1 | 1.2512 | 0.0610 |   15849 B |             |
 'PUT Entity - Kiota Request Builder'      | 170.4181 μs | 6.2186 μs | 18.2380 μs | 170.7155 μs | 10.16x slower |   1.35x |    2 | 1.4648 | 0.4883 |   18733 B |  1.18x more |
