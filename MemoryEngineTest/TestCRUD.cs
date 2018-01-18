using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rotown;
using MemoryDBEngine;
using System.Threading.Tasks;

namespace MemoryEngineTest
{
    [TestClass]
    public class TestCRUD
    {
        public static IEngine<MyModel> MyModelEngine = new Engine<MyModel>();

        public class MyModel: Model<MyModel>, IMemoryDBModel
        {
            public Guid Id { get; set; }

            public string Name { get; set; }

            public byte[] Image { get; set; }

            public int Score { get; set; }

            protected override IEngine<MyModel> Engine { get; } = MyModelEngine;

            public string Key()
            {
                return this.Id.ToString();
            }
        }
        [TestMethod]
        public async Task TestRetrieve()
        {
            var id1 = Guid.NewGuid();

            var m1 = new MyModel()
            {
                Id = id1,
                Name = id1.ToString(),
                Score = 1,
                Image = id1.ToByteArray(),
            };

            await m1.Save();
            var m2 = await MyModel.Objects.Retrieve(new MyModel() { Id = id1 });

            Assert.AreEqual(m1.Name, m2.Name);
            Assert.AreEqual(m1.Score, m2.Score);
            CollectionAssert.AreEqual(m1.Image, m2.Image);

            await m2.Delete();
            var m3 = await MyModel.Objects.Retrieve(new MyModel() { Id = id1 });

            Assert.IsNull(m3);
        }
    }
}
