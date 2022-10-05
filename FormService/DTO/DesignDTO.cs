using static FormService.DTO.DesignDTO;

namespace FormService.DTO;

public class DesignDTO
{
    public List<Element> elements { get; set; }
    public List<object> conditions { get; set; }
}

public class Element
{
    public int element_id { get; set; }
    public string element_type { get; set; }
    public int column_index { get; set; }
    public string text { get; set; }
    public bool on_hidden_row { get; set; }
    public int? column_span { get; set; }
    public int? whitespace_after { get; set; }
    public int? whitespace_before { get; set; }
    public Field field { get; set; }
}

public class Field
{
    public string type { get; set; }
    public int field_id { get; set; }
    public bool required { get; set; }
    public bool read_only { get; set; }
    public object default_value { get; set; }
    public string description { get; set; }
    public int text_lines { get; set; }
    public string name { get; set; }
    public bool? only_leaves { get; set; }
    public string initial_state { get; set; }
    public bool? hide_on_new_report { get; set; }
}
