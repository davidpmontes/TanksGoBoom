public class TitleMenu : Menu<TitleMenu>
{
    protected override void Awake()
    {
        base.Awake();
    }

    public static void Show()
    {
        Open();
    }

    public static void Hide()
    {
        Close();
    }

    public override void OnBackPressed()
    {
    }

    public override void OnEnterPressed()
    {
    }

    public override void OnLeftPressed()
    {
    }

    public override void OnRightPressed()
    {
    }
}
