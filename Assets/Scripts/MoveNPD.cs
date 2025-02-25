using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveNPD : Interactable
{
    public List<SpriteMover> spritesToMove = new List<SpriteMover>();

    public override void Interact()
    {
        foreach (SpriteMover character in spritesToMove)
        {
            character.spriteToMove.gameObject.transform.position = character.newLocation.position;
            character.spriteToMove.flipX = character.flipSprite;
            if (character.newSprite != null)
            {
                character.spriteToMove.sprite = character.newSprite;
            }
        }
    }

    [System.Serializable]
    public struct SpriteMover
    {
        public SpriteRenderer spriteToMove;
        public Transform newLocation;
        public Sprite newSprite;
        public bool flipSprite;
    }

}
