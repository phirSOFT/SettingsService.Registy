using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace phirSOFT.SettingsService.Registry
{
    public class RegistrySettingsService : ISettingsService
    {
        private readonly RegistryKey _baseKey;
        private readonly Dictionary<Type, IRegistryAdapter> _adapters;

        public RegistrySettingsService(RegistryKey baseKey) : this(baseKey, Enumerable.Empty<IRegistryAdapter>())
        {

        }

        public RegistrySettingsService(RegistryKey baseKey, params IRegistryAdapter[] adapters) : this(baseKey,
            (IEnumerable<IRegistryAdapter>) adapters)
        {

        }

        public RegistrySettingsService(RegistryKey baseKey, IEnumerable<IRegistryAdapter> adapters)
        {
            _baseKey = baseKey;
            _adapters = new Dictionary<Type, IRegistryAdapter>();
            foreach (IRegistryAdapter adapter in adapters)
            {
                try
                {
                    RegisterAdapter(adapter);
                }
                catch (ArgumentException e)
                {
                   throw new ArgumentException(e.Message, nameof(adapters), e);
                }
            }
        }

        public void RegisterAdapter(IRegistryAdapter adapter)
        {
            foreach (Type supportedType in adapter.GetSupportedTypes())
            {
                if (_adapters.ContainsKey(supportedType))
                    throw new ArgumentException($"Adapter for {supportedType} allready defined", nameof(adapter));
                _adapters.Add(supportedType, adapter);
            }
        }


        public void UnregisterAdapter(IRegistryAdapter adapter)
        {
            var toBeRemoved = new LinkedList<Type>();
            foreach (KeyValuePair<Type, IRegistryAdapter> registryAdapter in _adapters)
            {
                if (registryAdapter.Value == adapter)
                    toBeRemoved.AddLast(registryAdapter.Key);
            }

            foreach (Type type in toBeRemoved)
            {
                _adapters.Remove(type);
            }
        }

        public Task<object> GetSettingAsync(string key, Type type)
        {
            int lastPathSeperatorIndex = key.LastIndexOf(value: '\\');
            RegistryKey currentKey = _baseKey.CreateSubKey(key.Substring(0, lastPathSeperatorIndex - 1), writable: false);
            if (!_adapters.ContainsKey(type))
                throw new InvalidOperationException($"No adapter for '{type}' is registred.");

            return Task.FromResult(_adapters[type].ReadValue(currentKey, key.Substring(lastPathSeperatorIndex + 1), type));

        }

        public Task<bool> IsRegisterdAsync(string key)
        {
            int lastPathSeperatorIndex = key.LastIndexOf(value: '\\');
            RegistryKey currentKey = _baseKey.OpenSubKey(key.Substring(0, lastPathSeperatorIndex - 1), false);
            return Task.FromResult(currentKey != null && currentKey.GetValueNames().Contains(key.Substring(lastPathSeperatorIndex + 1)));
        }

        public Task SetSettingAsync(string key, object value, Type type)
        {
            int lastPathSeperatorIndex = key.LastIndexOf(value: '\\');
            RegistryKey currentKey = _baseKey.CreateSubKey(key.Substring(0, lastPathSeperatorIndex - 1), writable: true);
            if (!_adapters.ContainsKey(type))
                throw new InvalidOperationException($"No adapter for '{type}' is registred.");

            _adapters[type].WriteValue(currentKey, key.Substring(lastPathSeperatorIndex + 1), value, type);
            return Task.CompletedTask;
        }

        public Task RegisterSettingAsync(string key, object defaultValue, object initialValue, Type type)
        {
            return SetSettingAsync(key, initialValue, type);
        }

        public Task UnregisterSettingAsync(string key)
        {
            int lastPathSeperatorIndex = key.LastIndexOf(value: '\\');
            RegistryKey currentKey = _baseKey.CreateSubKey(key.Substring(0, lastPathSeperatorIndex - 1), writable: true);
            currentKey.DeleteValue(key.Substring(lastPathSeperatorIndex + 1));

            return Task.CompletedTask;
        }

        public Task StoreAsync()
        {
            return Task.CompletedTask;
        }

        public Task DiscardAsync()
        {
            return Task.CompletedTask;
        }
    }
}
