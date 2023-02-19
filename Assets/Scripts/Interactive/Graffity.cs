using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graffity : MonoBehaviour
{
    [SerializeField]
    private Sprite invisible;

    public List<SpriteRenderer> spriteRendererArray;
    public List<Sprite> newSpriteArray;

    // Start is called before the first frame update
    void Start()
    {
        ChangeSprite();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void ChangeSprite()
    {
        for (int i = 0; i < spriteRendererArray.Count; i++)
        {
            spriteRendererArray[i].sprite = newSpriteArray[i];
        }
    }

    public void DestroySquare(int objectIndex)
    {
        spriteRendererArray[objectIndex].sprite = invisible;
    }
}
