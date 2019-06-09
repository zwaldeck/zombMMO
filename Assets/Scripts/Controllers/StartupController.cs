using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Controllers
{
    // TODO: Needs to become a singleton component
    public class StartupController : MonoBehaviour
    {
        public void Start()
        {
            Random.InitState(DateTime.Now.Millisecond);
        }
    }
}