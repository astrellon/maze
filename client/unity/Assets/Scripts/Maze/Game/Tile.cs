using UnityEngine;
using System.Collections.Generic;

namespace Maze.Game 
{
    public class Tile
    {
        public string Name { get; protected set; }
        public bool Walkable { get; protected set; }

        public static Tile NoTile = new Tile("no_tile", false);

        public Tile()
        {

        }

        public Tile(string name, bool walkable)
        {
            Name = name;
            Walkable = walkable;
        }

        public bool Deserialise(Dictionary<string, object> obj)
        {
            if (obj == null)
            {
                return false;
            }
            
            if (obj.ContainsKey("name"))
            {
                Name = obj["name"].ToString();
            }
            if (obj.ContainsKey("walkable"))
            {
                Walkable = (bool)obj["walkable"];
            }
            return true;
        }
    }
}
