using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LayersManager : MonoBehaviour
{
    [SerializeField]
    List<GameObject> planes;
    [SerializeField]
    int layerNo;
    [SerializeField]
    Text layerText;
    [SerializeField]
    Button layerUpButton;


    [SerializeField]
    Button layerDownButton;
    [SerializeField]
    ShapesManager shapesManager;
    [SerializeField]
    GameObject activePlane;
    private int oldLayerNo;
    [SerializeField]
    Camera frontCamera;
    [SerializeField]
    Camera layersCamera;
    [SerializeField]
    Vector3 frontCameraShift; //shift of front camera from current plane

    public GameObject ActivePlane
    {
        get => activePlane;
        private set
        {
            activePlane = value;
            if (shapesManager != null)
            {
                shapesManager.activePlane = value;
                shapesManager.LayerNo = layerNo;
            }
        }
    }

    internal readonly Rect FrontCameraRect = new Rect(x: 0, y: 0, width: 0.53f, height: 0.845f);
    internal readonly Rect LayersCameraRect = new Rect(x: 0.53f, y: 0, width: 1.0f, height: 0.845f);

    internal readonly Rect FullCameraRect = new Rect(x: 0, y: 0, width: 1.0f, height: 0.844f);

    public void MoveLayerUp()
    {
        oldLayerNo = layerNo;
        layerNo -= 1;


        UpdateLayer();

        //TODO: Move planes visually with leantween/dotween
    }

    private void UpdateLayerUpButton()
    {
        layerUpButton.interactable = true;

        if (layerNo <= 0)
        {
            layerNo = 0;
            layerUpButton.interactable = false;
        }
    }

    public void MoveLayerDown()
    {
        oldLayerNo = layerNo;
        layerNo += 1;

        UpdateLayer();
    }

    private void UpdateLayerDownButton()
    {
        layerDownButton.interactable = true;

        if (layerNo >= planes.Count - 1)
        {
            layerNo = planes.Count - 1;
            layerDownButton.interactable = false;
        }
    }

    private void UpdateLayer()
    {
        UpdateLayerDownButton();
        UpdateLayerUpButton();

       Debug.Log("number of layers:" + planes.Count);

        for (int i = 0; i < planes.Count; i++)
        {
            var layerSwitcher = planes[i].GetComponentInChildren<LayerSwitcher>();
            if (i == layerNo)
                layerSwitcher.SetStatusActive();
            else
                layerSwitcher.ClearStatus();
        }

        ActivePlane = planes[layerNo];


        frontCamera.transform.position = ActivePlane.transform.position - frontCameraShift;


        layerText.text = $"{(layerNo + 1).ToString()} of {planes.Count}";
    }

    public void SwitchToLayersView()
    {
        frontCamera.rect = FrontCameraRect;

        layersCamera.rect = LayersCameraRect;
        layersCamera.gameObject.SetActive(true);
    }

    public void SwitchToFrontView()
    {
        frontCamera.rect = FullCameraRect;
        layersCamera.gameObject.SetActive(false);
    }

    internal void SelectLayer(
        int layerNo)
    {
        oldLayerNo = layerNo;
        this.layerNo = layerNo;

        UpdateLayer();
    }
}
