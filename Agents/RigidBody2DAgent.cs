using Godot;

namespace GSAI
{
    public class RigidBody2DAgent : SpecializedAgent
    {
        protected RigidBody2D body = null;
        protected Vector2 _last_position;
        protected WeakRef _body_ref;

        public RigidBody2D Body {
            set {
                body = value;
                _body_ref = WeakRef(value);

                position = Utils.ToVector3(_last_position);
                orientation = _last_orientation;
            }

            get {
                return body;
            }
        }

        public RigidBody2DAgent(RigidBody2D _body)
        {
            Body = _body;
            _body.GetTree().Connect("physics_frame", this, "_OnSceneTreeFrame");
        }

        public override void _ApplySteering(TargetAcceleration acceleration, float delta)
        {
            RigidBody2D _body = (RigidBody2D)_body_ref.GetRef();
            if(_body == null) return;

            _applied_steering = true;

            _body.ApplyCentralImpulse(Utils.ToVector2(acceleration.linear));
            _body.ApplyTorqueImpulse(acceleration.angular);
            if(calculate_velocities)
            {
                linear_velocity = Utils.ToVector3(_body.LinearVelocity);
                angular_velocity = _body.AngularVelocity;
            }
        }

        public void _OnSceneTreeFrame()
        {
            RigidBody2D _body = (RigidBody2D)_body_ref.GetRef();
            if(_body == null) return;

            var current_position = _body.GlobalPosition;
            var current_orientation = _body.Rotation;

            position = Utils.ToVector3(current_position);
            orientation = current_orientation;

            if(calculate_velocities)
            {
                if(_applied_steering)
                {
                    _applied_steering = false;
                }
                else
                {
                    linear_velocity = Utils.ToVector3(_body.LinearVelocity);
                    angular_velocity = _body.AngularVelocity;
                }
            }
        }
    }
}