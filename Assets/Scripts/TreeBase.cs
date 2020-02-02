using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeBase : MonoBehaviour
{
	private SpriteRenderer spriteRenderer;	

	public Sprite[] sprites;

	private void Start()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		spriteRenderer.sprite = GetRandomSprite();
	}

	private Sprite GetRandomSprite()
	{
		if (sprites.Length > 0)
			return sprites[Random.Range(0, sprites.Length)];
		return null;
	}
}
