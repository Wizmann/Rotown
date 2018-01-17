using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LevelDB;
using Rotown;
using Rotown.Models;

namespace LevelDBEngine
{
    public class Engine<T> : IEngine<T>, IDisposable
        where T: Model<T>, new()
    {
        private DB db;

        public Func<T, byte[]> Serializer { get; set; } = BsonHelper.Serialize<T>;

        public Func<byte[], T> Deserializer { get; set; } = BsonHelper.Deserialize<T>;

        public Func<T, string> LevelDBKey { get; }

        public Engine(string path, Func<T, string> key, Options options=null)
        {
            if (options == null)
            {
                options = new Options { CreateIfMissing = true };
            }
            this.db = DB.Open(path, options);
            this.LevelDBKey = key;
        }

        public async Task Delete(T model)
        {
            var key = this.LevelDBKey(model);
            this.db.Delete(WriteOptions.Default, key);
            await Task.CompletedTask;
        }

        public void Dispose()
        {
            this.db.Dispose();
        }

        public async Task<PartialResult<T>> QuerySegmented(T low, T high, int take = 20, string ct = null)
        {
            var lowKey = this.LevelDBKey(low);
            var highKey = this.LevelDBKey(high);
            var iter = this.db.NewIterator(ReadOptions.Default);
            if (!string.IsNullOrEmpty(ct))
            {
                iter.Seek(ct);
            }
            else
            {
                iter.Seek(lowKey);
            }

            List<T> result = new List<T>();
            int count = 0;
            for (/* pass */; iter.Valid() && iter.Key() < highKey; iter.Next())
            {
                result.Add(this.Deserializer(iter.Value().ToArray()));
                count++;

                if (count == take)
                {
                    break;
                }
            }

            if (!iter.Valid() || iter.Key() >= highKey)
            {
                var partialResult = new PartialResult<T>() { Result = result, ContinuationToken = null };
                return await Task.FromResult(partialResult);
            }
            else
            {
                var partialResult = new PartialResult<T>() { Result = result, ContinuationToken = iter.Key().ToString() };
                return await Task.FromResult(partialResult);
            }
        }

        public async Task<T> Retrieve(T model)
        {
            var key = this.LevelDBKey(model);
            byte[] data = null;
            try
            {
                data = this.db.Get(ReadOptions.Default, key).ToArray();
            }
            catch (LevelDBException dbe) when (dbe.Message.StartsWith("NotFound"))
            {
                return null;
            }
            return await Task.FromResult<T>(this.Deserializer(data));
        }

        public async Task Save(T model)
        {
            var data = this.Serializer(model);
            var key = this.LevelDBKey(model);
            this.db.Put(WriteOptions.Default, key, data);
            await Task.CompletedTask;
        }
    }
}
