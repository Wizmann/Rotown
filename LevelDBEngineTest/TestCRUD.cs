﻿using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LevelDBEngine;

namespace LevelDBEngineTest
{
    [TestClass]
    public class TestCRUD
    {
        public class MyModel: Model<MyModel>
        {
            public Guid Id { get; set; }

            public string Name { get; set; }

            public byte[] Image { get; set; }

            public int Score { get; set; }

            public override string Key
            {
                get
                {
                    return Id.ToString();
                }
            }
        }

        [TestMethod]
        public async Task TestRetrieve()
        {
            var e = new Engine<MyModel>($"{Guid.NewGuid()}.levdldb");

            var id1 = Guid.NewGuid();
            var m1 = new MyModel()
            {
                Id = id1,
                Name = id1.ToString(),
                Score = 1,
                Image = id1.ToByteArray(),
            };

            await e.Save(m1);
            var m2 = await e.Retrieve(id1.ToString());

            Assert.AreEqual(m1.Name, m2.Name);
            Assert.AreEqual(m1.Score, m2.Score);
            CollectionAssert.AreEqual(m1.Image, m2.Image);
        }
    }
}
