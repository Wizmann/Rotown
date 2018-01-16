using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rotown
{
    public interface IModel
    {
    }

    public interface IEngine<Model, PartialResult, ContinuationToken>
        where Model: IModel
    {
        Task<Model> Retrieve(Model model);

        Task Save(Model model);

        Task Delete(Model model);

        Task<PartialResult> Range(Model low, Model high, int take, ContinuationToken ct);
    }
}
