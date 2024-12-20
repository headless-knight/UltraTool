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
    private const int InsertionSortThreshold = 16;

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

        var length = right - left + 1;
        // 小于阈值，使用插入排序
        if (length <= InsertionSortThreshold)
        {
            InsertionSortInternal(list, left, right, comparer);
            return;
        }

        // 分区操作后基准元素的正确位置
        var mid = QuickSortPartition(list, left, right, comparer);
        // 递归排序左子数组
        QuickSortInternal(list, left, mid, comparer);
        // 递归排序右子数组
        QuickSortInternal(list, mid + 1, right, comparer);
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
        using var temporary = PooledArray.Get<T>(length, true);
        MergeSortInternal(list, index, index + length - 1, comparer ?? Comparer<T>.Default, temporary.Span, index);
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

    /// <summary>归并排序，内部实现</summary>
    private static void MergeSortInternal<T>(IList<T> list, int left, int right, IComparer<T> comparer,
        Span<T> temporary, int offset)
    {
        if (left >= right) return;

        var length = right - left + 1;
        // 小于阈值，使用插入排序
        if (length <= InsertionSortThreshold)
        {
            InsertionSortInternal(list, left, right, comparer);
            return;
        }

        var mid = (left + right) >> 1;
        // 递归地对左半部分进行排序
        MergeSortInternal(list, left, mid, comparer, temporary, offset);
        // 递归地对右半部分进行排序
        MergeSortInternal(list, mid + 1, right, comparer, temporary, offset);
        // 如果左边最大的元素小于或等于右边最小的元素，则不需要合并
        if (comparer.Compare(list[mid], list[mid + 1]) <= 0)
        {
            return;
        }

        // 合并已排序的两部分
        MergeSortMerge(list, left, mid, right, comparer, temporary.Slice(left - offset, length));
    }

    /// <summary>归并排序，合并操作</summary>
    private static void MergeSortMerge<T>(IList<T> list, int left, int mid, int right, IComparer<T> comparer,
        Span<T> temporary)
    {
        int i = left, j = mid + 1, k = 0;
        while (i <= mid && j <= right)
        {
            // 将较小的元素复制到临时数组
            if (comparer.Compare(list[i], list[j]) <= 0)
            {
                temporary[k++] = list[i++];
            }
            else
            {
                temporary[k++] = list[j++];
            }
        }

        // 如果左半部分还有剩余元素，将其复制到临时数组
        while (i <= mid)
        {
            temporary[k++] = list[i++];
        }

        // 如果右半部分还有剩余元素，将其复制到临时数组
        while (j <= right)
        {
            temporary[k++] = list[j++];
        }

        // 将临时数组的排序结果复制回原数组
        for (i = 0; i < k; i++)
        {
            list[left + i] = temporary[i];
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
        var length = right - left + 1;
        // 小于阈值，使用插入排序
        if (length <= InsertionSortThreshold)
        {
            InsertionSortInternal(list, left, right, comparer);
            return;
        }

        for (var i = left + (length / 2) - 1; i >= left; i--)
        {
            HeapSortHeapify(list, i, length, left, comparer);
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
        while (true)
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
            index = largest;
        }
    }

    #endregion

    #region 内观排序

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

    /// <summary>
    /// 内观排序
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="index">起始索引</param>
    /// <param name="length">排序长度</param>
    /// <param name="comparison">元素比较表达式</param>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void IntroSort<T>(this IList<T> list, int index, int length, Comparison<T> comparison) =>
        list.IntroSort(index, length, new ComparisonComparer<T>(comparison));

    /// <summary>内观排序，内部实现</summary>
    private static void IntroSortInternal<T>(IList<T> list, int left, int right, int depthLimit, IComparer<T> comparer)
    {
        var length = right - left + 1;
        // 小于阈值，使用插入排序
        if (length <= InsertionSortThreshold)
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

        using var temporary = PooledArray.Get<T>(length, true);
        for (var size = minRunLength; size < length; size *= 2)
        {
            for (var left = index; left < end; left += size * 2)
            {
                var mid = left + size - 1;
                var right = Math.Min(left + size * 2 - 1, end);
                if (mid < right)
                {
                    MergeSortMerge(list, left, mid, right, comparer, temporary.Slice(left - index, right - left + 1));
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
    /// 计数排序
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="descending">是否降序，默认为false</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void CountingSort(this IList<int> list, bool descending = false) =>
        list.CountingSort(0, list.Count, descending);

    /// <summary>
    /// 计数排序
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="index">起始索引</param>
    /// <param name="length">排序长度</param>
    /// <param name="descending">是否降序，默认为false</param>
    public static void CountingSort(this IList<int> list, int index, int length, bool descending = false)
    {
        if (length <= 0) return;

        var minValue = list[index];
        var maxValue = minValue;
        var end = index + length - 1;
        for (var i = index + 1; i <= end; i++)
        {
            var value = list[i];
            minValue = Math.Min(value, minValue);
            maxValue = Math.Max(value, maxValue);
        }

        list.CountingSort(index, length, minValue, maxValue, descending);
    }

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
        using var counting = PooledArray.GetCleared<int>(maxValue - minValue + 1, true);
        var end = index + length - 1;
        for (var i = index; i <= end; i++)
        {
            counting[list[i] - minValue]++;
        }

        if (descending)
        {
            for (var i = counting.Length - 1; i >= 0; i--)
            {
                for (var j = 0; j < counting[i]; j++)
                {
                    list[index++] = i + minValue;
                }
            }

            return;
        }

        for (var i = 0; i < counting.Length; i++)
        {
            for (var j = 0; j < counting[i]; j++)
            {
                list[index++] = i + minValue;
            }
        }
    }

    #endregion

    #region 基数排序

    /// <summary>基数排序最大位</summary>
    private const int RadixSortMaxExp = 1000000000;

    /// <summary>
    /// 基数排序
    /// </summary>
    /// <param name="list">列表</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void RadixSort(this IList<int> list) =>
        list.RadixSort(0, list.Count);

    /// <summary>
    /// 基数排序
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="index">索引</param>
    /// <param name="length">长度</param>
    public static void RadixSort(this IList<int> list, int index, int length)
    {
        if (length <= 0) return;

        ArgumentOutOfRangeHelper.ThrowIfNegative(index);
        ArgumentOutOfRangeHelper.ThrowIfGreaterThan(index + length, list.Count);
        var end = index + length - 1;
        var (mid, negativeMax, positiveMax) = RadixSortPartition(list, index, end);
        using var temporary = PooledArray.Get<int>(length, true);
        // 排序负数部分
        if (mid >= index && mid <= end)
        {
            RadixSortInternal(list, index, mid, negativeMax, temporary.Span, true);
        }

        // 排序正数部分
        if (mid + 1 <= end)
        {
            RadixSortInternal(list, mid + 1, end, positiveMax, temporary.Span, false);
        }
    }

    /// <summary>基数排序，分区操作</summary>
    private static (int, int, int) RadixSortPartition(IList<int> list, int start, int end)
    {
        var negativeMax = 0;
        var positiveMax = 0;
        var left = start;
        var right = end;
        // 将列表中的负数移动到左侧并取反，非负数移动到右侧
        while (true)
        {
            while (left <= right)
            {
                var value = list[left];
                if (value < 0)
                {
                    // 将负数取反保存
                    list[left] = -value;
                    negativeMax = Math.Max(negativeMax, -value);
                    left++;
                }
                else
                {
                    positiveMax = Math.Max(positiveMax, value);
                    break;
                }
            }

            while (left <= right)
            {
                var value = list[right];
                if (value >= 0)
                {
                    positiveMax = Math.Max(positiveMax, value);
                    right--;
                }
                else
                {
                    // 将负数取反保存
                    list[right] = -value;
                    negativeMax = Math.Max(negativeMax, -value);
                    break;
                }
            }

            if (left >= right) break;

            list.Swap(left++, right--);
        }

        return (right, Math.Min(negativeMax, RadixSortMaxExp), Math.Min(positiveMax, RadixSortMaxExp));
    }

    /// <summary>基数排序，内部实现</summary>
    private static void RadixSortInternal(IList<int> list, int start, int end, int maxValue, Span<int> temporary,
        bool isNegative)
    {
        // 按照从低位到高位的顺序遍历
        for (var exp = 1; exp <= maxValue; exp *= 10)
        {
            RadixSortRound(list, start, end, exp, temporary);
        }

        if (!isNegative) return;

        // 如果这部分数据原来是负数，需要将这部分数据取反并反转顺序
        var mid = (start + end) / 2;
        for (var left = start; left <= mid; left++)
        {
            var right = end - (left - start);
            (list[right], list[left]) = (-list[left], -list[right]);
        }
    }

    /// <summary>基数排序，轮次操作</summary>
    private static void RadixSortRound(IList<int> list, int start, int end, int exp, Span<int> temporary)
    {
        Span<int> counting = stackalloc int[10];
        for (var i = start; i <= end; i++)
        {
            counting[RadixSortDigit(list[i], exp)]++;
        }

        for (var i = 1; i < 10; i++)
        {
            counting[i] += counting[i - 1];
        }

        for (var i = end; i >= start; i--)
        {
            var count = counting[RadixSortDigit(list[i], exp)]--;
            temporary[count - 1] = list[i];
        }

        for (var i = start; i <= end; i++)
        {
            list[i] = temporary[i - start];
        }
    }

    /// <summary>基数排序，获取数字指定位</summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int RadixSortDigit(int value, int exp) =>
        value / exp % 10;

    #endregion
}