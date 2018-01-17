using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rotown.Models;

namespace Rotown
{
    public interface IEngine<T>
        where T: Model<T>, new()
    {
        Task<T> Retrieve(T model);

        Task Save(T model);

        Task Delete(T model);

        Task<PartialResult<T>> QuerySegmented(T low, T high, int take, string continuationToken);
    }
}
