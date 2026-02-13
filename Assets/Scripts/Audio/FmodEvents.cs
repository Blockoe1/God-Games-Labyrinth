using UnityEngine;
using FMODUnity;
using System.ComponentModel;

namespace GGL
{
    public class FmodEvents : MonoBehaviour
    {
        [field: Header("Music")]
        [field:SerializeField] public EventReference music { get; private set; }

        public static FmodEvents instance {  get; private set; }

        private void Awake()
        {
            instance = this;
        }
    }
}
