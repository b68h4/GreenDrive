namespace GreenDrive.Models
{

    public class GDriveConfiguration
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public bool EnableSharedDrive { get; set; }
        public bool EnableMainFolderCheck { get; set; }
        public string MainFolderId { get; set; }
        public string SharedDriveId { get; set; }
        public string AppName { get; set; }
        public string AuthFolder { get; set; }
    }

}