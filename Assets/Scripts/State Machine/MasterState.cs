using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MasterState : MonoBehaviour
{
    protected virtual void Start()
    {
        //Debug.Log("base start");
    }
    public abstract MasterState RunCurrentState();
}
