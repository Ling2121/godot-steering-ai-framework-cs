using System;
using System.Collections.Generic;
using Godot;

namespace GSAI
{
    public class Path
    {
        public class Segment
        {
            public static Segment Null_Segment = new Segment();

            public Vector3 begin = Vector3.Zero;
            public Vector3 end = Vector3.Zero;
            
            public float length = 0;
            public float cumulative_length = 0;

            public Segment(){}
            public Segment(Vector3 _begin,Vector3 _end)
            {
                begin = _begin;
                end = _end;
                length = _begin.DistanceTo(_end);
            }
        }

        public bool is_open;
        public float length;
        public List<Segment> _segments = new List<Segment>();
        public Vector3 _nearest_point_on_segment;
        public Vector3 _nearest_point_on_path;

        public static List<Vector3> __default_path = new List<Vector3>{Vector3.Zero,Vector3.Zero};

        public Path(List<Vector3> waypoints,bool _is_open = true)
        {
            is_open = _is_open;
            CreatePath(waypoints);
            _nearest_point_on_segment = waypoints[0];
            _nearest_point_on_path = waypoints[0];
        }

        public Path()
        {
            is_open = false;
            CreatePath(__default_path);
            _nearest_point_on_segment = Vector3.Zero;
            _nearest_point_on_path = Vector3.Zero;
        }

        public void CreatePath(List<Vector3> waypoints)
        {
            if(waypoints.Count < 2) return;

            _segments = new List<Segment>();
            length = 0;
            Vector3 current = waypoints[0];
            Vector3 previous;

            for(int i = 1;i < waypoints.Count;i++)
            {
                previous = current;
                if(i < waypoints.Count)
                {
                    current = waypoints[i];
                }
                else if(is_open)
                {
                    break;
                }
                else
                {
                    current = waypoints[0];
                }

                var segment = new Segment(previous,current);
                length += segment.length;
                segment.cumulative_length = length;
                _segments.Add(segment);
            }
        }

        public float CalculateDistance(Vector3 agent_current_position)
        {
            if(_segments.Count == 0) return 0;
            float smallest_distance_squared = Mathf.Inf;
            Segment nearest_segment = Segment.Null_Segment;
            foreach(Segment segment in _segments)
            {
                var distance_squared = _CalculatePointSegmentDistanceSquared(segment.begin,segment.end,agent_current_position);
                if(distance_squared < smallest_distance_squared)
                {
                    _nearest_point_on_path = _nearest_point_on_segment;
			        smallest_distance_squared = distance_squared;
			        nearest_segment = segment;
                }  
            }

            return (
		        nearest_segment.cumulative_length
		        -_nearest_point_on_path.DistanceTo(nearest_segment.end)
	        );
        }

        public Vector3 CalculateTargetPosition(float target_distance)
        {
            if(is_open)
            {
		        target_distance = Mathf.Clamp(target_distance, 0, length);
            }
	        else
            {
                if(target_distance < 0)
                {
                    target_distance = length + target_distance % length;
                }
                else if(target_distance > length)
                {
                    target_distance = target_distance % length;
                }
            }

            Segment desired_segment = null;
            foreach(Segment segment in _segments)
            {
                if(segment.cumulative_length >= target_distance)
                {
                    desired_segment = segment;
                    break;
                }
            }
                

            if(desired_segment == null)
            {
                desired_segment = _segments[Math.Max(0,_segments.Count - 1)];
            }

            var distance = desired_segment.cumulative_length - target_distance;

            return (
                ((desired_segment.begin - desired_segment.end) * (distance / desired_segment.length))
                + desired_segment.end
            );
        }

        public Vector3 GetStartPoint()
        {
            return _segments[0].begin;
        }

        public Vector3 GetEndPoint()
        {
            return _segments[Math.Max(0,_segments.Count - 1)].end;
        }

        public float _CalculatePointSegmentDistanceSquared(Vector3 start,Vector3 end,Vector3 position)
        {
            _nearest_point_on_segment = start;
            var start_end = end - start;
            var start_end_length_squared = start_end.LengthSquared();
            if(start_end_length_squared != 0)
            {
                var t = (position - start).Dot(start_end) / start_end_length_squared;
                _nearest_point_on_segment += start_end * Mathf.Clamp(t, 0, 1);
            }
            return _nearest_point_on_segment.DistanceSquaredTo(position);
        }
    }
}