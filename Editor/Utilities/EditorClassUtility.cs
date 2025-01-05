using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace MossWolfGames.Shared.Editor.Utilities
{
    public static class EditorClassUtility
    {
        private static readonly HashSet<string> validAssemblies = new HashSet<string>();

        public static List<T> CollectAllAssets<T>() where T : UnityEngine.Object
        {
            return CollectAllAssets<T>(null);
        }

        public static T CollectSingletonAsset<T>() where T : UnityEngine.Object
        {
            List<T> assetList = CollectAllAssets<T>(null);
            if (assetList == null || assetList.Count == 0)
            {
                Debug.LogError($"{nameof(EditorClassUtility)} - Could not find singleton asset of type {typeof(T).Name}");
                return null;
            }

            if (assetList.Count > 1)
            {
                Debug.LogError($"{nameof(EditorClassUtility)} - There is more than one {typeof(T).Name}. There should only be one.");
            }

            return assetList[0];
        }

        public static List<T> CollectAllAssets<T>(Func<T, bool> predicate) where T : UnityEngine.Object
        {
            List<T> assets = new List<T>();
            string[] guids = AssetDatabase.FindAssets($"t:{typeof(T).Name}");
            if (guids == null || guids.Length == 0)
            {
                return assets;
            }

            for (int i = 0; i < guids.Length; i++)
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[i]);
                var asset = AssetDatabase.LoadAssetAtPath<T>(path);
                if (predicate != null && !predicate(asset))
                {
                    continue;
                }

                assets.Add(asset);
            }
            return assets;
        }

        public static List<Type> FindAllClassesInheritingInterface<T>() where T : class
        {
            List<Type> subClassList = new List<Type>();

            Assembly[] assemblyList = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblyList)
            {
                Type[] classTypeList = assembly.GetTypes();
                foreach (Type classType in classTypeList)
                {
                    if (classType.IsAbstract || classType.Equals(typeof(T)))
                    {
                        continue;
                    }
                    if (typeof(T).IsAssignableFrom(classType))
                    {
                        subClassList.Add(classType);
                    }
                }
            }
            return subClassList;
        }

        public static List<T> GetAllClassInstancesInheritingInterface<T>() where  T : class
        {
            List<Type> types = FindAllClassesInheritingInterface<T>();
            List<T> instances = new List<T>();
            foreach(Type type in types)
            {
                T instance = Activator.CreateInstance(type) as T;
                instances.Add(instance);
            }
            return instances;
        }

        public static List<Type> FindAlphabeticalUnityCompiledClassTypes(bool onlyTypesWithStaticMethods = false)
        {
            List<Type> allTypes = new List<Type>();

            validAssemblies.Clear();
            UnityEditor.Compilation.Assembly[] unityAssemblies = UnityEditor.Compilation.CompilationPipeline.GetAssemblies();
            for(int i = 0; i < unityAssemblies.Length; i++)
            {
                UnityEditor.Compilation.Assembly unityAssembly = unityAssemblies[i];
                validAssemblies.Add(unityAssembly.name);
            }

            Assembly[] assemblyList = AppDomain.CurrentDomain.GetAssemblies();
            for (int i = 0; i < assemblyList.Length; i++)
            {
                Assembly assembly = assemblyList[i];
                string[] nameSplit = assembly.FullName.Split(',');
                if (!validAssemblies.Contains(nameSplit[0]))
                {
                    continue;
                }

                Type[] classTypeList = assembly.GetTypes();
                for (int j = 0; j < classTypeList.Length; j++)
                {
                    Type classType = classTypeList[j];
                    if(!classType.IsVisible || !classType.IsClass || classType.IsAbstract || classType.IsGenericType || classType.IsNotPublic)
                    {
                        continue;
                    }
                    if(onlyTypesWithStaticMethods)
                    {
                        MethodInfo[] allMethods = classType.GetMethods();
                        bool atLeastOneStaticMethod = System.Array.Exists(allMethods, x => x.IsStatic);
                        if(!atLeastOneStaticMethod)
                        {
                            continue;
                        }
                    }
                    allTypes.Add(classType);
                }
            }

            allTypes.Sort((x,y) => x.Name.CompareTo(y.Name));
            return allTypes;
        }
    }
}