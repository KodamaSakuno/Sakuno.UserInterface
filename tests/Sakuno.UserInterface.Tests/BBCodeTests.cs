using Sakuno.UserInterface.Documents.BBCode;
using Xunit;

namespace Sakuno.UserInterface.BBCode.Tests
{
    public static class BBCodeTests
    {
        [Fact]
        public static void Empty()
        {
            var document = new Parser(string.Empty).Parse();

            Assert.Empty(document.Elements);
        }

        [Fact]
        public static void SimpleText()
        {
            var document = new Parser("A simple text.").Parse();
            Assert.Equal(1, document.Elements.Count);

            var textElement = document.Elements[0] as TextElement;
            Assert.NotNull(textElement);
            Assert.Equal("A simple text.", textElement.Text.ToString());
        }

        [Fact]
        public static void SimpleItalicTag()
        {
            var code = "[i][/i]";
            var document = new Parser(code).Parse();
            Assert.Equal(1, document.Elements.Count);

            var tagElement = document.Elements[0] as Tag;
            Assert.NotNull(tagElement);
            Assert.Equal("i", tagElement.Name);
            Assert.Null(tagElement.Child);
        }

        [Fact]
        public static void SimpleBoldTagWithText()
        {
            var code = "[b]A simple text.[/b]";
            var document = new Parser(code).Parse();
            Assert.Equal(1, document.Elements.Count);

            var tagElement = document.Elements[0] as Tag;
            Assert.NotNull(tagElement);
            Assert.Equal("b", tagElement.Name);

            var textElement = tagElement.Child as TextElement;
            Assert.NotNull(textElement);
            Assert.Equal("A simple text.", textElement.Text.ToString());
        }

        [Fact]
        public static void BoldTagWithMultipleElements()
        {
            var code = "[b]Text A[i]Text B[/i][/b]";
            var document = new Parser(code).Parse();
            Assert.Equal(1, document.Elements.Count);

            var boldTag = document.Elements[0] as Tag;
            Assert.NotNull(boldTag);
            Assert.Equal("b", boldTag.Name);

            var elementGroup = boldTag.Child as DocumentElementGroup;
            Assert.NotNull(elementGroup);
            Assert.Equal(2, elementGroup.Elements.Count);

            var textElement = elementGroup.Elements[0] as TextElement;
            Assert.NotNull(textElement);
            Assert.Equal("Text A", textElement.Text.ToString());

            var italicTag = elementGroup.Elements[1] as Tag;
            Assert.NotNull(italicTag);
            Assert.Equal("i", italicTag.Name);

            var childOfItalicTag = italicTag.Child as TextElement;
            Assert.NotNull(childOfItalicTag);
            Assert.Equal("Text B", childOfItalicTag.Text.ToString());
        }

        [Fact]
        public static void NestingTags()
        {
            var code = "[b][i][/i][/b]";
            var document = new Parser(code).Parse();
            Assert.Equal(1, document.Elements.Count);

            var boldTag = document.Elements[0] as Tag;
            Assert.NotNull(boldTag);
            Assert.Equal("b", boldTag.Name);

            var italicTag = boldTag.Child as Tag;
            Assert.NotNull(italicTag);
            Assert.Equal("i", italicTag.Name);
        }

        [Fact]
        public static void SimpleParameterizedTag()
        {
            var code = "[color=red]RED[/color]";
            var document = new Parser(code).Parse();
            Assert.Equal(1, document.Elements.Count);

            var colorTag = document.Elements[0] as Tag;
            Assert.NotNull(colorTag);
            Assert.Equal("color", colorTag.Name);

            var childText = colorTag.Child as TextElement;
            Assert.NotNull(childText);
            Assert.Equal("RED", childText.Text.ToString());

            var parameter = colorTag.Parameter as SimpleParameter;
            Assert.NotNull(parameter);
            Assert.Equal("red", parameter.Value.ToString());
        }
    }
}
