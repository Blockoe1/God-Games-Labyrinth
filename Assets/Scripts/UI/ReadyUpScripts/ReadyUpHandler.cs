using NaughtyAttributes;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GGL
{
    public class ReadyUpHandler : MonoBehaviour
    {
        [SerializeField, Scene] private string mazeScene;
        [SerializeField] private GameObject Player1;
        [SerializeField] private GameObject Player2;
        [SerializeField] private GameObject Player3;
        [SerializeField] private GameObject Player4;

        [SerializeField] private TMP_Text timerText;

        private bool timerRunning = false;

        public string MazeScene
        {
            get { return mazeScene; }
            set { mazeScene = value; }
        }

        private void Update()
        {
            if (Player1.GetComponent<ReadyUpSystem1>().ready && Player2.GetComponent<ReadyUpSystem2>().ready && 
                Player3.GetComponent<ReadyUpSystem3>().ready && Player4.GetComponent<ReadyUpSystem4>().ready && !timerRunning)
            {
                StartCoroutine(StartGame());
            }
        }
        IEnumerator StartGame()
        {
            timerRunning = true;
            int i = 3;
            while (i > 0)
            {
                if (!Player1.GetComponent<ReadyUpSystem1>().ready || !Player2.GetComponent<ReadyUpSystem2>().ready || 
                    !Player3.GetComponent<ReadyUpSystem3>().ready || !Player4.GetComponent<ReadyUpSystem4>().ready)
                {
                    break;
                }
                timerText.text = "" + i;
                yield return new WaitForSecondsRealtime(1f);
                i--;
            }
            if (!Player1.GetComponent<ReadyUpSystem1>().ready || !Player2.GetComponent<ReadyUpSystem2>().ready ||
                    !Player3.GetComponent<ReadyUpSystem3>().ready || !Player4.GetComponent<ReadyUpSystem4>().ready)
            {
                StopCoroutine(StartGame());
                timerRunning = false;
                timerText.text = "";
            }
            else if (Player1.GetComponent<ReadyUpSystem1>().ready && Player2.GetComponent<ReadyUpSystem2>().ready &&
                Player3.GetComponent<ReadyUpSystem3>().ready && Player4.GetComponent<ReadyUpSystem4>().ready)
            {
                SceneManager.LoadScene(mazeScene);
            }
        }
    }
}
