using System.Collections;
using System.Collections.Generic;
using _Common;
using UnityEngine;

public class WateringCanController : ToolBase {
    [SerializeField] private GameObject waterPrefab;
    
    protected override bool InputTrigger => Input.GetMouseButton(0);
    
    protected override void Fire() {
        var water = Instantiate(waterPrefab, transform.position, Quaternion.identity).GetComponent<Projectile>();
        water.Init(KaleUtils.GetMousePosWorldCoordinates());
    }
}
