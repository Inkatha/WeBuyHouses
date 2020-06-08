namespace WeBuyHouses.Models
{
    public class Message : BaseColumns
    {
        public int MessageId { get; set; }
        public string FromPhoneNumber { get; set; }
        public string ToPhoneNumber { get; set; }
        public string Body { get; set; }
    }
}