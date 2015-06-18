#if !ZEN_NOT_UNITY3D

#pragma warning disable 414
using ModestTree;

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Zenject
{
	public sealed class GlobalCompositionRoot : CompositionRootBase
    {
		public DiContainer Container
		{
			get 
			{

				return _container;  
			}
		}

        public static GlobalCompositionRoot Instance
        {
			get;
			private set;
        }

        public void Awake()
        {
			if(Instance == null)
				SetUp();
			else 
				DestroyImmediate(this.gameObject);
        }

		public DiContainer GetContainer(bool allowNullBindings)
		{
			Initilize();
			return _container;
		}

		protected override void CreateContainer (bool allowNullBindings, DiContainer parentContainer)
		{
			Assert.That(allowNullBindings || gameObject != null);
			base.CreateContainer (allowNullBindings, parentContainer);
		}

		protected override void Initilize()
		{
			DontDestroyOnLoad(this.gameObject);
			CreateContainer(false, null);
			InitializeInstallers(new List<IInstaller>());
			Instance = this;
		}
    }
}

#endif

