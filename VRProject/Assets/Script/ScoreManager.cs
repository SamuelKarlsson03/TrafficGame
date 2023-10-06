using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public enum ImpactZone
{
    Front,
    Middle,
    Rear,
}




public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance; 
    public float currentScore;
    public float scoreAdded;
    public float highScore;
    private int currentCombo;
    private float comboTimer;
    [SerializeField] GameObject textObject;
    [SerializeField] Transform worldCanvas;

    public delegate void UpdateScoreDelegate(float score);
    public event UpdateScoreDelegate updateScoreEvent;
    

    private void Awake() //Singleton instance 
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void Start()
    {
        highScore = PlayerPrefs.GetFloat("Highscore");
        Debug.Log("Highscore : " + highScore);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UpdateScore(100);
        }
        comboTimer -= Time.deltaTime;
        if(comboTimer <= 0)
        {
            currentCombo = 0;
        }
    }


    public float AddScore(Vector3 driverVelocity, Vector3 victimVelocity, float driverMass, float victimMass)
    {
        //force on collision math
       float scoreToAdd = ((driverVelocity * driverVelocity.magnitude * Mathf.Pow(driverMass,0.33f)) - (victimVelocity *victimVelocity.magnitude * Mathf.Pow(victimMass, 0.33f))).magnitude;
        UpdateScore(scoreToAdd);
        return scoreToAdd;
    }

    public void AddScore(float amount, Vector3 point)
    {
        currentCombo++;
        comboTimer = 2f;
        UpdateScore(amount * currentCombo);
        GameObject text = Instantiate(textObject, worldCanvas);
        if(currentCombo > 1)
        {
            text.GetComponent<TMPro.TextMeshProUGUI>().text = Mathf.Floor(amount * currentCombo) + "(" + currentCombo + "x)";
        }
        else
        {
            text.GetComponent<TMPro.TextMeshProUGUI>().text = Mathf.Floor(amount * currentCombo) + "";
        }
        text.transform.position = point + Random.insideUnitSphere + Random.insideUnitSphere;
        Destroy(text,5f);
    }

   private void UpdateScore(float scoreAdded)
    {
        currentScore += scoreAdded;
        PlayerPrefs.SetFloat("CurrentScore", currentScore);
        if (updateScoreEvent != null)
        {
            updateScoreEvent.Invoke(currentScore);
        }

        if (currentScore >= highScore)
        {
            PlayerPrefs.SetFloat("Highscore", currentScore);
            Debug.Log("New Highscore : " + currentScore);
        }
    }
}
