using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ToonMaterialPropertyBlock : MonoBehaviour
{
    [SerializeField] MaterialPropertyBlock toonPropertyBlock = new MaterialPropertyBlock();
    [SerializeField] Color baseColor = Color.white;
    [SerializeField] Renderer renderer;
    public bool waitingColorUpdate = false;

    [SerializeField] MaterialIndex materialIndex;
    public enum MaterialIndex
    {
        _ToonColor1, 
        _ToonColor2,
        _ToonColor3,
        _ToonColor4,
        _ToonColor5,
        _ToonColor6,
    }
    // Start is called before the first frame update
    void Start()
    {
        if(!renderer) renderer = GetComponent<Renderer>();
        toonPropertyBlock = new MaterialPropertyBlock();
        toonPropertyBlock.SetColor(materialIndex.ToString(), baseColor);
        renderer.SetPropertyBlock(toonPropertyBlock);
        waitingColorUpdate = false;
    }
    
    //below code is for updating colors in editor without having to run the game
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!renderer) renderer = GetComponent<Renderer>();
        toonPropertyBlock = new MaterialPropertyBlock();
        toonPropertyBlock.SetColor(materialIndex.ToString(), baseColor);
        renderer.SetPropertyBlock(toonPropertyBlock);
        
    }
#endif
}