namespace UniDormMVC.Models
{
    public class Notification
    {
        public string? notifID { get; set; } // firebase unique id
        public string? notifTitle { get; set; }
        public string? notifText { get; set; }
    }
}
