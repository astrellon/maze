using UnityEngine;
using System;
using System.Collections.Generic;

namespace Maze.Service
{
    public class Response
    {
        public Dictionary<string, object> Result { get; protected set; }
        public object RawResult { get; protected set; }
        public bool IsError { get; protected set; }
        public int ResponseId { get; protected set; }
        public string ErrorMessage
        {
            get
            {
                if (HasValue("error"))
                {
                    return Result["error"].ToString();
                }
                return "";
            }
        }
        
        public Response(object result)
        {
            ResponseId = -1;
            RawResult = result;
            Dictionary<string, object> baseResult = result as Dictionary<string, object>;
            if (baseResult != null)
            {
                if (baseResult.ContainsKey("rid"))
                {
                    ResponseId = Convert.ToInt32(baseResult["rid"]);
                }
                if (baseResult.ContainsKey("error") && baseResult["error"] != null)
                {
                    IsError = true;
                }

                if (baseResult.ContainsKey("result"))
                {
                    Result = baseResult["result"] as Dictionary<string, object>;
                    IsError = Result == null;
                }
                else
                {
                    IsError = true;
                }
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
            object value = Result[key];
            Debug.Log("Value: " + value.ToString() + " | " + value.GetType().ToString() + " | " + typeof(T).ToString());
            if (typeof(T) == value.GetType())
            {
                return (T)value;
            }
            return defaultValue;
        }

        public bool HasValue(string key)
        {
            if (IsError || Result == null)
            {
                return false;
            }
            return Result.ContainsKey(key) && Result[key] != null;
        }

    }
}
