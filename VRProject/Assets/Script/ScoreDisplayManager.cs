using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreDisplayManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI displayText;
    [SerializeField] float textSpeed = 0.1f;
    [SerializeField] float textMoveAmount = 1f;

    [SerializeField] GameObject textObject;



    Vector3 startPos;
    Vector3 endPos;

    private float lastScore;


    void Start()
    {
        ScoreManager.Instance.updateScoreEvent += UpdateScoreText;
        GetPos();
        StartCoroutine(ScrollText());
    }

    private void Update()
    {
        if (ScoreManager.Instance.currentScore != lastScore)
        {
            //displayText.text =
           // lastScore = ScoreManager.Instance.currentScore;
        }
    }

    IEnumerator ScrollText()
    {
        textObject.transform.localPosition = startPos;
        while(textObject.transform.localPosition.x >= endPos.x)
        {
            textObject.transform.localPosition -= new Vector3(textMoveAmount, 0, 0)* Time.deltaTime;
            yield return new WaitForSeconds(textSpeed);
        }

        StartCoroutine(ScrollText());
    }

    private void GetPos()
    {
        endPos = transform.localPosition + new Vector3(-400, 0, 0);
        startPos = transform.localPosition + new Vector3(400, 0, 0);
    }

    private void UpdateScoreText(float score)
    {
        displayText.text = "SCORE : " + score.ToString("0000000000"); 
    }
    


}
