using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using testingform.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;
using static testingform.Controllers.HomeController;
using AdaptiveCards;

namespace testingform.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> IndexAsync()
        {
            HttpClient client = new HttpClient();
            var path = "https://msteams.zenya.work/api/cases/reporter_forms";
            var response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                //get list of reporter form
                var stringData = await response.Content.ReadAsStringAsync();
                var testinger = JsonConvert.DeserializeObject<List<Root>>(stringData);
                var test = new JsonResult (stringData);

                //get form design by form_id
                var number = testinger[0].form_id;
                var path2 = $"https://msteams.zenya.work/api/cases/reporter_forms/{number}?include_design=true";
                var response2 = await client.GetAsync(path2);
                var stringData2 = await response2.Content.ReadAsStringAsync();
                var testinger2 = JsonConvert.DeserializeObject<Root>(stringData2);

                var a = new AdaptiveCard(new AdaptiveSchemaVersion(1, 0));
                a.Body.Add(new AdaptiveTextBlock()
                {
                    Text = testinger2.title,
                    Size = AdaptiveTextSize.ExtraLarge,
                });

                return new JsonResult (a.ToJson);
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public class AllowedFramework
        {
            public string framework_id { get; set; }
            public string name { get; set; }
        }

        public class CardFile
        {
            public int card_file_id { get; set; }
            public string plural_name { get; set; }
            public string singular_name { get; set; }
        }

        public class CaseType
        {
            public int case_type_id { get; set; }
            public string name { get; set; }
        }

        public class Category
        {
            public int category_id { get; set; }
            public string name { get; set; }
        }

        public class Cell
        {
            public int x_id { get; set; }
            public int y_id { get; set; }
            public int value { get; set; }
            public int section { get; set; }
        }

        public class Child
        {
        }

        public class Condition
        {
            public int source_field_id { get; set; }
            public string condition_operator { get; set; }
            public Value value { get; set; }
            public List<int> target_element_ids { get; set; }
        }

        public class DefaultValue
        {

        }

        public class Design
        {
            public List<Element> elements { get; set; }
            public List<Condition> conditions { get; set; }
        }

        public class Element
        {
            public int element_id { get; set; }
            public string element_type { get; set; }
            public int column_span { get; set; }
            public int column_index { get; set; }
            public int whitespace_before { get; set; }
            public int whitespace_after { get; set; }
            public bool first_on_row { get; set; }
            public string text { get; set; }
            public Field field { get; set; }
            public Image image { get; set; }
            public bool on_hidden_row { get; set; }
        }

        public class Field
        {
            public string type { get; set; }
            public int field_id { get; set; }
            public bool required { get; set; }
            public bool read_only { get; set; }
            public object default_value { get; set; }
            public string description { get; set; }
            public bool allow_future_dates { get; set; }
            public bool allow_past_dates { get; set; }
            public string date_display_type { get; set; }
            public bool only_leaves { get; set; }
            public int min_numeric_value { get; set; }
            public int max_numeric_value { get; set; }
            public bool only_integers { get; set; }
            public bool multi_select { get; set; }
            public bool multi_line { get; set; }
            public bool show_default { get; set; }
            public string list_display_type { get; set; }
            public bool allow_other_viz { get; set; }
            public bool has_locations { get; set; }
            public int text_lines { get; set; }
            public int max_length { get; set; }
            public int user_filter_id { get; set; }
            public string name { get; set; }
            public RegularExpression regular_expression { get; set; }
            public List<ListItem> list_items { get; set; }
            public List<string> matrix_display_type { get; set; }
            public XField x_field { get; set; }
            public YField y_field { get; set; }
            public ZField z_field { get; set; }
            public List<RiskSection> risk_sections { get; set; }
            public List<Cell> cells { get; set; }
            public List<Field> fields { get; set; }
            public string operation { get; set; }
            public int external_source_id { get; set; }
            public List<LookupItem> lookup_items { get; set; }
            public SearchField search_field { get; set; }
            public string search_field_placeholder { get; set; }
            public string period_unit { get; set; }
            public CardFile card_file { get; set; }
            public FirstDate first_date { get; set; }
            public SecondDate second_date { get; set; }
            public string initial_state { get; set; }
            public bool allow_all_frameworks { get; set; }
            public List<AllowedFramework> allowed_frameworks { get; set; }
            public bool hide_on_new_report { get; set; }
        }

        public class Field2
        {
            public int field_id { get; set; }
            public string name { get; set; }
        }

        public class FirstDate
        {
            public int field_id { get; set; }
            public bool use_current_date { get; set; }
            public string name { get; set; }
        }

        public class GpsLocation
        {
            public int latitude { get; set; }
            public int longitude { get; set; }
        }

        public class Group
        {
            public int group_id { get; set; }
            public string form_type { get; set; }
            public bool active { get; set; }
            public Category category { get; set; }
            public Icon icon { get; set; }
            public string visibility { get; set; }
            public bool can_print_after_sending { get; set; }
            public bool can_mail_after_sending { get; set; }
            public bool show_navigate_to_case_after_sending { get; set; }
        }

        public class Icon
        {
            public string icon_color { get; set; }
            public string icon_material_design_name { get; set; }
        }

        public class Image
        {
            public string image_id { get; set; }
            public string size { get; set; }
            public string title { get; set; }
        }

        public class ListItem
        {
            public string external_source_unique_row_identifier { get; set; }
            public int parent_id { get; set; }
            public List<Child> children { get; set; }
            public int list_item_id { get; set; }
            public string name { get; set; }
            public int numeric_value { get; set; }
            public Location location { get; set; }
        }

        public class Location
        {
            public GpsLocation gps_Location { get; set; }
            public int location_id { get; set; }
            public string name { get; set; }
            public string country_code { get; set; }
        }

        public class LookupItem
        {
            public Field field { get; set; }
            public string external_field_name { get; set; }
            public bool required_lookup { get; set; }
        }

        public class RegularExpression
        {
            public int regex_id { get; set; }
            public string name { get; set; }
            public string regular_expression { get; set; }
            public string description { get; set; }
            public string validation_error_message { get; set; }
        }

        public class RiskSection
        {
            public int risk_section_id { get; set; }
            public int from { get; set; }
            public int to { get; set; }
            public string color { get; set; }
            public string name { get; set; }
        }

        public class Root
        {
            public string type { get; set; }
            public string description { get; set; }
            public string message_after_sending { get; set; }
            public Group group { get; set; }
            public string language { get; set; }
            public int form_id { get; set; }
            public string title { get; set; }
            public Design design { get; set; }
            public CaseType case_type { get; set; }
        }

        public class SearchField
        {
            public int field_id { get; set; }
            public string name { get; set; }
        }

        public class SecondDate
        {
            public int field_id { get; set; }
            public bool use_current_date { get; set; }
            public string name { get; set; }
        }

        public class Value
        {
        }

        public class XField
        {
            public int field_id { get; set; }
            public string name { get; set; }
        }

        public class YField
        {
            public int field_id { get; set; }
            public string name { get; set; }
        }

        public class ZField
        {
            public int field_id { get; set; }
            public string name { get; set; }
        }

        // Root myDeserializedClass = JsonConvert.DeserializeObject<List<Root>>(myJsonResponse);

        public class ReporterForms
        {
            public CaseType? case_type { get; set; }
        }

        public static async Task<List<CaseType>> GetFormList(string path)
        {
            HttpClient client = new HttpClient();
            path = "https://msteams.zenya.work/api/cases/reporter_forms";
            var response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                var stringData = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<CaseType>>(stringData);
            }
            return null;
        }
    }
}

    