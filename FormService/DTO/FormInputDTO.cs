namespace FormService.DTO;

public class FormInput
{
    public string form_id { get; set; }

    public List<FieldInput> fields { get; set; }
}

public class FieldInput
{
    public string field_id { get; set; }
    public object value { get; set; }
}
