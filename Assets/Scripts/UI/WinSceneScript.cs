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
            while (!zeusWin && !poseidonWin && !athenaWin && !aphroditeWin)
            {
                yield return null;
            }
            if (zeusWin)
            {
                zeusWinName.SetActive(true);
                zeusWinQuote.SetActive(true);
            }
            else if (poseidonWin)
            {
                poseidonWinName.SetActive(true);
                poseidonWinQuote.SetActive(true);
            }
            else if (athenaWin)
            {
                athenaWinName.SetActive(true);
                athenaWinQuote.SetActive(true);
            }
            else if (aphroditeWin)
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

        public void RecieveScores(int[] scores)
        {
            //zeus, poseidon, athena, aphrodite
            if (scores[0] > scores[1] && scores[0] > scores[2] && scores[0] > scores[3])
            {
                zeusWin = true;
            }
            else if (scores[1] > scores[0] && scores[1] > scores[2] && scores[1] > scores[3])
            {
                poseidonWin = true;
            }
            else if (scores[2] > scores[0] && scores[2] > scores[1] && scores[2] > scores[3])
            {
                athenaWin = true;
            }
            else if (scores[3] > scores[0] && scores[3] > scores[1] && scores[3] > scores[2])
            {
                aphroditeWin = true;
            }
        }
    }
}