using System;
using System.Runtime.InteropServices;
using _Common;
using UnityEngine;

public class HandController : MonoBehaviour
{
    [SerializeField] private float acceleration;
    [SerializeField] private Rigidbody2D rb;

    private Vector2 _targetPosition;

    private void Update()
    {
        if (Input.GetMouseButtonDown(1)) {
            UpdateTargetPosition(KaleUtils.GetMousePosWorldCoordinates());
        }

        Vector2 toTarget = _targetPosition - (Vector2)transform.position;
        rb.velocity += toTarget.normalized * (acceleration * Time.deltaTime);

        if (toTarget.magnitude < rb.velocity.magnitude * Time.deltaTime) {
            rb.velocity = toTarget / Time.deltaTime;
        }
    }

    private void UpdateTargetPosition(Vector2 newPosition) {
        _targetPosition = newPosition;
        rb.velocity = Vector2.zero;
    }
}