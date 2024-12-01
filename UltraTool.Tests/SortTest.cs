using UltraTool.Collections;

namespace UltraTool.Tests;

public class SortTest
{
    [Fact]
    public void InsertionSortTest()
    {
        var list = new List<int> { 5, 4, 2, 88, 1 };
        list.InsertionSort(1, 3);
        Assert.Equal([5, 2, 4, 88, 1], list);
        list.InsertionSort();
        Assert.Equal([1, 2, 4, 5, 88], list);
    }

    [Fact]
    public void QuickSortTest()
    {
        var list = new List<int>(50);
        for (var i = 0; i < 50; i++)
        {
            list.Add(Random.Shared.Next());
        }

        list.QuickSort(10, 10);
        for (var i = 10; i < 19; i++)
        {
            Assert.True(list[i] <= list[i + 1]);
        }

        list.QuickSort();
        for (var i = 0; i < list.Count - 1; i++)
        {
            Assert.True(list[i] <= list[i + 1]);
        }
    }

    [Fact]
    public void MergeSortTest()
    {
        var list = new List<int>(50);
        for (var i = 0; i < 50; i++)
        {
            list.Add(Random.Shared.Next());
        }

        list.MergeSort(10, 10);
        for (var i = 10; i < 19; i++)
        {
            Assert.True(list[i] <= list[i + 1]);
        }

        list.MergeSort();
        for (var i = 0; i < list.Count - 1; i++)
        {
            Assert.True(list[i] <= list[i + 1]);
        }
    }

    [Fact]
    public void HeapSortTest()
    {
        var list = new List<int>(50);
        for (var i = 0; i < 50; i++)
        {
            list.Add(Random.Shared.Next());
        }

        list.HeapSort(10, 10);
        for (var i = 10; i < 19; i++)
        {
            Assert.True(list[i] <= list[i + 1]);
        }

        list.HeapSort();
    }

    [Fact]
    public void IntroSortTest()
    {
        var list = new List<int>(100);
        for (var i = 0; i < 100; i++)
        {
            list.Add(Random.Shared.Next());
        }

        list.IntroSort(10, 40);
        for (var i = 10; i < 49; i++)
        {
            Assert.True(list[i] <= list[i + 1]);
        }

        list.IntroSort();
        for (var i = 0; i < list.Count - 1; i++)
        {
            Assert.True(list[i] <= list[i + 1]);
        }
    }

    [Fact]
    public void TimSortTest()
    {
        var list = new List<int>(100);
        for (var i = 0; i < 100; i++)
        {
            list.Add(Random.Shared.Next());
        }

        list.TimSort(10, 40);
        for (var i = 10; i < 49; i++)
        {
            Assert.True(list[i] <= list[i + 1]);
        }

        list.TimSort();
        for (var i = 0; i < list.Count - 1; i++)
        {
            Assert.True(list[i] <= list[i + 1]);
        }
    }

    [Fact]
    public void CountingSortTest()
    {
        var list = new List<int>(50);
        for (var i = 0; i < 50; i++)
        {
            list.Add(Random.Shared.Next(0, 100));
        }

        list.CountingSort(0, 99);
        for (var i = 0; i < list.Count - 1; i++)
        {
            Assert.True(list[i] <= list[i + 1]);
        }
    }
}