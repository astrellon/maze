using UnityEngine;
using System;
using System.Collections.Generic;

namespace Maze.Service
{
    public class LocalCommand
    {
        public Dictionary<string, object> HashData { get; protected set; }
        public object Data { get; protected set; }
        public int CommandId { get; protected set; }
        public bool IsError { get; protected set; }
        
        public LocalCommand(Dictionary<string, object> data)
        {
            CommandId = -1;
            if (data.ContainsKey("cid"))
            {
                CommandId = Convert.ToInt32(result["cid"]);
            }

            if (data.ContainsKey("data"))
            {
                Data = data["data"];
                HashData = Data as Dictionary<string, object>;
            }
            else
            {
                IsError = true;
            }
        }

        public T GrabValue<T>(string key, T defaultValue)
        {
            if (!HasValue(key))
            {
                Debug.Log("Did not find key: " + key);
                return defaultValue;
            }
            object value = HashData[key];
            if (typeof(T) == value.GetType())
            {
                return (T)value;
            }
            return defaultValue;
        }

        public bool HasValue(string key)
        {
            if (Data == null || HashData == null)
            {
                return false;
            }
            return HashData.ContainsKey(key) && HashData[key] != null;
        }
    }
}
