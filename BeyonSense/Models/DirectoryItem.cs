using Caliburn.Micro;

namespace BeyonSense
{
    /// <summary>
    /// Information about a directory item such as a drive, a file or a folder
    /// </summary>
    public class DirectoryItem : Screen
    {
        /// <summary>
        /// The type of this item
        /// </summary>
        /// 
        private DirectoryItemType type;
        public DirectoryItemType Type {
            get { return type; }
            set 
            {
                type = value;
                NotifyOfPropertyChange(() => Type);
            } 
        }

        /// <summary>
        /// The absolute path to this item
        /// </summary>
        /// 
        private string fullPath;
        public string FullPath {
            get { return fullPath; }
            set 
            {
                fullPath = value;
                NotifyOfPropertyChange(() => FullPath);
            } 
        }


        /// <summary>
        /// The name of this directory item
        /// </summary>
        public string Name { get { return DirectoryStructure.GetFileFolderName(this.FullPath); } }
    }
}
