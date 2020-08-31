using Godot;

namespace GSAI
{
    public static class Utils
    {
        public static Vector3 Clampedv3(Vector3 vector, float limit)
        {
            var length_squared = vector.LengthSquared();
            var limit_squared = limit * limit;
            if (length_squared > limit_squared)
            {
                vector *= Mathf.Sqrt(limit_squared / length_squared);
            }
            return vector;
        }

        public static float Vector3ToAngle(Vector3 vector)
        {
            return Mathf.Atan2(vector.x,vector.y);   
        }

        public static float Vector2ToAngle(Vector2 vector)
        {
            return Mathf.Atan2(vector.x,vector.y);   
        }

        public static Vector2 AngleToVector2(float angle)
        {
            return new Vector2(Mathf.Sin(-angle),Mathf.Cos(angle));
        }

       public static Vector2 ToVector2(Vector3 vector)
       {
	        return new Vector2(vector.x, vector.y);
       }

       public static Vector3 ToVector3(Vector2 vector)
       {
           return new Vector3(vector.x, vector.y, 0);
       }
    }
}