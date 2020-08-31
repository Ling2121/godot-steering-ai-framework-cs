namespace GSAI
{
    public class SteeringBehavior : Godot.Object
    {
        public bool is_enabled = true;
        public SteeringAgent agent;

        public SteeringBehavior(SteeringAgent agent)
        {
            this.agent = agent;
        }

        public void CalculateSteering(TargetAcceleration acceleration)
        {
            if(is_enabled)
            {
                _CalculateSteering(acceleration);
            }
            else
            {
                acceleration.SetZero();
            }
        }

        public virtual void _CalculateSteering(TargetAcceleration acceleration)
        {
            acceleration.SetZero();
        }
    }
}