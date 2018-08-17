using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace phirSOFT.SettingsService.Registry
{
    /// <summary>
    /// Provides extensions for a <see cref="IRegistryAdapter"/>
    /// </summary>
    public static class RegistryAdapterExtensions
    {
        /// <summary>
        /// Gets all types supported by an <see cref="IRegistryAdapter"/>
        /// </summary>
        /// <param name="adapter">The adapter to get the supported types of.</param>
        /// <returns>All types supported by this adapter.</returns>
        public static IEnumerable<Type> GetSupportedTypes(this IRegistryAdapter adapter)
        {
            return adapter.GetType()
                .GetTypeInfo()
                .GetCustomAttributes<RegistryAdapterAttribute>()
                .Select(att => att.SupportedType);
        }
    }
}
