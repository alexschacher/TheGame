using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAnimator : MonoBehaviour
{
    public enum Anim{Walk, Stand, Hurt, Attack}

    private SpriteRenderer spriteRenderer;
    private Anim currentAnim = Anim.Walk;
    private Dictionary<Anim, Sprite[]> charAnim;
    private Dictionary<Anim, float> animSpeed = new Dictionary<Anim, float>
    { // Default animation speed values
        { Anim.Walk, 0.15f},
        { Anim.Stand, 0.15f},
        { Anim.Hurt, 0.15f},
        { Anim.Attack, 0.15f}
    };

    private void Start()
    {
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
        
        // TEST, remove later, sets all created entities to look like Slimes, remove when Entity Factory is created
        charAnim = Resource.charAnim["Slime"];

        StartAnimation(Anim.Stand);
    }

    public void StartAnimation(Anim anim)
    {
        if (currentAnim != anim)
        {
            StopAllCoroutines();
            currentAnim = anim;
            StartCoroutine(Animate(charAnim[anim], animSpeed[anim]));
        }
    }

    private IEnumerator Animate(Sprite[] anim, float animSpeed)
    {
        int i = 0;
        while (i < anim.Length)
        {
            spriteRenderer.sprite = anim[i];
            i++;
            yield return new WaitForSeconds(animSpeed);
        }
        StartCoroutine(Animate(anim, animSpeed));
    }
}