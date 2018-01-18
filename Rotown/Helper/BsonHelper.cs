using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rotown
{
    public static class BsonHelper
    {
        public static byte[] Serialize<T>(T model)
        {
            using (var ms = new MemoryStream())
            {
                using (var writer = new BsonDataWriter(ms))
                {
                    var serializer = new JsonSerializer();
                    serializer.Serialize(writer, model);
                }
                return ms.ToArray();
            }
        }

        public static T Deserialize<T>(byte[] bytes)
        {
            using (var ms = new MemoryStream(bytes))
            {
                using (var reader = new BsonDataReader(ms))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    var model = serializer.Deserialize<T>(reader);
                    return model;
                }
            }
        }
    }
}
