using System.Collections.Generic;
using Unity.VisualScripting;


namespace BehaviorTree
{
    public enum NodeState
    {
        RUNNING,
        SUCCESS,
        FAILURE
    };

    public class Node
    {
        protected NodeState m_state;

        public Node parent;
        protected List<Node> m_children = new List<Node>();

        protected Tree m_tree;


        public Node()
        {
            parent = null;
        }

        public Node(List<Node> _children)
        {
            foreach (Node child in _children)
            {
                Attach(child);
            }
        }

        public void SetTree(Tree _tree)
        {
            m_tree = _tree;
            foreach (Node child in m_children)
            {
                child.SetTree(_tree);
            }
        }

        private void Attach(Node _node)
        {
            _node.parent = this;
            m_children.Add(_node);
        }

        public void OnDestroy()
        {
            
        }

    public virtual NodeState Evaluate() => NodeState.FAILURE;
    }
}
