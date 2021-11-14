using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Recruiter.Infrastructure
{
    public static class DBContextExtension
    {
        public static void HandleEntityUpdateChangedFields<TContext, TEntity>(this TContext dbContext, TEntity entity, IList<string> changedFields, string[] defaultFieldsUpdated) where TContext : DbContext where TEntity : class
        {
            if (entity == null) return;
            var entry = dbContext.Entry(entity);
            var isBeingTracked = entry.State == EntityState.Unchanged || entry.State == EntityState.Modified;

            if (changedFields?.Count > 0)
            {
                if (defaultFieldsUpdated?.Length > 0)
                {
                    changedFields = changedFields.Concat(defaultFieldsUpdated).Distinct().ToList();
                }

                foreach (var property in entry.Properties)
                {
                    if (property.Metadata.IsKey()) continue;
                    var fieldChanged = changedFields.FirstOrDefault(f => f.Equals(property.Metadata.Name, StringComparison.OrdinalIgnoreCase));
                    if (fieldChanged != null)
                    {
                        property.IsModified = true;
                    }
                    else
                    {
                        if (isBeingTracked && property.IsModified)
                        {
                            property.IsModified = false;
                        }
                    }
                }
            }
            else
            {
                if (isBeingTracked)
                {
                    if (entry.State == EntityState.Modified)
                    {
                        entry.State = EntityState.Unchanged;
                    }
                }
            }
        }
    }
}
