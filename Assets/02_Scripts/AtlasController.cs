using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;

public class AtlasController : MonoBehaviour
{
    Atlas atlas;
    AtlasRegion region;
    SpriteRenderer sr;
    SpineAtlasAsset atlasAsset;
    void Start()
    {
        atlasAsset = (SpineAtlasAsset)Resources.Load("items/items_Atlas");
        atlas = atlasAsset.GetAtlas();
        
    }

    //public Sprite GetAtlasSprite(string name)
    //{
    //    region = atlas.FindRegion(name);
    //    sr.material = region;
    //}
}
