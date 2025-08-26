```

BenchmarkDotNet v0.15.2, Linux Linux Mint 22.1 (Xia)
AMD Ryzen 7 7800X3D 5.05GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 8.0.413
  [Host]     : .NET 8.0.19 (8.0.1925.36514), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  DefaultJob : .NET 8.0.19 (8.0.1925.36514), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=DefaultJob  IterationCount=Default  LaunchCount=Default  
WarmupCount=Default  

```
| Method                         | Workers | OpsPerWorker | Mean      | Error     | StdDev    | Ratio | Allocated | Alloc Ratio |
|------------------------------- |-------- |------------- |----------:|----------:|----------:|------:|----------:|------------:|
| **Guid_NewGuid_Parallel**          | **1**       | **10000**        |  **3.648 ms** | **0.0054 ms** | **0.0050 ms** |  **1.00** |   **1.41 KB** |        **1.00** |
| Snowflake_Parallel_ManyWorkers | 1       | 10000        |  2.552 ms | 0.0052 ms | 0.0049 ms |  0.70 |   1.41 KB |        1.00 |
|                                |         |              |           |           |           |       |           |             |
| **Guid_NewGuid_Parallel**          | **2**       | **10000**        |  **3.911 ms** | **0.0239 ms** | **0.0224 ms** |  **1.00** |   **1.62 KB** |        **1.00** |
| Snowflake_Parallel_ManyWorkers | 2       | 10000        |  2.750 ms | 0.0231 ms | 0.0205 ms |  0.70 |   1.62 KB |        1.00 |
|                                |         |              |           |           |           |       |           |             |
| **Guid_NewGuid_Parallel**          | **4**       | **10000**        |  **4.243 ms** | **0.0190 ms** | **0.0177 ms** |  **1.00** |   **2.04 KB** |        **1.00** |
| Snowflake_Parallel_ManyWorkers | 4       | 10000        |  2.897 ms | 0.0136 ms | 0.0127 ms |  0.68 |   2.08 KB |        1.02 |
|                                |         |              |           |           |           |       |           |             |
| **Guid_NewGuid_Parallel**          | **8**       | **10000**        |  **8.789 ms** | **0.0141 ms** | **0.0125 ms** |  **1.00** |   **2.95 KB** |        **1.00** |
| Snowflake_Parallel_ManyWorkers | 8       | 10000        |  3.077 ms | 0.0180 ms | 0.0169 ms |  0.35 |      3 KB |        1.02 |
|                                |         |              |           |           |           |       |           |             |
| **Guid_NewGuid_Parallel**          | **16**      | **10000**        | **15.532 ms** | **0.0923 ms** | **0.0864 ms** |  **1.00** |   **4.77 KB** |        **1.00** |
| Snowflake_Parallel_ManyWorkers | 16      | 10000        |  3.003 ms | 0.0118 ms | 0.0099 ms |  0.19 |   4.74 KB |        0.99 |
|                                |         |              |           |           |           |       |           |             |
| **Guid_NewGuid_Parallel**          | **32**      | **10000**        | **30.840 ms** | **0.0832 ms** | **0.0738 ms** |  **1.00** |   **7.07 KB** |        **1.00** |
| Snowflake_Parallel_ManyWorkers | 32      | 10000        |  3.375 ms | 0.0139 ms | 0.0130 ms |  0.11 |   8.37 KB |        1.19 |
