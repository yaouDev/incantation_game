using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Autofill : MonoBehaviour
{
    public InputField inputField;
    public RectTransform resultsParent;
    public RectTransform prefab;

    private IncantationManager im;

    private void Awake()
    {
        inputField.onValueChanged.AddListener(OnInputValueChanged);
        im = IncantationManager.instance;
    }

    private void OnInputValueChanged(string newText)
    {
        ClearResults();
        FillResults(GetResults(newText));
    }

    public void ClearResults()
    {
        // Reverse loop since you destroy children
        for (int childIndex = resultsParent.childCount - 1; childIndex >= 0; --childIndex)
        {
            Transform child = resultsParent.GetChild(childIndex);
            child.SetParent(null);
            Destroy(child.gameObject);
        }
    }

    private void FillResults(List<string> results)
    {
        for (int resultIndex = 0; resultIndex < results.Count; resultIndex++)
        {
            RectTransform child = Instantiate(prefab) as RectTransform;
            child.GetComponentInChildren<Text>().text = results[resultIndex];
            child.SetParent(resultsParent);
        }
    }


    private List<string> GetResults(string input)
    {
        List<string> triggers = im.GetUnlockedTriggers();

        return triggers.FindAll((str) => str.IndexOf(input) >= 0);
    }

}