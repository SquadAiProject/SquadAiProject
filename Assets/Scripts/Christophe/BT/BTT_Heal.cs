using BehaviorTree;
using UnityEngine;

public class BTT_Heal : Node
{
    private Player m_player;
    private bool isInit = false;
    private static int m_allyLayerMask = 1 << 6;

    private GameObject m_gameObject;
    
    public BTT_Heal(GameObject _gameObject)
    {
        m_gameObject = _gameObject;
    }

    public override NodeState Evaluate()
    {
        if (!isInit)
        {
            m_player = (Player)m_tree.GetData("Player");
            isInit = true;
        }

        Agent target = DetectInRange().GetComponent<Agent>();
        
        if (target.health >= 100)
        {
            m_gameObject.GetComponent<BT_Healer>().ShutDownLine();
            m_state = NodeState.SUCCESS;
            return m_state;
        }
        
        target.health += Time.deltaTime * 10.0f;
        if (target != null)
        {
            m_gameObject.GetComponent<BT_Healer>().Heal(target.gameObject);
            m_state = NodeState.SUCCESS;
            return m_state;
        }
        
        m_state = NodeState.RUNNING;
        return m_state;
    }

    GameObject DetectInRange()
    {
        Collider[] colliders = Physics.OverlapSphere(m_gameObject.transform.position, (float)m_tree.GetData("HealRange"), m_allyLayerMask);

        if (colliders.Length > 0)
        {
            GameObject target = null;

            for (int i = 0; i < colliders.Length; i++)
            {
                Agent agent = colliders[i].gameObject.GetComponent<Agent>();
                if (agent == null)
                {
                    continue;
                }

                if (target == null)
                {
                    target = colliders[i].gameObject;
                }
                else
                {
                    Agent targetAgent = target.GetComponent<Agent>();
                    if (targetAgent != null && agent.health < targetAgent.health)
                    {
                        target = colliders[i].gameObject;
                    }
                }
            }

            return target;
        }

        return null;
    }

}
