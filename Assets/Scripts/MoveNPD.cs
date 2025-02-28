using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveNPD : Interactable
{
    // Tool used for moving, flipping, changing, and/or activating character sprite

    public List<SpriteMover> spritesToMove = new List<SpriteMover>();

    public override void Interact()
    {
        foreach (SpriteMover character in spritesToMove)
        {
            if (character.newLocation != null)
            {
                character.spriteToMove.gameObject.transform.position = character.newLocation.position;
            }
            character.spriteToMove.flipX = character.flipSprite;
            character.spriteToMove.gameObject.SetActive(character.isVisible);

            if (character.newSprite != null)
            {
                character.spriteToMove.sprite = character.newSprite;
            }
            
        }
        base.Complete();
    }

    [System.Serializable]
    public struct SpriteMover
    {
        public SpriteRenderer spriteToMove;
        public Transform newLocation;
        public Sprite newSprite;
        public bool flipSprite;
        public bool isVisible;
    }

}
