using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodePacket : MonoBehaviour
{
    public PathObject path;
    public int index;

    void Update()
    {
        if (path != null) 
        {
            PacketBehaviour();
        }
    }

    private void PacketBehaviour() 
    {
        //Reached Position, next index
        if ((path.NodesList[index].transform.position - this.transform.position).magnitude < 0.15f)
        {
            index++;

            //Is this the end point
            if (path.NodesList.Count == index) 
            {
                GameObject.Destroy(this.gameObject);
            }
        }
        else //Move towards next
        {
            transform.position = Vector3.MoveTowards(transform.position, path.NodesList[index].transform.position, 0.1f);
        }
    }

}
