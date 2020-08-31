namespace GSAI
{
    public class SpecializedAgent : SteeringAgent
    {
        public bool calculate_velocities = true;
        public bool apply_linear_drag = true;
        public bool apply_angular_drag = true;
        public float linear_drag_percentage = 0;
        public float angular_drag_percentage = 0;
        public float _last_orientation;
        public bool _applied_steering;

        public virtual void _ApplySteering(TargetAcceleration _acceleration,float delta)
        {

        }
    }
}