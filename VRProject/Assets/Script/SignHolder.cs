using UnityEngine;

public class SignHolder : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    private GameObject childSign;

    private void Start()
    {
        //childSign = GetComponentInChildren<SignManager>().gameObject;
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if(other.CompareTag("Sign"))
        {
            childSign.SetActive(false);
            this.GetComponent<MeshFilter>().mesh = other.gameObject.GetComponent<MeshFilter>().mesh;

            meshRenderer.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Sign"))
        {
            childSign.SetActive(true);
            meshRenderer.enabled = false;
        }
    }
}
