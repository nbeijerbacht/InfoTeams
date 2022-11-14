namespace ZenyaFacadeService.DTO;
public class ExternalSourceDTO
{
    public List<Values> Rows { get; set; }
}

public class Values
{
    public string field_name { get; set; }
    public string field_value { get; set; }
}