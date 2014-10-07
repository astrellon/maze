using UnityEngine;
using System;
using System.Collections.Generic;

namespace Maze.Service
{
    public class Response
    {
        public Dictionary<string, object> HashResult { get; protected set; }
        public object Result { get; protected set; }
        public Dictionary<string, object> Error { get; protected set; }
        public bool IsError { get; protected set; }
        public int ResponseId { get; protected set; }
        public string ErrorMessage
        {
            get
            {
                if (Error != null && Error.ContainsKey("message"))
                {
                    return Error["message"].ToString();
                }
                if (IsError)
                {
                    return "Unknown Error";
                }
                return "";
            }
        }
        
        public Response(Dictionary<string, object> result)
        {
            ResponseId = -1;
            if (result.ContainsKey("rid"))
            {
                ResponseId = Convert.ToInt32(result["rid"]);
            }
            else
            {
                throw new Exception("Cannot have a response without a rid!");
            }

            if (result.ContainsKey("error") && result["error"] != null)
            {
                Error = result["error"] as Dictionary<string, object>;
                IsError = true;
            }

            if (result.ContainsKey("result"))
            {
                Result = result["result"];
                HashResult = Result as Dictionary<string, object>;
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
            object value = HashResult[key];
            Debug.Log("Value: " + value.ToString() + " | " + value.GetType().ToString() + " | " + typeof(T).ToString());
            if (typeof(T) == value.GetType())
            {
                return (T)value;
            }
            return defaultValue;
        }

        public bool HasValue(string key)
        {
            if (IsError || Result == null || HashResult == null)
            {
                return false;
            }
            return HashResult.ContainsKey(key) && HashResult[key] != null;
        }

    }
}
