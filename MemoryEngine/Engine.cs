using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rotown;
using Rotown.Models;

namespace MemoryDBEngine
{
    public class Engine<T> : IEngine<T>
        where T : Model<T>, IMemoryDBModel, new()
    {
        private SortedDictionary<string, T> Dict { get; set; }
        private object Lock { get; } = new object();

        public Engine(SortedDictionary<string, T> dict = null)
        {
            this.Dict = dict ?? new SortedDictionary<string, T>();
        }

        public async Task<T> Retrieve(T model)
        {
            T result = null;
            lock(this.Lock)
            {
                var key = model.Key();
                this.Dict.TryGetValue(key, out result);
            }
            return await Task.FromResult(result);
        }

        public async Task Save(T model)
        {
            lock(this.Lock)
            {
                var key = model.Key();
                this.Dict[key] = model;
            }
            await Task.CompletedTask;
        }

        public async Task Delete(T model)
        {
            lock(this.Lock)
            {
                var key = model.Key();
                this.Dict.Remove(key);
            }
            await Task.CompletedTask;
        }

        public Task<PartialResult<T>> QuerySegmented(T low, T high, int take, string continuationToken)
        {
            throw new NotImplementedException();
        }
    }
}
