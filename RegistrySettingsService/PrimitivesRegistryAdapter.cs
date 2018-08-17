using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace phirSOFT.SettingsService.Registry
{
    [RegistryAdapter(typeof(bool))]
    [RegistryAdapter(typeof(byte))]
    [RegistryAdapter(typeof(sbyte))]
    [RegistryAdapter(typeof(short))]
    [RegistryAdapter(typeof(ushort))]
    [RegistryAdapter(typeof(char))]
    [RegistryAdapter(typeof(int))]
    [RegistryAdapter(typeof(uint))]
    [RegistryAdapter(typeof(float))]
    [RegistryAdapter(typeof(long))]
    [RegistryAdapter(typeof(ulong))]
    [RegistryAdapter(typeof(double))]
    [RegistryAdapter(typeof(string))]
    public class PrimitivesRegistryAdapter : IRegistryAdapter
    {
        private static readonly Type[] dwordType = {
            typeof(bool),
            typeof(byte),
            typeof(sbyte),
            typeof(short),
            typeof(ushort),
            typeof(char),
            typeof(int),
            typeof(uint),
            typeof(float)
        };

        private static readonly Type[] qwordType = {
            typeof(long),
            typeof(ulong),
            typeof(double)
        };

        public object ReadValue(RegistryKey key, string name, Type targetType)
        {
            return key.GetValue(name);
        }

        public void WriteValue(RegistryKey key, string name, object value, Type targetType)
        {
            RegistryValueKind kind;
            if (dwordType.Contains(targetType))
                kind = RegistryValueKind.DWord;
            else if (qwordType.Contains(targetType))
                kind = RegistryValueKind.QWord;
            else if (targetType == typeof(string))
                kind = RegistryValueKind.String;
            else
                kind = RegistryValueKind.Binary;

            key.SetValue(name, value, kind);
        }




    }
}