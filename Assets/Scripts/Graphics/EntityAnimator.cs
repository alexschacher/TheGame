using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAnimator : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private Sprite sprWalk1, sprWalk2, sprWalk3, sprWalk4, sprStand;
    private Sprite[] currentAnim, standAnim, walkAnim, hurtAnim, attackAnim;

    [SerializeField] private Texture2D spriteSheetTexture;

    private float standAnimSpeed = 0.15f;
    private float walkAnimSpeed = 0.15f;
    private float hurtAnimSpeed = 0.15f;
    private float attackAnimSpeed = 0.15f;

    void Awake()
    {
        CreateSprites();
        CreateAnimations();
        
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
        AnimateStand();
    }

    // Define Sprites and Animations

    private void CreateSprites()
    {
        sprWalk1 = Sprite.Create(spriteSheetTexture, new Rect(00, 48, 16, 16), new Vector2(0.5f, 0), 12);
        sprWalk2 = Sprite.Create(spriteSheetTexture, new Rect(16, 48, 16, 16), new Vector2(0.5f, 0), 12);
        sprWalk3 = Sprite.Create(spriteSheetTexture, new Rect(32, 48, 16, 16), new Vector2(0.5f, 0), 12);
        sprWalk4 = Sprite.Create(spriteSheetTexture, new Rect(48, 48, 16, 16), new Vector2(0.5f, 0), 12);

        sprStand = Sprite.Create(spriteSheetTexture, new Rect(00, 32, 16, 16), new Vector2(0.5f, 0), 12);
    }

    private void CreateAnimations()
    {
        standAnim  = new Sprite[1] { sprStand };
        walkAnim   = new Sprite[4] { sprWalk2, sprWalk3, sprWalk4, sprWalk1 };
        hurtAnim   = new Sprite[2] { sprWalk3, sprWalk1 };
        attackAnim = new Sprite[1] { sprWalk3 };
    }

    // Execute Animation

    private void StartAnimation(Sprite[] anim, float animSpeed)
    {
        if (currentAnim != anim)
        {
            StopAllCoroutines();
            StartCoroutine(Animate(anim, animSpeed));
        }
    }

    private IEnumerator Animate(Sprite[] anim, float animSpeed)
    {
        currentAnim = anim;
        int i = 0;
        while (i < anim.Length)
        {
            spriteRenderer.sprite = anim[i];
            i++;
            yield return new WaitForSeconds(animSpeed);
        }
        StartCoroutine(Animate(anim, animSpeed));
    }

    // Animation Commands

    public void AnimateStand()
    {
        StartAnimation(standAnim, standAnimSpeed);
    }

    public void AnimateWalk()
    {
        StartAnimation(walkAnim, walkAnimSpeed);
    }

    public void AnimateHurt()
    {
        StartAnimation(hurtAnim, hurtAnimSpeed);
    }

    public void AnimateAttack()
    {
        StartAnimation(attackAnim, attackAnimSpeed);
    }
}