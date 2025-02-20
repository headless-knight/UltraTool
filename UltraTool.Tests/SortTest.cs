﻿using UltraTool.Collections;

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
        var list = new List<int>(100);
        for (var i = 0; i < 100; i++)
        {
            list.Add(Random.Shared.Next());
        }

        list.QuickSort(10, 20);
        Assert.True(list.Skip(10).Take(20).IsOrdered());
        list.QuickSort();
        Assert.True(list.IsOrdered());
    }

    [Fact]
    public void MergeSortTest()
    {
        var list = new List<int>(100);
        for (var i = 0; i < 100; i++)
        {
            list.Add(Random.Shared.Next());
        }

        list.MergeSort(10, 20);
        Assert.True(list.Skip(10).Take(20).IsOrdered());
        list.MergeSort();
        Assert.True(list.IsOrdered());
    }

    [Fact]
    public void HeapSortTest()
    {
        var list = new List<int>(100);
        for (var i = 0; i < 100; i++)
        {
            list.Add(Random.Shared.Next());
        }

        list.HeapSort(10, 20);
        Assert.True(list.Skip(10).Take(20).IsOrdered());
        list.HeapSort();
        Assert.True(list.IsOrdered());
    }

    [Fact]
    public void IntroSortTest()
    {
        var list = new List<int>(500);
        for (var i = 0; i < 500; i++)
        {
            list.Add(Random.Shared.Next());
        }

        list.IntroSort(10, 50);
        Assert.True(list.Skip(10).Take(50).IsOrdered());
        list.IntroSort();
        Assert.True(list.IsOrdered());
    }

    [Fact]
    public void TimSortTest()
    {
        var list = new List<int>(500);
        for (var i = 0; i < 500; i++)
        {
            list.Add(Random.Shared.Next());
        }

        list.TimSort(10, 50);
        Assert.True(list.Skip(10).Take(50).IsOrdered());
        list.TimSort();
        Assert.True(list.IsOrdered());
    }
}