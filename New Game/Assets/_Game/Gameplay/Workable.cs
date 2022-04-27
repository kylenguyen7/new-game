using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Workable : MonoBehaviour {
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected float workTime;
    [SerializeField] protected float outlineThickness = 0.05f;
    
    private bool _working;
    private float _timer;

    public delegate void OnWorkFinished();

    public OnWorkFinished OnWorkFinishCallback;

    protected PlayerController _player;
    
    private void Awake() {
        _timer = workTime;
    }

    private void Start() {
        _player = FindObjectOfType<PlayerController>();
    }

    private void Update() {
        if (_working) {
            if (_timer <= 0) {
                FinishWorking();
            }
            _timer -= Time.deltaTime;
        }
        
        spriteRenderer.material.SetFloat("_Thickness", _player.ClosestWorkable == this ? outlineThickness : 0f);
    }

    public abstract bool CanWork();
    public abstract String GetErrorMessage();
    protected abstract void OnWorkStart();
    protected abstract void OnWorkFinish();

    protected abstract void OnWorkStop();

    public void StartWorking() {
        OnWorkStart();
        _working = true;
    }

    public void StopWorking() {
        OnWorkStop();
        _working = false;
        _timer = workTime;
    }

    private void FinishWorking() {
        OnWorkFinish();
        _timer = workTime;
        _working = false;
        OnWorkFinishCallback?.Invoke();
    }
}
