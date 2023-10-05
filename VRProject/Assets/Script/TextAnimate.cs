using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class TextAnimate : MonoBehaviour
{
    [SerializeField] float floatSpeed;
    Transform playerposition;
    // Start is called before the first frame update
    void Start()
    {
        playerposition = GameObject.Find("Left Controller").transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.up * Time.deltaTime * floatSpeed;
        transform.forward = (transform.position-playerposition.position).normalized;
    }
}
