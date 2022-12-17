using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Used for connections between shapes.
/// 
/// When user clicks on shape (and manager in connection state) this class shows
/// selection start and draw line to mouse from current selected shape.
/// 
/// After connection was made connector is disabled.
/// And user can cancel connection by right mouse button and ESC key
/// </summary>
public class ShapeConnector : MonoBehaviour
{
    [SerializeField]
    LineRenderer line;
    [SerializeField]
    Color selectedColor = Color.yellow;
    [SerializeField]
    Color normalColor = Color.white;

    ShapesManager manager;
    [SerializeField]
    Camera frontCamera;
    [SerializeField]
    Camera layersCamera;
    [SerializeField]
    string frontCameraTag = "FrontCam";
    [SerializeField]
    string layersCameraTag = "LayersCam";

    SpriteRenderer sprite;
    BoxCollider collider;
    Shape shape;

    bool selected = false;

    void Start()
    {
        manager = FindObjectOfType<ShapesManager>();
        if (frontCamera == null)
            frontCamera = Camera.allCameras.Where(c => c.CompareTag(frontCameraTag)).FirstOrDefault();
        if (layersCamera == null)
            layersCamera = Camera.allCameras.Where(c => c.CompareTag(layersCameraTag)).FirstOrDefault();

        sprite = GetComponent<SpriteRenderer>();
        collider = GetComponent<BoxCollider>();
        shape = GetComponent<Shape>();
    }

    void Update()
    {
       if(
           (manager != null)&&
           (manager.State == ShapesManager.ManagerState.Connecting))
        {
            var screenPosition = frontCamera.WorldToScreenPoint(transform.position);

            var pos = frontCamera.ScreenToWorldPoint(
                new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPosition.z));

            if (!CheckConnection(pos))//check layers camera
            {
                screenPosition = layersCamera.WorldToScreenPoint(transform.position);

                pos = layersCamera.ScreenToWorldPoint(
                    new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPosition.z));
                CheckConnection(pos);
            }

            if (manager.ConnectionState == ShapesManager.ConnectingState.SelectedSecond)
            {
                if (Input.GetMouseButtonUp(0))//released when manager selected connections
                {
                    if(manager.SecondConnectionShape == gameObject)
                        manager.FinishConnection();

                    selected = false;
                    line.enabled = false;
                }
            }

            if (manager.ConnectionState == ShapesManager.ConnectingState.SelectedFirst)
            {
                if (manager.FirstConnectionShape != gameObject)
                {
                    if (Input.GetMouseButton(0) && PosInsideCollider(pos))
                    {
                        manager.SetSecondConnection(gameObject);
                    }
                }
                else
                {
                    if (selected && Input.GetMouseButtonUp(0))//released mouse on first
                    {
                        selected = false;
                        line.enabled = false;
                        manager.SetToMovingShapes();
                    }
                }
            }

            if (manager.ConnectionState == ShapesManager.ConnectingState.Idle)
            {
                if (Input.GetMouseButton(0) && PosInsideCollider(pos)) //left button held inside shape
                {
                    manager.SelectFirstConnection(gameObject);
                    selected = true;
                    line.enabled = true;
                }
            }

            sprite.color = selected ? selectedColor : normalColor;

            if (selected)
            {
                pos.z = line.GetPosition(1).z;

                line.SetPosition(0, transform.position);
                line.SetPosition(1, pos);         
            }
        }
    }

    private bool CheckConnection(Vector3 pos)
    {
        if (manager.ConnectionState == ShapesManager.ConnectingState.SelectedSecond)
        {
            if (Input.GetMouseButtonUp(0))//released when manager selected connections
            {
                Reset();

                if (manager.SecondConnectionShape == gameObject)
                {
                    manager.FinishConnection();
                    return true;
                }
            }
        }

        if (manager.ConnectionState == ShapesManager.ConnectingState.SelectedFirst)
        {
            if (manager.FirstConnectionShape != gameObject)
            {
                if (Input.GetMouseButton(0) && PosInsideCollider(pos))
                {
                    manager.SetSecondConnection(gameObject);
                    return true;
                }
            }
            else
            {
                if (selected && Input.GetMouseButtonUp(0))//released mouse on first
                {
                    Reset();

                    manager.SetToMovingShapes();
                }
            }
        }

        if (manager.ConnectionState == ShapesManager.ConnectingState.Idle)
        {
            if (Input.GetMouseButton(0) && PosInsideCollider(pos)) //left button held inside shape
            {
                manager.SelectFirstConnection(gameObject);
                selected = true;
                //line.enabled = true;
                return true;
            }
            else
            {
                Reset();
            }
        }
        return false;
    }

    private bool PosInsideCollider(Vector3 pos)
    {
        return (pos.x > transform.position.x - collider.bounds.extents.x) &&
               (pos.x < transform.position.x + collider.bounds.extents.x) &&
               (pos.y > transform.position.y - collider.bounds.extents.y) &&
               (pos.y < transform.position.y + collider.bounds.extents.y);
    }

    internal void Reset()
    {
        selected = false;
        line.enabled = false;
    }
}
