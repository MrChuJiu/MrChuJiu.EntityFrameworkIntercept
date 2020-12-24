using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Text;

namespace MrChuJiu.EntityFrameworkIntercept.Config
{
    internal sealed class ContextConfigOptions
    {
        private IReadOnlyCollection<Func<EntityEntry, bool>> _entityFilters = Array.Empty<Func<EntityEntry, bool>>();

        public IReadOnlyCollection<Func<EntityEntry, bool>> EntityFilters
        {
            get => _entityFilters;
            set
            {
                if (value != null)
                    _entityFilters = value;
            }
        }

        private IReadOnlyCollection<Func<EntityEntry, PropertyEntry, bool>> _propertyFilters = Array.Empty<Func<EntityEntry, PropertyEntry, bool>>();

        public IReadOnlyCollection<Func<EntityEntry, PropertyEntry, bool>> PropertyFilters
        {
            get => _propertyFilters;
            set
            {
                if (value != null)
                    _propertyFilters = value;
            }
        }
    }
}
