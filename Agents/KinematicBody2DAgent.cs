using Godot;
using System;

namespace GSAI
{
    public enum MovementType
    {
        SLIDE,//move_and_slide
        COLLIDE,//move_and_collide
        POSITION,//global_position
    }
 
    public class KinematicBody2DAgent : SpecializedAgent
    {
        protected KinematicBody2D body = null;
        protected MovementType movement_type = MovementType.SLIDE;
        protected Vector2 _last_position;
        protected WeakRef _body_ref;
        public KinematicBody2D Body {
            set {
                body = value;
                _body_ref = WeakRef(body);

                _last_position = value.GlobalPosition;
                _last_orientation = value.Rotation;

                position = Utils.ToVector3(_last_position);
                orientation = _last_orientation;
            }

            get {
                return body;
            }
        }

        public KinematicBody2DAgent(KinematicBody2D _body,MovementType _movement_type = MovementType.SLIDE)
        {
            Body = _body;
            movement_type = _movement_type;

            _body.GetTree().Connect("physics_frame", this, "_OnSceneTreePhysicsFrame");
        }

        public override void _ApplySteering(TargetAcceleration acceleration, float delta)
        {
            _applied_steering = true;
            switch (movement_type)
            {
                case MovementType.COLLIDE :
                    _ApplyCollideSteering(acceleration.linear,delta); break;
                case MovementType.SLIDE :
                    _ApplySlidingSteering(acceleration.linear,delta); break;
                default:
                    _ApplyPositionSteering(acceleration.linear,delta); break;
            }

            _ApplyOrientationSteering(acceleration.angular,delta);
        }

        public virtual void _ApplySlidingSteering(Vector3 accel,float delta)
        {
            KinematicBody2D _body = (KinematicBody2D)_body_ref.GetRef();
            if(_body == null) return;

            var velocity = Utils.ToVector2(linear_velocity + accel * delta).Clamped(linear_speed_max);

            if(apply_linear_drag)
            {
                velocity = velocity.LinearInterpolate(Vector2.Zero, linear_drag_percentage);
            }
            velocity = _body.MoveAndSlide(velocity);
            if(calculate_velocities)
            {
                linear_velocity = Utils.ToVector3(velocity);
            }
        }

        public virtual void _ApplyCollideSteering(Vector3 accel,float delta)
        {
            KinematicBody2D _body = (KinematicBody2D)_body_ref.GetRef();
            if(_body == null) return;

            var velocity = Utils.Clampedv3(linear_velocity + accel * delta, linear_speed_max);

            if(apply_linear_drag)
            {
                velocity = velocity.LinearInterpolate(Vector3.Zero, linear_drag_percentage);
            }

            _body.MoveAndCollide(Utils.ToVector2(velocity) * delta);
            if(calculate_velocities)
            {
                linear_velocity = velocity;
            }
        }

        public virtual void _ApplyPositionSteering(Vector3 accel,float delta)
        {
            KinematicBody2D _body = (KinematicBody2D)_body_ref.GetRef();
            if(_body == null) return;

            var velocity = Utils.Clampedv3(linear_velocity + accel * delta, linear_speed_max);

            if(apply_linear_drag)
            {
                velocity = velocity.LinearInterpolate(Vector3.Zero, linear_drag_percentage);
            }
            _body.GlobalPosition += Utils.ToVector2(velocity) * delta;
            if(calculate_velocities)
            {
                linear_velocity = velocity;
            }
        }

        public virtual void _ApplyOrientationSteering(float angular_acceleration,float delta)
        {
            KinematicBody2D _body = (KinematicBody2D)_body_ref.GetRef();
            if(_body == null) return;

            var velocity = Mathf.Clamp(
                angular_velocity + angular_acceleration * delta,
                -angular_acceleration_max,
                angular_acceleration_max
	        );

            if(apply_angular_drag)
            {
		        velocity = Mathf.Lerp(velocity, 0, angular_drag_percentage);
            }
	        _body.Rotation += velocity * delta;
	        if(calculate_velocities)
            {
		        angular_velocity = velocity;
            }
        }

        public void _OnSceneTreePhysicsFrame()
        {
            KinematicBody2D _body = (KinematicBody2D)_body_ref.GetRef();
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
                    linear_velocity = Utils.Clampedv3(
                        Utils.ToVector3(current_position - _last_position), linear_speed_max
                    );
                    if(apply_linear_drag)
                    {
                        linear_velocity = linear_velocity.LinearInterpolate(
                            Vector3.Zero, linear_drag_percentage
                        );
                    }

                    angular_velocity = Mathf.Clamp(
                        _last_orientation - current_orientation, -angular_speed_max, angular_speed_max
                    );

                    if(apply_angular_drag)
                    {
                        angular_velocity = Mathf.Lerp(angular_velocity, 0, angular_drag_percentage);
                    }

                    _last_position = current_position;
                    _last_orientation = current_orientation;
                }
            }
        }

    }
}