using System;
using System.Collections.Generic;

namespace GSAI
{
    public class Proximity
    {
        public SteeringAgent agent;
        public List<SteeringAgent> agents = new List<SteeringAgent>();

        public Proximity(SteeringAgent _agent,List<SteeringAgent> _agents = null)
        {
            agent = _agent;
            if(_agent != null)
            {
                agents = _agents;
            }
        }

        public virtual int _FindNeighbors(Func<SteeringAgent,bool> _callback)
        {
            return 0;
        }
    }
}