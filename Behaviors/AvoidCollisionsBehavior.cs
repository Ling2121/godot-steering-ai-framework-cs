using Godot;
using System;

namespace GSAI
{
    public class AvoidCollisionsBehavior : GroupBehavior
    {
        public SteeringAgent _first_neighbor;
        public float _shortest_time;
        public float _first_minimum_separation;
        public float _first_distance;
        public Vector3 _first_relative_position;
        public Vector3 _first_relative_velocity;

        public AvoidCollisionsBehavior(SteeringAgent agent,Proximity _proximity) : base(agent,_proximity){}

        public override void _CalculateSteering(TargetAcceleration acceleration)
        {
            _shortest_time = Mathf.Inf;
            _first_neighbor = null;
            _first_minimum_separation = 0;
            _first_distance = 0;

            var neighbor_count = proximity._FindNeighbors(_ReportNeighbor);

            if(neighbor_count == 0 || _first_neighbor == null)
            {
                acceleration.SetZero();
            }
            else
            {
                if (
                    _first_minimum_separation <= 0
                    || _first_distance < agent.bounding_radius + _first_neighbor.bounding_radius)
                {
                    acceleration.linear = _first_neighbor.position - agent.position;
                }
                else
                {
                    acceleration.linear = (
                        _first_relative_position
                        + (_first_relative_velocity * _shortest_time)
                    );
                }
            }

            acceleration.linear = (acceleration.linear.Normalized() * -agent.linear_acceleration_max);
            acceleration.angular = 0;
        }

        public override bool _ReportNeighbor(SteeringAgent neighbor)
        {
            var relative_position = neighbor.position - agent.position;
	        var relative_velocity = neighbor.linear_velocity - agent.linear_velocity;
	        var relative_speed_squared = relative_velocity.LengthSquared();

            if(relative_speed_squared == 0) return false;

            var time_to_collision = -relative_position.Dot(relative_velocity) / relative_speed_squared;

            if(time_to_collision <= 0 || time_to_collision >= _shortest_time) return false;

            var distance = relative_position.Length();
            var minimum_separation = distance - Mathf.Sqrt(relative_speed_squared) * time_to_collision;
            
            if(minimum_separation > agent.bounding_radius + neighbor.bounding_radius) return false;

            _shortest_time = time_to_collision;
            _first_neighbor = neighbor;
            _first_minimum_separation = minimum_separation;
            _first_distance = distance;
            _first_relative_position = relative_position;
            _first_relative_velocity = relative_velocity;
            return true;
        }
    }
}