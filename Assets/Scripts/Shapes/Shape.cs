using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape : MonoBehaviour
{
    List<ConnectionShape> connections;
    ShapesManager manager;

    private void Awake()
    {
        connections = new List<ConnectionShape>();

        manager = FindObjectOfType<ShapesManager>();
    }

    public void AddConnection(ConnectionShape connection)
    {
        connections.Add(connection);
    }

    public virtual void CleanUp()
    {
        foreach (var item in connections)
        {
            item.CleanUp();
            manager.DispatchDeletion(item);
        }

        manager.DeleteDispatched();

        connections.Clear();
    }

    internal void RemoveConnection(ConnectionShape connection)
    {
        connections.Remove(connection);
    }
}
