using System.Collections;
using UnityEngine;

namespace GGL
{
    public class TimerScript : MonoBehaviour
    {
        private int time;
        private float displayedTime;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            time = 9000;
            StartCoroutine(Timer());
        }
        
        IEnumerator Timer()
        {
            while (time > 0)
            {
                yield return new WaitForSecondsRealtime(0.01f);
                time -= 1;
                Debug.Log(time);
                displayedTime = time / 100;
                Debug.Log(displayedTime);
            }
            //level end stuff
        }
    }
}
