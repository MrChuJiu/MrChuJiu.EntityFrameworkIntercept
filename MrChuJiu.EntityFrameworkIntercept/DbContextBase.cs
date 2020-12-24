using Microsoft.EntityFrameworkCore;
using MrChuJiu.EntityFrameworkIntercept.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MrChuJiu.EntityFrameworkIntercept
{
    public abstract class InterceptDbContext : DbContext
    {
        protected InterceptDbContext()
        {
        }

        protected InterceptDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
        }

        protected List<ChangeEntry> ChangeEntry { get; set; }

        protected virtual Task BeforeSaveChanges()
        {
            ChangeEntry = new List<ChangeEntry>();
            foreach (var entityEntry in ChangeTracker.Entries())
            {
                var ChangeEntryInfo = new ChangeEntry();
                if (ContextConfig.ContextConfigOptions.EntityFilters.Any(entityFilter =>
                        entityFilter.Invoke(entityEntry) == false))
                {
                    continue;
                }
                ChangeEntryInfo.TableName = entityEntry.Metadata.GetTableName();
                ChangeEntryInfo.NewValue = new Dictionary<string, object>();
                ChangeEntryInfo.usedValue = new Dictionary<string, object>();
                ChangeEntryInfo.EntityState = entityEntry.State;
                foreach (var propertyEntry in entityEntry.Properties)
                {
                    if (ContextConfig.ContextConfigOptions.PropertyFilters.Any(f => f.Invoke(entityEntry, propertyEntry) == false))
                    {
                        continue;
                    }

                    var columnName = propertyEntry.Metadata.GetColumnName();
                    switch (entityEntry.State)
                    {
                        case EntityState.Added:
                            ChangeEntryInfo.NewValue[columnName] = propertyEntry.CurrentValue;
                            break;

                        case EntityState.Deleted:
                            ChangeEntryInfo.NewValue[columnName] = propertyEntry.OriginalValue;
                            break;

                        case EntityState.Modified:
                            if (propertyEntry.IsModified)
                            {
                                ChangeEntryInfo.usedValue[columnName] = propertyEntry.OriginalValue;
                                ChangeEntryInfo.NewValue[columnName] = propertyEntry.CurrentValue;
                            }
                            break;
                    }
                }

                ChangeEntry.Add(ChangeEntryInfo);
            }

            return Task.CompletedTask;
        }

        protected virtual Task AfterSaveChanges()
        {
            if (ChangeEntry != null && ChangeEntry.Count > 0)
            {
                this.AfterIntercept(ChangeEntry);
            }
            return Task.CompletedTask;
        }

        public override int SaveChanges()
        {
            BeforeSaveChanges().Wait();
            var result = base.SaveChanges();
            AfterSaveChanges().Wait();
            return result;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await BeforeSaveChanges();
            var result = await base.SaveChangesAsync(cancellationToken);
            await AfterSaveChanges();
            return result;
        }

        public abstract Task AfterIntercept(List<ChangeEntry> ChangeEntry);
    }
}
