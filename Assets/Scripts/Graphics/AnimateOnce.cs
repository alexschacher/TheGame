using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateOnce : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] Sprite[] anim;
    [SerializeField] float animSpeed;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        int i = 0;
        while (i < anim.Length)
        {
            spriteRenderer.sprite = anim[i];
            i++;
            yield return new WaitForSeconds(animSpeed);
        }
        Destroy(this.gameObject);
    }
}
