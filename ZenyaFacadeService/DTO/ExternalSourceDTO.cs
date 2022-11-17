namespace ZenyaFacadeService.DTO;
public class ExternalSourceDTO
{
    public List<Row> rows { get; set; }
    public string result { get; set; }
}

public class Row
{
    public List<Value> values { get; set; }
}

public class Value
{
    public string field_name { get; set; }
    public string text_value { get; set; }
}