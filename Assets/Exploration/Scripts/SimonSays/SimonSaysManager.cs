using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SimonSaysManager : MonoBehaviour
{
    [Header("Buttons")]
    public SimonButton[] buttons;

    [Header("Game Settings")]
    public int roundsToWin = 5;
    public float timeBetweenFlashes = 0.25f;
    public float timeBeforePlayerTurn = 0.5f;

    [Header("UI")]
    public TextMeshProUGUI statusText;

    private List<int> sequence = new List<int>();
    private int playerInputIndex = 0;
    private bool acceptingInput = false;
    private bool gameActive = false;
    private bool gameCompleted = false;

    private int currentRound = 0;

    private void Start()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].manager = this;
            buttons[i].buttonIndex = i;
            buttons[i].SetInteractable(false);
        }
    }

    public void StartMiniGame()
    {
        if (gameActive) return;

        gameActive = true;
        gameCompleted = false;
        sequence.Clear();
        playerInputIndex = 0;

        currentRound = 1;

        AddStep();
        StartCoroutine(PlaySequenceRoutine());
    }

    private void AddStep()
    {
        int randomIndex = Random.Range(0, buttons.Length);
        sequence.Add(randomIndex);
    }

    private IEnumerator PlaySequenceRoutine()
    {
        acceptingInput = false;
        SetButtonsInteractable(false);

        UpdateStatus("Watch the sequence");

        yield return new WaitForSeconds(0.75f);

        for (int i = 0; i < sequence.Count; i++)
        {
            int index = sequence[i];
            yield return buttons[index].StartCoroutine(buttons[index].Flash());
            yield return new WaitForSeconds(timeBetweenFlashes);
        }

        yield return new WaitForSeconds(timeBeforePlayerTurn);

        playerInputIndex = 0;
        acceptingInput = true;
        SetButtonsInteractable(true);
        UpdateStatus("Your turn");
    }

    public void PlayerPressed(int buttonIndex)
    {
        if (!gameActive || gameCompleted || !acceptingInput) return;

        if (buttonIndex != sequence[playerInputIndex])
        {
            StartCoroutine(FailRoutine());
            return;
        }

        playerInputIndex++;

        if (playerInputIndex >= sequence.Count)
        {
            if (currentRound >= roundsToWin)
            {
                StartCoroutine(WinRoutine());
            }
            else
            {
                StartCoroutine(NextRoundRoutine());
            }
        }
    }

    private IEnumerator NextRoundRoutine()
    {
        acceptingInput = false;
        SetButtonsInteractable(false);

        UpdateStatus("Correct!");
        yield return new WaitForSeconds(1f);

        currentRound++;

        AddStep();
        StartCoroutine(PlaySequenceRoutine());
    }

    private IEnumerator FailRoutine()
    {
        acceptingInput = false;
        SetButtonsInteractable(false);

        UpdateStatus("Wrong sequence!");
        yield return new WaitForSeconds(1.5f);

        playerInputIndex = 0;

        UpdateStatus("Try again");
        yield return new WaitForSeconds(1f);

        StartCoroutine(PlaySequenceRoutine());
    }

    private IEnumerator WinRoutine()
    {
        acceptingInput = false;
        SetButtonsInteractable(false);
        gameCompleted = true;
        gameActive = false;

        UpdateStatus("You win!");
        Debug.Log("Simon Says completed.");

        yield return null;

    }

    private void SetButtonsInteractable(bool value)
    {
        foreach (SimonButton button in buttons)
        {
            button.SetInteractable(value);
        }
    }

    private void UpdateStatus(string message)
    {
        if (statusText != null)
        {
        statusText.text = message;
        }
    }
}