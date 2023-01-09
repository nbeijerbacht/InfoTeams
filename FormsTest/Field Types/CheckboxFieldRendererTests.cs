using AdaptiveCards;
using FluentAssertions;
using FormService.DTO;
using FormService.Logic.ElementRenderers.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormsTest.FieldTypes
{
    public class CheckboxFieldRendererTests
    {
        private CheckboxFieldRenderer render;

        public CheckboxFieldRendererTests()
        {
            this.render = new CheckboxFieldRenderer();
        }

        [Fact]
        public void Renders()
        {
            var outputs = render.RenderElements(new Element
            {
                element_type= "field",
                field = new Field
                {
                    type = "checkbox",
                    field_id = 123,
                    name = "name",
                    description = "description",
                    default_value = "true",
                },
            });

            outputs.Count().Should().Be(1);
            outputs.First().Should().BeOfType<AdaptiveToggleInput>();
            var el = (AdaptiveToggleInput)outputs.First();

            el.Title.Should().Be("name");
            el.Label.Should().Be("description");
            el.Id.Should().Be("123");
            el.Value.Should().Be("true");
        }

        [Fact]
        public void CanHandleCheckboxes()
        {
            var output = render.CanHandle(new Element
            {
                element_type = "field",
                field = new Field
                {
                    type = "checkbox",
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
