namespace FormService.DTO;

public class ReportFormDTO
{
    public string type { get; set; }
    public string description { get; set; }
    public string message_after_sending { get; set; }
    public string language { get; set; }
    public int form_id { get; set; }
    public string title { get; set; }
    public DesignDTO design { get; set; }
}
