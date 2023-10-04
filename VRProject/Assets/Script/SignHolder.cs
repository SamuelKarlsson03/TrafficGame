using UnityEngine;

public class SignHolder : MonoBehaviour
{
    [SerializeField] private GameObject signPrefab;
    private GameObject currentSign;

    private void Start()
    {
         SpawnSign();
    }

    private void SpawnSign()
    {
        currentSign = Instantiate<GameObject>(signPrefab, transform.position, transform.rotation, transform);
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == currentSign)
        {
            currentSign.transform.parent = null;
            currentSign = null;
            SpawnSign();
        }
    }
}
