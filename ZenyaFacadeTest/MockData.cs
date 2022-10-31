using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenyaFacadeTest
{
    public static class MockData
    {
        public static string mockAllForms = @"[
                                        {
                                            ""type"": ""full"",
                                            ""description"": ""Something to test with"",
                                            ""message_after_sending"": """",
                                            ""language"": ""nl - NL"",
                                            ""form_id"": 2216,
                                            ""title"": ""Miha Test Fontys"",
                                            ""design"": null
                                        },
                                        {
                                            ""type"": ""full"",
                                            ""description"": ""Testformulier voor random studenten"",
                                            ""message_after_sending"": """",
                                            ""language"": ""nl - NL"",
                                            ""form_id"": 2220,
                                            ""title"": ""something random"",
                                            ""design"": null
                                        }
                                        ]";
        public static string searchResult = @"[{
                                        ""type"": ""full"",
                                        ""description"": ""Something to test with"",
                                        ""message_after_sending"": """",
                                        ""language"": ""nl - NL"",
                                        ""form_id"": 2216,
                                        ""title"": ""Miha Test Fontys"",
                                        ""design"": null
                                      }]";
        public static string mockFormById = @"{
                                        ""type"": ""full"",
                                        ""description"": ""Something to test with"",
                                        ""message_after_sending"": """",
                                        ""language"": ""nl - NL"",
                                        ""form_id"": 2216,
                                        ""title"": ""Miha Test Fontys"",
                                        ""design"": null
                                      }";
    }
}
