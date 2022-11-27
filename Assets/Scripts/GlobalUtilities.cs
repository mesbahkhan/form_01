using UnityEngine;


public static class GlobalUtilities {

    public static bool ReportIssue()
        {
            Debug.Log("issue at mouse position");
            Debug.Log(Input.mousePosition);

            return true;

        }

    public static GameObject ReturnClickedObject(out RaycastHit hit)
    {
        GameObject targetObject = null;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        bool targetWasHit = Physics.Raycast(
            ray.origin,
            ray.direction * 10,
            out hit);

        if (targetWasHit)
        {
            targetObject = hit.collider.gameObject;
        }
        return targetObject;
    }

}