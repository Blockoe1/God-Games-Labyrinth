using UnityEngine;

namespace GGL
{
    public class RecieverDebugger : MonoBehaviour
    {
        private int[] scores;

        public void SetScores(int[] scores)
        {
            this.scores = scores;
        }

        private void OnGUI()
        {
            if (scores == null || scores.Length == 0) { return; }
            string scoresString = "";
            for(int i = 0; i < scores.Length; i++)
            {
                    scoresString += $"{(GodID)i}: {scores[i]}\n";
            }
             GUI.Label(new Rect(20, 20, 200, 500), scoresString);
        }
    }
}
