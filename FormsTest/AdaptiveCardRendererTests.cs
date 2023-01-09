using FormService.Logic;
using FormService.DTO;
using FluentAssertions;
using AdaptiveCards;
using FormService.Exceptions;

namespace FormsTest;

public class AdaptiveCardRendererTests
{
    private Mock<IElementRenderer> textRenderer;
    private Element textElement;
    private AdaptiveTextBlock textAdaptiveBlock;
    private Mock<IElementRenderer> numberRenderer;
    private Element numberElement;
    private AdaptiveNumberInput numberBlock;
    private AdaptiveCardRenderer renderer;

    public AdaptiveCardRendererTests()
    {
        this.textRenderer = new Mock<IElementRenderer>();
        this.textElement = new Element { text = "text" };
        this.textAdaptiveBlock = new AdaptiveTextBlock();

        this.numberRenderer = new Mock<IElementRenderer>();
        this.numberElement = new Element { text = "number" };
        this.numberBlock = new AdaptiveNumberInput();

        // mocks
        this.textRenderer.Setup(r => r.CanHandle(this.textElement)).Returns(true);
        this.textRenderer
            .Setup(r => r.RenderElements(this.textElement))
            .Returns(new AdaptiveElement[] { this.textAdaptiveBlock });

        this.numberRenderer.Setup(r => r.CanHandle(this.numberElement)).Returns(true);
        this.numberRenderer
            .Setup(r => r.RenderElements(this.numberElement))
            .Returns(new AdaptiveElement[] { this.numberBlock });
        
        // test target
        this.renderer = new AdaptiveCardRenderer(new IElementRenderer[]
        {
            this.textRenderer.Object,
            this.numberRenderer.Object,
        });
    }

    [Fact]
    public void RendersElementsInOrder()
    {
        var card = this.renderer.Render(new ReportFormDTO
        {
            form_id = 42,
            design = new DesignDTO
            {
                elements =
                {
                   this.textElement,
                }
            }
        });

        var id_field = new AdaptiveTextInput
        {
            IsVisible = false,
            Id = "form_id",
            Value = "42",
        };

        card.Body.Count.Should().Be(2);
        card.Body[0].Should().BeSameAs(this.textAdaptiveBlock);
        card.Body[1].Should().BeEquivalentTo(id_field);

        card = this.renderer.Render(new ReportFormDTO
        {
            form_id = 42,
            design = new DesignDTO
            {
                elements =
                {
                   this.textElement,
                   this.numberElement,
                }
            }
        });

        card.Body.Count.Should().Be(3);
        card.Body[0].Should().BeSameAs(this.textAdaptiveBlock);
        card.Body[1].Should().BeSameAs(this.numberBlock);
        card.Body[2].Should().BeEquivalentTo(id_field);

        // switch them the other way around
        card = this.renderer.Render(new ReportFormDTO
        {
            form_id = 42,
            design = new DesignDTO
            {
                elements =
                {
                   this.numberElement,
                   this.textElement,
                }
            }
        });

        card.Body.Count.Should().Be(3);
        card.Body[0].Should().BeSameAs(this.numberBlock);
        card.Body[1].Should().BeSameAs(this.textAdaptiveBlock);
        card.Body[2].Should().BeEquivalentTo(id_field);
    }

    [Fact]
    public void HandlesUnhandlableFieldsGracefully()
    {
        var card = this.renderer.Render(new ReportFormDTO
        {
            design = new DesignDTO
            {
                elements =
                {
                    new Element()
                    {
                        element_type = "some_type",
                        field = new Field
                        {
                            type = "some_field_type",
                        }
                    },
                }
            }
        });

        var text = card.Body.First();
        text.Should().BeOfType<AdaptiveTextBlock>();
        (text as AdaptiveTextBlock)!.Text.Should().ContainAll("Cannot render", "some_type", "some_field_type");
    }
}