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
		[SerializeField]
		private MonoInstaller[] _installers = new MonoInstaller[0];

		[SerializeField]
		private GameObject _groupInstallers;

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
			List<IInstaller> allInstallers = GetGroupInstallers().Concat(_installers).ToList();
			allInstallers = extraInstallers.Concat(allInstallers).ToList();
			CompositionRootHelper.InstallStandardInstaller(_container, this.gameObject);
			CompositionRootHelper.InstallSceneInstallers(_container, allInstallers);
		}

		private List<IInstaller> GetGroupInstallers()
		{
			if(_groupInstallers == null)
				return new List<IInstaller>();

			return new List<IInstaller>(_groupInstallers.GetComponents<MonoInstaller>());
		}
	}
}
#endif