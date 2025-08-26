```

BenchmarkDotNet v0.15.2, Linux Linux Mint 22.1 (Xia)
AMD Ryzen 7 7800X3D 5.05GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 8.0.413
  [Host]     : .NET 8.0.19 (8.0.1925.36514), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  DefaultJob : .NET 8.0.19 (8.0.1925.36514), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=DefaultJob  IterationCount=Default  LaunchCount=Default  
WarmupCount=Default  

```
| Method                    | Threads | OpsPerThread | Mean      | Error     | StdDev    | Ratio | RatioSD | Allocated | Alloc Ratio |
|-------------------------- |-------- |------------- |----------:|----------:|----------:|------:|--------:|----------:|------------:|
| **Guid_NewGuid_Parallel**     | **1**       | **10000**        |  **3.623 ms** | **0.0333 ms** | **0.0312 ms** |  **1.00** |    **0.01** |   **1.72 KB** |        **1.00** |
| Snowflake_Signed_Parallel | 1       | 10000        |  2.859 ms | 0.0097 ms | 0.0075 ms |  0.79 |    0.01 |   1.72 KB |        1.00 |
|                           |         |              |           |           |           |       |         |           |             |
| **Guid_NewGuid_Parallel**     | **2**       | **10000**        |  **3.976 ms** | **0.0224 ms** | **0.0199 ms** |  **1.00** |    **0.01** |   **1.96 KB** |        **1.00** |
| Snowflake_Signed_Parallel | 2       | 10000        |        NA |        NA |        NA |     ? |       ? |        NA |           ? |
|                           |         |              |           |           |           |       |         |           |             |
| **Guid_NewGuid_Parallel**     | **4**       | **10000**        |  **4.372 ms** | **0.0459 ms** | **0.0384 ms** |  **1.00** |    **0.01** |   **2.37 KB** |        **1.00** |
| Snowflake_Signed_Parallel | 4       | 10000        |        NA |        NA |        NA |     ? |       ? |        NA |           ? |
|                           |         |              |           |           |           |       |         |           |             |
| **Guid_NewGuid_Parallel**     | **8**       | **10000**        |  **8.835 ms** | **0.1031 ms** | **0.0964 ms** |  **1.00** |    **0.01** |   **3.27 KB** |        **1.00** |
| Snowflake_Signed_Parallel | 8       | 10000        | 25.458 ms | 0.3009 ms | 0.2668 ms |  2.88 |    0.04 |   3.31 KB |        1.01 |
|                           |         |              |           |           |           |       |         |           |             |
| **Guid_NewGuid_Parallel**     | **16**      | **10000**        | **16.674 ms** | **0.0745 ms** | **0.0697 ms** |  **1.00** |    **0.01** |   **5.11 KB** |        **1.00** |
| Snowflake_Signed_Parallel | 16      | 10000        |        NA |        NA |        NA |     ? |       ? |        NA |           ? |
|                           |         |              |           |           |           |       |         |           |             |
| **Guid_NewGuid_Parallel**     | **32**      | **10000**        | **31.637 ms** | **0.1287 ms** | **0.1204 ms** |  **1.00** |    **0.01** |   **5.92 KB** |        **1.00** |
| Snowflake_Signed_Parallel | 32      | 10000        |        NA |        NA |        NA |     ? |       ? |        NA |           ? |

Benchmarks with issues:
  MultiThreadedBenchmarks.Snowflake_Signed_Parallel: DefaultJob [Threads=2, OpsPerThread=10000]
  MultiThreadedBenchmarks.Snowflake_Signed_Parallel: DefaultJob [Threads=4, OpsPerThread=10000]
  MultiThreadedBenchmarks.Snowflake_Signed_Parallel: DefaultJob [Threads=16, OpsPerThread=10000]
  MultiThreadedBenchmarks.Snowflake_Signed_Parallel: DefaultJob [Threads=32, OpsPerThread=10000]
