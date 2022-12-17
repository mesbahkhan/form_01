using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomAndPanCamera : MonoBehaviour
{
    [SerializeField]
    Camera panCamera;
    [SerializeField]
    float zoomSpeed = 12.0f;
    [SerializeField]
    float panSpeed = 2.0f;
    private Vector3 oldMousePos;
    private float startOrtoSize;

    void Start()
    {
        startOrtoSize = panCamera.orthographicSize;        
    }

    void Update()
    {
        panCamera.orthographicSize += Input.mouseScrollDelta.y * Time.deltaTime * zoomSpeed;

        var delta = new Vector3();
        if (Input.GetKey(KeyCode.LeftArrow))
            delta.x = -panSpeed;
        else if (Input.GetKey(KeyCode.RightArrow))
            delta.x = panSpeed;
        else if (Input.GetKey(KeyCode.UpArrow))
            delta.y = panSpeed;
        else if (Input.GetKey(KeyCode.DownArrow))
            delta.y = -panSpeed;

        panCamera.transform.Translate(delta*Time.deltaTime);

        if (Input.GetMouseButtonUp(2))//mid up - reset zoom
            panCamera.orthographicSize = startOrtoSize;
    }
}
