using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;
using System.Text.RegularExpressions;

namespace EnumerationExtension
{
    /// <summary>
    /// TextAdornment1 places red boxes behind all the "a"s in the editor window
    /// </summary>
    internal sealed class EnumerationAdornment
    {
        public const string Name = "EnumerationAdornment";

        private readonly TextHighlighter _textHighlighter;
        private readonly IAdornmentLayer _layer;    
        private readonly IWpfTextView _view;    

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumerationAdornment"/> class.
        /// </summary>
        /// <param name="view">Text view to create the adornment for</param>
        public EnumerationAdornment(IWpfTextView view)
        {
            if (view == null)
            {
                throw new ArgumentNullException("view");
            }

            _layer = view.GetAdornmentLayer(EnumerationAdornment.Name);

            _view = view;
            _view.LayoutChanged += OnLayoutChanged;

            _textHighlighter = new TextHighlighter();
        }

        /// <summary>
        /// Handles whenever the text displayed in the view changes by adding the adornment to any reformatted lines
        /// </summary>
        /// <remarks><para>This event is raised whenever the rendered text displayed in the <see cref="ITextView"/> changes.</para>
        /// <para>It is raised whenever the view does a layout (which happens when DisplayTextLineContainingBufferPosition is called or in response to text or classification changes).</para>
        /// <para>It is also raised whenever the view scrolls horizontally or when its size changes.</para>
        /// </remarks>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        internal void OnLayoutChanged(object sender, TextViewLayoutChangedEventArgs e)
        {
            foreach (ITextViewLine line in e.NewOrReformattedLines)
            {
                CreateVisuals(line);
            }
        }

        /// <summary>
        /// Adds the scarlet box behind the 'a' characters within the given line
        /// </summary>
        /// <param name="line">Line to add the adornments</param>
        private void CreateVisuals(ITextViewLine line)
        {
            var text = _view.TextSnapshot.GetText(line.Start, line.Length);
            var regex = new Regex("(// *TODO *)(.*)");
            var matches = regex.Matches(text);

            foreach (var match in matches)
            {
                var m = (Match)match;

                var todoTag = GetPosition(m.Groups[1], line);
                var todoText = GetPosition(m.Groups[2], line);

                SnapshotSpan span = new SnapshotSpan(_view.TextSnapshot, todoText);
                Geometry geometry = _view.TextViewLines.GetMarkerGeometry(span);
                if (geometry != null)
                {
                    Image image = _textHighlighter.Highlight(geometry);
                    _layer.AddAdornment(AdornmentPositioningBehavior.TextRelative, span, null, image, null);
                }
            }
        }
        
        private Span GetPosition(Group matchedPart, ITextViewLine line)
        {
            var startPosition = line.Start + matchedPart.Index;
            return Span.FromBounds(startPosition, startPosition + matchedPart.Length);
        }        
    }
}
