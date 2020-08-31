using System;
using Godot;
using System.Collections.Generic;

namespace GSAI
{
    public class RadiusProximity : Proximity
    {
        public float radius = 0;
        public long _last_frame = 0;
        public SceneTree _scene_tree;

        public RadiusProximity(SteeringAgent _agent,List<SteeringAgent> _agents = null,float _radius = 20)
        :base(_agent,_agents)
        {
            radius = _radius;
            _scene_tree = (SceneTree)Engine.GetMainLoop();
        }

        public override int _FindNeighbors(Func<SteeringAgent,bool> _callback)
        {
            var agent_count = agents.Count;
	        var neighbor_count = 0;
            long current_frame = _scene_tree != null ? _scene_tree.GetFrame() : -_last_frame;
            if(current_frame != _last_frame)
            {
                _last_frame = current_frame;
                var owner_position = agent.position;

                foreach(var current_agent in agents)
                {
                    if(current_agent != agent)
                    {
                        var distance_squared = owner_position.DistanceSquaredTo(current_agent.position);
                        var range_to = radius + current_agent.bounding_radius;
                        if(distance_squared > range_to * range_to)
                        {
                            if(_callback(current_agent))
                            {
                                current_agent.is_tagged = true;
                                neighbor_count ++;
                                continue;
                            }
                        }
                    }
                    current_agent.is_tagged = false;
                }
            }
            else
            {
                foreach(var current_agent in agents)
                {
                    if(current_agent != agent && current_agent.is_tagged)
                    {
				        if(_callback(current_agent))
                        {
					        neighbor_count ++;
                        }
                    }
                }
            }
            return neighbor_count;
        }
    }
}