using AdaptiveCards;
using FluentAssertions;
using FormService.DTO;
using FormService.Logic;
using FormService.Logic.Field_Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormsTest.FieldTypes
{
    public class ListFieldRendererTests
    {
        private ListFieldRenderer render;

        public ListFieldRendererTests()
        {
            this.render = new ListFieldRenderer();
        }

        [Fact]
        public void Renders()
        {
            var outputs = render.RenderElements(new Element
            {
                element_type= "field",
                text= "name",
                field = new Field
                {
                    type = "list",
                    field_id = 123,
                    list_items = new List<ListItem>
                    {
                        new ListItem { list_item_id = 1, name = "One" },
                        new ListItem { list_item_id = 2, name = "Two" },
                        new ListItem { list_item_id = 3, name = "Three" },
                    },
                },
            });

            outputs.Count().Should().Be(1);
            outputs.First().Should().BeOfType<AdaptiveChoiceSetInput>();
            var el = (AdaptiveChoiceSetInput)outputs.First();

            el.Label.Should().Be("name");
            el.Id.Should().Be("123");

            el.Choices.Should().BeEquivalentTo(new AdaptiveChoice[]
            {
                new (){ Value = "1", Title = "One" },
                new (){ Value = "2", Title = "Two" },
                new (){ Value = "3", Title = "Three" },
            });
        }

        [Fact]
        public void RendersWithDefaultValue()
        {
            var outputs = render.RenderElements(new Element
            {
                element_type = "field",
                text = "name",
                field = new Field
                {
                    type = "list",
                    field_id = 123,
                    list_items = new List<ListItem>
                    {
                        new ListItem { list_item_id = 1, name = "One" },
                        new ListItem { list_item_id = 2, name = "Two" },
                        new ListItem { list_item_id = 3, name = "Three" },
                    },
                    default_value = 3,
                },
            });

            outputs.Count().Should().Be(1);
            outputs.First().Should().BeOfType<AdaptiveChoiceSetInput>();
            var el = (AdaptiveChoiceSetInput)outputs.First();

            el.Label.Should().Be("name");
            el.Id.Should().Be("123");
            el.Value.Should().Be("3");
            el.Choices.Should().BeEquivalentTo(new AdaptiveChoice[]
            {
                new (){ Value = "1", Title = "One" },
                new (){ Value = "2", Title = "Two" },
                new (){ Value = "3", Title = "Three" },
            });
        }

        [Fact]
        public void CanHandleDates()
        {
            var output = render.CanHandle(new Element
            {
                element_type = "field",
                field = new Field
                {
                    type = "list",
                },
            });

            output.Should().Be(true);
        }

        [Fact]
        public void CantHandleNumericField()
        {
            var output = render.CanHandle(new Element
            {
                element_type = "field",
                field = new Field
                {
                    type = "numeric",
                },
            });

            output.Should().Be(false);
        }

        [Fact]
        public void CantHandleHeader()
        {
            var output = render.CanHandle(new Element
            {
                element_type = "header",
            });

            output.Should().Be(false);
        }
    }
}
