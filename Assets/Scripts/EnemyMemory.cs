using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyMemory
{
    //Global timer coroutine for enemy memories
    static public IEnumerator Timer(float time)
    {
        yield return new WaitForSeconds(time);

        bool hasMemory = false;

        MemoryOfPlayer(hasMemory);
    }

    //Return true when the memory capacity has been reached in Timer
    static public bool MemoryOfPlayer(bool hasMemory)
    {
        if (hasMemory)
        {
            return true;
        }
        else return false;
    }

}
