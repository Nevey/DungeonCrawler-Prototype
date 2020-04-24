using CardboardCore.DI;

namespace CardboardCore.UI
{
    /// <summary>
    /// Core object for UIController to use.
    /// Extend from this class to add new ways of handling UI, much like UIScreen and UIWidget.
    /// </summary>
    public abstract class UIView : CardboardCoreBehaviour
    {
        // TODO: Check if UIController called Show/Hide, or someone else
        protected abstract bool AllowDirectControl { get; }
        
        protected abstract void OnShow();
        protected abstract void OnHide();

        private enum VisibleState
        {
            Shown,
            Hidden
        }

        private VisibleState visibleState = VisibleState.Hidden;

        public void Show()
        {
            if (visibleState == VisibleState.Shown)
            {
                return;
            }

            visibleState = VisibleState.Shown;

            gameObject.SetActive(true);
            OnShow();
        }

        public void Hide()
        {
            if (visibleState == VisibleState.Hidden)
            {
                return;
            }

            visibleState = VisibleState.Hidden;

            gameObject.SetActive(false);
            OnHide();
        }
    }
}