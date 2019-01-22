using UnityEngine;
using Zenject;

public class ManagerInstaller : MonoInstaller
{
    public GameObject FramePreafab;
    public GameObject BoardManager;
    public GameObject GameManager;
    
    public override void InstallBindings()
    {
        Container.BindFactory<int,int,PlayerClick, PlayerClick.Factory>().FromComponentInNewPrefab(FramePreafab).UnderTransformGroup("Board");
        Container.Bind<BoardManager>().FromComponentInNewPrefab(BoardManager).UnderTransformGroup("Manager").AsSingle().NonLazy();
        Container.Bind<GameManager>().FromComponentInNewPrefab(GameManager).UnderTransformGroup("Manager").AsSingle().NonLazy();
       
    }
}