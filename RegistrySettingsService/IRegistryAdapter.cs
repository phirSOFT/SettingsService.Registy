using System;
using Microsoft.Win32;

namespace phirSOFT.SettingsService.Registry
{
    /// <summary>
    /// Provides an adapter, that performs serialization for an object, so that the object can be written or read from the registry.
    /// </summary>
    /// <remarks>
    ///     Each class, that implements this interface should be marked with an <see cref="RegistryAdapterAttribute"/>.
    /// </remarks>
    public interface IRegistryAdapter
    {
        /// <summary>
        /// Read a value from a given key.
        /// </summary>
        /// <param name="key">The key to read from.</param>
        /// <param name="name">The name of the value.</param>
        /// <param name="targetType">The type of the value.</param>
        /// <returns></returns>
        object ReadValue(RegistryKey key, string name, Type targetType);

        /// <summary>
        /// Writes a value to a given key.
        /// </summary>
        /// <param name="key">The key to write to.</param>
        /// <param name="name">The name of the value.</param>
        /// <param name="value">The value to write</param>
        /// <param name="targetType">The type of the value.</param>
        void WriteValue(RegistryKey key, string name, object value, Type targetType);
    }
}