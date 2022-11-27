using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeMover : MonoBehaviour
{
    [SerializeField]
    Color normalColor = Color.yellow;

    [SerializeField]
    Color highlightColor = Color.green;

    Camera mainCamera;
    SpriteRenderer sprite;

    bool isMouseOver = false;
    bool isDragging = false;

    ShapesManager shapeManager;

    void Start()
    {
        Debug.Log("Shape Mover Starting");
        mainCamera = Camera.main;

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
                mainCamera.
                    WorldToScreenPoint(
                        transform.position);

             var currentPosition = mainCamera.ScreenToWorldPoint(
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
