# UltraTool

一款致力于简单易用、减少重复代码的 .NET 工具库，提供丰富的扩展方法、工具类和实用函数，帮助开发者提高开发效率。

## 🚀 功能特性

- **多框架支持**：支持 .NET 10.0、8.0、6.0 和 .NET Standard 2.1
- **高性能优化**：使用现代 .NET 特性，提供高性能的扩展方法
- **类型安全**：严格的类型检查和空值处理
- **直观易用**：流畅的API设计，减少重复代码编写
- **全面覆盖**：涵盖集合操作、字符串处理、数值计算、时间日期、随机数生成等常用场景

## 📦 安装

```bash
dotnet add package UltraTool
```

## 🎯 快速开始

### 集合操作 - 强大的序列处理能力

```csharp
using UltraTool.Collections;

var numbers = new[] { 1, 2, 3, 4, 5 };
var strings = new[] { "apple", "banana", "cherry" };

// 检查集合状态
bool hasNull = strings.IsAnyNull();        // 是否有null元素
bool allNull = strings.IsAllNull();        // 是否全为null元素
bool isSorted = numbers.IsOrdered();       // 是否有序

// 极值获取
var (min, max) = numbers.MinMax();         // 同时获取最小值和最大值
var (minDef, maxDef) = numbers.MinMaxOrDefault(0, 100); // 带默认值的极值获取

// 遍历操作
numbers.ForEach(x => Console.WriteLine(x)); // 简单遍历
numbers.ForEach((x, i) => Console.WriteLine($"索引{i}: {x}")); // 带索引遍历

// 序列操作
var indexed = numbers.Index();              // 转换为带索引的序列
var joined = strings.Join(", ");            // 元素间插入分隔符
var replaced = numbers.Replace(3, 30);      // 替换指定元素

// 统计功能
var countMap = strings.CountMap();         // 统计元素出现次数
bool found = numbers.TryFind(x => x > 3, out var result); // 条件查找

// 字典操作
var mergedDict = numbers.ToMergedDictionary(x => x % 2, x => x); // 合并字典
var nestedDict = strings.ToNestedDictionary(s => s.Length, s => s[0]); // 嵌套字典
```

### 字符串操作 - 安全的空值处理和编码转换

```csharp
using UltraTool.Text;

string? text = "Hello World";
string? nullText = null;

// 空值安全检查
string safeText = text.EmptyIfNull();       // null时返回空字符串
bool isEmpty = text.IsEmpty();              // 是否空字符串
bool isBlank = "   ".IsBlank();             // 是否全为空白符
bool isValid = text.IsNotNullOrBlank();     // 是否有效字符串

// 编码转换
byte[] bytes = text.GetBytes();             // UTF8编码转换
byte[] gbBytes = text.GetBytes(Encoding.GetEncoding("GB2312")); // 指定编码
using var pooledBytes = text.GetBytesPooled(); // 池化字节数组（自动释放）
```

### 数值操作 - 高效的位运算和数学计算

```csharp
using UltraTool.Numerics;

int number = 42;  // 二进制: 101010

// 基础判断
bool isOdd = number.IsOdd();                // 是否为奇数 → true
bool isEven = number.IsEven();               // 是否为偶数 → false

// 位操作
int bitCount = number.GetBitOneCount();     // 二进制中1的个数 → 3
bool isBitSet = number.IsBitOne(3);          // 第3位是否为1 → true (从0开始)
int setBit = number.CalcSetBitOne(0);        // 设置第0位为1 → 43
int clearBit = number.CalcSetBitZero(1);    // 清除第1位 → 40

// 浮点数操作
float value = 3.14f;
bool isNaN = float.IsNaN(value);
bool isInfinity = float.IsInfinity(value);
```

### 时间日期操作 - 丰富的时间处理功能

```csharp
using UltraTool.Times;

DateTime now = DateTime.Now;
DateTime birthday = new DateTime(1990, 5, 15);

// 时间判断
bool isWeekend = now.IsWeekEnd();            // 是否为周末
bool isLeapYear = birthday.IsLeapYear();     // 是否为闰年
bool isAm = now.IsAm();                     // 是否为上午
bool sameDay = now.IsSameDay(birthday);      // 是否同一天

// Unix时间戳转换
long unixSeconds = now.ToUnixTimeSeconds();          // 秒级时间戳
long unixMillis = now.ToUnixTimeMilliseconds();       // 毫秒级时间戳
long unixWithOffset = now.ToUnixTimeSeconds(TimeSpan.FromHours(8)); // 带时区偏移

// 周数计算
int weekOfYear = now.WeekOfYear();          // 当年第几周
int dayOfWeek = now.TodayOfWeek();          // 周几（周一为1，周日为7）
int adjustedDay = now.TodayOfWeek(TimeSpan.FromHours(6)); // 带临界值调整

// .NET 6+ 特性
#if NET6_0_OR_GREATER
var dateOnly = now.GetDateOnly();            // 获取日期部分
var timeOnly = now.GetTimeOnly();            // 获取时间部分
#endif
```

### 随机数生成 - 强大的随机化工具

```csharp
using UltraTool.Randoms;

var random = Random.Shared;
var items = new[] { "A", "B", "C", "D", "E" };
var weightedItems = new Dictionary<string, int>
{
    ["A"] = 10,
    ["B"] = 30,
    ["C"] = 60
};

// 基础随机生成
bool randomBool = random.NextBool();        // 随机布尔值
byte randomByte = random.NextByte();        // 随机字节 (0-255)
char randomChar = random.NextChar();        // 随机字符
float randomFloat = random.NextSingle(0f, 100f); // 随机浮点数
double randomDouble = random.NextDouble(50.0, 150.0); // 随机双精度

// 字符串随机
string randomStr = random.NextString(10);   // 10位随机字符串
string customStr = random.NextString(5, "ABCD123"); // 从指定字符池生成

// 时间随机
TimeSpan randomTime = random.NextTimeSpan(); // 随机时间量
DateTime randomDate = random.NextDateTime(DateTime.Now, DateTime.Now.AddDays(30)); // 随机日期

// 序列操作
var singleChoice = random.Choice(items);     // 随机选择一个元素
var multiChoice = random.Choice(items, 3);   // 放回抽取3个元素
var sample = random.Sample(items, 2);        // 不放回抽取2个元素

// 带权随机
var weightedChoice = random.ChoiceWeighted(weightedItems); // 带权随机选择
var weightedSample = random.SampleWeighted(weightedItems, 2); // 不放回带权抽样

// 洗牌操作
random.Shuffle(items);                       // 打乱数组顺序
var list = new List<string> { "X", "Y", "Z" };
random.Shuffle(list);                        // 打乱列表顺序
```

### 数据转换 - 高效的格式转换工具

```csharp
using UltraTool.Helpers;

// 十六进制转换
byte[] data = { 0xAB, 0xCD, 0xEF, 0x12, 0x34 };
string hexUpper = ConvertHelper.ToHexString(data);        // "ABCDEF1234"
string hexLower = ConvertHelper.ToHexString(data, true);  // "abcdef1234"

char[] hexChars = ConvertHelper.ToHexChars(data);         // 字符数组形式
byte[] restored = ConvertHelper.FromHexString(hexUpper); // 还原字节数据

// 支持奇数字符串长度
string oddHex = "ABC";
byte[] fromOdd = ConvertHelper.FromHexString(oddHex);   // 自动补0 → [0x0A, 0xBC]
```

### 异常构建器 - 优雅的错误收集与抛出

```csharp
using UltraTool;

// 基本用法：收集多个错误后统一抛出
var builder = ExceptionBuilder.CreateDefault("参数校验失败");
if (string.IsNullOrEmpty(name))
    builder.AddError("名称不能为空");
if (age < 0)
    builder.AddError("年龄不能为负数");

builder.ThrowIfHasError(); // 有错误时抛出包含所有错误信息的异常

// 使用 using 语法：作用域结束时自动检查并抛出
using (var checker = ExceptionBuilder.CreateDefault("数据校验"))
{
    checker.AddError("字段A不合法");
    checker.AddError("字段B超出范围");
} // Dispose 时自动抛出异常

// 自定义异常类型
var custom = ExceptionBuilder.Create<ArgumentException>("参数错误");
custom.AddError("参数X无效");
custom.ThrowIfHasError(); // 抛出 ArgumentException

// 值类型版本（零堆分配）
var valueBuilder = ValueExceptionBuilder.CreateDefault("校验");
valueBuilder.AddError("错误信息");
if (valueBuilder.HasError)
{
    Exception ex = valueBuilder.Build(); // 手动构建异常对象
}
```

### 池化数组 - 基于 ArrayPool 的高性能定长数组

```csharp
using UltraTool;

// 基本用法：从池中租借固定长度数组，using 结束自动归还
using var array = PooledArray.Get<int>(100);
array[0] = 42;
array[50] = 99;
Console.WriteLine(array.Length);    // 100
Console.WriteLine(array.Capacity);  // >= 100（池分配的实际容量）

// 从已有数据创建
byte[] source = { 1, 2, 3, 4, 5 };
using var fromSpan = PooledArray.From<byte>(source);
Console.WriteLine(fromSpan[2]);     // 3

// 获取清零的池化数组
using var cleared = PooledArray.GetCleared<int>(50);
Console.WriteLine(cleared[0]);      // 0（已清零）

// 丰富的访问方式
using var data = PooledArray.Get<int>(10);
Span<int> span = data.Span;                    // 获取 Span
ReadOnlySpan<int> roSpan = data.ReadOnlySpan;  // 获取只读 Span
Memory<int> memory = data.Memory;              // 获取 Memory
int[] rawArray = data.ToArray();               // 拷贝为普通数组

// 查找与排序
using var nums = PooledArray.From<int>(new[] { 5, 3, 1, 4, 2 });
nums.Sort();                                   // 排序 → [1, 2, 3, 4, 5]
nums.Reverse();                                // 反转 → [5, 4, 3, 2, 1]
int idx = nums.IndexOf(3);                     // 查找元素索引
int found = nums.Find(x => x > 3) ?? -1;      // 条件查找

// 从 LINQ 序列直接转换
using var pooled = Enumerable.Range(0, 1000).ToPooledArray();
```

### 池化动态数组 - 基于 ArrayPool 的高性能可变长数组

```csharp
using UltraTool;

// 基本用法：类似 List<T>，但底层使用 ArrayPool
using var list = new PooledDynamicArray<string>(capacity: 16);
list.Add("Hello");
list.Add("World");
list.AddRange(new[] { "Foo", "Bar" });
Console.WriteLine(list.Length);     // 4
Console.WriteLine(list[1]);        // "World"

// 从已有集合初始化
using var fromCollection = new PooledDynamicArray<int>(new[] { 1, 2, 3, 4, 5 });
Console.WriteLine(fromCollection.Length); // 5

// 插入与删除
using var dynamic = new PooledDynamicArray<int>();
dynamic.AddRange(new[] { 10, 20, 30, 40, 50 });
dynamic.Insert(2, 25);             // [10, 20, 25, 30, 40, 50]
dynamic.RemoveAt(0);               // [20, 25, 30, 40, 50]
dynamic.Remove(40);                // [20, 25, 30, 50]
dynamic.RemoveAll(x => x > 28);   // [20, 25]

// 批量操作
using var batch = new PooledDynamicArray<int>(8);
batch.AddRange(new[] { 3, 1, 4, 1, 5, 9 });
batch.InsertRange(2, new[] { 100, 200 });  // [3, 1, 100, 200, 4, 1, 5, 9]
batch.RemoveRange(1, 3);                   // [3, 4, 1, 5, 9]

// 获取 Span 进行高性能操作
using var spanArray = new PooledDynamicArray<int>(new[] { 5, 3, 1, 4, 2 });
Span<int> span = spanArray.GetSpan();
ReadOnlySpan<int> roSpan = spanArray.GetReadOnlySpan();
spanArray.Reverse();                // [2, 4, 1, 3, 5]
```

### 值类型秒表 - 零分配的高精度计时器

```csharp
using UltraTool;

// 基本用法：创建并启动秒表
var sw = ValueStopwatch.StartNew();
// ... 执行耗时操作 ...
Thread.Sleep(100);
Console.WriteLine(sw.ElapsedMilliseconds); // ≈ 100
Console.WriteLine(sw.Elapsed);             // TimeSpan 对象

// 停止与继续
var sw2 = ValueStopwatch.StartNew();
Thread.Sleep(50);
sw2.Stop();                                // 停止计时
long ms1 = sw2.ElapsedMilliseconds;       // ≈ 50
Thread.Sleep(100);                         // 停止期间不计时
Console.WriteLine(sw2.ElapsedMilliseconds == ms1); // true
sw2.Start();                               // 继续计时（累加）
Thread.Sleep(50);
sw2.Stop();
Console.WriteLine(sw2.ElapsedMilliseconds); // ≈ 100

// 重置与重启
var sw3 = ValueStopwatch.StartNew();
Thread.Sleep(50);
sw3.Restart();                             // 重置并重新开始计时
Thread.Sleep(30);
Console.WriteLine(sw3.ElapsedMilliseconds); // ≈ 30（仅计算 Restart 后的时间）

sw3.Reset();                               // 完全重置，秒表停止
Console.WriteLine(sw3.IsRunning);          // false
Console.WriteLine(sw3.ElapsedMilliseconds); // 0

// 静态工具方法：计算两个时间戳之间的耗时
long start = ValueStopwatch.GetTimestamp();
// ... 执行操作 ...
TimeSpan elapsed = ValueStopwatch.GetElapsedTime(start);

// 带初始偏移量启动
var sw4 = ValueStopwatch.StartNew(TimeSpan.FromSeconds(5));
Console.WriteLine(sw4.ElapsedMilliseconds); // ≈ 5000（从5秒开始计时）
```

## 📚 API 参考手册

### 集合扩展 (`UltraTool.Collections`)

#### 状态检查
- `IsAnyNull<T>()` - 检查序列中是否有null元素
- `IsAllNull<T>()` - 检查序列中是否全为null元素
- `IsOrdered<T>()` - 检查序列是否有序（升序）
- `IsOrderedDescending<T>()` - 检查序列是否有序（降序）

#### 极值操作
- `MinMax<T>()` - 获取序列的最小值和最大值
- `MinMaxOrDefault<T>()` - 带默认值的极值获取

#### 遍历操作
- `ForEach<T>(Action<T>)` - 简单遍历
- `ForEach<T>(Action<T, int>)` - 带索引遍历
- `Index<T>()` - 转换为带索引的序列

#### 序列处理
- `Join<T>(T separator)` - 元素间插入分隔符
- `Replace<T>(T oldValue, T newValue)` - 替换指定元素
- `StartsWith<T>(IEnumerable<T> values)` - 判断是否以指定序列开头
- `EndsWith<T>(IEnumerable<T> values)` - 判断是否以指定序列结尾

#### 统计查找
- `CountMap<T>()` - 统计元素出现次数
- `TryFind<T>(Predicate<T>, out T)` - 条件查找元素

#### 字典转换
- `ToMergedDictionary<T>()` - 转换为合并字典（重复键合并）
- `ToNestedDictionary<T>()` - 转换为嵌套字典
- `ToNestedReadOnlyDictionary<T>()` - 转换为只读嵌套字典

### 字符串扩展 (`UltraTool.Text`)

#### 空值安全
- `EmptyIfNull()` - null时返回空字符串
- `IsEmpty()` - 是否空字符串
- `IsNotEmpty()` - 是否非空字符串
- `IsBlank()` - 是否全为空白符
- `IsNotNullOrBlank()` - 是否有效非空字符串

#### 编码转换
- `GetBytes()` - UTF8编码转换
- `GetBytes(Encoding)` - 指定编码转换
- `GetBytesPooled()` - 池化字节数组转换

### 数值扩展 (`UltraTool.Numerics`)

#### 基础判断
- `IsOdd()` - 是否为奇数
- `IsEven()` - 是否为偶数

#### 位操作
- `GetBitOneCount()` - 二进制中1的个数
- `IsBitOne(int index)` - 判断指定位是否为1
- `CalcSetBitOne(int index)` - 计算设置指定位后的值
- `CalcSetBitZero(int index)` - 计算清除指定位后的值

### 时间日期扩展 (`UltraTool.Times`)

#### 时间判断
- `IsWeekEnd()` - 是否为周末
- `IsLeapYear()` - 是否为闰年
- `IsLeapMonth()` - 是否为闰月
- `IsAm()` - 是否为上午
- `IsPm()` - 是否为下午
- `IsSameDay(DateTime)` - 是否同一天
- `IsSameMonth(DateTime)` - 是否同一月
- `IsSameYear(DateTime)` - 是否同一年

#### 时间戳转换
- `ToUnixTimeSeconds()` - 转换为秒级Unix时间戳
- `ToUnixTimeMilliseconds()` - 转换为毫秒级Unix时间戳
- 支持带时区偏移的版本

#### 周数计算
- `WeekOfYear()` - 获取当年第几周
- `TodayOfWeek()` - 获取周几（周一为1）
- 支持临界值调整版本

### 随机数扩展 (`UltraTool.Randoms`)

#### 基础随机
- `NextBool()` - 随机布尔值
- `NextByte()` - 随机字节
- `NextChar()` - 随机字符
- `NextSingle()` - 随机单精度浮点数
- `NextDouble()` - 随机双精度浮点数
- `NextString(int length)` - 随机字符串
- `NextTimeSpan()` - 随机时间量
- `NextDateTime()` - 随机日期时间

#### 序列操作
- `Choice<T>()` - 随机选择一个元素
- `TryChoice<T>()` - 尝试随机选择元素
- `Sample<T>()` - 不放回抽样
- `SampleShuffle<T>()` - 洗牌式抽样
- `Shuffle<T>()` - 打乱序列顺序

#### 带权随机
- `ChoiceWeighted<T>()` - 带权随机选择
- `SampleWeighted<T>()` - 不放回带权抽样
- 支持字典和IWeighted接口两种形式

### 转换工具 (`UltraTool.Helpers`)

#### 十六进制转换
- `ToHexString(byte[])` - 字节数组转十六进制字符串
- `ToHexChars(byte[])` - 字节数组转十六进制字符数组
- `FromHexString(string)` - 十六进制字符串转字节数组
- 支持大小写控制和奇数字符串自动补0

### 异常构建器 (`UltraTool`)

#### ExceptionBuilder（引用类型）
- `ExceptionBuilder.CreateDefault(title?)` - 创建默认异常构建器
- `ExceptionBuilder.Create<T>(title?, creator?)` - 创建指定异常类型的构建器
- `HasError` - 是否有错误信息
- `AddError(string)` - 添加错误信息
- `GetErrorString()` - 获取错误信息字符串
- `ThrowIfHasError()` - 有错误时抛出异常
- `Build()` - 构建异常对象
- `Dispose()` - 释放时自动检查并抛出异常

#### ValueExceptionBuilder（值类型，零堆分配）
- `ValueExceptionBuilder.CreateDefault(title?)` - 创建默认值类型异常构建器
- `ValueExceptionBuilder.Create<T>(title?, creator?)` - 创建指定异常类型的值类型构建器
- API 与引用类型版本一致

### 池化数组 (`UltraTool`)

#### PooledArray（静态工厂）
- `PooledArray.Get<T>(length)` - 从池中租借指定长度数组
- `PooledArray.GetCleared<T>(length)` - 租借并清零
- `PooledArray.From<T>(span)` - 从 Span 拷贝创建
- `ToPooledArray<T>()` - IEnumerable 扩展方法，序列转池化数组

#### PooledArray\<T\>（实例方法）
- `Length` / `Capacity` / `IsEmpty` - 基础属性
- `Span` / `ReadOnlySpan` / `Memory` / `ReadOnlyMemory` - 内存访问
- `IndexOf(T)` / `LastIndexOf(T)` / `Contains(T)` - 查找
- `Find(Predicate)` / `FindAll(Predicate)` / `FindIndex(Predicate)` - 条件查找
- `BinarySearch(T)` - 二分查找
- `Sort()` / `Reverse()` / `Swap(int, int)` - 排序与操作
- `Slice(start, length)` / `GetRange(start, length)` - 切片
- `CopyTo(T[])` / `ToArray()` - 拷贝
- `Dispose()` - 归还数组到池

### 池化动态数组 (`UltraTool`)

#### PooledDynamicArray\<T\>
- `Length` / `Capacity` / `IsEmpty` - 基础属性
- `Add(T)` / `AddRange(IEnumerable)` / `AddRange(ReadOnlySpan)` - 添加元素
- `Insert(int, T)` / `InsertRange(int, IEnumerable)` - 插入元素
- `Remove(T)` / `RemoveAt(int)` / `RemoveRange(int, int)` / `RemoveAll(Predicate)` - 删除元素
- `GetSpan()` / `GetReadOnlySpan()` / `GetMemory()` / `GetReadOnlyMemory()` - 内存访问
- `IndexOf(T)` / `LastIndexOf(T)` / `Find(Predicate)` / `BinarySearch(T)` - 查找
- `Reverse()` / `EnsureCapacity(int)` / `Clear()` - 操作
- `Dispose()` - 归还数组到池

### 值类型秒表 (`UltraTool`)

#### ValueStopwatch
- `ValueStopwatch.StartNew()` - 创建并启动秒表
- `ValueStopwatch.StartNew(TimeSpan)` - 带初始偏移量启动
- `ValueStopwatch.GetTimestamp()` - 获取当前时间戳
- `ValueStopwatch.GetElapsedTime(long)` - 计算时间戳至今的耗时
- `ValueStopwatch.GetElapsedTime(long, long)` - 计算两个时间戳之间的耗时
- `ValueStopwatch.FromTimestamp(long, long)` - 从时间戳构造已停止的秒表
- `IsRunning` - 秒表是否在运行
- `Elapsed` / `ElapsedTicks` / `ElapsedMilliseconds` - 获取耗时
- `Start()` / `Stop()` / `Restart()` / `Reset()` - 控制秒表

## 🔧 性能优化特性

- **池化内存**：使用 `PooledArray<T>` 和 `PooledDynamicArray<T>` 减少内存分配，底层基于 `ArrayPool<T>`
- **值类型计时**：`ValueStopwatch` 为零堆分配的高精度计时器，替代 `System.Diagnostics.Stopwatch`
- **值类型异常构建**：`ValueExceptionBuilder<T>` 在栈上运行，避免堆分配
- **内联优化**：大量使用 `MethodImplOptions.AggressiveInlining`
- **跨度操作**：充分利用 `Span<T>` 和 `ReadOnlySpan<T>`
- **非枚举计数**：优先使用 `TryGetNonEnumeratedCount()` 避免枚举
- **泛型约束**：使用现代泛型特性提升性能

## 🤝 贡献指南

欢迎提交 Issue 和 Pull Request 来改进这个项目！

### 开发环境要求
- .NET 8.0 SDK 或更高版本
- JetBrains Annotations 支持

### 代码规范
- 遵循 C# 编码规范
- 使用 XML 文档注释
- 包含单元测试
- 确保多框架兼容性

## 📄 许可证

本项目采用 MIT 许可证。详见 [LICENSE](LICENSE) 文件。

## 🔗 相关链接

- [GitHub 仓库](https://github.com/headless-knight/UltraTool)
- [NuGet 包](https://www.nuget.org/packages/UltraTool/)
- [问题反馈](https://github.com/headless-knight/UltraTool/issues)

---

**UltraTool** - 让 .NET 开发更高效、更简洁！