﻿using AdaptiveCards;
using FormService.DTO;

namespace FormService.Logic
{
    public class IgnoreRenderer : IElementRenderer
    {
        private static string[] _ignoreElementTypes = new string[]
        {
            "page_break",
        };
        private static string[] _ignoreFieldTypes = new string[]
        {
        };
        public bool CanHandle(Element e) => _ignoreElementTypes.Contains(e.element_type) || _ignoreFieldTypes.Contains(e.field?.type);

        public IEnumerable<AdaptiveElement> RenderElements(Element e)
        {
            return Array.Empty<AdaptiveElement>();
        }
    }
}
