using UnityEngine;

namespace ShootEmUp
{
    public class CoroutineStarter : MonoBehaviour, ICoroutineStarter
    {
        MonoBehaviour ICoroutineStarter.CoroutineStarter => this;
    }
}