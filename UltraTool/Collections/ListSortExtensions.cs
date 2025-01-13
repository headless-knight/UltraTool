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
    ///  插入排序
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="comparison">元素比较表达式</param>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InsertionSort<T>(this IList<T> list, Comparison<T> comparison) =>
        list.InsertionSort(0, list.Count, comparison);

    /// <summary>
    ///  插入排序
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="index">起始索引</param>
    /// <param name="count">排序数量</param>
    /// <param name="comparison">元素比较表达式</param>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public static void InsertionSort<T>(this IList<T> list, int index, int count, Comparison<T> comparison)
    {
        ArgumentOutOfRangeHelper.ThrowIfNegative(index);
        ArgumentOutOfRangeHelper.ThrowIfNegative(count);
        ArgumentOutOfRangeHelper.ThrowIfGreaterThan(index + count, list.Count);
        if (count == 0) return;

        InsertionSortInternal(list, index, index + count - 1, new ValueComparisonComparer<T>(comparison));
    }

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
    /// <param name="count">排序数量</param>
    /// <param name="comparer">元素比较器，默认为null</param>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public static void InsertionSort<T>(this IList<T> list, int index, int count, IComparer<T>? comparer = null)
    {
        ArgumentOutOfRangeHelper.ThrowIfNegative(index);
        ArgumentOutOfRangeHelper.ThrowIfNegative(count);
        ArgumentOutOfRangeHelper.ThrowIfGreaterThan(index + count, list.Count);
        if (count == 0) return;

        InsertionSortInternal(list, index, index + count - 1, comparer ?? Comparer<T>.Default);
    }

    /// <summary>插入排序，内部实现</summary>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    private static void InsertionSortInternal<T, TComparer>(IList<T> list, int left, int right, TComparer comparer)
        where TComparer : IComparer<T>
    {
        if (left >= right) return;

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
    ///  快速排序
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="comparison">元素比较表达式</param>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void QuickSort<T>(this IList<T> list, Comparison<T> comparison) =>
        list.QuickSort(0, list.Count, comparison);

    /// <summary>
    ///  快速排序
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="index">起始索引</param>
    /// <param name="count">排序数量</param>
    /// <param name="comparison">元素比较表达式</param>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public static void QuickSort<T>(this IList<T> list, int index, int count, Comparison<T> comparison)
    {
        ArgumentOutOfRangeHelper.ThrowIfNegative(index);
        ArgumentOutOfRangeHelper.ThrowIfNegative(count);
        ArgumentOutOfRangeHelper.ThrowIfGreaterThan(index + count, list.Count);
        if (count == 0) return;

        QuickSortInternal(list, index, count, new ValueComparisonComparer<T>(comparison));
    }

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
    /// <param name="count">排序数量</param>
    /// <param name="comparer">元素比较器，默认为null</param>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public static void QuickSort<T>(this IList<T> list, int index, int count, IComparer<T>? comparer = null)
    {
        ArgumentOutOfRangeHelper.ThrowIfNegative(index);
        ArgumentOutOfRangeHelper.ThrowIfNegative(count);
        ArgumentOutOfRangeHelper.ThrowIfGreaterThan(index + count, list.Count);
        if (count == 0) return;

        QuickSortInternal(list, index, index + count - 1, comparer ?? Comparer<T>.Default);
    }

    /// <summary>快速排序，内部实现</summary>
    private static void QuickSortInternal<T, TComparer>(IList<T> list, int left, int right, TComparer comparer)
        where TComparer : IComparer<T>
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
    private static int QuickSortPartition<T, TComparer>(IList<T> list, int left, int right, TComparer comparer)
        where TComparer : IComparer<T>
    {
        // 三值取中，获取基准值
        var pivot = MathHelper.Middle(list[left], list[right], list[left + (right - left >> 1)], comparer);
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
    ///  归并排序
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="comparison">元素比较表达式</param>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void MergeSort<T>(this IList<T> list, Comparison<T> comparison) =>
        list.MergeSort(0, list.Count, comparison);

    /// <summary>
    ///  归并排序
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="index">起始索引</param>
    /// <param name="count">长度</param>
    /// <param name="comparison">元素比较表达式</param>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public static void MergeSort<T>(this IList<T> list, int index, int count, Comparison<T> comparison)
    {
        ArgumentOutOfRangeHelper.ThrowIfNegative(index);
        ArgumentOutOfRangeHelper.ThrowIfNegative(count);
        ArgumentOutOfRangeHelper.ThrowIfGreaterThan(index + count, list.Count);
        if (count == 0) return;

        var temporary = ArrayHelper.AllocateUninitializedArray<T>(list.Count);
        MergeSortInternal(list, index, index + count - 1, new ValueComparisonComparer<T>(comparison), temporary);
    }

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
    /// <param name="count">长度</param>
    /// <param name="comparer">元素比较器，默认为null</param>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public static void MergeSort<T>(this IList<T> list, int index, int count, IComparer<T>? comparer = null)
    {
        ArgumentOutOfRangeHelper.ThrowIfNegative(index);
        ArgumentOutOfRangeHelper.ThrowIfNegative(count);
        ArgumentOutOfRangeHelper.ThrowIfGreaterThan(index + count, list.Count);
        if (count == 0) return;

        var temporary = ArrayHelper.AllocateUninitializedArray<T>(list.Count);
        MergeSortInternal(list, index, index + count - 1, comparer ?? Comparer<T>.Default, temporary);
    }

    /// <summary>归并排序，内部实现</summary>
    private static void MergeSortInternal<T, TComparer>(IList<T> list, int left, int right, TComparer comparer,
        Span<T> temporary) where TComparer : IComparer<T>
    {
        if (left >= right) return;

        var length = right - left + 1;
        // 小于阈值，使用插入排序
        if (length <= InsertionSortThreshold)
        {
            InsertionSortInternal(list, left, right, comparer);
            return;
        }

        var mid = left + (right - left >> 1);
        // 递归地对左半部分进行排序
        MergeSortInternal(list, left, mid, comparer, temporary);
        // 递归地对右半部分进行排序
        MergeSortInternal(list, mid + 1, right, comparer, temporary);
        // 如果左边最大的元素小于或等于右边最小的元素，则不需要合并
        if (comparer.Compare(list[mid], list[mid + 1]) <= 0)
        {
            return;
        }

        // 合并已排序的两部分
        MergeSortMerge(list, left, mid, right, comparer, temporary.Slice(left, length));
    }

    /// <summary>归并排序，合并操作</summary>
    private static void MergeSortMerge<T, TComparer>(IList<T> list, int left, int mid, int right, TComparer comparer,
        Span<T> temporary) where TComparer : IComparer<T>
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
    /// <param name="comparison">元素比较表达式</param>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void HeapSort<T>(this IList<T> list, Comparison<T> comparison) =>
        list.HeapSort(0, list.Count, comparison);

    /// <summary>
    /// 堆排序
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="index">起始索引</param>
    /// <param name="count">排序数量</param>
    /// <param name="comparison">元素比较表达式</param>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public static void HeapSort<T>(this IList<T> list, int index, int count, Comparison<T> comparison)
    {
        ArgumentOutOfRangeHelper.ThrowIfNegative(index);
        ArgumentOutOfRangeHelper.ThrowIfNegative(count);
        ArgumentOutOfRangeHelper.ThrowIfGreaterThan(index + count, list.Count);
        if (count == 0) return;

        HeapSortInternal(list, index, index + count - 1, new ValueComparisonComparer<T>(comparison));
    }

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
    /// <param name="count">排序数量</param>
    /// <param name="comparer">元素比较器，默认为null</param>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public static void HeapSort<T>(this IList<T> list, int index, int count, IComparer<T>? comparer = null)
    {
        ArgumentOutOfRangeHelper.ThrowIfNegative(index);
        ArgumentOutOfRangeHelper.ThrowIfNegative(count);
        ArgumentOutOfRangeHelper.ThrowIfGreaterThan(index + count, list.Count);
        if (count == 0) return;

        HeapSortInternal(list, index, index + count - 1, comparer ?? Comparer<T>.Default);
    }

    /// <summary>堆排序，内部实现</summary>
    private static void HeapSortInternal<T, TComparer>(IList<T> list, int left, int right, TComparer comparer)
        where TComparer : IComparer<T>
    {
        var length = right - left + 1;
        // 小于阈值，使用插入排序
        if (length <= InsertionSortThreshold)
        {
            InsertionSortInternal(list, left, right, comparer);
            return;
        }

        for (var i = left + (length >> 1) - 1; i >= left; i--)
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
    private static void HeapSortHeapify<T, TComparer>(IList<T> list, int index, int count, int offset,
        TComparer comparer) where TComparer : IComparer<T>
    {
        while (true)
        {
            var largest = index;
            var leftChild = ((index - offset) << 1) + 1 + offset;
            var rightChild = ((index - offset) << 1) + 2 + offset;
            if (leftChild < count + offset && comparer.Compare(list[leftChild], list[largest]) > 0)
            {
                largest = leftChild;
            }

            if (rightChild < count + offset && comparer.Compare(list[rightChild], list[largest]) > 0)
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
    /// <param name="comparison">元素比较表达式</param>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void IntroSort<T>(this IList<T> list, Comparison<T> comparison) =>
        list.IntroSort(0, list.Count, comparison);

    /// <summary>
    /// 内观排序
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="index">起始索引</param>
    /// <param name="count">排序数量</param>
    /// <param name="comparison">元素比较表达式</param>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public static void IntroSort<T>(this IList<T> list, int index, int count, Comparison<T> comparison)
    {
        ArgumentOutOfRangeHelper.ThrowIfNegative(index);
        ArgumentOutOfRangeHelper.ThrowIfNegative(count);
        ArgumentOutOfRangeHelper.ThrowIfGreaterThan(index + count, list.Count);
        if (count == 0) return;

        var depthLimit = IntroSortGetDepthLimit(count);
        IntroSortInternal(list, index, index + count - 1, depthLimit, new ValueComparisonComparer<T>(comparison));
    }

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
    /// <param name="count">排序数量</param>
    /// <param name="comparer">比较器，默认为null</param>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public static void IntroSort<T>(this IList<T> list, int index, int count, IComparer<T>? comparer = null)
    {
        ArgumentOutOfRangeHelper.ThrowIfNegative(index);
        ArgumentOutOfRangeHelper.ThrowIfNegative(count);
        ArgumentOutOfRangeHelper.ThrowIfGreaterThan(index + count, list.Count);
        if (count == 0) return;

        var depthLimit = IntroSortGetDepthLimit(count);
        IntroSortInternal(list, index, index + count - 1, depthLimit, comparer ?? Comparer<T>.Default);
    }

    /// <summary>内观排序，内部实现</summary>
    private static void IntroSortInternal<T, TComparer>(IList<T> list, int left, int right, int depthLimit,
        TComparer comparer) where TComparer : IComparer<T>
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

    /// <summary>内观排序，计算深度限制</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int IntroSortGetDepthLimit(int length) => (int)(2 * Math.Log(length, 2));

    #endregion

    #region Tim排序

    /// <summary>Tim排序最小运行长度</summary>
    private const int TimSortMinRunLength = 32;

    /// <summary>
    /// Tim排序
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="comparison">元素比较表达式</param>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void TimSort<T>(this IList<T> list, Comparison<T> comparison) =>
        list.TimSort(0, list.Count, comparison);

    /// <summary>
    /// Tim排序
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="index">起始索引</param>
    /// <param name="count">排序数量</param>
    /// <param name="comparison">元素比较表达式</param>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public static void TimSort<T>(this IList<T> list, int index, int count, Comparison<T> comparison)
    {
        ArgumentOutOfRangeHelper.ThrowIfNegative(index);
        ArgumentOutOfRangeHelper.ThrowIfNegative(count);
        ArgumentOutOfRangeHelper.ThrowIfGreaterThan(index + count, list.Count);
        if (count == 0) return;

        TimSortInternal(list, index, count, new ValueComparisonComparer<T>(comparison));
    }

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
    /// <param name="count">排序数量</param>
    /// <param name="comparer">元素比较器，默认为null</param>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public static void TimSort<T>(this IList<T> list, int index, int count, IComparer<T>? comparer = null)
    {
        ArgumentOutOfRangeHelper.ThrowIfNegative(index);
        ArgumentOutOfRangeHelper.ThrowIfNegative(count);
        ArgumentOutOfRangeHelper.ThrowIfGreaterThan(index + count, list.Count);
        if (count == 0) return;

        TimSortInternal(list, index, count, comparer ?? Comparer<T>.Default);
    }

    /// <summary>Tim排序，内部实现</summary>
    private static void TimSortInternal<T, TComparer>(IList<T> list, int index, int count, TComparer comparer)
        where TComparer : IComparer<T>
    {
        var end = index + count - 1;
        var minRunLength = TimSortGetMinRunLength(count);
        for (var i = index; i <= end; i += minRunLength)
        {
            InsertionSortInternal(list, i, Math.Min(i + minRunLength - 1, end), comparer);
        }

        var temporary = ArrayHelper.AllocateUninitializedArray<T>(count);
        for (var size = minRunLength; size < count; size <<= 1)
        {
            for (var left = index; left < end; left += (size << 1))
            {
                var mid = left + size - 1;
                var right = Math.Min(left + (size << 1) - 1, end);
                if (mid < right)
                {
                    MergeSortMerge(list, left, mid, right, comparer, temporary.AsSpan(left - index, right - left + 1));
                }
            }
        }
    }

    /// <summary>Tim排序，计算最小运行长度</summary>
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
}