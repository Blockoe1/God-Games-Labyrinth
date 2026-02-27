using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

namespace GGL
{
    public class TimerScript : MonoBehaviour
    {
        [SerializeField] float time;
        [SerializeField] TMP_Text timerText;
        [SerializeField] UnityEvent OnTimerComplete;
        [SerializeField] UnityEvent MinotaurSpawn;
        [SerializeField] UnityEvent MinotaurEnrage;
        [SerializeField] Slider Timer1;
        [SerializeField] Slider Timer2;
        [SerializeField] Slider Timer3;
        [SerializeField] Slider Timer4;
        [SerializeField] Image minotaurImage;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            StartCoroutine(Timer());
        }
        
        IEnumerator Timer()
        {
            Timer1.value = 0.5f;
            timerText.text = "" + time;
            while (time > 75)
            {
                yield return null;
                time -= Time.deltaTime;
                timerText.text = "" + time;
                Timer1.value = ((time - 60) / 30);
            }
            while (time > 60)
            {
                yield return null;
                time -= Time.deltaTime;
                timerText.text = "" + time;
                Timer1.value = ((time - 60) / 30);
            }

            //30 seconds have passed, spawn Minotaur
            MinotaurSpawn?.Invoke();
            minotaurImage.gameObject.SetActive(false);

            while (time > 45)
            {
                yield return null;
                time -= Time.deltaTime;
                timerText.text = "" + time;
                Timer2.value = ((time - 45) / 15);
            }
            while (time > 30)
            {
                yield return null;
                time -= Time.deltaTime;
                timerText.text = "" + time;
                Timer3.value = ((time - 15) / 30);
            }

            //60 seconds have passed, 30 left
            MinotaurEnrage?.Invoke();

            while (time > 15)
            {
                yield return null;
                time -= Time.deltaTime;
                timerText.text = "" + time;
                Timer3.value = ((time - 15) / 30);
            }
            while (time > 0)
            {
                yield return null;
                time -= Time.deltaTime;
                timerText.text = "" + time;
                Timer4.value = (time / 15);
            }
            OnTimerComplete?.Invoke();
            SceneManager.LoadScene("WinScene");
        }
    }
}
