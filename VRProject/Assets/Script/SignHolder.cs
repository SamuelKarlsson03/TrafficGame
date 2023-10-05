using UnityEngine;

public class SignHolder : MonoBehaviour
{
    [SerializeField] private GameObject signPrefab;
    [SerializeField] private float spawnDelay = 2.5f;
    private GameObject currentSign;

    private void Start()
    {
        SpawnSign();
    }

    private void SpawnSign()
    {
        currentSign = Instantiate<GameObject>(signPrefab, transform.position, transform.rotation);
        currentSign.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
        currentSign.transform.parent = transform;
    }

    private void FixedUpdate()
    {
        if (currentSign != null)
        {
            if (currentSign.GetComponent<SignControls>().held)
            {
                currentSign.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            }

            if (!currentSign.GetComponent<SignControls>().held)
            {
                currentSign.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
                currentSign.transform.position = transform.position;
                currentSign.transform.rotation = transform.rotation;
            }
         }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == currentSign)
        {
            currentSign.transform.parent = null;
            currentSign = null;
            Invoke(nameof(SpawnSign), spawnDelay);
        }
    }
}

//Despawn timer quicker (done)
//can spawn signs by hitting with a sing (Fixed)
//spawn and despawn anim
//spawn delay (done)