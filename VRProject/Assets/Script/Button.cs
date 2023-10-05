using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{

    [SerializeField] GameObject leftHand;
    [SerializeField] GameObject rightHand;
    [SerializeField] Vector3 offset;
    [SerializeField] float range;
    [SerializeField] Transform topPos;
    [SerializeField] Transform bottomPos;
    [SerializeField] float riseSpeed;

    [SerializeField] bool isResetButton = false;

    [SerializeField] bool isMoveonButton = false;

    private void Start()
    {

    }



    private void Update()
    {

        float leftHeight = -1000f;
        float rightHeight = -1000f;


        if (Vector3.Distance(leftHand.transform.position, transform.position) < range)
        {
            leftHeight = leftHand.transform.position.y + offset.y;
        }

        if (Vector3.Distance(rightHand.transform.position, transform.position) < range)
        {
            rightHeight = rightHand.transform.position.y + offset.y;
        }
        bool shouldRise = true;
        if (leftHeight + rightHeight > -1999f)
        {
            float y = 0;
             
            if (leftHeight > rightHeight)
            {
                y = Mathf.Clamp(leftHeight, bottomPos.position.y, topPos.position.y);
            }

            if (rightHeight > leftHeight)
            {
                y = Mathf.Clamp(rightHeight, bottomPos.position.y, topPos.position.y);
            }
            if(y <= transform.position.y)
            {
                shouldRise = false;
                if(y <= bottomPos.position.y)
                {
                    ButtonPressed();
                }
                transform.position = new Vector3(transform.position.x, y, transform.position.z);
            }
        }
        if(shouldRise)
        {
            transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y + riseSpeed * Time.deltaTime, bottomPos.position.y, topPos.position.y), transform.position.z);
        }

    }

    public void ButtonPressed()
    {
        if (isResetButton)
        {
            SceneManager.LoadScene("Intro");
        }

        if (isMoveonButton)
        {
            SceneManager.LoadScene("NewGameScene");
        }
    }

}
