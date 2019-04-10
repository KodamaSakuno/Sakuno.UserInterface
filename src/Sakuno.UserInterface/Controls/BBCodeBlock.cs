using Sakuno.UserInterface.Documents.BBCode;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using TextElement = Sakuno.UserInterface.Documents.BBCode.TextElement;

namespace Sakuno.UserInterface.BBCode
{
    [ContentProperty(nameof(Code))]
    public class BBCodeBlock : FrameworkElement
    {
        public static readonly DependencyProperty CodeProperty =
            DependencyProperty.Register(nameof(Code), typeof(string), typeof(BBCodeBlock),
                new FrameworkPropertyMetadata(string.Empty,
                    FrameworkPropertyMetadataOptions.AffectsMeasure,
                    OnCodeChanged));

        static void OnCodeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            ((BBCodeBlock)d).Update((string)e.NewValue);

        public string Code
        {
            get => (string)GetValue(CodeProperty);
            set => SetValue(CodeProperty, value);
        }

        protected override int VisualChildrenCount => 1;

        TextBlock _container = new TextBlock();

        protected override Visual GetVisualChild(int index) => _container;

        void Update(string code)
        {
            var document = new Parser(code).Parse();

            _container.Inlines.Clear();

            if (document.Elements.Count > 0)
                Construct(document);
        }
        void Construct(Document document)
        {
            foreach (var element in document.Elements)
                Construct(element, _container.Inlines);
        }
        void Construct(DocumentElement element, InlineCollection inlines)
        {
            if (element is TextElement textElement)
#if !NET462
                inlines.Add(new string(textElement.Text.Span));
#else
                inlines.Add(textElement.Text.ToString());
#endif
            else if (element is DocumentElementGroup group)
            {
                var span = new Span();

                foreach (var subElement in group.Elements)
                    Construct(subElement, span.Inlines);

                inlines.Add(span);
            }
            else if (element is Tag tag)
            {
                if (tag == KnownTags.Breakline)
                {
                    inlines.Add(new LineBreak());
                    return;
                }

                Span inline;

                if (tag.Name.OICEquals("b"))
                    inline = new Bold();
                else if (tag.Name.OICEquals("i"))
                    inline = new Italic();
                else if (tag.Name.OICEquals("u"))
                    inline = new Underline();
                else if (tag.Name.OICEquals("color"))
                {
#if !NET462
                    var colorString = new string(((SimpleParameter)tag.Parameter).Value.Span);
#else
                    var colorString = ((SimpleParameter)tag.Parameter).Value.ToString();
#endif
                    var brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(colorString));
                    brush.Freeze();

                    inline = new Span() { Foreground = brush };
                }
                else
                    throw new InvalidOperationException();

                Construct(tag.Child, inline.Inlines);

                inlines.Add(inline);
            }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            _container.Measure(availableSize);

            return _container.DesiredSize;
        }
        protected override Size ArrangeOverride(Size finalSize)
        {
            _container.Arrange(new Rect(finalSize));

            return finalSize;
        }
    }
}
