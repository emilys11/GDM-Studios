using UnityEngine;
using TMPro;
using System.Collections;

public class MiniGameUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messageText;
    private Coroutine messageRoutine;

    private void Start()
    {
        if (messageText != null)
        {
            messageText.text = "";
            messageText.gameObject.SetActive(false);
        }
    }

    public void ShowMessage(string message, float duration)
    {
        if (messageRoutine != null)
        {
            StopCoroutine(messageRoutine);
        }

        messageRoutine = StartCoroutine(ShowMessageRoutine(message, duration));
    }

    private IEnumerator ShowMessageRoutine(string message, float duration)
    {
        if (messageText != null)
        {
            messageText.text = message;
            messageText.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(duration);

        if (messageText != null)
        {
            messageText.text = "";
            messageText.gameObject.SetActive(false);
        }

        messageRoutine = null;
    }
}