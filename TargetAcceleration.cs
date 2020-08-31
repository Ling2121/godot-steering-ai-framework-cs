using System;
using Godot;

namespace GSAI
{
    public class TargetAcceleration
    {
        public Vector3 linear = new Vector3(0,0,0);
        public float angular = 0;

        public void SetZero()
        {
            linear.x = 0;
            linear.y = 0;
            linear.z = 0;
            angular = 0;
        }

        public void AddScaledAccel(TargetAcceleration accel,float scalar)
        {
            linear += accel.linear * scalar;
	        angular += accel.angular * scalar;
        }

        public float GetMagnitudeSquared()
        {
            return linear.LengthSquared() + angular * angular;
        }

        public float GetMagnitude()
        {
            return Mathf.Sqrt(GetMagnitudeSquared());
        }
    }
}