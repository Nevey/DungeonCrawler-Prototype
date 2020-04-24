namespace CardboardCore.UI
{
    /// <summary>
    /// Extend from this to create your own UI Widget.
    /// Needs to be manually shown and hidden. Ignores UI Screen's show/hide calls.
    /// </summary>
    public abstract class UIWidget : UIView
    {
        protected override bool AllowDirectControl => true;
    }
}