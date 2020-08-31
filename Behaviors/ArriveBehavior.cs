using System;
using Godot;

namespace GSAI
{
    public class ArriveBehavior : SteeringBehavior
    {
        public AgentLocation target;
        public float arrival_tolerance;
        public float deceleration_radius;
        public float time_to_reach = 0.1f;

        public ArriveBehavior(SteeringAgent agent,AgentLocation _target):base(agent)
        {
            target = _target;
        }

        public virtual void _Arrive(TargetAcceleration acceleration,Vector3 target_position)
        {
            var to_target = target_position - agent.position;
            var distance = to_target.Length();

            if(distance <= arrival_tolerance)
            {
                acceleration.SetZero();
            }
            else
            {
                var desired_speed = agent.linear_speed_max;

                if(distance <= deceleration_radius)
                {
                    desired_speed *= distance / deceleration_radius;
                }

                var desired_velocity = to_target * desired_speed / distance;

                desired_velocity = ((desired_velocity - agent.linear_velocity) * 1.0f / time_to_reach);

                acceleration.linear = Utils.Clampedv3(desired_velocity, agent.linear_acceleration_max);
            }
        }

        public override void _CalculateSteering(TargetAcceleration acceleration)
        {
            _Arrive(acceleration,target.position);
        }
    }
}