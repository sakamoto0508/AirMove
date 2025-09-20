using UnityEngine;

public class TutorialGoal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.CompleteTutorial();
        }
    }
}
