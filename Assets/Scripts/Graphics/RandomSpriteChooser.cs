using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpriteChooser : MonoBehaviour
{
    [SerializeField] private List<Sprite> randomSpriteList;

    private void Awake()
    {
        if (randomSpriteList.Count > 0)
        {
            GetComponent<SpriteRenderer>().sprite = randomSpriteList[Random.Range(0, randomSpriteList.Count)];
        }
    }
}
