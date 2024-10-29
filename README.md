# UltraTool

一款致力于简单易用、减少重复代码的工具库，提供丰富的扩展方法、工具类、工具函数等，帮助开发者提高开发效率，让您的开发速度更快更流畅。

## 安装使用

```shell
dotnet add package UltraTool
```

接下来，您就可以在代码中使用UltraTool的所有功能了。

## 功能介绍

此工具库目前仍在持续开发中，功能与文档正在持续补全更新中。

### 目录

* [池化数组](#池化数组)
* [异常构建器](#异常构建器)
* [集合](#集合)
    * [列表拓展](#列表拓展)
    * [字典拓展](#字典拓展)
* [随机](#随机)

### 池化数组

池化数组为工具库提供的一个基于数组池封装的数组类型，利用数组池，可以减少内存的申请和释放，从而提高性能。

以下是基本使用方法：

```csharp
// 创建一个容量为100的池化数组
var array = PooledArray.Get<int>(100);
// 之后操作与数组基本相同
// 赋值
array[0] = 1;
// 获取索引0的值
var value = array[0];

// 为了让数组正确归还到池中，需要您在恰当的时机调用Dispose方法归还数组到池中
array.Dispose();
// 您可以显式调用Dispose方法，也可以使用using语句块，以释放资源

// 您也可以自己创建数组池作为池使用，并且可以指定归还数组时是否清空数组内容
// 创建数组池
var pool = ArrayPool<int>.Create();
// 通过自定义的池创建一个容量为100的池化数组，并且在归还时清空数组内容
var array2 = PooledArray.Get<int>(100, pool, true);
```

### 异常构建器

异常构建器是工具库提供的一个快速构建异常的辅助类，调用Dispose方法时，若有异常信息则会自动抛出异常，可以用于一些需要进行规则检查并抛出异常的场景。

```csharp
// 如下场景，如果TestMethod执行过程中AddError被调用了
// 则在退出方法时会抛出异常并附带错误信息
public void TestMethod(int x)
{
    // 创建一个异常构建器
    using var builder = ExceptionBuilder.CreateDefault("测试异常构建");
    if (x < 0)
    {
        builder.AddError("x不能小于0");
    }

    if (x % 2 != 0)
    {
        builder.AddError("x不能为奇数");
    }

    if (x == 2)
    {
        builder.AddError("x不能为2");
    }
}
```

### 集合

### 列表拓展

以下为此库提供的部分列表类型的拓展方法：

1. 判断索引是否合法

```csharp
var list = new List<int> { 1, 2, 3, 4, 5 };
// 返回true
list.IsValidIndex(2);
// 返回false
list.IsValidIndex(5);
// 判断是否为非法索引，返回true
list.IsInvalidIndex(-1);
```

2. 获取指定索引位置的元素

```csharp
var list = new List<int> { 1, 2, 3, 4, 5 };
// 获取索引2的元素，返回true，value为3
list.TryGetValue(2, out var value);
// 获取索引5的元素，返回false，value为default(int)
list.TryGetValue(5, out var value);
// 获取指定索引的元素，获取不到则返回默认值
// 索引5没有元素，因此返回100
list.GetValueOrDefault(5, 100);
```

3. 批量添加元素

```csharp
IList<int> list = new List<int>();
list.AddRange(new[] { 1, 2, 3 });
```

4. 删除元素

```csharp
var list = new List<int> { 1, 2, 3, 4, 5 };
// 删除列表第一个元素
list.RemoveFirst();
// 删除列表最后一个元素
list.RemoveLast();

// 尝试删除列表中第一个匹配的元素，匹配不到返回false
list.TryRemoveFirst(x => x == 3, out var removedFirst);
// 尝试删除列表中从后往前匹配的第一个元素，匹配不到返回false
list.TryRemoveLast(x => x == 5, out var removedLast);
```

5. 交换元素位置

```csharp
var list = new List<int> { 1, 2, 3, 4, 5 };
// 交换索引1和索引3的位置
list.Swap(1, 3);
```

6. 打乱列表顺序

```csharp
var list = new List<int> { 1, 2, 3, 4, 5 };
// 打乱顺序
list.Shuffle();
```

7. 查找索引

```csharp
IList<int> list = new List<int>() { 1, 1, 3, 3, 5 };
// 查找列表中第一个匹配的元素，匹配不到返回-1
var index = list.IndexOf(3);
// 查找列表中从后往前匹配的第一个元素，匹配不到返回-1
var index2 = list.LastIndexOf(5);
// 查找列表中所有匹配的元素索引
var indexes = list.IndexesOf();

// 按条件查找列表中第一个匹配的元素，匹配不到返回-1
var index3 = list.FindIndex(x => x == 3);
// 按条件查找列表中从后往前匹配的第一个元素，匹配不到返回-1
var index4 = list.FindLastIndex(x => x == 5);
// 按条件查找列表中所有匹配的元素索引
var indexes2 = list.FindIndexes(x => x == 3);
```

8. 查找元素

```csharp
IList<int> list = new List<int>() { 1, 1, 3, 3, 5 };
// 尝试查找列表中第一个匹配的元素，匹配不到返回false
list.TryFindFirst(x => x == 3, out var found);
// 尝试查找列表中从后往前匹配的第一个元素，匹配不到返回false
list.TryFindLast(x => x == 5, out var found2);
```

9. 排序

```csharp
var list = new List<int> { 5, 3, 1, 4, 2 };
// 插入排序
list.InsertionSort();
// 快速排序
list.QuickSort();
// 归并排序
list.MergeSort();
```

### 字典拓展

以下为此库提供的部分字典类型的拓展方法：

1. 获取或添加

```csharp
var dict = new Dictionary<int, string>();
// 获取或添加，如果key不存在，则添加键值对
dict.GetOrAdd(1, "one");
// 也可以指定委托生成值，入参为key
dict.GetOrAdd(2, args => $"{args}");
```

2. 获取或创建

```csharp
var dict = new Dictionary<int, List<string>>();
// 获取或创建，如果key不存在，则创建一个值并添加键值对，字典的Value类型必须提供无参构造函数
dict.GetOrCreate(1);
```

3. 交换键值

```csharp
var dict = new Dictionary<int, string>() { { 1, "one" }, { 2, "two" } };
// 交换键值，将1和2的值互换
dict.Swap(1, 2);
```

4. 打乱键值

```csharp
var dict = new Dictionary<int, string>() { { 1, "one" }, { 2, "two" } };
// 打乱所有的键值对映射
dict.Shuffle();
```

### 随机

以下为此库提供的部分随机拓展方法：

1. 获取随机元素

```csharp
var random = new Random();
var list = new List<int> { 1, 2, 3, 4, 5 };
// 获取列表中一个随机元素
var value = random.NextItem(list);
```

2. 获取多个随机元素

```csharp
var random = new Random();
var list = new List<int> { 1, 2, 3, 4, 5 };
// 放回抽取多个随机元素，即可能重复抽取同一个元素，此处为获取3个
var values = random.NextItemsSelection(list, 3);
// 不放回抽取多个随机元素，每次只能抽取一个元素，此处为获取3个
var values2 = random.NextItemsSample(list, 3);
```

3. 带权重抽取

```csharp
var random = new Random();
// 键为元素，值为权重
var itemWeight = new Dictionary<int, int>() { { 1, 10 }, { 2, 5 }, { 3, 2 } };
var item = random.NextWeighted(itemWeight);
```

4. 带权重抽取多个

```csharp
var random = new Random();
// 键为元素，值为权重
var itemWeight = new Dictionary<int, int>() { { 1, 10 }, { 2, 5 }, { 3, 2 } };
// 放回抽取多个元素，即可能重复抽取同一个元素，此处为获取3个
var items = random.NextWeightedSelection(itemWeight, 3);
// 不放回抽取多个元素，每次只能抽取一个元素，此处为获取3个
var items2 = random.NextWeightedSample(itemWeight, 3);
```