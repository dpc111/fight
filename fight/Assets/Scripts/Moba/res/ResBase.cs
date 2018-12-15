using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResBase<T> where T : class {
    public Dictionary<string, T> resList = new Dictionary<string, T>();

    public virtual void Init() {
        resList.Clear();
    }

    public virtual T Get(string name) {
        if (!resList.ContainsKey(name)) {
            return null;
        }
        return resList[name];
    }

    public virtual void Add(string name, T value) {
        if (resList.ContainsKey(name)) {
            return;
        }
        resList[name] = value;
    }

    public virtual void Remove(string name) {
        if (!resList.ContainsKey(name)) {
            return;
        }
        resList.Remove(name);
    }

    public virtual void Load(string path, string name) {
        if (Get(name) != null) {
            return;
        }
        T res = Resources.Load(path + name) as T;
        if (res == null) {
            return;
        }
        Add(name, res);
    }
}
