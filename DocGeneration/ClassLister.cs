using AchiSplatoon2.Helpers;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace AchiSplatoon2.DocGeneration
{
    public class ClassLister
    {
        private Assembly _assembly;
        public ClassLister(Assembly assembly)
        {
            _assembly = assembly;
        }

        public List<Type> GetTypesInNamespace(string @namespace)
        {
            Type[] types = _assembly.GetTypes();
            List<Type> matchingTypes = new();

            foreach (var type in types)
            {
                if (!string.IsNullOrWhiteSpace(type.Namespace) && type.Namespace.Contains(@namespace))
                {
                    matchingTypes.Add(type);
                }
            }

            DebugHelper.PrintDebug($"Found {matchingTypes.Count} classess that match the namespace '{@namespace}'");
            return matchingTypes;
        }
    }
}
