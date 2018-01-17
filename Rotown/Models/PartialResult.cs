using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rotown.Models
{
    public class PartialResult<T>
        where T: Model<T>, new()
    {
        public IEnumerable<T> Result { get; set; }

        public string ContinuationToken { get; set; }
    }
}
