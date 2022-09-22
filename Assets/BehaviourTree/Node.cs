using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Xml.Linq;

namespace BehaviourTree
{
    public enum NodeState
    {
        RUNNING,
        SUCCESS,
        FAILURE
    }
    public class Node
    {
        public Node parent;

        protected NodeState state;
        protected List<Node> children;

        //shared data
        private Dictionary<string, object> _dataContext = new Dictionary<string, object>();

        public Node()
        {
            parent = null;
        }

        public Node(List<Node> children)
        {
            foreach (Node child in children)
            {
                _Attach(child);
            }
        }

        private void _Attach(Node child)
        {
            child.parent = this;
            children.Add(child);
        }

        public virtual NodeState Evaluate() => NodeState.FAILURE;

        
        public void SetData(string key, object value)
        {
            _dataContext[key] = value;
        }


        // checks if data exists in this node then returns data
        // otherwise goes up the tree to find data
        public object GetData(string key)
        {
            object value = null;
            if (_dataContext.TryGetValue(key, out value))
                return value;

            Node node = parent;

            while(node != null)
            {
                value = node.GetData(key);
                if(value != null) return value;

                node = node.parent;
            }

            return null;
        }

        public bool ClearData(string key)
        {
            if(_dataContext.ContainsKey(key)) {
                _dataContext.Remove(key);
                return true;
            }

            Node node = parent;

            while(node != null)
            {
                bool cleared = node.ClearData(key);

                if (cleared) return true;

                node = node.parent;
            }

            return false;
        }
    }
}
