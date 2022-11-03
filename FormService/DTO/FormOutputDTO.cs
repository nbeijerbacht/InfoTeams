namespace FormService.DTO;

public class FormOutputDTO
{
    public string form_id { get; set; }

    public List<FieldOutput> fields { get; set; }
}

public class FieldOutput
{
    public FieldOutput(FieldInput field)
    {
        this.field_id = field.field_id;
        this.value = field.value;
    }

    public string field_id { get; set; }

    public object value { get; set; }
}
