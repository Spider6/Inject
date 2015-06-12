#if !ZEN_NOT_UNITY3D

#pragma warning disable 414
using ModestTree;

using System;
using System.Collections.Generic;
using ModestTree.Util.Debugging;
using System.Linq;
using UnityEngine;

namespace Zenject
{
    // Define this class as a component of a top-level game object of your scene heirarchy
    // Then any children will get injected during resolve stage
	public sealed class CompositionRoot : CompositionRootBase
    {
        public static Action<DiContainer> BeforeInstallHooks;
        public static Action<DiContainer> AfterInstallHooks;

        public bool OnlyInjectWhenActive = true;
        public bool InjectFullScene = false;

        static List<IInstaller> _staticInstallers = new List<IInstaller>();

        public DiContainer Container
        {
            get { return _container;  }
        }

        // This method is used for cases where you need to create the CompositionRoot entirely in code
        // Necessary because the Awake() method is called immediately after AddComponent<CompositionRoot>
        // so there's no other way to add installers to it
        public static CompositionRoot AddComponent(GameObject gameObject, IInstaller rootInstaller)
        {
            return AddComponent(gameObject, new List<IInstaller>() { rootInstaller });
        }

        public static CompositionRoot AddComponent(GameObject gameObject, List<IInstaller> installers)
        {
            Assert.That(_staticInstallers.IsEmpty());
            _staticInstallers.AddRange(installers);
            return gameObject.AddComponent<CompositionRoot>();
        }

		private void Start()
		{
			SetUp();
		}

		protected override void Initilize ()
		{
			CreateContainer(false, GlobalCompositionRoot.Instance.Container, _staticInstallers);
			_staticInstallers.Clear();
			CheckInjectScene();
		}

		protected override void CreateContainer (bool allowNullBindings, DiContainer parentContainer)
		{
			base.CreateContainer (allowNullBindings, parentContainer);
			_container.Bind<CompositionRoot>().ToInstance(this);
		}

        public DiContainer CreateContainer( bool allowNullBindings, DiContainer parentContainer, List<IInstaller> extraInstallers)
        {
			CreateContainer(allowNullBindings, parentContainer);
			OnBeforeInitializeInstallers();
			InitializeInstallers(extraInstallers);
			OnAfterInitializeInstallers();
            return _container;
        }

		private void OnBeforeInitializeInstallers()
		{
			if (BeforeInstallHooks != null)
			{
				BeforeInstallHooks(_container);
				// Reset extra bindings for next time we change scenes
				BeforeInstallHooks = null;
			}
		}

		private void OnAfterInitializeInstallers()
		{
			if (AfterInstallHooks != null)
			{
				AfterInstallHooks(_container);
				// Reset extra bindings for next time we change scenes
				AfterInstallHooks = null;
			}
			
		}

		private void CheckInjectScene ()
		{
			if (InjectFullScene) 
			{
				var rootGameObjects = GameObject.FindObjectsOfType<Transform> ().Where (x => x.parent == null && x.GetComponent<GlobalCompositionRoot> () == null && (x.GetComponent<CompositionRoot> () == null || x == this.transform)).Select (x => x.gameObject).ToList ();
				foreach (var rootObj in rootGameObjects) 
				{
					_container.InjectGameObject (rootObj, true, !OnlyInjectWhenActive);
				}
			}
			else
			{
				_container.InjectGameObject (gameObject, true, !OnlyInjectWhenActive);
			}
		}
    }
}

#endif
