using UnityEngine;
using System;
using System.Collections.Generic;
using Maze.Service;

namespace Maze.Game
{
    public class World
    {
        public TileManager Tiles { get; protected set; }
        public Dictionary<string, Map> Maps { get; protected set; }
        public Map FirstMap = null;

        public World()
        {
            Tiles = new TileManager();
            Maps = new Dictionary<string, Map>();
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
                DeserialiseWorld(obj["world"] as Dictionary<string, object>);
            }
        }

        protected void DeserialiseWorld(Dictionary<string, object> worldObj)
        {
            if (worldObj == null)
            {
                Debug.Log("No world data");
                return;
            }

            if (worldObj.ContainsKey("maps"))
            {
                DeserialiseMaps(worldObj["maps"] as Dictionary<string, object>);
            }
        }

        protected void DeserialiseMaps(Dictionary<string, object> mapObj)
        {
            if (mapObj == null)
            {
                Debug.Log("- World map data invalid");
                return;
            }
            foreach (var pair in mapObj)
            {
                Map newMap = new Map(pair.Key, this);
                Debug.Log("Deserialising map: " + pair.Key);
                newMap.Deserialise(pair.Value as Dictionary<string, object>);
                Maps[pair.Key] = newMap;

                if (FirstMap == null)
                {
                    FirstMap = newMap;
                }
            }
        }
    }
}
