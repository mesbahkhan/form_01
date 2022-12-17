using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShapeMover : MonoBehaviour
{
    [SerializeField]
    Color normalColor = Color.white;

    [SerializeField]
    Color highlightColor = Color.yellow;

    Camera frontCamera;
    SpriteRenderer sprite;

    string frontCameraTag = "FrontCam";

    bool isMouseOver = false;
    bool isDragging = false;

    ShapesManager shapeManager;

    void Start()
    {
        Debug.Log("Shape Mover Starting");

        if (frontCamera == null)
            frontCamera = 
                Camera.allCameras.Where(
                    c => c.CompareTag(frontCameraTag)).
                        FirstOrDefault();

        sprite = GetComponent<SpriteRenderer>();       

        shapeManager = FindObjectOfType<ShapesManager>();
    }

    void Update()
    {
        bool canMove = true;

        if(shapeManager != null)
        {
            canMove = 
                shapeManager.State == ShapesManager.ManagerState.Moving;
        }

        if (canMove)
        {
            ManageShapeMovement();
        }
    }

    private void ManageShapeMovement()
    {
        if (isDragging)
        {
            Debug.Log("Shape Mover in Dragging");
            var screenPosition = 
                frontCamera.
                    WorldToScreenPoint(
                        transform.position);

             var currentPosition = frontCamera.ScreenToWorldPoint(
                new Vector3(
                        Input.mousePosition.x, 
                        Input.mousePosition.y, 
                        screenPosition.z));

            transform.position = currentPosition;

            if (!Input.GetMouseButton(0))
                isDragging = false;

            sprite.color = 
                isDragging ?
                    highlightColor : normalColor;
        }

        else
        {
            isDragging = 
                Input.GetMouseButton(0) && 
                isMouseOver;
        }
    }

    private void OnMouseOver()
    {
        isMouseOver = true;
    }

    private void OnMouseExit()
    {
        isMouseOver = false;
    }
}
