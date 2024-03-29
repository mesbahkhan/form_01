using UnityEngine;

public class BoxBehaviour : MonoBehaviour
{
    // Start is called before the first frame update

    GameObject target;
    public Vector3 screenPosition;
    public Vector3 offset;
    public bool isMouseDragging;
    RaycastHit hit;
    Ray ray;
    public GameObject prefab;

    public int lengthOfLineRenderer = 10;

    public GameObject OtherGameObject;
    LineRenderer line;

    public float lineWidth = 0.01f;

    void Start()
    {
        Debug.Log("Box Behaviour Starting");
        isMouseDragging = false; 

        if (gameObject.GetComponent<LineRenderer>() == null)        {
            

            line = gameObject.AddComponent<LineRenderer>();

            line.material = new Material(Shader.Find("Sprites/Default"));

            line.widthMultiplier = 0.1f;

            line.positionCount = lengthOfLineRenderer;

            line.SetPosition(0, gameObject.transform.position);                

        }

        if (OtherGameObject == null)
        {
            Debug.LogWarning("Please Attach Other GameObject in inspector");
            return;
        }

        line = GetComponent<LineRenderer>();

        AnimationCurve curve = new AnimationCurve();
        curve.AddKey(0, lineWidth);
        curve.AddKey(1, lineWidth);

        line.widthCurve = curve;

        
        line.useWorldSpace = true;

        line.positionCount = 2;

        line.SetPosition(0, gameObject.transform.position);
        line.SetPosition(1, OtherGameObject.transform.position);


    }
    
    void Update()
    {

        line.SetPosition(0, gameObject.transform.position);

        line.SetPosition(1, OtherGameObject.transform.position);

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))

        {
            Debug.Log("Managing MouseDown");
            ManageMouseDownBehaviour();
        }

        if (isMouseDragging & 
            target != null & 
            target.transform.parent !=null)
        {
            Debug.Log("Managing Dragging");
            ManageDragginBehaviour();
        }

        if (Input.GetMouseButtonUp(0))
        {
            isMouseDragging = false;
        }

        // Unlock and show cursor when right mouse button released
        if (Input.GetMouseButtonUp(1))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

    }

    private void ManageDragginBehaviour()
    {
        if (target.transform.parent.name == gameObject.name)
        {

            //tracking mouse position.
            Vector3 currentScreenSpace = 
                new Vector3(
                    Input.mousePosition.x, 
                    Input.mousePosition.y, 
                    screenPosition.z);

            //convert screen position to world position with offset changes.
            Vector3 currentPosition = Camera.main.ScreenToWorldPoint(currentScreenSpace) + offset;

            //It will update target gameobject's current postion.
            gameObject.transform.position = currentPosition;

        }
    }

    private void ManageMouseDownBehaviour()
    {
        isMouseDragging = true;
        RaycastHit hitInfo;

        target = GlobalUtilities.ReturnClickedObject(out hitInfo);

        if (target is null)
        {

        }
        else
        {

            bool targetObjectIsMe = 
                target == gameObject;

            if (targetObjectIsMe)
            {
                //add code when Box is hit
                Debug.Log("box hit!!");
            }

            //Convert world position to screen position.
            screenPosition = Camera.main.WorldToScreenPoint(target.transform.position);

            offset =
                 target.transform.position -
                 Camera.main.ScreenToWorldPoint(
                     new Vector3(
                         Input.mousePosition.x,
                         Input.mousePosition.y,
                         screenPosition.z));

        }
    }

}
