using System;
using System.Collections.Generic;
using CardboardCore.Utilities;

namespace CardboardCore.StateMachines
{
    /// <summary>
    /// Simple to use state machine. Create Transitions from State and to State and set an initial State before starting it.
    /// Use "ToNextState" to Transition to the next state, if this Transition is available.
    /// </summary>
    public abstract class StateMachine
    {
        private Dictionary<Type, State> stateDict =
            new Dictionary<Type, State>();

        private Dictionary<State, KeyValuePair<State, Transition>> transitionDict =
            new Dictionary<State, KeyValuePair<State, Transition>>();

        private State initialState;
        private State currentState;

        public event Action StartedEvent;
        public event Action StoppedEvent;

        /// <summary>
        /// Creates a new State, or gets an existing one if this state type already exists
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private State CreateState<T>()
            where T : State, new()
        {
            Type type = typeof(T);

            if (stateDict.ContainsKey(type))
            {
                return stateDict[type];
            }

            T state = new T();
            stateDict[type] = state;

            return state;
        }

        /// <summary>
        /// Gets a state. Set "catchException" to true to fail if State does not exist
        /// </summary>
        /// <param name="catchException"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private State GetState<T>(bool catchException = false)
            where T : State, new()
        {
            Type type = typeof(T);
            if (stateDict.ContainsKey(type))
            {
                return stateDict[type];
            }

            if (catchException)
            {
                throw new Exception($"State of type {type.Name} could not be found!");
            }
            else
            {
                return CreateState<T>();
            }
        }

        /// <summary>
        /// Get a Transition. Set "catchException" to true to fail if Transition does not exist
        /// </summary>
        /// <param name="catchException"></param>
        /// <typeparam name="TFrom"></typeparam>
        /// <typeparam name="TTo"></typeparam>
        /// <returns></returns>
        private Transition GetTransition<TFrom, TTo>(bool catchException = false)
            where TFrom : State, new()
            where TTo : State, new()
        {
            State fromState = GetState<TFrom>(true);
            State toState = GetState<TTo>(true);

            return GetTransition(fromState, toState, catchException);
        }

        /// <summary>
        /// Get a Transition, from the current State. Set "catchException" to true to fail if Transition does not exist
        /// </summary>
        /// <param name="catchException"></param>
        /// <typeparam name="TTo"></typeparam>
        /// <returns></returns>
        private Transition GetTransition<TTo>(bool catchException = false)
            where TTo : State, new()
        {
            State toState = GetState<TTo>(true);

            return GetTransition(currentState, toState, catchException);
        }

        /// <summary>
        /// Get a Transition. Set "catchException" to true to fail if Transition does not exist
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="catchException"></param>
        /// <returns></returns>
        private Transition GetTransition(State from, State to, bool catchException = false)
        {
            if (transitionDict.ContainsKey(from)
                && transitionDict[from].Key == to)
            {
                return transitionDict[from].Value;
            }

            if (catchException)
            {
                throw new Exception($"Transition from State {from.GetType().Name} "
                    + $"to State {to.GetType().Name} does not exist!");
            }

            return null;
        }

        /// <summary>
        /// Add a transition from State A to State B
        /// </summary>
        /// <typeparam name="TFrom"></typeparam>
        /// <typeparam name="TTo"></typeparam>
        /// <returns></returns>
        protected void AddTransition<TFrom, TTo>()
            where TFrom : State, new()
            where TTo : State, new()
        {
            State from = CreateState<TFrom>();
            State to = CreateState<TTo>();

            Transition transition = GetTransition<TFrom, TTo>();
            if (transition == null)
            {
                transition = new Transition(from, to);
                transitionDict[from] = new KeyValuePair<State, Transition>(to, transition);
            }
        }

        /// <summary>
        /// Set the initial State for the start of the State Machine
        /// </summary>
        /// <typeparam name="T"></typeparam>
        protected void SetInitialState<T>()
            where T : State, new()
        {
            initialState = GetState<T>();

            if (initialState == null)
            {
                initialState = CreateState<T>();
            }
        }

        public void Start()
        {
            foreach (var item in stateDict)
            {
                item.Value.Initialize(this);
            }

            currentState = initialState ?? throw Log.Exception("Initial State is null!");
            currentState.Enter();
            
            StartedEvent?.Invoke();
        }

        public void Stop()
        {
            if (currentState == null)
            {
                return;
            }

            currentState.Exit();
            currentState = null;
            
            StoppedEvent?.Invoke();
        }

        // TODO: Check if we want to keep this method, seems useless if there's no support for splitting state flows
        public void ToState<T>()
            where T : State, new()
        {
            Transition transition = GetTransition<T>(true);
            transition.Do(out currentState);
        }

        public void ToNextState()
        {
            Transition transition = transitionDict[currentState].Value;
            transition.Do(out currentState);
        }
    }
}