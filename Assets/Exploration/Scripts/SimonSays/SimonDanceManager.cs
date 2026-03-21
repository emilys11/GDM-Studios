using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SimonDanceManager : MonoBehaviour
{
    [Header("Sections")]
    public SimonFloorSection[] sections;

    [Header("Game Settings")]
    public int roundsToWin = 5;
    public float timeBetweenFlashes = 0.25f;
    public float timeBeforePlayerTurn = 0.5f;

    [Header("Floor Lighting")]
    public ColorChanger[] allTiles;
    public float dimMultiplier = 0.4f;

    [Header("Player Timing")]
    public float playerResponseTime = 5f;

    private Coroutine timeoutCoroutine;

    [Header("UI")]
    public TextMeshProUGUI statusText;

    private List<int> sequence = new List<int>();
    private int playerInputIndex = 0;
    private bool acceptingInput = false;
    private bool gameActive = false;
    private bool gameCompleted = false;
    private int currentRound = 0;

    public GameObject gate;

    private void Start()
    {
        for (int i = 0; i < sections.Length; i++)
        {
            sections[i].manager = this;
            sections[i].sectionIndex = i;
            sections[i].SetInteractable(false);
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

        foreach (ColorChanger tile in allTiles)
        {
            tile.SetSimonGameActive(true);
        }

    }

    private void AddStep()
    {
        int randomIndex;

        do
        {
            randomIndex = Random.Range(0, sections.Length);
        }
        while (
            sequence.Count >= 2 &&
            sequence[sequence.Count - 1] == randomIndex &&
            sequence[sequence.Count - 2] == randomIndex
        );

        sequence.Add(randomIndex);
    }

    private IEnumerator PlaySequenceRoutine()
    {
        acceptingInput = false;
        SetSectionsInteractable(false);
        UpdateStatus("Watch the sequence");

        yield return new WaitForSeconds(0.75f);

        for (int i = 0; i < sequence.Count; i++)
        {
            int index = sequence[i];
            yield return StartCoroutine(sections[index].Flash());
            yield return new WaitForSeconds(timeBetweenFlashes);
        }

        yield return new WaitForSeconds(timeBeforePlayerTurn);

        playerInputIndex = 0;
        acceptingInput = true;
        SetSectionsInteractable(true);
        UpdateStatus("Your turn");

        StartPlayerTimeout();
    }

    public void PlayerPressed(int sectionIndex)
    {
        if (!gameActive || gameCompleted || !acceptingInput) return;

        RestartPlayerTimeout();

        if (sectionIndex != sequence[playerInputIndex])
        {
            StartCoroutine(FailRoutine());
            return;
        }

        playerInputIndex++;

        if (playerInputIndex >= sequence.Count)
        {
            StopPlayerTimeout();

            if (currentRound >= roundsToWin)
                StartCoroutine(WinRoutine());
            else
                StartCoroutine(NextRoundRoutine());
        }
    }

    private IEnumerator NextRoundRoutine()
    {
        StopPlayerTimeout();
        acceptingInput = false;
        SetSectionsInteractable(false);

        UpdateStatus("Correct!");
        yield return new WaitForSeconds(1f);

        currentRound++;
        AddStep();
        StartCoroutine(PlaySequenceRoutine());
    }

    private IEnumerator FailRoutine()
    {
        StopPlayerTimeout();
        acceptingInput = false;
        SetSectionsInteractable(false);

        UpdateStatus("Wrong sequence!");
        yield return new WaitForSeconds(1.5f);

        sequence.Clear();
        playerInputIndex = 0;
        currentRound = 1;

        AddStep();

        UpdateStatus("Starting over...");
        yield return new WaitForSeconds(1f);

        StartCoroutine(PlaySequenceRoutine());
    }

    private IEnumerator WinRoutine()
    {
        StopPlayerTimeout();
        acceptingInput = false;
        SetSectionsInteractable(false);
        gameCompleted = true;
        gameActive = false;

        UpdateStatus("You win!");
        Debug.Log("Dance floor Simon Says completed.");
        yield return null;

        foreach (ColorChanger tile in allTiles)
        {
            tile.SetSimonGameActive(false);
        }

        //Gate logic
        gate.SetActive(false);

    }

    private void SetSectionsInteractable(bool value)
    {
        foreach (SimonFloorSection section in sections)
        {
            section.SetInteractable(value);
        }
    }

    private void UpdateStatus(string message)
    {
        if (statusText != null)
            statusText.text = message;
    }


    private void StartPlayerTimeout()
    {
        StopPlayerTimeout();
        timeoutCoroutine = StartCoroutine(PlayerTimeoutRoutine());
    }

    private void RestartPlayerTimeout()
    {
        if (!acceptingInput) return;
        StartPlayerTimeout();
    }

    private void StopPlayerTimeout()
    {
        if (timeoutCoroutine != null)
        {
            StopCoroutine(timeoutCoroutine);
            timeoutCoroutine = null;
        }
    }

    private IEnumerator PlayerTimeoutRoutine()
    {
        yield return new WaitForSeconds(playerResponseTime);

        if (!acceptingInput || !gameActive || gameCompleted)
            yield break;

        acceptingInput = false;
        SetSectionsInteractable(false);

        UpdateStatus("Too slow! Watch again...");
        yield return new WaitForSeconds(1f);

        StartCoroutine(PlaySequenceRoutine());
    }
}