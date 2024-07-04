using UnityEngine;

namespace Game.StateControl
{
    public class StateMachine : MonoBehaviour
    {
        protected State currentState;

        protected bool inTransition;

        public virtual State CurrentState
        {
            get
            {
                return currentState;
            }
            set
            {
                Transition(value);
            }
        }

        public virtual T GetState<T>() where T : State
        {
            T val = GetComponent<T>();
            if (val == null)
            {
                val = gameObject.AddComponent<T>();
            }
            return val;
        }

        public virtual void ChangeState<T>() where T : State
        {
            CurrentState = GetState<T>();
        }

        protected virtual void Transition(State value)
        {
            if (!(currentState == value) && !inTransition)
            {
                inTransition = true;
                if (currentState != null)
                {
                    currentState.Exit();
                }
                currentState = value;
                if (currentState != null)
                {
                    currentState.Enter();
                }
                inTransition = false;
            }
        }
    }
}