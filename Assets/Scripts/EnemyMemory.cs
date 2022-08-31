using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMemory 
{
    //Global timer coroutine
    public IEnumerator Timer(float time)
    {
        yield return new WaitForSeconds(time);

        bool hasMemory = false;

        MemoryOfPlayer(hasMemory);
    }

    //Return true when the memory capacity has counted down in GlobalTimer is up
    static public bool MemoryOfPlayer(bool hasMemory)
    {
        if (hasMemory)
        {
            return true;
        }
        else return false;
    }

}
