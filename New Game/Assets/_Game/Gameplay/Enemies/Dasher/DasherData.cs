using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(order = 0, menuName = "Data/Dasher", fileName = "DasherData")]
public class DasherData : ScriptableObject {
    // Roaming
    public float roamSpeed;
    public float roamDestinationRadius;
    public float roamArrivedRange;
    
    // Waiting
    public float waitTime;
    
    // Chasing
    public float chaseSpeed;
    public float chaseStartRadius;
    public float chaseFinishedRadius;

    // Preparing
    public float prepTime;
    
    // Dashing
    public float dashSpeed;
    public float dashTime;
    
    // Recovering
    public float recoverTime;
}
