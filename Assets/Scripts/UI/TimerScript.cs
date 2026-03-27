using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GGL
{
    public class TimerScript : MonoBehaviour
    {
        [SerializeField] private float time;
        //[SerializeField] TMP_Text timerText;
        [SerializeField] UnityEvent OnTimerComplete;
        [SerializeField] UnityEvent MinotaurSpawn;
        [SerializeField] UnityEvent MinotaurEnrage;
        [SerializeField] Slider Timer1;
        [SerializeField] Slider Timer2;
        [SerializeField] Slider Timer3;
        [SerializeField] Slider Timer4;
        [SerializeField] Image minotaurImage;
        [SerializeField] Image circleTimer;
        [SerializeField] TMP_Text timerText;
        [SerializeField] GameObject redFadeIn;
        [SerializeField] Volume volume;
        [SerializeField] Vignette vignette;

        [SerializeField] float currentTime;

        public static event Action<float, float> OnTimerUpdate;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            currentTime = time;
            StartCoroutine(Timer());
            volume.profile.TryGet(out vignette);
        }
        
        IEnumerator Timer()
        {
            Timer1.value = 0.5f;
            //timerText.text = "" + time;
            while (currentTime > 75)
            {
                yield return null;
                currentTime -= Time.deltaTime;
                //timerText.text = "" + time;
                Timer1.value = ((currentTime - 60) / 30);
                circleTimer.fillAmount = currentTime / 90;
                OnTimerUpdate?.Invoke(currentTime, time);
            }
            while (currentTime > 60)
            {
                yield return null;
                currentTime -= Time.deltaTime;
                //timerText.text = "" + time;
                Timer1.value = ((currentTime - 60) / 30);
                circleTimer.fillAmount = currentTime / 90;
                OnTimerUpdate?.Invoke(currentTime, time);
            }

            //30 seconds have passed, spawn Minotaur
            MinotaurSpawn?.Invoke();
            minotaurImage.gameObject.SetActive(false);

            while (currentTime > 45)
            {
                yield return null;
                currentTime -= Time.deltaTime;
                //timerText.text = "" + time;
                Timer2.value = ((currentTime - 45) / 15);
                circleTimer.fillAmount = currentTime / 90;
                OnTimerUpdate?.Invoke(currentTime, time);
            }
            while (currentTime > 30)
            {
                yield return null;
                currentTime -= Time.deltaTime;
                //timerText.text = "" + time;
                Timer3.value = ((currentTime - 15) / 30);
                circleTimer.fillAmount = currentTime / 90;
                OnTimerUpdate?.Invoke(currentTime, time);
            }

            //60 seconds have passed, 30 left
            MinotaurEnrage?.Invoke();

            while (currentTime > 15)
            {
                yield return null;
                currentTime -= Time.deltaTime;
                //timerText.text = "" + time;
                Timer3.value = ((currentTime - 15) / 30);
                circleTimer.fillAmount = currentTime / 90;
                OnTimerUpdate?.Invoke(currentTime, time);
            }
            timerText.gameObject.SetActive(true);
            while (currentTime > 0)
            {
                yield return null;
                currentTime -= Time.deltaTime;
                timerText.text = "" + MathF.Floor(currentTime);
                Timer4.value = (currentTime / 15);
                circleTimer.fillAmount = currentTime / 90;
                OnTimerUpdate?.Invoke(currentTime, time);
                //redFadeIn.GetComponent<Image>().color = new Vector4(1,0,0, ((15-currentTime)/100));
                vignette.intensity.value = (15 - currentTime) / 15;
            }
            OnTimerComplete?.Invoke();
            SceneManager.LoadScene("WinScene");
        }
    }
}
