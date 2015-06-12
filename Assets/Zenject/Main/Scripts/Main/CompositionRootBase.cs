#if !ZEN_NOT_UNITY3D

#pragma warning disable 414
using ModestTree;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Zenject
{
	public abstract class CompositionRootBase : MonoBehaviour 
	{
		public MonoInstaller[] Installers = new MonoInstaller[0];
		protected DiContainer _container;
		protected IDependencyRoot _dependencyRoot = null;

		protected abstract void Initilize();

		protected void SetUp()
		{
			Initilize();
			_dependencyRoot = _container.Resolve<IDependencyRoot>();
		}

		protected virtual void CreateContainer(bool allowNullBindings, DiContainer parentContainer)
		{
			_container = new DiContainer();
			_container.AllowNullBindings = allowNullBindings;

			if(parentContainer != null)
				_container.FallbackProvider = new DiContainerProvider(parentContainer);
		}

		protected void InitializeInstallers(List<IInstaller> extraInstallers)
		{
			List<IInstaller> allInstallers = extraInstallers.Concat(Installers).ToList();
			CompositionRootHelper.InstallStandardInstaller(_container, this.gameObject);
			CompositionRootHelper.InstallSceneInstallers(_container, allInstallers);
		}
	}
}
#endif