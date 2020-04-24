namespace CardboardCore.UI
{
    /// <summary>
    /// Extend from this to create your own UI Screen.
    /// Only one UI Screen will be active at all times.
    /// </summary>
    public abstract class UIScreen : UIView
    {
        protected override bool AllowDirectControl => false;
    }
}