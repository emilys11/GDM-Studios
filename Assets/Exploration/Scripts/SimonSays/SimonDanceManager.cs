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

    [Header("Audio")]
    [SerializeField] private AudioSource failAudioSource;
    [SerializeField] private AudioClip failSound;
    [SerializeField] private float failVolume = 1f;

    [SerializeField] private AudioSource winAudioSource;
    [SerializeField] private AudioClip winSound;
    [SerializeField] private float winVolume = 1f;

    private List<int> sequence = new List<int>();
    private int playerInputIndex = 0;
    private bool acceptingInput = false;
    private bool gameActive = false;
    private bool gameCompleted = false;
    private int currentRound = 0;

    public GameObject gate;

    private void Awake()
    {
        if (failAudioSource != null)
        {
            failAudioSource.playOnAwake = false;
            failAudioSource.spatialBlend = 0f;
        }

        if (winAudioSource != null)
        {
            winAudioSource.playOnAwake = false;
            winAudioSource.spatialBlend = 0f;
        }
    }

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

        if (failSound != null && failAudioSource != null)
        {
            failAudioSource.PlayOneShot(failSound, failVolume);
        }

        yield return new WaitForSeconds(1.5f);

        sequence.Clear();
        playerInputIndex = 0;
        currentRound = 1;

        AddStep();

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

        if (winSound != null && winAudioSource != null)
        {
            winAudioSource.PlayOneShot(winSound, winVolume);
        }

        Debug.Log("Dance floor Simon Says completed.");

        foreach (ColorChanger tile in allTiles)
        {
            tile.SetSimonGameActive(false);
        }

        gate.SetActive(false);

        yield return null;
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

        if (failSound != null && failAudioSource != null)
        {
            failAudioSource.PlayOneShot(failSound, failVolume);
        }

        yield return new WaitForSeconds(1f);

        StartCoroutine(PlaySequenceRoutine());
    }
}