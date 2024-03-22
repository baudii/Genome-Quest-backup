using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TutorialComponent : MonoBehaviour
{
    [SerializeField]
    GameObject[] tutorialGameobjectsOrdered;

    private void OnEnable()
    {
        StartCoroutine(DisplayTutorial());
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator DisplayTutorial()
    {
        foreach (var go in tutorialGameobjectsOrdered)
        {
            yield return null;
            go.SetActive(true);
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
            go.SetActive(false);
        }

        foreach (var go in tutorialGameobjectsOrdered)
            Destroy(go);
        Destroy(gameObject);
    }
}


