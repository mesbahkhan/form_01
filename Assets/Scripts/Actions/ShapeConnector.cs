using System.Collections;
using System.Collections.Generic;
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
    Camera mainCamera;
    SpriteRenderer sprite;
    BoxCollider collider;

    bool selected = false;

    void Start()
    {
        manager = FindObjectOfType<ShapesManager>();
        mainCamera = Camera.main;
        sprite = GetComponent<SpriteRenderer>();
        collider = GetComponent<BoxCollider>();
    }

    void Update()
    {
       if(
           (manager != null)&&
           (manager.State == ShapesManager.ManagerState.Connecting))
        {
            var screenPosition = mainCamera.WorldToScreenPoint(transform.position);

            var pos = mainCamera.ScreenToWorldPoint(
                new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPosition.z));

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

    private bool PosInsideCollider(Vector3 pos)
    {
        return (pos.x > transform.position.x - collider.bounds.extents.x) &&
               (pos.x < transform.position.x + collider.bounds.extents.x) &&
               (pos.y > transform.position.y - collider.bounds.extents.y) &&
               (pos.y < transform.position.y + collider.bounds.extents.y);
    }
}
