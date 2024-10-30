using JetBrains.Annotations;

namespace UltraTool.Collections;

/// <summary>
/// 单链表节点
/// </summary>
[PublicAPI]
public class SingleLinkedListNode<T>
{
    /// <summary>
    /// 节点数据
    /// </summary>
    public T Value { get; set; }

    /// <summary>
    /// 下一节点
    /// </summary>
    public SingleLinkedListNode<T>? Next { get; internal set; }

    /// <summary>
    /// 节点所属链表
    /// </summary>
    public SingleLinkedList<T>? List { get; internal set; }

    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="value">节点数据</param>
    internal SingleLinkedListNode(T value)
    {
        Value = value;
    }
}