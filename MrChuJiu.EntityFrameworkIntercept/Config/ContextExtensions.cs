using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace MrChuJiu.EntityFrameworkIntercept.Config
{
    public static class ContextExtensions
    {
        public static IContextConfigBuilder IgnoreEntity(this IContextConfigBuilder configBuilder, Type entityType)
        {
            configBuilder.WithEntityFilter(entityEntry => entityEntry.Entity.GetType() != entityType);
            return configBuilder;
        }
        public static IContextConfigBuilder IgnoreEntity<TEntity>(this IContextConfigBuilder configBuilder) where TEntity : class
        {
            configBuilder.WithEntityFilter(entityEntry => entityEntry.Entity.GetType() != typeof(TEntity));
            return configBuilder;
        }
        public static IContextConfigBuilder IgnoreTable(this IContextConfigBuilder configBuilder, string tableName)
        {
            configBuilder.WithEntityFilter(entityEntry => entityEntry.Metadata.GetTableName() != tableName);
            return configBuilder;
        }
        public static IContextConfigBuilder IgnoreProperty<TEntity>(this IContextConfigBuilder configBuilder, Expression<Func<TEntity, object>> propertyExpression) where TEntity : class
        {
            var propertyName = typeof(TEntity).Name;
            configBuilder.WithPropertyFilter(propertyEntry => propertyEntry.Metadata.Name != propertyName);
            return configBuilder;
        }

        public static IContextConfigBuilder IgnoreProperty(this IContextConfigBuilder configBuilder, string propertyName)
        {
            configBuilder.WithPropertyFilter(propertyEntry => propertyEntry.Metadata.Name != propertyName);
            return configBuilder;
        }

        public static IContextConfigBuilder WithPropertyFilter(this IContextConfigBuilder configBuilder, Func<PropertyEntry, bool> filterFunc)
        {
            configBuilder.WithPropertyFilter((entity, prop) => filterFunc.Invoke(prop));
            return configBuilder;
        }


    }
}
