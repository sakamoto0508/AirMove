using UnityEngine;

public class TutorialGoal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag==("Player"))
        {
            GameManager.Instance.CompleteTutorial();
        }
    }

    public void Goal()
    {
        GameManager.Instance.CompleteTutorial();
    }
}
