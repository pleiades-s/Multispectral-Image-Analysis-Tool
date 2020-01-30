using Caliburn.Micro;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace BeyonSense
{
    /// <summary>
    /// A view model for each directory item
    /// </summary>
    public class DirectoryItemViewModel : Screen
    {
        #region Public Properties

        /// <summary>
        /// The type of this item
        /// </summary>
        /// 
        private DirectoryItemType type;
        public DirectoryItemType Type {
            get { return type; }
            set {
                type = value;
                NotifyOfPropertyChange(() => Type);
                    
                } 
        }

        public string ImageName => (Type == DirectoryItemType.File ? "file" : (IsExpanded ? "folder-open" : "folder-closed"));

        /// <summary>
        /// The full path to the item
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

        /// <summary>
        /// A list of all children contained inside this item
        /// </summary>
        /// 
        private ObservableCollection<DirectoryItemViewModel> children;
        public ObservableCollection<DirectoryItemViewModel> Children 
        {
            get { return children; }
            set 
            {
                children = value;
                NotifyOfPropertyChange(() => Children);
            }
        }

        /// <summary>
        /// Indicates if this item can be expanded
        /// </summary>
        public bool CanExpand { get { return this.Type != DirectoryItemType.File; } }

        /// <summary>
        /// Indicates if the current item is expanded or not
        /// </summary>
        public bool IsExpanded
        {
            get
            {
                return this.Children?.Count(f => f != null) > 0;
            }
            set
            {
                // If the UI tells us to expand...
                if (value == true)
                    // Find all children
                    Expand();
                // If the UI tells us to close
                else
                    this.ClearChildren();

                NotifyOfPropertyChange(() => IsExpanded);
            }
        }

        #endregion

        #region Public Commands

        /// <summary>
        /// The command to expand this item
        /// </summary>
        /// 
        private ICommand expandCommand;
        public ICommand ExpandCommand
        {
            get { return expandCommand; }
            set
            {
                expandCommand = value;
                NotifyOfPropertyChange(() => ExpandCommand);
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="fullPath">The full path of this item</param>
        /// <param name="type">The type of item</param>
        public DirectoryItemViewModel(string fullPath, DirectoryItemType type)
        {
            // Create commands
            this.ExpandCommand = new RelayCommand(Expand);

            // Set path and type
            this.FullPath = fullPath;
            this.Type = type;

            // Setup the children as needed
            this.ClearChildren();
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Removes all children from the list, adding a dummy item to show the expand icon if required
        /// </summary>
        private void ClearChildren()
        {
            // Clear items
            this.Children = new ObservableCollection<DirectoryItemViewModel>();

            // Show the expand arrow if we are not a file
            if (this.Type != DirectoryItemType.File)
                this.Children.Add(null);
        }

        #endregion

        /// <summary>
        ///  Expands this directory and finds all children
        /// </summary>
        private void Expand()
        {
            // We cannot expand a file
            if (this.Type == DirectoryItemType.File)
                return;

            // Find all children
            var children = DirectoryStructure.GetDirectoryContents(this.FullPath);
            this.Children = new ObservableCollection<DirectoryItemViewModel>(
                                children.Select(content => new DirectoryItemViewModel(content.FullPath, content.Type)));
        }
    }
}
