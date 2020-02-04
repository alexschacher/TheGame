using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateUV : MonoBehaviour
{
    [SerializeField]
    private Vector2 offset = Vector2.zero;

    [SerializeField]
    private float frameDelay = 1f;

    [SerializeField]
    private float numberOfFrames = 2;

    [SerializeField]
    private float textureSize = 1024;

    private Renderer rend;
    private float timer;
    private float frame;
    private Vector2 currentOffset;
	
	void Start()
	{
        rend = GetComponent<Renderer>();
	}
	
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= frameDelay)
        {
            timer -= frameDelay;
            gotoNextFrame();
        }
    }

    private void gotoNextFrame()
    {
        frame++;
        if (frame >= numberOfFrames)
        {
            frame = 0;
            rend.material.SetTextureOffset("_MainTex", Vector2.zero);
        }
        else
        {
            rend.material.SetTextureOffset("_MainTex", rend.material.mainTextureOffset + (offset / textureSize));
        }
    }
}
