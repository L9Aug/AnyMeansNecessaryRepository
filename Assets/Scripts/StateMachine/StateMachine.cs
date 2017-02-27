// State Machine interpreted from Adaberto's lecture slides in AI workshops (Interpreter: Tristan Bampton).

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SM
{

    public delegate void Action();

    public class StateMachine
    {

        public List<State> States = new List<State>();
        public State InitialState;
        public State CurrentState;

        private Transition triggeredTransition;
        private Transition ForcedTransition;

        /// <summary>
        /// Constructor for StateMachine
        /// </summary>
        /// <param name="initialState">The state that the machine should start on, if null will start at the first state passed in.</param>
        /// <param name="states">The states this machine will have.</param>
        public StateMachine(State initialState, List<State> states)
        {
            SetupMachine(initialState, states.ToArray());
        }

        /// <summary>
        /// Constructor for StateMachine
        /// </summary>
        /// <param name="initialState">The state that the machine should start on, if null will start at the first state passed in.</param>
        /// <param name="states">The states this machine will have.</param>
        public StateMachine(State initialState, params State[] states)
        {
            SetupMachine(initialState, states);
        }

        public void InitMachine()
        {
            CurrentState = InitialState;

            foreach (Action action in CurrentState.EntryActions)
            {
                action();
            }
        }

        public void ForceTransition(Transition trans)
        {
            ForcedTransition = trans;
            SMUpdate();
        }

        public void SMUpdate()
        {
            triggeredTransition = null;
            List<Action> ReturnList = new List<Action>();

            if (ForcedTransition == null)
            {
                // Go through each possible transition until one if found to be triggered.
                foreach (Transition transition in CurrentState.Transitions)
                {
                    if (transition.IsTriggered)
                    {
                        triggeredTransition = transition;
                        break;
                    }
                }
            }
            else
            {
                triggeredTransition = ForcedTransition;
                ForcedTransition = null;
            }

            // If a transition has been triggered queue up the necessary actions.
            if (triggeredTransition != null)
            {
                State targetState = triggeredTransition.TargetState;

                if (CurrentState.ExitActions.Count > 0)
                {
                    ReturnList.AddRange(CurrentState.ExitActions);
                }

                if (triggeredTransition.Actions.Count > 0)
                {
                    ReturnList.AddRange(triggeredTransition.Actions);
                }

                CurrentState = targetState;

                if (targetState.EntryActions.Count > 0)
                {
                    ReturnList.AddRange(targetState.EntryActions);
                }
            }
            else // If no transition has happened continue with this states actions.
            {
                if (CurrentState.Actions.Count > 0)
                {
                    ReturnList.AddRange(CurrentState.Actions);
                }
            }

            foreach (Action a in ReturnList)
            {
                a();
            }
        }

        public string GetCurrentState()
        {
            return CurrentState.Name;
        }

        private void SetupMachine(State initialState, State[] states)
        {
            States.AddRange(states);

            if (initialState != null)
            {
                InitialState = initialState;
            }
            else
            {
                InitialState = States[0];
            }
        }
    }

    public class State
    {
        public string Name;
        public List<Transition> Transitions = new List<Transition>();
        public List<Action> EntryActions = new List<Action>();
        public List<Action> Actions = new List<Action>();
        public List<Action> ExitActions = new List<Action>();

        /// <summary>
        /// Constructor for State
        /// </summary>
        /// <param name="name">The name of the state.</param>
        /// <param name="transitions">A list of transitions that this state has.</param>
        /// <param name="entryActions">A list of this states entry actions.</param>
        /// <param name="actions">A list of this states actions.</param>
        /// <param name="exitActions">A list of this states exit actions.</param>
        public State(string name, List<Transition> transitions, List<Action> entryActions, List<Action> actions, List<Action> exitActions)
        {
            SetupState(name,
                (transitions != null) ? transitions.ToArray() : null,
                (entryActions != null) ? entryActions.ToArray() : null,
                (actions != null) ? actions.ToArray() : null,
                (exitActions != null) ? exitActions.ToArray() : null);
        }

        /// <summary>
        /// Constructor for State
        /// </summary>
        /// <param name="name">The name of the state.</param>
        /// <param name="transitions">A list of transitions that this state has.</param>
        /// <param name="entryActions">A list of this states entry actions.</param>
        /// <param name="actions">A list of this states actions.</param>
        /// <param name="exitActions">A list of this states exit actions.</param>
        public State(string name, Transition[] transitions, Action[] entryActions, Action[] actions, Action[] exitActions)
        {
            SetupState(name, transitions, entryActions, actions, exitActions);
        }

        private void SetupState(string name, Transition[] transitions, Action[] entryActions, Action[] actions, Action[] exitActions)
        {
            Name = name;
            SetupTransitions(transitions);
            SetupEntryActions(entryActions);
            SetupActions(actions);
            SetupExitActions(exitActions);
        }

        private void SetupTransitions(Transition[] transitions)
        {
            if (transitions != null)
            {
                if (transitions.Length > 0)
                {
                    Transitions.AddRange(transitions);
                }
            }
        }

        private void SetupEntryActions(Action[] entryActions)
        {
            if (entryActions != null)
            {
                if (entryActions.Length > 0)
                {
                    EntryActions.AddRange(entryActions);
                }
            }
        }

        private void SetupActions(Action[] actions)
        {
            if (actions != null)
            {
                if (actions.Length > 0)
                {
                    Actions.AddRange(actions);
                }
            }
        }

        private void SetupExitActions(Action[] exitActions)
        {
            if (exitActions != null)
            {
                if (exitActions.Length > 0)
                {
                    ExitActions.AddRange(exitActions);
                }
            }
        }

    }

    public class Transition
    {
        public string Name;
        public List<Action> Actions = new List<Action>();
        public Condition.ICondition TransitionCondition;
        public State TargetState;

        /// <summary>
        /// Constructor for Transition (Don't forget to add the Target state after states have been made)
        /// </summary>
        /// <param name="name">The name of the transition</param>
        /// <param name="condition">The condition for the transition to fire.</param>
        /// <param name="actions">Any actions that should be performed whist transitioning.</param>
        public Transition(string name, Condition.ICondition condition, List<Action> actions)
        {
            SetupTransition(name, condition, actions.ToArray());
        }

        /// <summary>
        /// Constructor for Transition (Don't forget to add the Target state after states have been made)
        /// </summary>
        /// <param name="name">The name of the transition</param>
        /// <param name="condition">The condition for the transition to fire.</param>
        /// <param name="actions">Any actions that should be performed whist transitioning.</param>
        public Transition(string name, Condition.ICondition condition, params Action[] actions)
        {
            SetupTransition(name, condition, actions);
        }

        public void SetTargetState(State targetState)
        {
            TargetState = targetState;
        }

        public bool IsTriggered
        {
            get
            {
                return TransitionCondition.Test();
            }
        }

        private void SetupTransition(string name, Condition.ICondition condition, Action[] actions)
        {
            Name = name;
            TransitionCondition = condition;
            if (actions.Length > 0)
            {
                Actions.AddRange(actions);
            }
        }
    }

}


namespace Condition
{

    public interface ICondition
    {
        bool Test();
    }

    public class BoolCondition : ICondition
    {
        public delegate bool BoolParameter();
        public BoolParameter Condition;

        public BoolCondition() { }

        public BoolCondition(BoolParameter condition)
        {
            Condition = condition;
        }

        public bool Test()
        {
            return Condition();
        }
    }

    public class AndCondition : ICondition
    {
        public ICondition A;
        public ICondition B;

        public AndCondition() { }

        public AndCondition(ICondition a, ICondition b)
        {
            A = a;
            B = b;
        }

        public bool Test()
        {
            return A.Test() && B.Test();
        }
    }

    public class OrCondition : ICondition
    {
        public ICondition A;
        public ICondition B;

        public OrCondition() { }

        public OrCondition(ICondition a, ICondition b)
        {
            A = a;
            B = b;
        }

        public bool Test()
        {
            return A.Test() || B.Test();
        }
    }

    public class NotCondition : ICondition
    {
        public ICondition Condition;

        public NotCondition() { }

        public NotCondition(ICondition condition)
        {
            Condition = condition;
        }

        public bool Test()
        {
            return !Condition.Test();
        }
    }
}
