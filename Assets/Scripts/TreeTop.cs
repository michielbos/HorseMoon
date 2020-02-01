using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HorseMoon
{

    public class TreeTop : MonoBehaviour
    {
        public Sprite[] sprites;
        SpriteRenderer SpriteRenderer { get; set; }

        private void Start()
        {
            SpriteRenderer = GetComponent<SpriteRenderer>();
            RandomSprite();
        }

        

        private void OnTriggerEnter2D(Collider2D collision)
        {
            SpriteRenderer.color = new Color(1f, 1f, 1f, .5f);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            SpriteRenderer.color = new Color(1f, 1f, 1f, 1f);
        }


        void RandomSprite()
        {
            if (sprites.Length > 0)
            {
                SpriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
            }
        }
    }

}
