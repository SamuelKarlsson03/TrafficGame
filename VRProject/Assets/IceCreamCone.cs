using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceCreamCone : MonoBehaviour
{

    Rigidbody rigidbody;

    [SerializeField] bool getBlastedUp = true;
    [SerializeField] float launchPower = 10f;
    [SerializeField] float horizontalPower = 3f;

    [SerializeField] GameObject cone;
    [SerializeField] GameObject iceCream;

    AudioClip sploshAudioClip;

    float randomDirection;
    void Start()
    {
        Color randomConeColor = new Color(Random.value, Random.value, Random.value);

        Color randomIceCreamColor = new Color(Random.value, Random.value, Random.value);

        rigidbody = GetComponent<Rigidbody>();

        cone.GetComponent<Renderer>().material.color = randomConeColor;
        iceCream.GetComponent<Renderer>().material.color = randomIceCreamColor;

        if (getBlastedUp)
        {
            GetBlastedUp();
        }

        Destroy(gameObject, 10f);
    }


    private void GetBlastedUp()
    {
        launchPower *= Random.Range(0.9f, 1.1f) * 0.1f;

        float forceX = Random.Range(-horizontalPower, horizontalPower) * 0.1f;
        float forceZ = Random.Range(-horizontalPower, horizontalPower) * 0.1f;

        Vector3 forceVector = new Vector3(forceX, 1f * launchPower, forceZ);

        rigidbody.AddForce(forceVector, ForceMode.Impulse);

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Car"))
        {
            Destroy(this.gameObject);

            if (sploshAudioClip != null)
            {
                SoundManager.Instance.PlayAudioOnLocation(sploshAudioClip, transform.position);
            }
        }
    }


}
