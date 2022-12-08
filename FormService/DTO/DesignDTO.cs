using Newtonsoft.Json.Linq;

namespace FormService.DTO;

public class DesignDTO
{
    public List<Element> elements { get; set; } = new List<Element>();
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
    public bool? isRequired { get; set; }
    public bool read_only { get; set; }
    public JToken default_value { get; set; }
    public string description { get; set; }
    public int text_lines { get; set; }
    public string name { get; set; }
    public bool? only_leaves { get; set; }
    public string initial_state { get; set; }
    public bool? hide_on_new_report { get; set; }

    // only for dropdowns
    public List<ListItem>? list_items { get; set; }

    // Only present for numeric fields
    public double? min_numeric_value { get; set; }
    public double? max_numeric_value { get; set; }
}

public class ListItem
{
    public int list_item_id { get; set; }
    public string name { get; set; }
    public double numeric_value { get; set; }
}
