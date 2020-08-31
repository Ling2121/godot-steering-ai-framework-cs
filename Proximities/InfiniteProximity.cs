using Godot;
using System;
using System.Collections.Generic;

namespace GSAI
{
    public class InfiniteProximity : Proximity
    {
        public InfiniteProximity(SteeringAgent _agent,List<SteeringAgent> _agents = null)
        :base(_agent,_agents){}

        public override int _FindNeighbors(Func<SteeringAgent, bool> _callback)
        {
            var neighbor_count = 0;
            foreach(var current_agent in agents)
            {
                if(current_agent != agent)
                {
                    if(_callback(current_agent))
                    {
                        neighbor_count ++;
                    }
                }
            }
            return neighbor_count;
        }
    }
}