using UnityEngine;
using VContainer;
using VContainer.Unity;
using OpenCity.Player.FSM;
using OpenCity.Player.InputHandling;
using OpenCity.Player.CameraDirection;

namespace OpenCity.Player.Installers
{
    /// <summary>
    /// Composition root for the Player feature. The only file allowed to
    /// know about concrete implementations - everything else in the
    /// feature depends on interfaces only.
    /// </summary>
    public class PlayerLifetimeScope : LifetimeScope
    {
        [SerializeField] private PlayerLocomotionConfig locomotionConfig;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(locomotionConfig);
            builder.Register<IInputReader, InputReader>(Lifetime.Singleton);
            builder.Register<ICameraDirectionProvider, WorldAxisCameraDirectionProvider>(Lifetime.Singleton);
            builder.RegisterComponentInHierarchy<PlayerStateMachine>();
        }
    }
}
