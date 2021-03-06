﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlocksAttributes", menuName = "TerrainGenerator/Blocks Attributes")]
public class BlocksAttributes : ScriptableObject
{

	[SerializeField]
	private Material material;
	[SerializeField]
	private Material transparentMaterial;
	[SerializeField]
	private BlockType[] blocktypes;
	
	public Material Material { get => material; }
	public Material TransparentMaterial { get => transparentMaterial; }

	[SerializeField]
	private int textureAtlasSizeInBlocks;

	public int TextureAtlasSizeInBlocks { get => textureAtlasSizeInBlocks; }

	public float NormalizedBlockTextureSize
	{

		get { return 1f / (float)textureAtlasSizeInBlocks; }

	}

	public BlockType[] Blocktypes { get => blocktypes; }
	
}

[System.Serializable]
public class BlockType
{

	public string blockName;
	public bool isTransparent;
	public bool isSolid;
	public bool isLiquid;

	[Header("Texture Values")]
	public int backFaceTexture;
	public int frontFaceTexture;
	public int topFaceTexture;
	public int bottomFaceTexture;
	public int leftFaceTexture;
	public int rightFaceTexture;

	// Back, Front, Top, Bottom, Left, Right

	public int GetTextureID(int faceIndex)
	{

		switch (faceIndex)
		{

			case 0:
				return backFaceTexture;
			case 1:
				return frontFaceTexture;
			case 2:
				return topFaceTexture;
			case 3:
				return bottomFaceTexture;
			case 4:
				return leftFaceTexture;
			case 5:
				return rightFaceTexture;
			default:
				Debug.Log("Error in GetTextureID; invalid face index");
				return 0;

		}

	}

}
