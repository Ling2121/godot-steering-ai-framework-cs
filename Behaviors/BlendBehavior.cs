using System;
using System.Collections.Generic;
using Godot;

namespace GSAI
{
    public class BlendBehavior : SteeringBehavior
    {
        public class Behavior
        {
            public SteeringBehavior behavior;
            public float weight;

        }

        public List<Behavior> _behaviors = new List<Behavior>();
        public TargetAcceleration _accel = new TargetAcceleration();

        public BlendBehavior(SteeringAgent agent):base(agent){}

        public void Add(SteeringBehavior behavior,float weight)
        {
            behavior.agent = agent;
            _behaviors.Add(new Behavior{behavior = behavior,weight = weight});
        }

        public Behavior GetBehaviorAt(int index)
        {
            if(_behaviors.Count > index) return _behaviors[index];
            return null;
        }

        public override void _CalculateSteering(TargetAcceleration blended_accel)
        {
            foreach(Behavior behavior in _behaviors)
            {
                behavior.behavior.CalculateSteering(_accel);
                blended_accel.AddScaledAccel(_accel,behavior.weight);
            }

            blended_accel.linear = Utils.Clampedv3(blended_accel.linear,agent.linear_acceleration_max);
            blended_accel.angular = Mathf.Clamp(
                blended_accel.angular, -agent.angular_acceleration_max, agent.angular_acceleration_max
            );
        }
    }
}