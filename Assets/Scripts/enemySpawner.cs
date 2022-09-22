using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawner : MonoBehaviour
{
    [Header("-----Spawn Types-----")]
    public bool bossOK; //this is checked to see if a boss can spawn here
    public bool rangedOnly; //this is checked to see if only the ranged enemy can spawn here
    [Header("-----Don't change in editor-----")]
    public bool isSpawning;

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (bossOK)
        {
            Gizmos.color = Color.red;
        }
        else if (rangedOnly)
        {
            Gizmos.color = Color.yellow;
        }
        else
            Gizmos.color = Color.blue;
        
        Gizmos.DrawSphere(transform.position, 1);
        Gizmos.color = Color.red;
        Vector3 dir = transform.TransformDirection(Vector3.forward) * 3;
        Gizmos.DrawRay(transform.position, dir);
    }
#endif
}
