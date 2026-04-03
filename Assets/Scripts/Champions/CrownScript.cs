using GGL.Scoring;
using UnityEngine;

namespace GGL
{
    public class CrownScript : MonoBehaviour
    {
        [SerializeField] private GameObject zeusCrown;
        [SerializeField] private GameObject poseidonCrown;
        [SerializeField] private GameObject athenaCrown;
        [SerializeField] private GameObject aphroditeCrown;

        void Update()
        {
            if (GameplayScoreManager.GameScores[0] > GameplayScoreManager.GameScores[1] &&
                   GameplayScoreManager.GameScores[0] > GameplayScoreManager.GameScores[2] &&
                   GameplayScoreManager.GameScores[0] > GameplayScoreManager.GameScores[3])
            {
                if (!zeusCrown.activeSelf)
                {
                    zeusCrown.SetActive(true);
                }
            }
            else
            {
                if (zeusCrown.activeSelf)
                {
                    zeusCrown.SetActive(false);
                }
            }

            if (GameplayScoreManager.GameScores[1] > GameplayScoreManager.GameScores[0] &&
                GameplayScoreManager.GameScores[1] > GameplayScoreManager.GameScores[2] &&
                GameplayScoreManager.GameScores[1] > GameplayScoreManager.GameScores[3])
            {
                if (!poseidonCrown.activeSelf)
                {
                    poseidonCrown.SetActive(true);
                }
            }
            else
            {
                if (poseidonCrown.activeSelf)
                {
                    poseidonCrown.SetActive(false);
                }
            }

            if (GameplayScoreManager.GameScores[2] > GameplayScoreManager.GameScores[0] &&
                GameplayScoreManager.GameScores[2] > GameplayScoreManager.GameScores[1] &&
                GameplayScoreManager.GameScores[2] > GameplayScoreManager.GameScores[3])
            {
                if (!athenaCrown.activeSelf)
                {
                    athenaCrown.SetActive(true);
                }
            }
            else
            {
                if (athenaCrown.activeSelf)
                {
                    athenaCrown.SetActive(false);
                }
            }

            if (GameplayScoreManager.GameScores[3] > GameplayScoreManager.GameScores[0] &&
               GameplayScoreManager.GameScores[3] > GameplayScoreManager.GameScores[1] &&
               GameplayScoreManager.GameScores[3] > GameplayScoreManager.GameScores[2])
            {
                if (!aphroditeCrown.activeSelf)
                {
                    aphroditeCrown.SetActive(true);
                }
            }
            else
            {
                if (aphroditeCrown.activeSelf)
                {
                    aphroditeCrown.SetActive(false);
                }
            }
        }
    }
}
