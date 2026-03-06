using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GGL
{
    public class WinSceneScript : MonoBehaviour
    {
        [SerializeField] private TMP_Text timerText;
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
            //check which god won
            //enable their UI
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