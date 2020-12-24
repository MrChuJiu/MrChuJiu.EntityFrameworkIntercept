using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Text;

namespace MrChuJiu.EntityFrameworkIntercept.Config
{
    internal sealed class ContextConfigBuilder : IContextConfigBuilder
    {
        private readonly List<Func<EntityEntry, bool>> _entityFilters = new List<Func<EntityEntry, bool>>();
        private readonly List<Func<EntityEntry, PropertyEntry, bool>> _propertyFilters = new List<Func<EntityEntry, PropertyEntry, bool>>();
        public ContextConfigOptions Build()
        {
            return new ContextConfigOptions()
            {
                EntityFilters = _entityFilters,
                PropertyFilters = _propertyFilters,
            };
        }

        public IContextConfigBuilder WithEntityFilter(Func<EntityEntry, bool> entityFilter)
        {
            if (null != entityFilter)
            {
                _entityFilters.Add(entityFilter);
            }
            return this;
        }

        public IContextConfigBuilder WithPropertyFilter(Func<EntityEntry, PropertyEntry, bool> propertyFilter)
        {
            if (null != propertyFilter)
            {
                _propertyFilters.Add(propertyFilter);
            }
            return this;
        }
    }
}
