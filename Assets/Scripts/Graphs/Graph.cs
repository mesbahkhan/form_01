using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{

  public TextAsset file;
  public GameObject nodePreFab;
  public GameObject edgePreFab; 

  public float width;
  public float length;
  public float height;
  
    void Start()
    {
		
	if (file==null){

		InstantialDefaultGraph();		

      } else {	

		LoadGMLFromFile(
			file);

      }      
    }

    void Update(){}

    void LoadGMLFromFile(
		TextAsset gmlFileName){

      string[] lines = gmlFileName.text.Split('\n');

      int currentobject = -1; // 0 = graph, 1 = node, 2 = edge

      int stage = -1; // 0 waiting to open, 1 = waiting for attribute, 2 = waiting for id, 3 = waiting for label, 4 = waiting for source, 5 = waiting for target

      Nodes node = null;

      Dictionary<string,Nodes> nodes = 
			new Dictionary<string,Nodes>();

      foreach (string line in lines){

		string l = line.Trim();

		string [] words = l.Split(' ');

		foreach(string word in words){

		  if (word == "graph" && stage == -1) {
			currentobject = 0;
		  }

		  if (word == "node" && stage == -1) {
			currentobject = 1;
			stage = 0;	    
		  }

		  if (word == "edge" && stage == -1) {
			currentobject = 2;
			stage = 0;	    
		  }

		  if (word == "[" && stage == 0 && currentobject == 2){
			stage = 1;
		  }

		  if (word == "[" && stage == 0 && currentobject == 1){
			stage = 1;
					node = CreateNode("");			
			continue;
		  }

		  if (word == "]"){
			stage = -1;
		  }

		  if (word == "id" && stage == 1 && currentobject == 1){
			stage = 2;
			continue;
		  }

		  if (word == "label" && stage == 1 && currentobject == 1){
			stage = 3;
			continue;
		  }

		  if (stage == 2){

			nodes.Add(word, node);

			stage = 1;
			break;
		  }

		  if (stage == 3){
			node.SetName(word);			
			stage = 1;
			break;
		  }

		  if (word == "source" && stage == 1 && currentobject == 2){
			stage = 4;
			continue;
		  }

		  if (word == "target" && stage == 1 && currentobject == 2){
			stage = 5;
			continue;
		  }

		  if (stage == 4){
			node = nodes[word];
			stage = 1;
			break;
		  }

		  if (stage == 5){
			node.AddEdge(
				edgePreFab,
				nodes[word]);
			stage = 1;
			break;
		  }
		}
      }
    }
    
	void InstantialDefaultGraph()
    {

        // instantiate A, B, C, D, E
        Nodes AS =
		   CreateNode("node A");

        Nodes BS =
		   CreateNode("node B");

        Nodes CS =
			CreateNode("node C");

        Nodes DS =
			CreateNode("node D");

        Nodes ES =
			CreateNode("node E");
	

		AddEdge(AS, BS);

		AddEdge(AS, CS);

		AddEdge(CS, DS);

		AddEdge(DS, ES);

		AddEdge(DS, AS);

	}

	private Nodes CreateNode(
		string nodeName)
    {

        GameObject node = Instantiate(
                    nodePreFab,
                    new Vector3(
                        Random.Range(-width / 2, width / 2),
                        Random.Range(-length / 2, length / 2),
                        Random.Range(-height / 2, height / 2)),
                    Quaternion.identity);

        // make nodes children of graph object

        node.transform.parent = transform;

		node.name = nodeName;

        // get script instances

        Nodes nodeObject = node.GetComponent<Nodes>();

		return nodeObject;



    }

    private void AddEdge(
		Nodes nodePlace1, 
		Nodes nodePlace2)
    {
        // add edges      

        nodePlace1.AddEdge(
            edgePreFab,
            nodePlace2);
    }
}
