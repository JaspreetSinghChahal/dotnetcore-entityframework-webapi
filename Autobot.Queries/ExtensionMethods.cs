namespace Autobot.Queries
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Converts success bool value to yes or no to filter in table
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="isSuccess"></param>
        /// <returns></returns>
        public static string ToSuccessText(this bool isSuccess)
        {
            string successText;
            if (isSuccess == true)
            {
                successText = "Yes";
            }
            else
            {
                successText = "No";
            }
            return successText;
        }
    }
}
