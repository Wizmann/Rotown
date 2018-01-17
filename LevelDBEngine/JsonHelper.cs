using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LevelDBEngine
{
    public static class JsonHelper
    {
        public static byte[] Serialize<T>(T model)
        {
            var data = JsonConvert.SerializeObject(model);
            return Encoding.UTF8.GetBytes(data);
        }

        public static T Deserialize<T>(byte[] bytes)
        {
            var data = Encoding.UTF8.GetString(bytes);
            return JsonConvert.DeserializeObject<T>(data);
        }
    }
}
