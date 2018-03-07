using System;
using Trendyol.CaseStudy.Common.Messages;

namespace Trendyol.CaseStudy.Providers
{
    public class ContentTypeAttribute : Attribute
    {
        private readonly MailContentType[] _contentTypes;

        
        public ContentTypeAttribute(params MailContentType[] contentTypes)
        {
            _contentTypes = contentTypes;
        }
        
        /// <summary>
        /// Returns matched MailContentType using the contentType 
        /// </summary>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public virtual bool Match(MailContentType contentType)
        {
            if (_contentTypes != null)
                return Array.IndexOf(_contentTypes, contentType) != -1;

            return false;
        }
    }
}