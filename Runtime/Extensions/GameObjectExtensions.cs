using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MossWolfGames.Shared.Runtime.Extensions
{
    public static class GameObjectExtensions
    {
        public static string GetFullPath(this GameObject gameObject)
        {
            string path = "/" + gameObject.name;

            while(gameObject.transform.parent != null)
            {
                gameObject = gameObject.transform.parent.gameObject;
                path = "/" + gameObject.name + path;
            }

            return path;
        }
    }
}