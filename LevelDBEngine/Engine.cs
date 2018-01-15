using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LevelDB;
using Rotown;

namespace LevelDBEngine
{
    public abstract class Model<T>: IModel<string>
    {
        public abstract string Key { get; }
    }

    public class PartialResult<T>
    {
        public IEnumerable<Model<T>> Result { get; set; }

        public string ContinuationToken { get; set; }
    }

    public class Engine<T> : IEngine<string, T, PartialResult<T>, string>, IDisposable
        where T: Model<T>
    {
        private DB db;
        public Engine(string path, Options options=null)
        {
            if (options == null)
            {
                options = new Options { CreateIfMissing = true };
            }
            this.db = DB.Open(path, options);
        }

        public async Task Delete(string key)
        {
            this.db.Delete(WriteOptions.Default, key);
            await Task.CompletedTask;
        }

        public void Dispose()
        {
            this.db.Dispose();
        }

        public async Task<PartialResult<T>> Range(string lowKey, string highKey, int take = 20, string ct = null)
        {
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
                result.Add(BsonHelper.Deserialize<T>(iter.Value().ToArray()));
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

        public async Task<T> Retrieve(string key)
        {
            var data = this.db.Get(ReadOptions.Default, key);
            return await Task.FromResult<T>(BsonHelper.Deserialize<T>(data.ToArray()));
        }

        public async Task Save(T model)
        {
            var data = BsonHelper.Serialize(model);
            var key = model.Key;
            this.db.Put(WriteOptions.Default, key, data);
            await Task.CompletedTask;
        }
    }
}
