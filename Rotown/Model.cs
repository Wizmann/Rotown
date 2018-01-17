using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Rotown.Models;

namespace Rotown
{
    public abstract class Model<T> where T: Model<T>, new()
    {
        protected abstract IEngine<T> Engine { get; }

        public static ModelObject<T> Objects { get; } = new ModelObject<T>();

        public IEngine<T> GetEngine() => Engine;

        public ModelObject<T> GetObjects() => Objects;

        public async Task Save()
        {
            await Engine.Save((T) this);
        }

        public async Task Delete()
        {
            await Engine.Delete((T) this);
        }
    }
}
