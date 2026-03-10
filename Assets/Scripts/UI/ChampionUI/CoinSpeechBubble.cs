using GGL.Minotaur;
using GGL.Scoring;
using UnityEngine;

namespace GGL
{
    public class CoinSpeechBubble : MonoBehaviour
    {

        public GameObject speechBubble; // Assign in Inspector
        public Collector collector;   // Coinbag script reference
        





        void Update()
        {

            if (collector.HeldGold >= collector.GoldCapacity && !speechBubble.activeSelf)
            {
                speechBubble.SetActive(true);

                Debug.Log("True is working");

            }
            else if (collector.HeldGold < collector.GoldCapacity && speechBubble.activeSelf)
            {
                speechBubble.SetActive(false);

                Debug.Log("False is working");

            }

        }
    }
}
