namespace ZenyaBot.DTO;

public class SubmitFormDTO
{
    public string form_id { get; set; }

    public List<FieldDTO> fields { get; set; }

    public bool is_draft { get; set; }
    
}

public class FieldDTO
{
    public string field_id { get; set; }
    public object value { get; set; }
}
