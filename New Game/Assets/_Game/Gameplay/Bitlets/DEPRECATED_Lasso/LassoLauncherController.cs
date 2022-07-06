using _Common;
using UnityEngine;

public class LassoLauncherController : MonoBehaviour {
    [SerializeField] private GameObject lassoPrefab;

    public void Update() {
        if (Input.GetMouseButtonDown(0)) {
            var lasso = Instantiate(lassoPrefab, transform.position, Quaternion.identity).GetComponent<LassoController>();
            lasso.Init(KaleUtils.GetMousePosWorldCoordinates() - (Vector2)transform.position, transform);
        }
    }
}
