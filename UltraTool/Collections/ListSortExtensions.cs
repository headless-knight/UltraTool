using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UltraTool.Compares;
using UltraTool.Helpers;

namespace UltraTool.Collections;

/// <summary>
/// 列表排序拓展类
/// </summary>
[PublicAPI]
public static class ListSortExtensions
{
    /// <summary>插入排序阈值</summary>
    private const int InsertionSortThreshold = 1 << 6;

    /// <summary>
    /// 插入排序
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="comparer">元素比较器，默认为null</param>
    [CollectionAccess(CollectionAccessType.UpdatedContent)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InsertionSort<T>(this IList<T> list, IComparer<T>? comparer = null) =>
        list.InsertionSort(0, list.Count, comparer);

    /// <summary>
    /// 插入排序
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="index">起始索引</param>
    /// <param name="length">长度</param>
    /// <param name="comparer">元素比较器，默认为null</param>
    [CollectionAccess(CollectionAccessType.UpdatedContent)]
    public static void InsertionSort<T>(this IList<T> list, int index, int length, IComparer<T>? comparer = null)
    {
        if (length <= 0) return;

        ArgumentOutOfRangeHelper.ThrowIfNegative(index);
        ArgumentOutOfRangeHelper.ThrowIfGreaterThan(index + length, list.Count);
        InsertionSortInternal(list, index, index + length - 1, comparer ?? Comparer<T>.Default);
    }

    /// <summary>
    ///  插入排序
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="comparison">元素比较表达式</param>
    [CollectionAccess(CollectionAccessType.UpdatedContent)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InsertionSort<T>(this IList<T> list, Comparison<T> comparison) =>
        list.InsertionSort(new ComparisonComparer<T>(comparison));

    /// <summary>
    ///  插入排序
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="index">起始索引</param>
    /// <param name="length">长度</param>
    /// <param name="comparison">元素比较表达式</param>
    [CollectionAccess(CollectionAccessType.UpdatedContent)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InsertionSort<T>(this IList<T> list, int index, int length, Comparison<T> comparison) =>
        list.InsertionSort(index, length, new ComparisonComparer<T>(comparison));

    /// <summary>插入排序，内部实现</summary>
    [CollectionAccess(CollectionAccessType.UpdatedContent)]
    private static void InsertionSortInternal<T>(IList<T> list, int left, int right, IComparer<T> comparer)
    {
        for (var i = left + 1; i <= right; i++)
        {
            var current = list[i];
            var j = i - 1;
            while (j >= left && comparer.Compare(current, list[j]) < 0)
            {
                list[j + 1] = list[j];
                j--;
            }

            list[j + 1] = current;
        }
    }

    /// <summary>
    /// 快速排序
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="comparer">元素比较器，默认为null</param>
    [CollectionAccess(CollectionAccessType.UpdatedContent)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void QuickSort<T>(this IList<T> list, IComparer<T>? comparer = null) =>
        list.QuickSort(0, list.Count, comparer);

    /// <summary>
    /// 快速排序
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="index">起始索引</param>
    /// <param name="length">长度</param>
    /// <param name="comparer">元素比较器，默认为null</param>
    [CollectionAccess(CollectionAccessType.UpdatedContent)]
    public static void QuickSort<T>(this IList<T> list, int index, int length, IComparer<T>? comparer = null)
    {
        if (length <= 0) return;

        ArgumentOutOfRangeHelper.ThrowIfNegative(index);
        ArgumentOutOfRangeHelper.ThrowIfGreaterThan(index + length, list.Count);
        // 数量小于阈值使用插入排序
        if (length <= InsertionSortThreshold)
        {
            InsertionSortInternal(list, index, index + length - 1, comparer ?? Comparer<T>.Default);
            return;
        }

        QuickSortInternal(list, index, index + length - 1, comparer ?? Comparer<T>.Default);
    }

    /// <summary>
    ///  快速排序
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="comparison">元素比较表达式</param>
    [CollectionAccess(CollectionAccessType.UpdatedContent)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void QuickSort<T>(this IList<T> list, Comparison<T> comparison) =>
        list.QuickSort(new ComparisonComparer<T>(comparison));

    /// <summary>
    ///  快速排序
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="index">起始索引</param>
    /// <param name="length">长度</param>
    /// <param name="comparison">元素比较表达式</param>
    [CollectionAccess(CollectionAccessType.UpdatedContent)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void QuickSort<T>(this IList<T> list, int index, int length, Comparison<T> comparison) =>
        list.QuickSort(index, length, new ComparisonComparer<T>(comparison));

    /// <summary>快速排序，内部实现</summary>
    private static void QuickSortInternal<T>(this IList<T> list, int left, int right, IComparer<T> comparer)
    {
        if (left >= right) return;

        // 数量小于阈值使用插入排序
        if (right - left + 1 <= InsertionSortThreshold)
        {
            InsertionSortInternal(list, left, right, comparer);
            return;
        }

        // 分区操作后基准元素的正确位置
        var pivotIndex = QuickSortPartition(list, left, right, comparer);
        // 递归排序左子数组
        QuickSortInternal(list, left, pivotIndex, comparer);
        // 递归排序右子数组
        QuickSortInternal(list, pivotIndex + 1, right, comparer);
    }

    /// <summary>快速排序分区操作</summary>
    private static int QuickSortPartition<T>(IList<T> list, int left, int right, IComparer<T> comparer)
    {
        // 三值取中，获取基准值
        var pivot = CompareHelper.Middle(list[left], list[right], list[(left + right) >> 1], comparer);
        while (true)
        {
            // 从左向右搜索，直到找到第一个大于等于基准值的元素
            while (comparer.Compare(list[left], pivot) < 0)
            {
                left++;
            }

            // 从右向左搜索，直到找到第一个小于等于基准值的元素
            while (comparer.Compare(list[right], pivot) > 0)
            {
                right--;
            }

            // 完成分区，中止循环
            if (left >= right) return right;

            list.Swap(left++, right--);
        }
    }

    /// <summary>
    /// 归并排序
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="comparer">元素比较器，默认为null</param>
    [CollectionAccess(CollectionAccessType.UpdatedContent)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void MergeSort<T>(this IList<T> list, IComparer<T>? comparer = null) =>
        list.MergeSort(0, list.Count, comparer);

    /// <summary>
    /// 归并排序
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="index">起始索引</param>
    /// <param name="length">长度</param>
    /// <param name="comparer">元素比较器，默认为null</param>
    [CollectionAccess(CollectionAccessType.UpdatedContent)]
    public static void MergeSort<T>(this IList<T> list, int index, int length, IComparer<T>? comparer = null)
    {
        if (length <= 0) return;

        ArgumentOutOfRangeHelper.ThrowIfNegative(index);
        ArgumentOutOfRangeHelper.ThrowIfGreaterThan(index + length, list.Count);
        // 数量小于阈值使用插入排序
        if (length <= InsertionSortThreshold)
        {
            InsertionSortInternal(list, index, index + length - 1, comparer ?? Comparer<T>.Default);
            return;
        }

        var temp = ArrayHelper.AllocateUninitializedArray<T>(list.Count);
        MergeSortInternal(list, index, index + length - 1, comparer ?? Comparer<T>.Default, temp);
    }

    /// <summary>
    ///  归并排序
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="comparison">元素比较表达式</param>
    [CollectionAccess(CollectionAccessType.UpdatedContent)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void MergeSort<T>(this IList<T> list, Comparison<T> comparison) =>
        list.MergeSort(new ComparisonComparer<T>(comparison));

    /// <summary>
    ///  归并排序
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="index">起始索引</param>
    /// <param name="length">长度</param>
    /// <param name="comparison">元素比较表达式</param>
    [CollectionAccess(CollectionAccessType.UpdatedContent)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void MergeSort<T>(this IList<T> list, int index, int length, Comparison<T> comparison) =>
        list.MergeSort(index, length, new ComparisonComparer<T>(comparison));

    /// <summary>
    /// 并行归并排序
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="comparer">元素比较器，默认为null</param>
    [CollectionAccess(CollectionAccessType.UpdatedContent)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void MergeSortParallel<T>(this IList<T> list, IComparer<T>? comparer = null) =>
        list.MergeSortParallel(0, list.Count, comparer);

    /// <summary>
    /// 并行归并排序
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="index">起始索引</param>
    /// <param name="length">长度</param>
    /// <param name="comparer">元素比较器，默认为null</param>
    [CollectionAccess(CollectionAccessType.UpdatedContent)]
    public static void MergeSortParallel<T>(this IList<T> list, int index, int length, IComparer<T>? comparer = null)
    {
        if (length <= 0) return;

        ArgumentOutOfRangeHelper.ThrowIfNegative(index);
        ArgumentOutOfRangeHelper.ThrowIfGreaterThan(index + length, list.Count);
        // 数量小于阈值使用插入排序
        if (length <= InsertionSortThreshold)
        {
            InsertionSortInternal(list, index, index + length - 1, comparer ?? Comparer<T>.Default);
            return;
        }

        var temp = ArrayHelper.AllocateUninitializedArray<T>(list.Count);
        MergeSortParallelInternal(list, index, index + length - 1, comparer ?? Comparer<T>.Default, temp);
    }

    /// <summary>
    ///  并行归并排序
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="comparison">元素比较表达式</param>
    [CollectionAccess(CollectionAccessType.UpdatedContent)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void MergeSortParallel<T>(this IList<T> list, Comparison<T> comparison) =>
        list.MergeSortParallel(new ComparisonComparer<T>(comparison));

    /// <summary>
    ///  并行归并排序
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="index">起始索引</param>
    /// <param name="length">长度</param>
    /// <param name="comparison">元素比较表达式</param>
    [CollectionAccess(CollectionAccessType.UpdatedContent)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void MergeSortParallel<T>(this IList<T> list, int index, int length, Comparison<T> comparison) =>
        list.MergeSortParallel(index, length, new ComparisonComparer<T>(comparison));

    /// <summary>归并排序，内部实现</summary>
    private static void MergeSortInternal<T>(IList<T> list, int left, int right, IComparer<T> comparer, Span<T> temp)
    {
        if (left >= right) return;

        // 数量小于阈值使用插入排序
        if (right - left + 1 <= InsertionSortThreshold)
        {
            InsertionSortInternal(list, left, right, comparer);
            return;
        }

        var mid = (left + right) >> 1;
        // 递归地对左半部分进行排序
        MergeSortInternal(list, left, mid, comparer, temp);
        // 递归地对右半部分进行排序
        MergeSortInternal(list, mid + 1, right, comparer, temp);
        // 如果左边最大的元素小于或等于右边最小的元素，则不需要合并
        if (comparer.Compare(list[mid], list[mid + 1]) <= 0)
        {
            return;
        }

        // 合并已排序的两部分
        MergeSortMerge(list, left, mid, right, comparer, temp.Slice(left, right - left + 1));
    }

    /// <summary>并行归并排序，内部实现</summary>
    private static void MergeSortParallelInternal<T>(IList<T> list, int left, int right, IComparer<T> comparer,
        T[] temp)
    {
        if (left >= right) return;

        // 数量小于阈值使用插入排序
        if (right - left + 1 <= InsertionSortThreshold)
        {
            InsertionSortInternal(list, left, right, comparer);
            return;
        }

        var mid = (left + right) >> 1;
        Parallel.Invoke(() => MergeSortParallelInternal(list, left, mid, comparer, temp),
            () => MergeSortParallelInternal(list, mid + 1, right, comparer, temp));
        // 如果左边最大的元素小于或等于右边最小的元素，则不需要合并
        if (comparer.Compare(list[mid], list[mid + 1]) <= 0)
        {
            return;
        }

        // 合并已排序的两部分
        MergeSortMerge(list, left, mid, right, comparer, temp.AsSpan(left, right - left + 1));
    }

    /// <summary>归并排序，合并操作</summary>
    private static void MergeSortMerge<T>(IList<T> list, int left, int mid, int right, IComparer<T> comparer,
        Span<T> temp)
    {
        int i = left, j = mid + 1, k = 0;
        while (i <= mid && j <= right)
        {
            // 将较小的元素复制到临时数组
            if (comparer.Compare(list[i], list[j]) <= 0)
            {
                temp[k++] = list[i++];
            }
            else
            {
                temp[k++] = list[j++];
            }
        }

        // 如果左半部分还有剩余元素，将其复制到临时数组
        while (i <= mid)
        {
            temp[k++] = list[i++];
        }

        // 如果右半部分还有剩余元素，将其复制到临时数组
        while (j <= right)
        {
            temp[k++] = list[j++];
        }

        // 将临时数组的排序结果复制回原数组
        for (i = 0; i < k; i++)
        {
            list[left + i] = temp[i];
        }
    }
}