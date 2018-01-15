using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rotown
{
    public interface IModel<IKey>
    {
        IKey Key { get; }
    }

    public interface IEngine<Key, Model, PartialResult, ContinuationToken>
        where Model: IModel<Key>
    {
        Task<Model> Retrieve(Key key);

        Task Save(Model model);

        Task Delete(Key key);

        Task<PartialResult> Range(Key lowKey, Key highKey, int take, ContinuationToken ct);
    }
}
