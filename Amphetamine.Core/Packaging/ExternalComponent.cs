namespace Amphetamine.Core.Packaging
{
    public class ExternalComponent
    {
        private string name;
        private string description;
        private string url;
        private string licenseUrl;

        public string Name
        {
            get => this.name;
            set => this.name = value;
        }

        public string Description
        {
            get => this.description;
            set => this.description = value;
        }

        public string Url
        {
            get => this.url;
            set => this.url = value;
        }

        public string LicenseUrl
        {
            get => this.licenseUrl;
            set => this.licenseUrl = value;
        }
    }
}