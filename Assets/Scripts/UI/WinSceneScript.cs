using GGL.Scoring;
using JetBrains.Annotations;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GGL
{
    public class WinSceneScript : MonoBehaviour
    {
        [SerializeField] private TMP_Text timerText;
        
        private bool zeusWin = false;
        private bool poseidonWin = false;
        private bool athenaWin = false;
        private bool aphroditeWin = false;

        [SerializeField] GameObject zeusNameGameObject;
        [SerializeField] GameObject zeusWinTextGameObject;
        [SerializeField] GameObject zeusBoxGameObject;
        [SerializeField] GameObject athenaNameGameObject;
        [SerializeField] GameObject athenaWinTextGameObject;
        [SerializeField] GameObject athenaBoxGameObject;
        [SerializeField] GameObject aphroditeNameGameObject;
        [SerializeField] GameObject aphroditeWinTextGameObject;
        [SerializeField] GameObject aphroditeBoxGameObject;
        [SerializeField] GameObject poseidonNameGameObject;
        [SerializeField] GameObject poseidonWinTextGameObject;
        [SerializeField] GameObject poseidonBoxGameObject;

        [SerializeField] Sprite zeusNameSprite;
        [SerializeField] Sprite zeusWinTextSprite;
        [SerializeField] Sprite zeusWinBoxSprite;
        [SerializeField] Sprite athenaNameSprite;
        [SerializeField] Sprite athenaWinTextSprite;
        [SerializeField] Sprite athenaWinBoxSprite;
        [SerializeField] Sprite aphroditeNameSprite;
        [SerializeField] Sprite aphroditeWinTextSprite;
        [SerializeField] Sprite aphroditeWinBoxSprite;
        [SerializeField] Sprite poseidonNameSprite;
        [SerializeField] Sprite poseidonWinTextSprite;
        [SerializeField] Sprite poseidonWinBoxSprite;
        [SerializeField] Sprite winnerTextSprite;


        private void Start()
        {
            StartCoroutine(WinScreenTimer());
        }

        public void BacktoMenu()
        {
            SceneManager.LoadScene("StartScene");
        }

        IEnumerator WinScreenTimer()
        {
            yield return new WaitForSecondsRealtime(1f);
            if (GameplayScoreManager.GameScores[0] >= GameplayScoreManager.GameScores[1] &&
                GameplayScoreManager.GameScores[0] >= GameplayScoreManager.GameScores[2] &&
                GameplayScoreManager.GameScores[0] >= GameplayScoreManager.GameScores[3])
            {
                zeusNameGameObject.GetComponent<SpriteRenderer>().sprite = winnerTextSprite;
                zeusBoxGameObject.GetComponent<SpriteRenderer>().sprite = zeusWinBoxSprite;

                athenaNameGameObject.GetComponent<SpriteRenderer>().sprite = zeusNameSprite;
                aphroditeNameGameObject.GetComponent<SpriteRenderer>().sprite = zeusNameSprite;
                poseidonNameGameObject.GetComponent<SpriteRenderer>().sprite = zeusNameSprite;

                athenaWinTextGameObject.GetComponent<SpriteRenderer>().sprite = zeusWinTextSprite;
                aphroditeWinTextGameObject.GetComponent<SpriteRenderer>().sprite = zeusWinTextSprite;
                poseidonWinTextGameObject.GetComponent<SpriteRenderer>().sprite = zeusWinTextSprite;

                zeusWinTextGameObject.SetActive(false);
            }
            else if (GameplayScoreManager.GameScores[1] >= GameplayScoreManager.GameScores[0] &&
                GameplayScoreManager.GameScores[1] >= GameplayScoreManager.GameScores[2] &&
                GameplayScoreManager.GameScores[1] >= GameplayScoreManager.GameScores[3])
            {
                poseidonNameGameObject.GetComponent<SpriteRenderer>().sprite = winnerTextSprite;
                poseidonBoxGameObject.GetComponent<SpriteRenderer>().sprite = poseidonWinBoxSprite;

                athenaNameGameObject.GetComponent<SpriteRenderer>().sprite = poseidonNameSprite;
                aphroditeNameGameObject.GetComponent<SpriteRenderer>().sprite = poseidonNameSprite;
                zeusNameGameObject.GetComponent<SpriteRenderer>().sprite = poseidonNameSprite;

                athenaWinTextGameObject.GetComponent<SpriteRenderer>().sprite = poseidonWinTextSprite;
                aphroditeWinTextGameObject.GetComponent<SpriteRenderer>().sprite = poseidonWinTextSprite;
                zeusWinTextGameObject.GetComponent<SpriteRenderer>().sprite = poseidonWinTextSprite;

                poseidonWinTextGameObject.SetActive(false);
            }
            else if (GameplayScoreManager.GameScores[2] >= GameplayScoreManager.GameScores[0] &&
                GameplayScoreManager.GameScores[2] >= GameplayScoreManager.GameScores[1] &&
                GameplayScoreManager.GameScores[2] >= GameplayScoreManager.GameScores[3])
            {
                athenaNameGameObject.GetComponent<SpriteRenderer>().sprite = winnerTextSprite;
                athenaBoxGameObject.GetComponent<SpriteRenderer>().sprite = athenaWinBoxSprite;

                poseidonNameGameObject.GetComponent<SpriteRenderer>().sprite = athenaNameSprite;
                aphroditeNameGameObject.GetComponent<SpriteRenderer>().sprite = athenaNameSprite;
                zeusNameGameObject.GetComponent<SpriteRenderer>().sprite = athenaNameSprite;

                poseidonWinTextGameObject.GetComponent<SpriteRenderer>().sprite = athenaWinTextSprite;
                aphroditeWinTextGameObject.GetComponent<SpriteRenderer>().sprite = athenaWinTextSprite;
                zeusWinTextGameObject.GetComponent<SpriteRenderer>().sprite = athenaWinTextSprite;

                athenaWinTextGameObject.SetActive(false);
            }
            else if (GameplayScoreManager.GameScores[3] >= GameplayScoreManager.GameScores[0] &&
                GameplayScoreManager.GameScores[3] >= GameplayScoreManager.GameScores[1] &&
                GameplayScoreManager.GameScores[3] >= GameplayScoreManager.GameScores[2])
            {
                aphroditeNameGameObject.GetComponent<SpriteRenderer>().sprite = winnerTextSprite;
                aphroditeBoxGameObject.GetComponent<SpriteRenderer>().sprite = aphroditeWinBoxSprite;

                poseidonNameGameObject.GetComponent<SpriteRenderer>().sprite = aphroditeNameSprite;
                athenaNameGameObject.GetComponent<SpriteRenderer>().sprite = aphroditeNameSprite;
                zeusNameGameObject.GetComponent<SpriteRenderer>().sprite = aphroditeNameSprite;

                poseidonWinTextGameObject.GetComponent<SpriteRenderer>().sprite = aphroditeWinTextSprite;
                athenaWinTextGameObject.GetComponent<SpriteRenderer>().sprite = aphroditeWinTextSprite;
                zeusWinTextGameObject.GetComponent<SpriteRenderer>().sprite = aphroditeWinTextSprite;

                aphroditeWinTextGameObject.SetActive(false);
            }
            int i = 5;
            while (i > 0)
            {
                timerText.text = "" + i;
                yield return new WaitForSecondsRealtime(1f);
                i--;
            }
            SceneManager.LoadScene("StartScene");
        }
    }
}