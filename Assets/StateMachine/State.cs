using UnityEngine;

namespace Game.StateControl
{
    public class State : MonoBehaviour
    {
        public virtual void Enter()
        {
            AddListeners();
        }

        public virtual void Exit()
        {
            RemoveListeners();
        }

        protected virtual void OnDestroy()
        {
            RemoveListeners();
        }

        protected virtual void AddListeners()
        {
        }

        protected virtual void RemoveListeners()
        {
        }

        public virtual void Reload()
        {
        }
    }
}