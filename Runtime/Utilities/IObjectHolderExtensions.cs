using System;
using System.Collections.Generic;

namespace MossWolfGames.Shared.Runtime.Utilities
{
    public static class IObjectHolderExtensions
    {
        public static void GetObjectsWithType<T>(this IObjectHolder objectHolder, List<T> objectTypeList)
        {
            List<object> objectList = new List<object>();
            objectHolder.GetObjects(objectList);

            for (int i = 0; i < objectList.Count; i++)
            {
                object currentObject = objectList[i];
                if (currentObject is T targetObject)
                {
                    objectTypeList.Add(targetObject);
                }
                if (currentObject is IObjectHolder subObjectHolder)
                {
                    subObjectHolder.GetObjectsWithType(objectList);
                }
            }
        }

        public static T FindObjectMatching<T>(this IObjectHolder objectHolder, Predicate<T> predicate)
        {
            List<object> objectList = new List<object>();
            objectHolder.GetObjects(objectList);

            for (int i = 0; i < objectList.Count; i++)
            {
                object currentObject = objectList[i];
                if (currentObject is T targetObject && predicate(targetObject))
                {
                    return targetObject;
                }
                if (currentObject is IObjectHolder subObjectHolder)
                {
                    T objectFound = subObjectHolder.FindObjectMatching(predicate);
                    if (objectFound != null && !objectFound.Equals(default(T)))
                    {
                        return objectFound;
                    }
                }
            }

            return default(T);
        }

        public static T FindObjectMatchingReverseOrder<T>(this IObjectHolder objectHolder, Predicate<T> predicate)
        {
            List<object> objectList = new List<object>();
            objectHolder.GetObjects(objectList);

            for (int i = objectList.Count - 1; i >= 0; i--)
            {
                object currentObject = objectList[i];
                if (currentObject is T targetObject && predicate(targetObject))
                {
                    return targetObject;
                }
                if (currentObject is IObjectHolder subObjectHolder)
                {
                    T objectFound = subObjectHolder.FindObjectMatching(predicate);
                    if (objectFound != null && !objectFound.Equals(default(T)))
                    {
                        return objectFound;
                    }
                }
            }

            return default(T);
        }
    }
}