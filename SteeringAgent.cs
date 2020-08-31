using Godot;

namespace GSAI
{
    public class SteeringAgent : AgentLocation
    {
        public float zero_linear_speed_threshold = 0;
        public float linear_speed_max = 0;
        public float linear_acceleration_max = 0;
        public float angular_speed_max = 0;
        public float angular_acceleration_max = 0;
        public Vector3 linear_velocity = Vector3.Zero;
        public float angular_velocity = 0;
        public float bounding_radius = 0;
        public bool is_tagged = false;

    }
}