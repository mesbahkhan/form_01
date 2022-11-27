using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeSelector : MonoBehaviour
{
    [SerializeField]
    Shape parentShape;
    [SerializeField]
    ShapesManager manager;

    public Shape ParentShape { get => parentShape; set => parentShape = value; }

    public ShapesManager Manager { get => manager; set => manager = value; }

    private void Start()
    {
        manager = FindObjectOfType<ShapesManager>();
    }

    private void OnMouseUpAsButton()
    {
        Manager.SelectShape(ParentShape);
    }
}
