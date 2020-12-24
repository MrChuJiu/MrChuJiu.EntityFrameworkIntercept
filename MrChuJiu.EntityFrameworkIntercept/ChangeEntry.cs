using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MrChuJiu.EntityFrameworkIntercept
{
    public class ChangeEntry
    {
        public string TableName { get; set; }
        public EntityState EntityState { get; set; }
        public Dictionary<string, object> NewValue { get; set; }
        public Dictionary<string, object> usedValue { get; set; }
    }
}
