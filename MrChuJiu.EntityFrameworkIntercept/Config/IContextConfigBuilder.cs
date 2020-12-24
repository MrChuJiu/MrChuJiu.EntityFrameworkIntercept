using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Text;

namespace MrChuJiu.EntityFrameworkIntercept.Config
{
    public interface IContextConfigBuilder
    {
        IContextConfigBuilder WithEntityFilter(Func<EntityEntry, bool> entityFilter);

        IContextConfigBuilder WithPropertyFilter(Func<EntityEntry, PropertyEntry, bool> propertyFilter);
    }
}
