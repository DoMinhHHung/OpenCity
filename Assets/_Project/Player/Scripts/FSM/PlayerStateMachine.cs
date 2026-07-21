using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using OpenCity.Player.InputHandling;
using OpenCity.Player.CameraDirection;

namespace OpenCity.Player.FSM
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerStateMachine : MonoBehaviour, IPlayerStateMachine
    {
        private readonly Dictionary<Type, IPlayerState> _states = new();
        private IPlayerState _currentState;

        private IInputReader _inputReader;
        private ICameraDirectionProvider _cameraDirectionProvider;

        public PlayerContext Context { get; private set; }

        [Inject]
        public void Construct(IInputReader inputReader, ICameraDirectionProvider cameraDirectionProvider)
        {
            _inputReader = inputReader;
            _cameraDirectionProvider = cameraDirectionProvider;
        }

        private void Awake()
        {
            var controller = GetComponent<CharacterController>();
            Context = new PlayerContext(controller, transform, _inputReader, _cameraDirectionProvider, this);
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
            _currentState?.Tick(Time.deltaTime);
        }
    }
}
