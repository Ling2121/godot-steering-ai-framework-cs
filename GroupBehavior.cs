using System;
namespace GSAI
{
    public class GroupBehavior : SteeringBehavior
    {
        public Func<SteeringAgent,bool> _callback = (SteeringAgent _neighbor)=>{ return false;};

        public Proximity proximity;
        public GroupBehavior(SteeringAgent agent,Proximity _proximity) : base(agent)
        {
            proximity = _proximity;
        }

        public virtual bool _ReportNeighbor(SteeringAgent _neighbor)
        {
            return false;
        }
    }
}