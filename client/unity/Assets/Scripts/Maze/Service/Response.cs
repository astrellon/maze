using System.Collections.Generic;

namespace Maze.Service
{
    public class Response
    {
        public Dictionary<string, object> Result { get; protected set; }
        public object RawResult { get; protected set; }
        public bool IsError { get; protected set; }
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
            RawResult = result;
            Result = result as Dictionary<string, object>;
            if (Result != null)
            {
                if (Result.ContainsKey("error") && Result["error"] != null)
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
                return defaultValue;
            }
            object value = Result[key];
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
