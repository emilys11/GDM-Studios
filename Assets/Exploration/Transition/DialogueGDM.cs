using TMPro;
using UnityEngine;
using System.Collections;

public class TypewriterDialogue : MonoBehaviour
{
    public TMP_Text dialogueText;
    public float letterDelay = 0.03f;

    public void Line1()
    {
        StopAllCoroutines();
        StartCoroutine(TypeLine(
            "ASTRONAUT\n" +
            "Hello, GDM, can you hear me? Over.\n\n" +

            "GDM\n" +
            "Reading you loud and clear. What has been happening? Over.\n\n" +

            "ASTRONAUT\n" +
            "I have just left the first planet, Classico, and am heading over to the second planet, Popula’. The third planet’s name is still unknown. Over.\n\n" +

            "GDM\n" +
            "Excellent. What have you learned on Classico? Over.\n\n" +

            "ASTRONAUT\n" +
            "They are, indeed, dependent on music, as you have said. However, there is a Great Evil that has taken it away from them, and they are suffering.\n\n" +
            "I have convinced Classico’s ruler, Bachovenpinanoff, to go back to being a good ruler with the promise of trade negotiations. Over.\n\n" +

            "GDM\n" +
            "Very interesting, and good job. Continue your journey, and contact us again when you have spoken to Popula’’s ruler. Over and out."
        ));
    }

    IEnumerator TypeLine(string line)
    {
        dialogueText.text = "";

        foreach (char c in line)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(letterDelay);
        }
    }
    void Start()
    {
        Line1();
    }
}