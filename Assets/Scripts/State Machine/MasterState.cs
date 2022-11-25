using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MasterState : MonoBehaviour
{
    public abstract MasterState RunCurrentState();
}
