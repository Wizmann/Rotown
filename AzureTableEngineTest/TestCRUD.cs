using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rotown;
using AzureTableEngine;
using Microsoft.WindowsAzure.Storage;
using System.Threading.Tasks;

namespace AzureTableEngineTest
{
    [TestClass]
    public class TestCRUD
    {
        public class MyModel: Model<MyModel>, IAzureTableModel
        {
            public string App { get; set; }

            public string Id { get; set; }

            public string Name { get; set; }

            public int Score { get; set; }

            public AzureTableKey Key()
            {
                return new AzureTableKey
                {
                    PartitionKey = App,
                    RowKey = Id,
                };
            }

            protected override IEngine<MyModel> Engine { get; } = MyModelEngine;
        }

        public static IEngine<MyModel> MyModelEngine = new Engine<MyModel>(CloudStorageAccount.DevelopmentStorageAccount);

        [TestMethod]
        public async Task TestRetrieve()
        {
            var id1 = Guid.NewGuid().ToString();
            var m1 = new MyModel()
            {
                App = "fun",
                Id = id1,
                Name = "name",
                Score = 100,
            };

            await m1.Save();

            var m2 = await MyModel.Objects.Retrieve(new MyModel() { App = "fun", Id = id1 });

            Assert.AreEqual(m1.App, m2.App);
            Assert.AreEqual(m1.Id, m2.Id);
            Assert.AreEqual(m1.Name, m2.Name);
            Assert.AreEqual(m1.Score, m2.Score);

            await m2.Delete();

            var m3 = await MyModel.Objects.Retrieve(new MyModel() { App = "fun", Id = id1 });
            Assert.IsNull(m3);
        }
    }
}
