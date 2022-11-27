using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionShape : Shape
{
    private GameObject startShapeObj;
    private GameObject finalShapeObj;
    private Shape startShape;
    private Shape finalShape;

    LineRenderer line;

    public BoxCollider Collider { get; set; }

    public ShapesManager Manager { get; set; }

    public override void CleanUp()
    {
        base.CleanUp();
        startShape.RemoveConnection(this);
        finalShape.RemoveConnection(this);
    }

    private void Start()
    {
        line = GetComponent<LineRenderer>(); 
    }

    internal void SetStartShape(GameObject firstConnectionShape)
    {
        startShapeObj = firstConnectionShape;
        startShape = startShapeObj.GetComponent<Shape>();
        startShape.AddConnection(this);
    }

    internal void SetFinalShape(GameObject secondConnectionShape)
    {
        finalShapeObj = secondConnectionShape;
        finalShape = finalShapeObj.GetComponent<Shape>();
        finalShape.AddConnection(this);
    }

    private void Update()
    {
        if ((line == null) || (startShapeObj == null) || (finalShapeObj == null))
            return;

        //TODO: Make it as message and save cpu cycles
        if ((line.GetPosition(0) != startShapeObj.transform.position) ||
            (line.GetPosition(1) != finalShapeObj.transform.position))
        {
            line.SetPosition(0, startShapeObj.transform.position);
            line.SetPosition(1, finalShapeObj.transform.position);
            PlaceCollider(startShapeObj.transform.position, finalShapeObj.transform.position, Collider);
        }
    }

    public void CreateCollider(GameObject first, GameObject second)
    {
        var startPos = first.transform.position;
        var endPos = second.transform.position;

        var objCollider = new GameObject("Collider");
        BoxCollider col = objCollider.AddComponent<BoxCollider>();
        col.transform.parent = transform; // Collider is added as child object of line

        var selector = objCollider.AddComponent<ShapeSelector>();
        selector.ParentShape = this;
        selector.Manager = Manager;

        Collider = col;

        PlaceCollider(startPos, endPos, col);
    }

    private static void PlaceCollider(Vector3 startPos, Vector3 endPos, BoxCollider col)
    {
        if (col == null)
            return;

        float lineLength = Vector3.Distance(startPos, endPos); // length of line
        col.size = new Vector3(lineLength, 0.1f, 1f); // size of collider is set where X is length of line, Y is width of line, Z will be set as per requirement

        Vector3 midPoint = (startPos + endPos) / 2;
        col.transform.position = midPoint; // setting position of collider object

        // Following lines calculate the angle between startPos and endPos
        float angle = (Mathf.Abs(startPos.y - endPos.y) / Mathf.Abs(startPos.x - endPos.x));

        if ((startPos.y < endPos.y && startPos.x > endPos.x) || (endPos.y < startPos.y && endPos.x > startPos.x))
        {
            angle *= -1;
        }

        angle = Mathf.Rad2Deg * Mathf.Atan(angle);

        col.transform.eulerAngles = new Vector3(0, 0, angle);
    }
}
