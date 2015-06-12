using UnityEngine;
using System.Collections;
using Zenject;

public class GlobalTestInstaller : MonoInstaller
{
	public override void InstallBindings()
	{
		Container.Bind<Deaths>().ToSingle();
	}
}
