using UltraTool.Numerics;

namespace UltraTool.Tests;

public class IntegerTest
{
    [Fact]
    public void SpliceOrSplitTest()
    {
        var splicedShort = IntegerHelper.SpliceToShort((byte)2, 3);
        Assert.Equal(2, IntegerHelper.SplitToByte(splicedShort).Upper);
        Assert.Equal(3, IntegerHelper.SplitToByte(splicedShort).Lower);
        splicedShort = IntegerHelper.SpliceToShort(5, -9);
        Assert.Equal(5, IntegerHelper.SplitToSByte(splicedShort).Upper);
        Assert.Equal(-9, IntegerHelper.SplitToSByte(splicedShort).Lower);
        var splicedInt = IntegerHelper.SpliceToInt(splicedShort, 4);
        Assert.Equal(splicedShort, IntegerHelper.SplitToShort(splicedInt).Upper);
        Assert.Equal(4, IntegerHelper.SplitToShort(splicedInt).Lower);
        var splicedUInt = IntegerHelper.SpliceToUInt(5, -9);
        Assert.Equal(5, IntegerHelper.SplitToShort(splicedUInt).Upper);
        Assert.Equal(-9, IntegerHelper.SplitToShort(splicedUInt).Lower);
    }

    [Fact]
    public void IsEvenOrOddTest()
    {
        Assert.True(2.IsEven());
        Assert.False(3.IsEven());
        Assert.True(3.IsOdd());
        Assert.True(2UL.IsEven());
        Assert.False(3UL.IsEven());
        Assert.True(3UL.IsOdd());
    }
}