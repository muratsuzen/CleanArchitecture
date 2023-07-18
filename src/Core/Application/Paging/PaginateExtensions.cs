using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Paging
{
    public static class PaginateExtensions
    {
        public static async Task<IPaginate<T>> ToPaginateAsync<T>(
            this IQueryable<T> source,
            int index,
            int size,
            int from = 0,
            CancellationToken cancellationToken = default
            )
        {
            if(from>index)
                throw new ArgumentException($"index:{index} must greater than from:{from}");

            int count = await source.CountAsync(cancellationToken).ConfigureAwait(false);

            List<T> items = await source.Skip((index-from)*size).Take(size).ToListAsync(cancellationToken).ConfigureAwait(false);

            Paginate<T> list = new(){
             Index = index,
             Count = count,
             From = from,
             Items = items,
             Size = size,
             Pages = (int)Math.Ceiling(count/(double)size)
            };

            return list;
        }
    }
}
