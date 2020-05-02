using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ScriptShader : MonoBehaviour {

    public Color colour = Color.white;
    private SpriteRenderer sr;

    void OnEnable()
    {
        sr = GetComponent<SpriteRenderer>();
        UpdateOutline(true);
    }

    void OnDisable()
    {
        UpdateOutline(false);
    }

    void Update()
    {
        UpdateOutline(true);
    }

    void UpdateOutline(bool outline)
    {
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();

        sr.GetPropertyBlock(mpb);
        mpb.SetFloat("_Outline", outline ? 10.0f : 0);
        mpb.SetColor("_OutlineColor", colour);
        sr.SetPropertyBlock(mpb);
    }
    
}
