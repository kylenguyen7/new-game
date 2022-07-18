using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MonsterController : MonoBehaviour {
    private NestController _nest;
    public String NestId => _nest.NestId;

    [SerializeField] private String speciesName;
    public String SpeciesName => speciesName;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float roamSpeed;
    [SerializeField] private Animator animator;
    private float _waitTime;
    private Vector2 _targetPosition;
    private bool _roaming;

    private void Awake() {
        _waitTime = Random.Range(2f, 5f);
    }

    public void Init(NestController nest) {
        _nest = nest;
    }

    private void Update() {
        if (_roaming) {
            if (Vector2.Distance(transform.position, _targetPosition) <= 0.05f) {
                _roaming = false;
                _waitTime = Random.Range(3f, 6f);
                animator.SetBool("roaming", false);
            } else {
                rb.velocity = (_targetPosition - (Vector2)transform.position).normalized * roamSpeed;
            }
        } else {
            if (_waitTime <= 0) {
                _targetPosition = _nest.GetRandomPositionInBounds(); 
                _roaming = true;
                animator.SetBool("roaming", true);
            } else {
                _waitTime -= Time.deltaTime;
            }
        }
    }
}