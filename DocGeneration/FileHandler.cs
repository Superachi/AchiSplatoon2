using AchiSplatoon2.Helpers;
using System.IO;

namespace AchiSplatoon2.DocGeneration
{
    public static class FileHandler
    {
        public static void CopyDirectory(string sourceDir, string destinationDir, bool recursive, string fileExtension, bool overwriteDirectory = false)
        {
            DebugHelper.PrintInfo($"Copying files from '{sourceDir}' to '{destinationDir}' (recursive: {recursive.ToString()})");

            var directory = new DirectoryInfo(sourceDir);
            if (!directory.Exists)
            {
                DebugHelper.PrintError($"Source directory not found: {directory.FullName}");
                return;
            }
            DirectoryInfo[] directories = directory.GetDirectories();

            var destinationDirectory = new DirectoryInfo(destinationDir);

            if (!overwriteDirectory)
            {
                if (destinationDirectory.Exists)
                {
                    DebugHelper.PrintError($"Destination directory already exists: {destinationDirectory.FullName}");
                    return;
                }
            }
            else
            {
                if (destinationDirectory.Exists)
                {
                    DebugHelper.PrintWarning($"Deleting destination directory ({destinationDirectory.FullName}) to make room for new data");
                    destinationDirectory.Delete(true);
                }
            }

            Directory.CreateDirectory(destinationDir);

            // Get the files in the source directory and copy to the destination directory
            foreach (FileInfo file in directory.GetFiles())
            {
                if (file.Extension != fileExtension)
                {
                    continue;
                }

                string targetFilePath = Path.Combine(destinationDir, file.Name);
                file.CopyTo(targetFilePath);
            }

            // If recursive and copying subdirectories, recursively call this method
            if (recursive)
            {
                foreach (DirectoryInfo subDir in directories)
                {
                    string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                    CopyDirectory(subDir.FullName, newDestinationDir, true, fileExtension);
                }
            }
        }
    }
}
