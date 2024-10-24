using System;
using System.Collections.Generic;

namespace BehaviorTree
{
    public class Sequence : Node
    {
        public Sequence() : base() {}
        
        public Sequence(List<Node> _children) : base(_children) {}

        public override NodeState Evaluate()
        {
            bool isAnyChildRunning = false;

            foreach (Node node in m_children)
            {
                switch (node.Evaluate())
                {
                    case NodeState.RUNNING:
                        isAnyChildRunning = true;
                        continue;
                    case NodeState.SUCCESS:
                        continue;
                    case NodeState.FAILURE:
                        m_state = NodeState.FAILURE;
                        return m_state;
                    default:
                        m_state = NodeState.SUCCESS;
                        return m_state;
                }
            }

            m_state = isAnyChildRunning ? NodeState.RUNNING : NodeState.SUCCESS;
            return m_state;
        }
    }
}