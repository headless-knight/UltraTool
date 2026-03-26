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

## 🔧 性能优化特性

- **池化内存**：使用 `PooledArray<T>` 减少内存分配
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