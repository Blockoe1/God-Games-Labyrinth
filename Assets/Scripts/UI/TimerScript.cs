using System.Collections;
using TMPro;
using UnityEngine;

namespace GGL
{
    public class TimerScript : MonoBehaviour
    {
        [SerializeField] int time;
        [SerializeField] TMP_Text timerText;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            StartCoroutine(Timer());
        }
        
        IEnumerator Timer()
        {
            timerText.text = "" + time;
            while (time > 0)
            {
                yield return new WaitForSeconds(1f);
                time -= 1;
                timerText.text = "" + time;
            }
            //level end stuff
        }
    }
}
