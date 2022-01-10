namespace FitnessApp01.Interfaces
{
    public interface IStatusBar
    {
        /// <summary>
        /// Nastaví barvu status baru
        /// </summary>
        /// <param name="hexColor">např. "#FFFFFF"</param>
        void SetStatusBarColor(string hexColor);
    }
}