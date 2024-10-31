namespace Manage.LoadAssetsManage
{
    public class AssetsName
    {
        public static readonly AssetContent Menu = new AssetContent(AssetsType.prefab, "UI/UIMenu.prefab", PrefabParent.GameMenu);
        public static readonly AssetContent MainTetris = new AssetContent(AssetsType.prefab, "Game/UIMainTetris.prefab", PrefabParent.Game);
        public static readonly AssetContent ChooseLevel = new AssetContent(AssetsType.prefab, "UI/UIChooseLevel.prefab",PrefabParent.GameUI);
        public static readonly AssetContent ChangeVolume = new AssetContent(AssetsType.prefab, "UI/UIChangeVolume.prefab",PrefabParent.GameUI);
    }
}