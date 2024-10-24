using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BehaviorTree
{
    public abstract class Tree : MonoBehaviour
    {
        private Node m_root = null;
        
        // BlackBoard
        protected Dictionary<string, object> m_blackBoard = new Dictionary<string, object>();

        public GameObject gameObject;


        protected void Start()
        {
            m_root = SetupTree();
            m_root.SetTree(this);
        }

        private void Update()
        {
            if (m_root != null)
            {
                m_root.Evaluate();
            }
        }

        protected abstract Node SetupTree();

        public void SetData(string _key, object _value)
        {
            m_blackBoard[_key] = _value;
        }

        public object GetData(string _key)
        {
            object value = null;
            if (m_blackBoard.TryGetValue(_key, out value))
            {
                return value;
            }
            return null;
        }

        public bool ClearData(string _key)
        {
            if (m_blackBoard.ContainsKey(_key))
            {
                m_blackBoard.Remove(_key);
                return true;
            }
            return false;
        }
    }
}