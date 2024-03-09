using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalOffice_BE.Data;

public abstract class BaseTable
{
    public DateTime InsertDate { get; set; }
    public DateTime? UpdateDate { get; set; }
}

public abstract class BaseTableKey<TKey> : BaseTable
{
    public TKey Id { get; set; } = default!;
}
