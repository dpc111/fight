namespace NetModel
{
    using UnityEngine;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;

    public class ObjectPool<T> where T : new()
    {
        private static Stack<T> objects = new Stack<T>();
        private static T v;

        public static T CreateObject()
        {
            lock(objects)
            {
                if (objects.Count > 0)
                {
                    v = objects.Pop();
                    return v;
                }
                else
                {
                    return new T();
                }
            }
        }

        public static void ReclaimObject(T item)
        {
            lock (objects)
            {
                objects.Push(item);
            }
        }
    }
}