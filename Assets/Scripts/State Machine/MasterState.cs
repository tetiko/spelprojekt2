using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MasterState : MonoBehaviour
{
    protected StateManager stateManager;

    public abstract void Update();
}
