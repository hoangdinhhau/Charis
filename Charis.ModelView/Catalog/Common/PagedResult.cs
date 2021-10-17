using System.Collections.Generic;

namespace Charis.ModelView.Common
{
    public class PagedResult<T> : PagedResultBase
    {
        public List<T> Items { set; get; }
    }
}