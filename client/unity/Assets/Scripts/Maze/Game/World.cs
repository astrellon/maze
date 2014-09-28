using UnityEngine;
using System;
using System.Collections.Generic;
using Maze.Service;

namespace Maze.Game
{
    public class World
    {
        public TileManager Tiles { get; protected set; }

        public World()
        {
            Tiles = new TileManager();
        }

        public void Deserialise(Dictionary<string, object> obj)
        {
            if (obj == null)
            {
                Debug.Log("Error deserialising world as obj is null");
                return;
            }

            if (obj.ContainsKey("tiles"))
            {
                Tiles.Deserialise(obj["tiles"] as Dictionary<string, object>);
            }
            else
            {
                Debug.Log("- World does not contain tile data");
            }

            if (obj.ContainsKey("world"))
            {
                Dictionary<string, object> worldObj = obj["world"] as Dictionary<string, object>;
                if (worldObj == null)
                {
                    Debug.Log("No world data");
                    return;
                }
                
                if (worldObj.ContainsKey("maps"))
                {
                    Dictionary<string, object> maps = worldObj["maps"] as Dictionary<string, object>;
                    if (maps == null)
                    {
                        Debug.Log("- World map data invalid");
                    }
                    else
                    {
                        foreach (var pair in maps)
                        {
                            Map newMap = new Map(pair.Key, this);
                            Debug.Log("Deserialising map: " + pair.Key);
                            newMap.Deserialise(pair.Value as Dictionary<string, object>);
                        }
                    }
                }
            }
        }
    }
}
