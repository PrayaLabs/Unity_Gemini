using UnityEngine;

public class AnswerObject : MonoBehaviour
{
    [HideInInspector] public QuizManager manager;
    [HideInInspector] public int answerIndex;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            manager.CheckAnswer(answerIndex);
        }
    }
}