using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Rotown;

namespace AzureTableEngine
{
    public class EntityAdapter<T>: TableEntity
        where T: Model<T>, IAzureTableModel, new()
    {
        public T InnerObject { get; set; }

        public EntityAdapter()
        {
            this.InnerObject = new T();
        }

        public EntityAdapter(T model)
        {
            this.InnerObject = model;
        }

        public override void ReadEntity(IDictionary<string, EntityProperty> properties, OperationContext operationContext)
        {
            TableEntity.ReadUserObject(this.InnerObject, properties, operationContext);
        }

        public override IDictionary<string, EntityProperty> WriteEntity(OperationContext operationContext)
        {
            return TableEntity.WriteUserObject(this.InnerObject, operationContext);
        }
    }
}
