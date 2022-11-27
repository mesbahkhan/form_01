using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nodes : MonoBehaviour
{

  GameObject edgePreFab;

  List<GameObject>  edges  = new List<GameObject> ();
  List<SpringJoint> joints = new List<SpringJoint>();  
  
  void Start(){
    transform.GetChild(0).GetComponent<TextMesh>().text = name;
  }
  
  void Update(){
        
    int index = 0;

    foreach (GameObject edge in edges){

      edge.transform.position = 
                new Vector3(
                    transform.position.x,
                    transform.position.y, 
                    transform.position.z);

      SpringJoint springJoint = joints[index];

      GameObject target = 
                springJoint.
                    connectedBody.
                        gameObject;

      edge.transform.LookAt(target.transform);

      Vector3 localScale = 
                edge.transform.localScale;

      localScale.z = Vector3.Distance(
          transform.position, 
          target.transform.position);

      edge.transform.localScale = localScale;

      edge.transform.position = 
                new Vector3(
                    (transform.position.x+target.transform.position.x)/2,
                    (transform.position.y+target.transform.position.y)/2,
                    (transform.position.z+target.transform.position.z)/2);
      index++;
    }
  }

  public void SetEdgePrefab(GameObject edgePreFab){
    this.edgePreFab = edgePreFab;
  }
  
  public void AddEdge(Nodes node){

    SpringJoint springJoint = 
            gameObject.AddComponent<SpringJoint> ();  

    springJoint.autoConfigureConnectedAnchor = 
            false;

    springJoint.anchor = 
            new Vector3(0,0.5f,0);

    springJoint.connectedAnchor = 
            new Vector3(0,0,0);    

    springJoint.enableCollision = 
            true;

    springJoint.connectedBody = node.
            GetComponent<Rigidbody>();

    GameObject edge = Instantiate(
        this.edgePreFab, 
        new Vector3(
            transform.position.x, 
            transform.position.y, 
            transform.position.z), 
        Quaternion.identity);

    edges.Add(edge);

    joints.Add(springJoint);
  }
    
}
