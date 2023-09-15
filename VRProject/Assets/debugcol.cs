using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class debugcol : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        Debug.Log(collision.transform.root.name);
    }
}
