using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerSwitcher : MonoBehaviour
{
    [SerializeField]
    LayersManager layersManager;
    [SerializeField]
    int layerNo;
    [SerializeField]
    SpriteRenderer layerStatusSprite;
    [SerializeField]
    Color selectedColor = Color.yellow;
    [SerializeField]
    Color normalColor = Color.white;

    private void Start()
    {
        if (layersManager == null)
            layersManager = FindObjectOfType<LayersManager>();
    }

    private void OnMouseUpAsButton()
    {
        layersManager.SelectLayer(layerNo);
    }

    public void ClearStatus()
    {
        layerStatusSprite.color = normalColor;
    }

    internal void SetStatusActive()
    {
        layerStatusSprite.color = selectedColor;
    }
}
