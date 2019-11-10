using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTree<T> : MonoBehaviour
{

    protected BehaviourNode<T> root;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void updateBehaviour()
    {
        root.process();
    }
}

public enum ProcessResult
{
    Failed,
    Running,
    Success
}

public abstract class BehaviourNode<T>
{
    protected T m_tree;

    public virtual void init(T tree)
    {
        m_tree = tree;
    }

    public virtual void addNode(BehaviourNode<T> node)
    {

    }

    public abstract ProcessResult process();
}

public abstract class CompositeNode<T> : BehaviourNode<T>
{
    protected List<BehaviourNode<T>> m_children;

    public CompositeNode() {
        m_children = new List<BehaviourNode<T>>();
    }

    public override void init(T tree)
    {
        base.init(tree);
        foreach(var children in m_children)
        {
            children.init(tree);
        }
    }

    public override void addNode(BehaviourNode<T> node)
    {
        m_children.Add(node);
    }
}

public abstract class LeafNode<T> : BehaviourNode<T>
{

}

public class SequentialNode<T> : CompositeNode<T>
{
    public override ProcessResult process()
    {
        ProcessResult result = ProcessResult.Success;
        foreach (var children in m_children)
        {
            result = children.process();
            if(result != ProcessResult.Success)
            {
                break;
            }
        }
        return result;
    }
}

public class SelectorNode<T> : CompositeNode<T>
{
    public override ProcessResult process()
    {
        ProcessResult result = ProcessResult.Success;
        foreach (var children in m_children)
        {
            result = children.process();
            if (result != ProcessResult.Failed)
            {
                break;
            }
        }
        return result;
    }
}

public class ParralelNode<T> : CompositeNode<T>
{
    public override ProcessResult process()
    {
        ProcessResult result = ProcessResult.Success;
        foreach (var children in m_children)
        {
            var childrenResult = children.process();
            if(childrenResult == ProcessResult.Failed)
            {
                result = childrenResult;
            }
        }
        return result;
    }
}

public abstract class ConditionalLeaf<T> : LeafNode<T>
{
    ConditionalLeaf()
    {

    }
}


