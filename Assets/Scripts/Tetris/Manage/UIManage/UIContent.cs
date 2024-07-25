namespace Tetris.Manage.UIManage
{
    public struct UIContent
    {
        public UIType uiType;
        public string uiPath;
        
        public UIContent(UIType type, string path)
        {
            uiType = type;
            uiPath = path;
        }
    }
}