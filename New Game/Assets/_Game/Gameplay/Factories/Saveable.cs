using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Saveable : MonoBehaviour
{
    protected void Start() {
        Load();
    }

    /**
     * When this object is created, how does it read the data from SaveData.Instance
     * and what does it do with that data?
     */
    protected abstract void Load();
    
    /**
     * When this object is destroyed (i.e. OnSceneChange), what state changes does it save to SaveData.Instance?
     */
    public abstract void Save();
}
