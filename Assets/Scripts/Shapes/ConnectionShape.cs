using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionShape : Shape
{
    protected GameObject startShapeObj;
    protected GameObject finalShapeObj;
    protected Shape startShape;
    protected Shape finalShape;

    public Shape StartShape => startShape;
    public Shape FinalShape => finalShape;

    [SerializeField]
    Color normalColor = Color.blue;
    [SerializeField]
    Color selectedColor = Color.yellow;
    [SerializeField]
    Color highlightedColor = Color.red;

    LineRenderer line;

    public BoxCollider Collider { get; set; }

    public ShapesManager Manager { get; set; }

    public override void CleanUp()
    {
        base.CleanUp();
        if (startShape != null)
            startShape.RemoveConnection(this);
        if (finalShape != null)
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
        UpdateConnectionShape();
    }

    protected virtual void UpdateConnectionShape()
    {
        if ((line == null) || (startShapeObj == null) || (finalShapeObj == null))
            return;

        //TODO: Make it as message and save cpu cycles
        if ((line.GetPosition(0) != startShapeObj.transform.position) ||
            (line.GetPosition(line.positionCount - 1) != finalShapeObj.transform.position))
        {
            var startPos = startShapeObj.transform.position;
            startPos.z += 0.1f;
            var endPos = finalShapeObj.transform.position;
            endPos.z += 0.1f;

            line.SetPosition(0, startPos);
            line.SetPosition(line.positionCount - 1, endPos);

            PlaceCollider(startShapeObj.transform.position, finalShapeObj.transform.position, Collider);
        }

        //TODO: Make color changes as subscribtion event (to increase performance when many shapes exists)
        var newColor = normalColor;
        if ((Manager.SelectedShape != null) && (Manager.SelectedShape.gameObject == gameObject))
            newColor = selectedColor;

        line.startColor = line.endColor = newColor;
    }

    public virtual void CreateCollider(GameObject first, GameObject second)
    {
        var startPos = first.transform.position;
        var endPos = second.transform.position;


        var objCollider = new GameObject("Collider");
        BoxCollider col = objCollider.AddComponent<BoxCollider>();
        col.transform.parent = transform; // Collider is added as child object of line

        ShapeSelector selector = CreateShapeSelector(objCollider);
        selector.ParentShape = this;
        selector.Manager = Manager;

        Collider = col;

        PlaceCollider(startPos, endPos, col);
    }

    protected virtual ShapeSelector CreateShapeSelector(GameObject objCollider)
    {
        return objCollider.AddComponent<ShapeSelector>();
    }

    protected static void PlaceCollider(Vector3 startPos, Vector3 endPos, BoxCollider col)
    {
        if (col == null)
            return;

        const float shiftFromEnds = 1.0f;

        Vector3 midPoint = (startPos + endPos) / 2;

        col.transform.position = midPoint + Vector3.forward * .5f; // setting position of collider object

        float lineLength = Vector3.Distance(startPos, endPos); // length of line

        col.size = new Vector3(lineLength - shiftFromEnds, .25f, .25f); // size of collider is set where X is length of line, Y is width of line, Z will be set as per requirement


        var dir = endPos - startPos;
        col.transform.rotation = Quaternion.FromToRotation(Vector3.up, dir);

        var rotation = col.transform.rotation;
        rotation *= Quaternion.Euler(0, 0, 90);
        col.transform.rotation = rotation;
    }

    /// <summary>
    /// Find bounding box containing each connection points
    /// </summary>
    /// <returns></returns>
    internal virtual Rect BoundingBox()
    {
        var bbox = new Rect();

        if (line == null)
            return bbox;

        Vector3[] positions = new Vector3[line.positionCount];
        var positionCount = line.GetPositions(positions);

        var pos = line.GetPosition(0);
        bbox.xMin = pos.x;
        bbox.yMin = pos.y;
        bbox.xMax = pos.x;
        bbox.yMax = pos.y;

        for (int i = 1; i < positions.Length; i++)
        {
            pos = line.GetPosition(i);

            if (bbox.xMin > pos.x) bbox.xMin = pos.x;
            if (bbox.xMax < pos.x) bbox.xMax = pos.x;
            if (bbox.yMin > pos.x) bbox.yMin = pos.y;
            if (bbox.yMax < pos.x) bbox.yMax = pos.y;
        }

        return bbox;
    }

    internal Vector3 GetPosition(int num)
    {
        if (line == null)
        {
            line = GetComponent<LineRenderer>();
            if (line == null)
                return Vector3.zero;
        }

        return line.GetPosition(num);
    }

    internal virtual void AddNode(Vector3 newNode)
    {
        if (line == null)
        {
            line = GetComponent<LineRenderer>();
            if (line == null)
                return;
        }

        //Add new node before last one
        line.positionCount += 1;
        line.SetPosition(line.positionCount - 1, line.GetPosition(line.positionCount - 2));
        line.SetPosition(line.positionCount - 2, newNode);

        //TODO: Add manager shape for node (rectangle)
    }
}
