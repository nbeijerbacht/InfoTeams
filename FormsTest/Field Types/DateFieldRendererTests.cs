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
    public class DateFieldRendererTests
    {
        private DateFieldRenderer render;

        public DateFieldRendererTests()
        {
            this.render = new DateFieldRenderer();
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
                    type = "date",
                    field_id = 123,
                    default_value = "current_date",
                },
            });

            outputs.Count().Should().Be(1);
            outputs.First().Should().BeOfType<AdaptiveDateInput>();
            var el = (AdaptiveDateInput)outputs.First();

            el.Label.Should().Be("name");
            el.Id.Should().Be("123");
            el.Value.Should().Be(DateTime.Now.ToString("yyyy-MM-dd"));
        }

        [Fact]
        public void CanRenderWithEmptyDefault()
        {
            var outputs = render.RenderElements(new Element
            {
                element_type = "field",
                text = "name",
                field = new Field
                {
                    type = "date",
                    field_id = 123,
                    default_value = "empty",
                },
            });

            outputs.Count().Should().Be(1);
            outputs.First().Should().BeOfType<AdaptiveDateInput>();
            var el = (AdaptiveDateInput)outputs.First();

            el.Label.Should().Be("name");
            el.Id.Should().Be("123");
            el.Value.Should().Be("");
        }

        [Fact]
        public void CanHandleDates()
        {
            var output = render.CanHandle(new Element
            {
                element_type = "field",
                field = new Field
                {
                    type = "date",
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
