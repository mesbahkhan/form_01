using System.Collections.Generic;
using UnityEngine;

public class ShapesManager : MonoBehaviour
{
    [SerializeField]
    GameObject activePlane;
    [SerializeField]
    ClassShape classShapePrefab;
    [SerializeField]
    ConnectionShape connectionShapePrefab;


    [SerializeField]
    List<Shape> shapes;

    Shape selectedShape;

    List<Shape> dispatchedToDeletionShapes = new List<Shape>();

    public enum ConnectorType
    {
        Unknown,
        Link,
        Aggregation,
        Composition,
        Generalization
    }

    internal void DispatchDeletion(Shape item)
    {
        dispatchedToDeletionShapes.Add(item);
    }

    internal void DeleteDispatched()
    {
        for (int i = 0; i < dispatchedToDeletionShapes.Count; i++)
        {
            Destroy(dispatchedToDeletionShapes[i]);
        }
        dispatchedToDeletionShapes.Clear();
    }

    ConnectorType connectorType;

    public enum ManagerState
    {
        Unknown,
        Moving,
        Connecting
    }

    ManagerState managerState = ManagerState.Moving;

    public ManagerState State => managerState;

    public enum ConnectingState
    {
        Unknown,
        Idle,
        SelectedFirst,
        SelectedSecond
    }

    ConnectingState connectionState = ConnectingState.Idle;
    public ConnectingState ConnectionState => connectionState;

    private GameObject firstConnectionShape;
    public GameObject FirstConnectionShape => firstConnectionShape;


    private GameObject secondConnectionShape;
    public GameObject SecondConnectionShape => secondConnectionShape;


    private void Start()
    {
        shapes = new List<Shape>();
    }

    public void CreateClassShape()
    {
        var newShapeObj = GameObject.Instantiate(classShapePrefab, activePlane.transform);

        var newShape = newShapeObj.GetComponent<Shape>();

        shapes.Add(newShape);

        newShape.name = $"ClassShape{shapes.Count}";
    }

    public void SetLinkConnectorState()
    {
        connectorType = ConnectorType.Link;
    }

    public void SetToConnectingShapes()
    {
        managerState = ManagerState.Connecting;
    }

    internal void SelectFirstConnection(GameObject firstShape)
    {
        connectionState = ConnectingState.SelectedFirst;
        firstConnectionShape = firstShape;
    }

    internal void SetSecondConnection(GameObject secondShape)
    {
        connectionState = ConnectingState.SelectedSecond;
        secondConnectionShape = secondShape;
    }

    internal void FinishConnection()
    {
        var newConnection = Instantiate(connectionShapePrefab, parent: activePlane.transform);

        newConnection.SetStartShape(firstConnectionShape);

        newConnection.SetFinalShape(secondConnectionShape);

        newConnection.CreateCollider(firstConnectionShape, secondConnectionShape);

        newConnection.Manager = this;

        SetToMovingShapes();
    }
    

    internal void SetToMovingShapes()
    {
        managerState = ManagerState.Moving;
        connectionState = ConnectingState.Idle;

        firstConnectionShape = null;
        secondConnectionShape = null;
    }

    public void DeleteSelectedShape()
    {
        DeleteShape(selectedShape);
    }

    internal void DeleteShape(Shape shape)
    {
        shape.CleanUp();
        Destroy(shape.gameObject);
    }

    internal void SelectShape(Shape shape)
    {
        selectedShape = shape;
    }
}
