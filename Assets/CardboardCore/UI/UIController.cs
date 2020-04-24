using System.Linq;
using CardboardCore.DI;
using CardboardCore.Utilities;
using UnityEngine;

namespace CardboardCore.UI
{
    /// <summary>
    /// Use this controller to show UIScreens and to show/hide UIWidgets.
    /// </summary>
    [Injectable(Singleton = true)]
    public class UIController : MonoBehaviour
    {
        private UIScreen[] screens;
        private UIWidget[] widgets;
        private UIScreen currentUIScreen;

        private void Awake()
        {
            screens = GetComponentsInChildren<UIScreen>(true);
            for (int i = 0; i < screens.Length; i++)
            {
                screens[i].gameObject.SetActive(false);
            }

            widgets = GetComponentsInChildren<UIWidget>(true);
            for (int i = 0; i < widgets.Length; i++)
            {
                widgets[i].gameObject.SetActive(false);
            }
        }

        public T ShowScreen<T>() where T : UIScreen
        {
            UIScreen newUIScreen = screens.FirstOrDefault(x => x.GetType() == typeof(T));

            if (newUIScreen == null)
            {
                throw Log.Exception(
                    $"Cannot find UIScreen {typeof(T).Name}. Did you add it as a child of the <b>UIController</b>?");
            }

            if (currentUIScreen != null)
            {
                currentUIScreen.Hide();
            }

            newUIScreen.Show();
            currentUIScreen = newUIScreen;

            return newUIScreen as T;
        }
        
        public T ShowWidget<T>() where T : UIWidget
        {
            UIWidget uiWidget = widgets.FirstOrDefault(x => x.GetType() == typeof(T));

            if (uiWidget == null)
            {
                return null;
            }

            uiWidget.Show();

            return uiWidget as T;
        }

        public void HideScreen<T>() where T : UIScreen
        {
            UIScreen uiScreen = screens.FirstOrDefault(x => x.GetType() == typeof(T));

            if (uiScreen == null)
            {
                return;
            }

            uiScreen.Hide();
        }

        public void HideWidget<T>() where T : UIWidget
        {
            UIWidget uiWidget = widgets.FirstOrDefault(x => x.GetType() == typeof(T));

            if (uiWidget == null)
            {
                return;
            }

            uiWidget.Hide();
        }

        public T GetScreen<T>() where T : UIScreen
        {
            return screens.FirstOrDefault(x => x.GetType() == typeof(T)) as T;
        }

        public T GetWidget<T>() where T : UIWidget
        {
            return widgets.FirstOrDefault(x => x.GetType() == typeof(T)) as T;
        }
    }
}