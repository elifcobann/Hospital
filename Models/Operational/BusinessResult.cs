namespace Hospital.Models.Operational{
    public class BusinessResult {
        public bool Result {get; set;}
        public string? ErrorMessage {get; set;}
        public string? InfoMessage {get; set;}
        public int RecordId {get; set;}
    }
}