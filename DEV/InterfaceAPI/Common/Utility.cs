namespace InterfaceAPI.CommonUtility
{
    public static class Utility
    {
        public static string GetFileExtension(string fileName)
        {
            int lastIndexOfDot = 0;
            string extension = string.Empty;

            if (fileName != null)
            {
                lastIndexOfDot = fileName.LastIndexOf('.');
                if (lastIndexOfDot != -1)
                {
                    extension = fileName.Substring(lastIndexOfDot, fileName.Length - lastIndexOfDot).ToLower();
                }
            }
            return extension;
        }
        public static string GetFileName(string fileName)
        {
            int lastIndexOfDot = 0;
            string name = string.Empty;

            if (fileName != null)
            {
                lastIndexOfDot = fileName.LastIndexOf('.');
                if (lastIndexOfDot != -1)
                {
                    name = fileName.Substring(0, lastIndexOfDot);
                }
            }
            return name.Trim();
        }
    }
}