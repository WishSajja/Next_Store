using System.Xml.Linq;
using Newtonsoft.Json;

namespace Next_Store.Infrastructure
{
    public static class SessionExtensions 
    {
        public static void SetJson(this ISession session, string key, Object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));    
        }
        public static T GetJson<T>(this ISession session, string key)
        {
            var sessionData = session.GetString(key);
            return sessionData == null ? default(T) : JsonConvert.DeserializeObject<T>(sessionData);
        }

    }
}
