using static FormService.DTO.Design;

namespace FormService.DTO
{
    public class Design
    {
            public List<Element> elements { get; set; }
            public List<Condition> conditions { get; set; }
    }

    public class CardFile
    {
        public int card_file_id { get; set; }
        public string plural_name { get; set; }
        public string singular_name { get; set; }
    }

    public class Condition
    {
        public int source_field_id { get; set; }
        public string condition_operator { get; set; }
        public Value value { get; set; }
        public List<int> target_element_ids { get; set; }
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
        public int text_lines { get; set; }
        public string name { get; set; }
        public bool? show_default { get; set; }
        public bool? multi_select { get; set; }
        public CardFile card_file { get; set; }
        public string description { get; set; }
        public string list_display_type { get; set; }
        public bool? allow_other_viz { get; set; }
        public bool? has_locations { get; set; }
        public List<ListItem> list_items { get; set; }
        public string default_value { get; set; }
        public bool? allow_future_dates { get; set; }
        public bool? allow_past_dates { get; set; }
        public string date_display_type { get; set; }
        public int? user_filter_id { get; set; }
        public bool? only_leaves { get; set; }
        public bool? allow_all_frameworks { get; set; }
        public string initial_state { get; set; }
        public bool? hide_on_new_report { get; set; }
    }

    public class ListItem
    {
        public int list_item_id { get; set; }
        public string name { get; set; }
    }

    public class Value
    {
        public List<int> list_item_ids { get; set; }
    }
}
