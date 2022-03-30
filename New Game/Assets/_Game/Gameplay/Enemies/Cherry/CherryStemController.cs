

using System;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D))]
public class CherryStemController : MonoBehaviour {
    [SerializeField] private float travelTime;
    [SerializeField] private float explosionRadius;
    [SerializeField] private float damage;
    [SerializeField] private float knockbackMagnitude;
    private float _timer;
    private Rigidbody2D _rb;
    private Vector2 _targetPos;
    private Vector2 _targetVelocity;
    
    [SerializeField] private GameObject sprite;
    [SerializeField] private float heightMagnitude;
    private float _spriteHeight;

    
    [SerializeField] private GameObject explosionPrefab;
    
    private void Awake() {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start() {
        var player = FindObjectOfType<PlayerController>();
        if (player == null) {
            _targetPos = (Vector2)transform.position + Random.insideUnitCircle * Random.Range(2f, 5f);
        } else {
            _targetPos = player.transform.position;
        }

        _targetVelocity = (_targetPos - (Vector2) transform.position) / travelTime;
    }

    private void OnDrawGizmos() {
        Gizmos.DrawLine(transform.position, _targetPos);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    private void FixedUpdate() {
        _rb.velocity = _targetVelocity;
        if (_timer >= travelTime) {
            Explode();
        }

        _spriteHeight = heightMagnitude * (-Mathf.Pow(_timer, 2) + _timer * travelTime);
        sprite.transform.localPosition = new Vector3(0f, _spriteHeight, 0f);
        
        _timer += Time.deltaTime;
    }

    private void Explode() {
        var collider = Physics2D.OverlapCircle(transform.position, 3f, 1 << ApothecaryConstants.LAYER_PLAYER);
        if (collider != null)
        {
            //Get the component for EnemyHealth
            var player = collider.gameObject.GetComponent<PlayerController>();
            var dir = collider.transform.position - transform.position;
            player.TakeDamage(damage, dir, knockbackMagnitude);
        }

        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}