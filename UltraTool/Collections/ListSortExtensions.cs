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
    #region 插入排序

    /// <summary>
    /// 插入排序
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="comparer">元素比较器，默认为null</param>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InsertionSort<T>(this IList<T> list, IComparer<T>? comparer = null) =>
        list.InsertionSort(0, list.Count, comparer);

    /// <summary>
    /// 插入排序
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="index">起始索引</param>
    /// <param name="length">排序长度</param>
    /// <param name="comparer">元素比较器，默认为null</param>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
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
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InsertionSort<T>(this IList<T> list, Comparison<T> comparison) =>
        list.InsertionSort(0, list.Count, new ComparisonComparer<T>(comparison));

    /// <summary>
    ///  插入排序
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="index">起始索引</param>
    /// <param name="length">排序长度</param>
    /// <param name="comparison">元素比较表达式</param>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InsertionSort<T>(this IList<T> list, int index, int length, Comparison<T> comparison) =>
        list.InsertionSort(index, length, new ComparisonComparer<T>(comparison));

    /// <summary>插入排序，内部实现</summary>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
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

    #endregion

    #region 快速排序

    /// <summary>
    /// 快速排序
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="comparer">元素比较器，默认为null</param>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void QuickSort<T>(this IList<T> list, IComparer<T>? comparer = null) =>
        list.QuickSort(0, list.Count, comparer);

    /// <summary>
    /// 快速排序
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="index">起始索引</param>
    /// <param name="length">排序长度</param>
    /// <param name="comparer">元素比较器，默认为null</param>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public static void QuickSort<T>(this IList<T> list, int index, int length, IComparer<T>? comparer = null)
    {
        if (length <= 0) return;

        ArgumentOutOfRangeHelper.ThrowIfNegative(index);
        ArgumentOutOfRangeHelper.ThrowIfGreaterThan(index + length, list.Count);
        QuickSortInternal(list, index, index + length - 1, comparer ?? Comparer<T>.Default);
    }

    /// <summary>
    ///  快速排序
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="comparison">元素比较表达式</param>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void QuickSort<T>(this IList<T> list, Comparison<T> comparison) =>
        list.QuickSort(0, list.Count, new ComparisonComparer<T>(comparison));

    /// <summary>
    ///  快速排序
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="index">起始索引</param>
    /// <param name="length">排序长度</param>
    /// <param name="comparison">元素比较表达式</param>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void QuickSort<T>(this IList<T> list, int index, int length, Comparison<T> comparison) =>
        list.QuickSort(index, length, new ComparisonComparer<T>(comparison));

    /// <summary>快速排序，内部实现</summary>
    private static void QuickSortInternal<T>(this IList<T> list, int left, int right, IComparer<T> comparer)
    {
        if (left >= right) return;

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
        var pivot = MathHelper.Middle(list[left], list[right], list[(left + right) >> 1], comparer);
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

    #endregion

    #region 归并排序

    /// <summary>
    /// 归并排序
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="comparer">元素比较器，默认为null</param>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
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
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public static void MergeSort<T>(this IList<T> list, int index, int length, IComparer<T>? comparer = null)
    {
        if (length <= 0) return;

        ArgumentOutOfRangeHelper.ThrowIfNegative(index);
        ArgumentOutOfRangeHelper.ThrowIfGreaterThan(index + length, list.Count);
        using var temp = PooledArray.Get<T>(list.Count, true);
        MergeSortInternal(list, index, index + length - 1, comparer ?? Comparer<T>.Default, temp.Span);
    }

    /// <summary>
    ///  归并排序
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="comparison">元素比较表达式</param>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void MergeSort<T>(this IList<T> list, Comparison<T> comparison) =>
        list.MergeSort(0, list.Count, new ComparisonComparer<T>(comparison));

    /// <summary>
    ///  归并排序
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="index">起始索引</param>
    /// <param name="length">长度</param>
    /// <param name="comparison">元素比较表达式</param>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void MergeSort<T>(this IList<T> list, int index, int length, Comparison<T> comparison) =>
        list.MergeSort(index, length, new ComparisonComparer<T>(comparison));

    /// <summary>
    /// 并行归并排序
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="comparer">元素比较器，默认为null</param>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
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
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public static void MergeSortParallel<T>(this IList<T> list, int index, int length, IComparer<T>? comparer = null)
    {
        if (length <= 0) return;

        ArgumentOutOfRangeHelper.ThrowIfNegative(index);
        ArgumentOutOfRangeHelper.ThrowIfGreaterThan(index + length, list.Count);
        using var temp = PooledArray.Get<T>(list.Count, true);
        MergeSortParallelInternal(list, index, index + length - 1, comparer ?? Comparer<T>.Default, temp);
    }

    /// <summary>
    ///  并行归并排序
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="comparison">元素比较表达式</param>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void MergeSortParallel<T>(this IList<T> list, Comparison<T> comparison) =>
        list.MergeSortParallel(0, list.Count, new ComparisonComparer<T>(comparison));

    /// <summary>
    ///  并行归并排序
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="index">起始索引</param>
    /// <param name="length">长度</param>
    /// <param name="comparison">元素比较表达式</param>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void MergeSortParallel<T>(this IList<T> list, int index, int length, Comparison<T> comparison) =>
        list.MergeSortParallel(index, length, new ComparisonComparer<T>(comparison));

    /// <summary>归并排序，内部实现</summary>
    private static void MergeSortInternal<T>(IList<T> list, int left, int right, IComparer<T> comparer, Span<T> temp)
    {
        if (left >= right) return;

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
        PooledArray<T> temp)
    {
        if (left >= right) return;

        var mid = (left + right) >> 1;
        Parallel.Invoke(() => MergeSortParallelInternal(list, left, mid, comparer, temp),
            () => MergeSortParallelInternal(list, mid + 1, right, comparer, temp));
        // 如果左边最大的元素小于或等于右边最小的元素，则不需要合并
        if (comparer.Compare(list[mid], list[mid + 1]) <= 0)
        {
            return;
        }

        // 合并已排序的两部分
        MergeSortMerge(list, left, mid, right, comparer, temp.Slice(left, right - left + 1));
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

    #endregion

    #region 堆排序

    /// <summary>
    /// 堆排序
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="comparer">元素比较器，默认为null</param>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void HeapSort<T>(this IList<T> list, IComparer<T>? comparer = null) =>
        list.HeapSort(0, list.Count, comparer);

    /// <summary>
    /// 堆排序
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="index">起始索引</param>
    /// <param name="length">排序长度</param>
    /// <param name="comparer">元素比较器，默认为null</param>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public static void HeapSort<T>(this IList<T> list, int index, int length, IComparer<T>? comparer = null)
    {
        if (length <= 0) return;

        ArgumentOutOfRangeHelper.ThrowIfNegative(index);
        ArgumentOutOfRangeHelper.ThrowIfGreaterThan(index + length, list.Count);
        HeapSortInternal(list, index, index + length - 1, comparer ?? Comparer<T>.Default);
    }

    /// <summary>
    /// 堆排序
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="comparison">元素比较表达式</param>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void HeapSort<T>(this IList<T> list, Comparison<T> comparison) =>
        list.HeapSort(0, list.Count, new ComparisonComparer<T>(comparison));

    /// <summary>
    /// 堆排序
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="index">起始索引</param>
    /// <param name="length">排序长度</param>
    /// <param name="comparison">元素比较表达式</param>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void HeapSort<T>(this IList<T> list, int index, int length, Comparison<T> comparison) =>
        list.HeapSort(index, length, new ComparisonComparer<T>(comparison));

    /// <summary>堆排序，内部实现</summary>
    private static void HeapSortInternal<T>(IList<T> list, int left, int right, IComparer<T> comparer)
    {
        var count = right - left + 1;
        for (var i = left + (count / 2) - 1; i >= left; i--)
        {
            HeapSortHeapify(list, i, count, left, comparer);
        }

        for (var i = right; i > left; i--)
        {
            list.Swap(left, i);
            HeapSortHeapify(list, left, i - left, left, comparer);
        }
    }

    /// <summary>堆排序，堆化操作</summary>
    private static void HeapSortHeapify<T>(IList<T> list, int index, int length, int offset, IComparer<T> comparer)
    {
        var largest = index;
        var leftChild = 2 * (index - offset) + 1 + offset;
        var rightChild = 2 * (index - offset) + 2 + offset;
        if (leftChild < length + offset && comparer.Compare(list[leftChild], list[largest]) > 0)
        {
            largest = leftChild;
        }

        if (rightChild < length + offset && comparer.Compare(list[rightChild], list[largest]) > 0)
        {
            largest = rightChild;
        }

        if (largest == index) return;

        list.Swap(index, largest);
        HeapSortHeapify(list, largest, length, offset, comparer);
    }

    #endregion

    #region 内观排序

    /// <summary>内观排序转插入排序阈值</summary>
    private const int IntroInsertionSortThreshold = 16;

    /// <summary>
    /// 内观排序
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="comparer">比较器，默认为null</param>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void IntroSort<T>(this IList<T> list, IComparer<T>? comparer = null) =>
        list.IntroSort(0, list.Count, comparer);

    /// <summary>
    /// 内观排序
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="index">起始索引</param>
    /// <param name="length">排序长度</param>
    /// <param name="comparer">比较器，默认为null</param>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public static void IntroSort<T>(this IList<T> list, int index, int length, IComparer<T>? comparer = null)
    {
        if (length <= 0) return;

        ArgumentOutOfRangeHelper.ThrowIfNegative(index);
        ArgumentOutOfRangeHelper.ThrowIfGreaterThan(index + length, list.Count);
        var depthLimit = (int)(2 * Math.Log(list.Count, 2));
        IntroSortInternal(list, index, index + length - 1, depthLimit, comparer ?? Comparer<T>.Default);
    }

    /// <summary>
    /// 内观排序
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="comparison">元素比较表达式</param>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void IntroSort<T>(this IList<T> list, Comparison<T> comparison) =>
        list.IntroSort(0, list.Count, new ComparisonComparer<T>(comparison));

    /// <summary>内观排序，内部实现</summary>
    private static void IntroSortInternal<T>(IList<T> list, int left, int right, int depthLimit, IComparer<T> comparer)
    {
        var length = right - left + 1;
        // 数量小于阈值，使用插入排序
        if (length < IntroInsertionSortThreshold)
        {
            InsertionSortInternal(list, left, right, comparer);
            return;
        }

        // 达到深度限制，使用堆排序
        if (depthLimit <= 0)
        {
            HeapSortInternal(list, left, right, comparer);
            return;
        }

        var pivotIndex = QuickSortPartition(list, left, right, comparer);
        IntroSortInternal(list, left, pivotIndex, depthLimit - 1, comparer);
        IntroSortInternal(list, pivotIndex + 1, right, depthLimit - 1, comparer);
    }

    #endregion

    #region Tim排序

    /// <summary>Tim排序最小运行长度</summary>
    private const int TimSortMinRunLength = 32;

    /// <summary>
    /// Tim排序
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="comparer">元素比较器，默认为null</param>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void TimSort<T>(this IList<T> list, IComparer<T>? comparer = null) =>
        TimSort(list, 0, list.Count, comparer);

    /// <summary>
    /// Tim排序
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="index">起始索引</param>
    /// <param name="length">排序长度</param>
    /// <param name="comparer">元素比较器，默认为null</param>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public static void TimSort<T>(this IList<T> list, int index, int length, IComparer<T>? comparer = null)
    {
        if (length <= 0) return;

        ArgumentOutOfRangeHelper.ThrowIfNegative(index);
        ArgumentOutOfRangeHelper.ThrowIfGreaterThan(index + length, list.Count);
        comparer ??= Comparer<T>.Default;
        var end = index + length - 1;
        var minRunLength = TimSortGetMinRunLength(length);
        for (var i = index; i <= end; i += minRunLength)
        {
            InsertionSortInternal(list, i, Math.Min(i + minRunLength - 1, end), comparer);
        }

        using var temp = PooledArray.Get<T>(length, true);
        for (var size = minRunLength; size < length; size *= 2)
        {
            for (var left = index; left < end; left += size * 2)
            {
                var mid = left + size - 1;
                var right = Math.Min(left + size * 2 - 1, end);
                if (mid < right)
                {
                    MergeSortMerge(list, left, mid, right, comparer, temp.Span.Slice(left - index, right - left + 1));
                }
            }
        }
    }

    /// <summary>
    /// Tim排序
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="comparison">元素比较表达式</param>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void TimSort<T>(this IList<T> list, Comparison<T> comparison) =>
        list.TimSort(0, list.Count, new ComparisonComparer<T>(comparison));

    /// <summary>
    /// Tim排序
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="index">起始索引</param>
    /// <param name="length">排序长度</param>
    /// <param name="comparison">元素比较表达式</param>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void TimSort<T>(this IList<T> list, int index, int length, Comparison<T> comparison) =>
        list.TimSort(index, length, new ComparisonComparer<T>(comparison));

    /// <summary>获取最小运行长度</summary>
    private static int TimSortGetMinRunLength(int length)
    {
        var r = 0;
        while (length >= TimSortMinRunLength)
        {
            r |= (length & 1);
            length >>= 1;
        }

        return length + r;
    }

    #endregion

    #region 计数排序

    /// <summary>
    /// 计数排序，元素值范围[0,maxValue]
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="maxValue">最大值</param>
    /// <param name="descending">是否降序，默认为false</param>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void CountingSort(this IList<int> list, int maxValue, bool descending = false) =>
        list.CountingSort(0, list.Count, 0, maxValue, descending);

    /// <summary>
    /// 计数排序，元素值范围[minValue,maxValue]
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="minValue">最小值</param>
    /// <param name="maxValue">最大值</param>
    /// <param name="descending">是否降序，默认为false</param>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void CountingSort(this IList<int> list, int minValue, int maxValue, bool descending = false) =>
        list.CountingSort(0, list.Count, minValue, maxValue, descending);

    /// <summary>
    /// 计数排序，元素值范围[0,maxValue]
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="index">起始索引</param>
    /// <param name="length">排序长度</param>
    /// <param name="maxValue">最大值</param>
    /// <param name="descending">是否降序，默认为false</param>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void CountingSort(this IList<int> list, int index, int length, int maxValue, bool descending = false)
        => list.CountingSort(index, length, 0, maxValue, descending);

    /// <summary>
    /// 计数排序，元素值范围[minValue,maxValue]
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="index">起始索引</param>
    /// <param name="length">排序长度</param>
    /// <param name="minValue">最小值</param>
    /// <param name="maxValue">最大值</param>
    /// <param name="descending">是否降序，默认为false</param>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public static void CountingSort(this IList<int> list, int index, int length, int minValue, int maxValue,
        bool descending = false)
    {
        if (length <= 0) return;

        ArgumentOutOfRangeHelper.ThrowIfNegative(index);
        ArgumentOutOfRangeHelper.ThrowIfGreaterThan(index + length, list.Count);
        var counter = new int[maxValue - minValue + 1];
        for (var i = index; i < index + length; i++)
        {
            counter[list[i] - minValue]++;
        }

        if (descending)
        {
            for (var i = length - 1; i >= 0; i--)
            {
                for (var j = 0; j < counter[i]; j++)
                {
                    list[index++] = i + minValue;
                }
            }

            return;
        }

        for (var i = 0; i < counter.Length; i++)
        {
            for (var j = 0; j < counter[i]; j++)
            {
                list[index++] = i + minValue;
            }
        }
    }

    #endregion
}