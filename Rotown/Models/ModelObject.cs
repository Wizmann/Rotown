using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rotown.Models
{
    public class ModelObject<T> where T: Model<T>, new()
    {
        static private T obj = new T();
        static private IEngine<T> Engine { get; } = obj.GetEngine();

        public async Task<T> Retrieve(T model)
        {
            return await Engine.Retrieve(model);
        }

        public async Task<PartialResult<T>> QuerySegmented(T low, T high, int take, string continuationToken)
        {
            return await Engine.QuerySegmented(low, high, take, continuationToken);
        }
    }
}
