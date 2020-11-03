namespace Booking.API.Model
{
     public class EmailModel
    {
        public string Email { get; set; }
        public NameEmail To { get; set; }
        public NameEmail Bcc { get; set; }
        public NameEmail Cc { get; set; }
        public string From { get; set; } = "info@vizbook.co.uk";
        public string BusinessNameForEmail { get; set; } = "Hotel Management System";
        public string Subject { get; set; }
        public string Text { get; set; }

        public EmailModel()
        {
            To = new NameEmail();
            Bcc = new NameEmail();
            Cc = new NameEmail();
        }
    }

    public class NameEmail
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
}