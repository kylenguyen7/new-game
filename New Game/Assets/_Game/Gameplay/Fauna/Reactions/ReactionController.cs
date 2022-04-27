using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactionController : MonoBehaviour {
    [SerializeField] private SpriteRenderer bubbleRenderer;
    [SerializeField] private SpriteRenderer reactionRenderer;
    [SerializeField] private Sprite heartReact;
    [SerializeField] private Sprite sadReact;
    [SerializeField] private Sprite moneyReact;

    private Coroutine _coroutine;

    public enum Reaction {
        HEART,
        SAD,
        MONEY
    }

    private void Awake() {
        bubbleRenderer.enabled = false;
        reactionRenderer.enabled = false;
    }

    public void React(Reaction reaction, bool clear) {
        bubbleRenderer.enabled = true;
        reactionRenderer.enabled = true;
        reactionRenderer.sprite = GetSprite(reaction);
        
        if (_coroutine != null) {
            StopCoroutine(_coroutine);
        }
        
        if (clear) {
            _coroutine = StartCoroutine(ClearReactionCoroutine());
        }
    }

    private IEnumerator ClearReactionCoroutine() {
        yield return new WaitForSeconds(2f);
        bubbleRenderer.enabled = false;
        reactionRenderer.enabled = false;
        _coroutine = null;
    }

    private Sprite GetSprite(Reaction reaction) {
        switch (reaction) {
            case Reaction.HEART:
                return heartReact;
            case Reaction.SAD:
                return sadReact;
            case Reaction.MONEY:
                return moneyReact;
            default:
                throw new ArgumentOutOfRangeException(nameof(reaction), reaction, null);
        }
    }
}
