using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VFX_controller : MonoBehaviour
{
    [SerializeField] GameObject explosion, iceCreamExplosion;
    public Transform explosionSpawn;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Vehicle"))
        {
            Destroy(this.gameObject);

            Explosion();
        }
    }
    public void Explosion()
    {
        GameObject newExplosion = Instantiate(explosion, explosionSpawn.transform.position, Quaternion.identity);
        Destroy(newExplosion, 3f);
    }

    /*
    public void IceCreamExplosion()
    {
        GameObject newIceCreamExplosion = Instantiate(iceCreamExplosion, transform.position, Quaternion.identity);
        Destroy(newIceCreamExplosion, 3f);
    }
    */
}
