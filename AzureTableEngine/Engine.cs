using Microsoft.WindowsAzure.Storage.Table;
using Rotown;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rotown.Models;

namespace AzureTableEngine
{
    public class Engine<T> : IEngine<T>
        where T : Model<T>, new()
    {
        public Task Delete(T model)
        {
            throw new NotImplementedException();
        }

        public Task<PartialResult<T>> QuerySegmented(T low, T high, int take, string continuationToken)
        {
            throw new NotImplementedException();
        }

        public Task<T> Retrieve(T model)
        {
            throw new NotImplementedException();
        }

        public Task Save(T model)
        {
            throw new NotImplementedException();
        }
    }
}
