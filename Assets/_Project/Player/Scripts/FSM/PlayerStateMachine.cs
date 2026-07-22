using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using OpenCity.Player.InputHandling;
using OpenCity.Player.CameraDirection;
using OpenCity.Player.FSM.States;

namespace OpenCity.Player.FSM
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerStateMachine : MonoBehaviour, IPlayerStateMachine
    {
        private readonly Dictionary<Type, IPlayerState> _states = new();
        private IPlayerState _currentState;

        public PlayerContext Context { get; private set; }

        [Inject]
        public void Construct(IInputReader inputReader, ICameraDirectionProvider cameraDirectionProvider, PlayerLocomotionConfig config)
        {
            var controller = GetComponent<CharacterController>();
            Context = new PlayerContext(controller, transform, inputReader, cameraDirectionProvider, config, this);
        }

        private void Start()
        {
            RegisterState(new IdleState(Context));
            RegisterState(new WalkState(Context));
            SetInitialState<IdleState>();
        }

        public void RegisterState<TState>(TState state) where TState : IPlayerState
        {
            _states[typeof(TState)] = state;
        }

        public void SetInitialState<TState>() where TState : IPlayerState
        {
            if (!_states.TryGetValue(typeof(TState), out var state))
            {
                Debug.LogError($"[PlayerStateMachine] Cannot set initial state - {typeof(TState).Name} is not registered.");
                return;
            }

            _currentState = state;
            _currentState.Enter();
        }

        public void ChangeState<TState>() where TState : IPlayerState
        {
            if (_currentState is TState) return;

            if (!_states.TryGetValue(typeof(TState), out var nextState))
            {
                Debug.LogError($"[PlayerStateMachine] Cannot change state - {typeof(TState).Name} is not registered.");
                return;
            }

            _currentState?.Exit();
            _currentState = nextState;
            _currentState.Enter();
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime;

            _currentState?.Tick(deltaTime);
            ApplyGravity(deltaTime);

            Vector3 motion = Context.Motion.HorizontalVelocity + Vector3.up * Context.Motion.VerticalVelocity;
            Context.Controller.Move(motion * deltaTime);
        }

        private void ApplyGravity(float deltaTime)
        {
            if (Context.Controller.isGrounded && Context.Motion.VerticalVelocity < 0f)
            {
                Context.Motion.VerticalVelocity = Context.Config.GroundedStickVelocity;
            }

            Context.Motion.VerticalVelocity += Context.Config.Gravity * deltaTime;
        }
    }
}
