using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphCamera : MonoBehaviour
{

  public GameObject graph;
  public float speed = 15.0f;
  
    void Start(){}

    void Update()
    {
      // find centre of the graph

      float minx = 1000000.0f; float maxx = 0.0f;
      float miny = 1000000.0f; float maxy = 0.0f;
      float minz = 1000000.0f; float maxz = 0.0f;
        
      foreach (Transform child in graph.transform){
        if (child.position.x < minx) minx = child.position.x;
        if (child.position.x > maxx) maxx = child.position.x;
        if (child.position.y < miny) miny = child.position.y;
        if (child.position.y > maxy) maxy = child.position.y;
        if (child.position.z < minz) minz = child.position.z;
        if (child.position.z > maxz) maxz = child.position.z;			
      }
      transform.LookAt(new Vector3((minx+maxx)/2,(miny+maxy)/2,(minz+maxz)/2));

        if (Input.GetKey("w")){
	            transform.position += new Vector3(0,0,Time.deltaTime*speed);
        }

        if (Input.GetKey("s")){
	        transform.position -= new Vector3(0,0,Time.deltaTime*speed);
        }

        if (Input.GetKey("up"))
        {
            transform.position += new Vector3(0, Time.deltaTime * speed, 0);
        }

        if (Input.GetKey("down"))
        {
            transform.position -= new Vector3(0, Time.deltaTime * speed, 0);
        }

        if (Input.GetKey("right"))
        {
            transform.position += new Vector3(Time.deltaTime * speed, 0, 0);
        }

        if (Input.GetKey("left"))
        {
            transform.position -= new Vector3(Time.deltaTime * speed, 0, 0);
        }

    }
    
}
