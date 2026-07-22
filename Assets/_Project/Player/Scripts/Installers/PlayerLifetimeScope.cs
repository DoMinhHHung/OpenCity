using UnityEngine;
using VContainer;
using VContainer.Unity;
using OpenCity.Player.FSM;
using OpenCity.Player.InputHandling;
using OpenCity.Player.CameraDirection;

namespace OpenCity.Player.Installers
{
    public class PlayerLifetimeScope : LifetimeScope
    {
        [SerializeField] private PlayerLocomotionConfig locomotionConfig;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(locomotionConfig);
            builder.Register<IInputReader, InputReader>(Lifetime.Singleton);
            builder.Register<ICameraDirectionProvider>(_ => new MainCameraDirectionProvider(UnityEngine.Camera.main.transform), Lifetime.Singleton);
            builder.RegisterComponentInHierarchy<PlayerStateMachine>();
        }
    }
}
