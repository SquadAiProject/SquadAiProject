using System.Collections.Generic;


namespace BehaviorTree
{
    public class Selector : Node
    {
        public Selector() : base() {}
        
        public Selector(List<Node> _children) : base(_children) {}

        public override NodeState Evaluate()
        {
            foreach (Node node in m_children)
            {
                switch (node.Evaluate())
                {
                    case NodeState.RUNNING:
                        m_state = NodeState.RUNNING;
                        return m_state;
                    case NodeState.SUCCESS:
                        m_state = NodeState.SUCCESS;
                        return m_state;
                    case NodeState.FAILURE:
                        continue;
                    default:
                        continue;
                }
            }

            m_state = NodeState.FAILURE;
            return m_state;
        }
    }
}
