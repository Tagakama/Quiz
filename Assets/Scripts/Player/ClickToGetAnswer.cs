using QuestionSystem;
using UnityEngine;

public class ClickToGetAnswer : MonoBehaviour
{
    [SerializeField] private AnswerHandler _answerHandler;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    QuestObject questObject = hit.collider.GetComponent<QuestObject>();
                    _answerHandler.CheckAnswer(questObject.NameObject,questObject.SpriteRenderer);
                }
            }
            else
            {
                Debug.Log("Clicked on empty space");
            }
        }
    }
}