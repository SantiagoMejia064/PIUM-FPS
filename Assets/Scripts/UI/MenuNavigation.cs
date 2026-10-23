using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuNavigation : MonoBehaviour
{
    public GameObject firstSelectedButton;

    public bool IsInteractable;

    private Coroutine selectionCoroutine;
    
    private void OnEnable(){
        selectionCoroutine = StartCoroutine(selectFirstButton());
    }

    private void OnDisable(){
        if (selectionCoroutine != null){
            StopCoroutine(selectionCoroutine);
            selectionCoroutine = null;
        }
    }

    private IEnumerator selectFirstButton()
    {
        while(true)
        {
            yield return null;

            if (EventSystem.current == null)
            {
                continue;
            }

            if (firstSelectedButton == null)
            {
                yield break;
            }

            if (!firstSelectedButton.activeInHierarchy)
            {
                continue;
            }

            Selectable selectable = firstSelectedButton.GetComponent<Selectable>();

            if (selectable == null)
            {
                yield break;
            }

            if (!selectable.interactable)
            {
                if(IsInteractable)
                {
                    continue;
                }
                yield break;
            }

            EventSystem.current.SetSelectedGameObject(null);

            EventSystem.current.SetSelectedGameObject(firstSelectedButton);

            yield break;
        }
    }
}
