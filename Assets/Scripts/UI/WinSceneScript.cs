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

        [SerializeField] GameObject zeusWinName;
        [SerializeField] GameObject zeusWinQuote;
        [SerializeField] GameObject poseidonWinName;
        [SerializeField] GameObject poseidonWinQuote;
        [SerializeField] GameObject athenaWinName;
        [SerializeField] GameObject athenaWinQuote;
        [SerializeField] GameObject aphroditeWinName;
        [SerializeField] GameObject aphroditeWinQuote;

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
                zeusWinName.SetActive(true);
                zeusWinQuote.SetActive(true);
            }
            else if (GameplayScoreManager.GameScores[1] >= GameplayScoreManager.GameScores[0] &&
                GameplayScoreManager.GameScores[1] >= GameplayScoreManager.GameScores[2] &&
                GameplayScoreManager.GameScores[1] >= GameplayScoreManager.GameScores[3])
            {
                poseidonWinName.SetActive(true);
                poseidonWinQuote.SetActive(true);
            }
            else if (GameplayScoreManager.GameScores[2] >= GameplayScoreManager.GameScores[0] &&
                GameplayScoreManager.GameScores[2] >= GameplayScoreManager.GameScores[1] &&
                GameplayScoreManager.GameScores[2] >= GameplayScoreManager.GameScores[3])
            {
                athenaWinName.SetActive(true);
                athenaWinQuote.SetActive(true);
            }
            else if (GameplayScoreManager.GameScores[3] >= GameplayScoreManager.GameScores[0] &&
                GameplayScoreManager.GameScores[3] >= GameplayScoreManager.GameScores[1] &&
                GameplayScoreManager.GameScores[3] >= GameplayScoreManager.GameScores[2])
            {
                aphroditeWinName.SetActive(true);
                aphroditeWinQuote.SetActive(true);
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