using UnityEngine;
using TMPro;

public class QuizManager : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text questionText;
    public TMP_Text scoreText;

    [Header("Questions (Type in Inspector)")]
    public string[] questions = new string[8];

    [Header("Answer Objects (Drag 8 Objects Here)")]
    public GameObject[] answerObjects = new GameObject[8];

    private int currentIndex = 0;
    private int score = 0;

    void Start()
    {
        SetupAnswers();
        UpdateUI();
    }

    void SetupAnswers()
    {
        if (questions.Length != answerObjects.Length)
        {
            Debug.LogError("Questions and Answer Objects must be same size!");
        }

        for (int i = 0; i < answerObjects.Length; i++)
        {
            if (answerObjects[i] == null) continue;

            AnswerObject ao = answerObjects[i].GetComponent<AnswerObject>();
            if (ao == null)
                ao = answerObjects[i].AddComponent<AnswerObject>();

            ao.manager = this;
            ao.answerIndex = i;
        }
    }

    public void CheckAnswer(int hitIndex)
    {
        if (hitIndex == currentIndex)
        {
            score++;
            scoreText.text = "Score: " + score;

            currentIndex++;

            if (currentIndex >= questions.Length)
            {
                questionText.text = "Game Finished!";
                return;
            }

            questionText.text = questions[currentIndex];
        }
        else
        {
            Debug.Log("Wrong Object!");
        }
    }

    void UpdateUI()
    {
        if (questions.Length > 0)
            questionText.text = questions[currentIndex];

        scoreText.text = "Score: 0";
    }
}