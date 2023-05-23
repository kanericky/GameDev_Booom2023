using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransferZone : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Env"))
        {
            collision.transform.position = new Vector3(
                collision.transform.position.x + 300, 
                collision.transform.position.y, 
                collision.transform.position.z);
        }
    }
}
