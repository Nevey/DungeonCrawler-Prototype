namespace DungeonCrawler.UserInput
{
    public enum InputDirection
    {
        Horizontal,
        Vertical
    }

    public static class InputDirectionExtensions
    {
        public static InputDirection Swap(this InputDirection inputDirection)
        {
            switch (inputDirection)
            {
                case InputDirection.Horizontal:
                    return InputDirection.Vertical;

                case InputDirection.Vertical:
                    return InputDirection.Horizontal;
            }

            return inputDirection;
        }
    }
}