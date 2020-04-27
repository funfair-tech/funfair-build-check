namespace FunFair.BuildCheck
{
    public sealed class Project
    {
        public Project(string fileName, string displayName)
        {
            this.FileName = fileName;
            this.DisplayName = displayName;
        }

        public string FileName { get; }

        public string DisplayName { get; }
    }
}