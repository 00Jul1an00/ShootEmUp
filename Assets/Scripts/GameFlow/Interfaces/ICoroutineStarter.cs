using UnityEngine;

namespace ShootEmUp
{
    public interface ICoroutineStarter
    {
        public MonoBehaviour CoroutineStarter { get; }
    }
}