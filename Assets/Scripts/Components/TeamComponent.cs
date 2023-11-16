using UnityEngine;

namespace ShootEmUp
{
    public sealed class TeamComponent : MonoBehaviour
    {
        public bool IsPlayer
        {
            get { return this._isPlayer; }
        }
        
        [SerializeField]
        private bool _isPlayer;
    }
}