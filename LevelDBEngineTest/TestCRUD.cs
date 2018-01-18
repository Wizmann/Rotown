using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LevelDBEngine;
using Rotown;

namespace LevelDBEngineTest
{
    [TestClass]
    public class TestCRUD
    {
        public static Engine<MyModel> MyModelEngine
            = new LevelDBEngine.Engine<MyModel>("MyModel.leveldb");

        public static Engine<MyJsonModel> MyJsonModelEngine
            = new LevelDBEngine.Engine<MyJsonModel>("MyJsonModel.leveldb")
            {
                Serializer = JsonHelper.Serialize<MyJsonModel>,
                Deserializer = JsonHelper.Deserialize<MyJsonModel>
            };

        public class MyModel: Model<MyModel>, ILevelDBModel
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

        public class MyJsonModel: Model<MyJsonModel>, ILevelDBModel
        {
            public Guid Id { get; set; }

            public string Name { get; set; }

            public byte[] Image { get; set; }

            public int Score { get; set; }

            protected override IEngine<MyJsonModel> Engine { get; } = MyJsonModelEngine;
            public string Key()
            {
                return this.Id.ToString();
            }
        }

        [TestMethod]
        public async Task TestRetrieve()
        {
            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();

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

            var m4 = new MyJsonModel()
            {
                Id = id2,
                Name = id2.ToString(),
                Score = 2,
                Image = id2.ToByteArray(),
            };

            await m4.Save();

            var m5 = await MyJsonModel.Objects.Retrieve(new MyJsonModel() { Id = id2 });
            Assert.AreEqual(m4.Id, m5.Id);
            Assert.AreEqual(m4.Name, m5.Name);
            Assert.AreEqual(m4.Score, m5.Score);
            CollectionAssert.AreEqual(m4.Image, m5.Image);
        }
    }
}
