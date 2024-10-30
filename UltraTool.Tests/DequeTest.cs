using UltraTool.Collections;

namespace UltraTool.Tests;

public class DequeTest
{
    [Fact]
    public void Test()
    {
        var deque = new Deque<int>();
        deque.EnqueueFirst(1);
        Assert.Equal(1, deque.PeekFirst());
        deque.EnqueueFirst(2);
        Assert.Equal(2, deque.PeekFirst());
        Assert.Equal(1, deque.PeekLast());
        deque.EnqueueLast(3);
        deque.EnqueueLast(4);
        deque.EnqueueLast(6);
        Assert.Equal(2, deque.DequeueFirst());
        Assert.Equal(6, deque.DequeueLast());
        Assert.Equal(3, deque.Count);
        Assert.Equal([1, 3, 4], deque.ToArray());
    }
}