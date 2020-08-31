using Godot;

namespace GSAI
{
    public class FollowPathBehavior : ArriveBehavior
    {
        public Path path;
        public float path_offset = 0;
        public bool is_arrive_enabled = true;
        public float prediction_time = 0;

        public FollowPathBehavior(SteeringAgent agent,Path _path,float _path_offset = 0,float _prediction_time = 0)
        :base(agent,null)
        {
            path = _path;
            path_offset = _path_offset;
            prediction_time = _prediction_time;
        }

        public override void _CalculateSteering(TargetAcceleration acceleration)
        {
            var location = prediction_time == 0 ? agent.position : agent.position + (agent.linear_velocity * prediction_time);

            var distance = path.CalculateDistance(location);
            var target_distance = distance + path_offset;

            if(prediction_time > 0 && path.is_open)
            {
                if(target_distance < path.CalculateDistance(agent.position))
                {
                    target_distance = path.length;
                }
            }

            var target_position = path.CalculateTargetPosition(target_distance);

            if(is_arrive_enabled && path.is_open)
            {
                if(path_offset >= 0)
                {
                    if(target_distance > path.length - deceleration_radius)
                    {
                        _Arrive(acceleration, target_position);
                        return;
                    }
                }
                else
                {
                    if(target_distance < deceleration_radius)
                    {
                        _Arrive(acceleration, target_position);
                        return;
                    }
                }
            }

            acceleration.linear = (target_position - agent.position).Normalized();
            acceleration.linear *= agent.linear_acceleration_max;
            acceleration.angular = 0;
        }
    }
}