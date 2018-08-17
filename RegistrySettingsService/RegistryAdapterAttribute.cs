using System;
using System.Collections.Generic;
using System.Text;

namespace phirSOFT.SettingsService.Registry
{
    /// <inheritdoc />
    /// <summary>
    /// Marks an class as an <see cref="T:phirSOFT.SettingsService.Registry.IRegistryAdapter" /> for a given type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
    public sealed class RegistryAdapterAttribute : Attribute
    {
        /// <summary>
        /// Gets the type supported by this adapter.
        /// </summary>
        public Type SupportedType { get; }

        /// <summary>
        ///  Marks an class as an <see cref="T:phirSOFT.SettingsService.Registry.IRegistryAdapter" /> for a given type.
        /// </summary>
        /// <param name="supportedType">The type supported by this adapter.</param>
        public RegistryAdapterAttribute(Type supportedType)
        {
            SupportedType = supportedType;
        }
    }
}
