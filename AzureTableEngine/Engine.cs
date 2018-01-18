using Microsoft.WindowsAzure.Storage.Table;
using Rotown;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rotown.Models;
using Microsoft.WindowsAzure.Storage;

namespace AzureTableEngine
{
    public class Engine<T> : IEngine<T>
        where T : Model<T>, IAzureTableModel, new()
    {
        private CloudStorageAccount StorageAccount { get; set; }

        private string TableName { get; set; }

        private CloudTableClient Client { get; set; }

        private CloudTable Table { get; set; }

        public Engine(CloudStorageAccount account, string tableName="")
        {
            this.StorageAccount = account;

            if (string.IsNullOrEmpty(tableName))
            {
                tableName = typeof(T).Name;
            }
            this.TableName = tableName;

            this.Client = this.StorageAccount.CreateCloudTableClient();
            this.Table = this.Client.GetTableReference(this.TableName);

            this.Table.CreateIfNotExists();
        }

        public async Task Delete(T model)
        {
            var key = model.Key();
            var entity = new TableEntity()
            {
                PartitionKey = key.PartitionKey,
                RowKey = key.RowKey,
                ETag = "*",
            };
            var op = TableOperation.Delete(entity);

            await this.Table.ExecuteAsync(op);
        }

        public Task<PartialResult<T>> QuerySegmented(T low, T high, int take, string continuationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<T> Retrieve(T model)
        {
            var key = model.Key();
            var op = TableOperation.Retrieve<EntityAdapter<T>>(key.PartitionKey, key.RowKey);
            var result = await this.Table.ExecuteAsync(op);
            return (result.Result as EntityAdapter<T>)?.InnerObject;
        }

        public async Task Save(T model)
        {
            var key = model.Key();
            var entity = new EntityAdapter<T>(model)
            {
                PartitionKey = key.PartitionKey,
                RowKey = key.RowKey,
            };

            var op = TableOperation.InsertOrReplace(entity);
            await this.Table.ExecuteAsync(op);
        }
    }
}
