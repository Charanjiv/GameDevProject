using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerUIPopUpManager : MonoBehaviour
{
    [Header("Message Pop Up")]
    [SerializeField] TextMeshProUGUI popUpMessageText;
    [SerializeField] GameObject popUpMessageGameObject;
    public PlayerManager playerManager;
    [Header("YOU DIED Pop Up")]
    [SerializeField] GameObject youDiedPopUpGameObject;
    [SerializeField] TextMeshProUGUI youDiedPopUpBackgroundText;
    [SerializeField] TextMeshProUGUI youDiedPopUpText;
    [SerializeField] CanvasGroup youDiedPopUpCanvasGroup;   //  Allows us to set the alpha to fade over time

    [Header("BOSS DEFEATED Pop Up")]
    [SerializeField] GameObject bossDefeatedPopUpGameObject;
    [SerializeField] TextMeshProUGUI bossDefeatedPopUpBackgroundText;
    [SerializeField] TextMeshProUGUI bossDefeatedPopUpText;
    [SerializeField] CanvasGroup bossDefeatedPopUpCanvasGroup;   //  Allows us to set the alpha to fade over time

    [Header("GRACE RESTORED Pop Up")]
    [SerializeField] GameObject graceRestoredPopUpGameObject;
    [SerializeField] TextMeshProUGUI graceRestoredPopUpBackgroundText;
    [SerializeField] TextMeshProUGUI graceRestoredPopUpText;
    [SerializeField] CanvasGroup graceRestoredPopUpCanvasGroup;   //  Allows us to set the alpha to fade over time

    [Header("Player Performance Pop Up")]
    [SerializeField] GameObject playerPerformancePopUpGameObject;
    [SerializeField] TextMeshProUGUI healthPerformanceText;
    [SerializeField] TextMeshProUGUI killPerformanceText;
    [SerializeField] TextMeshProUGUI LifePerformanceText;
    [SerializeField] TextMeshProUGUI OverallPerformanceText;
    [SerializeField] CanvasGroup playerPerformancePopUpCanvasGroup;

    //private GameObject playerGameObject;
    public void Update()
    {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (SceneManager.GetActiveScene().name == "Scene_World_01")
        {
            if(playerObject != null)
            {
                PlayerManager player = playerObject.GetComponent<PlayerManager>();
                playerManager = player;
            }
            // Do something only when this scene is active
            //DisplayPerformance();
        }

    }
    public void CloseAllPopUpWindows()
    {
        popUpMessageGameObject.SetActive(false);

        PlayerUIManager.instance.popUpWindowIsOpen = false;
    }

    public void SendPlayerMessagePopUp(string messageText)
    {
        PlayerUIManager.instance.popUpWindowIsOpen = true;
        popUpMessageText.text = messageText;
        popUpMessageGameObject.SetActive(true);
    }

    public void SendYouDiedPopUp()
    {
        //  ACTIVATE POST PROCESSING EFFECTS

        youDiedPopUpGameObject.SetActive(true);
        youDiedPopUpBackgroundText.characterSpacing = 0;
        StartCoroutine(StretchPopUpTextOverTime(youDiedPopUpBackgroundText, 8, 19));
        StartCoroutine(FadeInPopUpOverTime(youDiedPopUpCanvasGroup, 5));
        StartCoroutine(WaitThenFadeOutPopUpOverTime(youDiedPopUpCanvasGroup, 2, 5));
    }

    public void SendBossDefeatedPopUp(string bossDefeatedMessage)
    {
        bossDefeatedPopUpText.text = bossDefeatedMessage;
        bossDefeatedPopUpBackgroundText.text = bossDefeatedMessage;
        bossDefeatedPopUpGameObject.SetActive(true);
        bossDefeatedPopUpBackgroundText.characterSpacing = 0;
        StartCoroutine(StretchPopUpTextOverTime(bossDefeatedPopUpBackgroundText, 8, 19));
        StartCoroutine(FadeInPopUpOverTime(bossDefeatedPopUpCanvasGroup, 5));
        StartCoroutine(WaitThenFadeOutPopUpOverTime(bossDefeatedPopUpCanvasGroup, 2, 5));
    }

    public void SendGraceRestoredPopUp(string graceRestoredMessage)
    {
        graceRestoredPopUpText.text = graceRestoredMessage;
        graceRestoredPopUpBackgroundText.text = graceRestoredMessage;
        graceRestoredPopUpGameObject.SetActive(true);
        graceRestoredPopUpBackgroundText.characterSpacing = 0;
        StartCoroutine(StretchPopUpTextOverTime(graceRestoredPopUpBackgroundText, 8, 19));
        StartCoroutine(FadeInPopUpOverTime(graceRestoredPopUpCanvasGroup, 5));
        StartCoroutine(WaitThenFadeOutPopUpOverTime(graceRestoredPopUpCanvasGroup, 2, 5));
    }

        private IEnumerator StretchPopUpTextOverTime(TextMeshProUGUI text, float duration, float stretchAmount)
    {
        if (duration > 0f)
        {
            text.characterSpacing = 0;  //  RESETS OUR CHARACTER SPACING
            float timer = 0;

            yield return null;

            while (timer < duration)
            {
                timer = timer + Time.deltaTime;
                text.characterSpacing = Mathf.Lerp(text.characterSpacing, stretchAmount, duration * (Time.deltaTime / 20));
                yield return null;
            }
        }
    }

    private IEnumerator FadeInPopUpOverTime(CanvasGroup canvas, float duration)
    {
        if (duration > 0)
        {
            canvas.alpha = 0;
            float timer = 0;
            yield return null;

            while (timer < duration)
            {
                timer = timer + Time.deltaTime;
                canvas.alpha = Mathf.Lerp(canvas.alpha, 1, duration * Time.deltaTime);
                yield return null;
            }
        }

        canvas.alpha = 1;

        yield return null;
    }

    private IEnumerator WaitThenFadeOutPopUpOverTime(CanvasGroup canvas, float duration, float delay)
    {
        if (duration > 0)
        {
            while (delay > 0)
            {
                delay = delay - Time.deltaTime;
                yield return null;
            }

            canvas.alpha = 1;
            float timer = 0;

            yield return null;

            while (timer < duration)
            {
                timer = timer + Time.deltaTime;
                canvas.alpha = Mathf.Lerp(canvas.alpha, 0, duration * Time.deltaTime);
                yield return null;
            }
        }

        canvas.alpha = 0;

        yield return null;
    }

    float RoundToDecimals(float value, int decimalPlaces)
    {
        float factor = Mathf.Pow(10, decimalPlaces);
        return Mathf.Round(value * factor) / factor;
    }

    public void DisplayPerformance()
    {
        playerPerformancePopUpGameObject.SetActive(true);
        StartCoroutine(FadeInPopUpOverTime(graceRestoredPopUpCanvasGroup, 5));
        healthPerformanceText.SetText("Health Performance: " + RoundToDecimals(playerManager.HealthPerformance(playerManager.pHealth),2).ToString());
        killPerformanceText.SetText("Kill Performance: " + RoundToDecimals(playerManager.KillPerformance(playerManager.pKills), 2).ToString());
        LifePerformanceText.SetText("Life Performance: " + RoundToDecimals(playerManager.LifePerformance(playerManager.playerDeaths), 2).ToString());
        OverallPerformanceText.SetText("Overall Performance: " + RoundToDecimals(playerManager.overallPerformance, 2).ToString());




    }
}
