using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Trendyol.CaseStudy.Common.Exceptions;
using Trendyol.CaseStudy.Common.Messages;

namespace Trendyol.CaseStudy.Providers.Factories
{
    public class MailProviderFactory : IMailProviderFactory
    {
        private readonly IDictionary<MailContentType, Type> _registeredImplementation;
        private readonly ILifetimeScope _scope;

        public MailProviderFactory(ILifetimeScope scope)
        {
            _scope = scope;
            _registeredImplementation = new Dictionary<MailContentType, Type>();
        }

        public IMailProvider GetProvider(MailContentType contentType)
        {
            var providerType = Find(contentType);

            return _scope.ResolveNamed<IMailProvider>(providerType.Name);
        }

        public IMailProvider GetProvider(MailContent mailContent)
        {
            return GetProvider(mailContent.ContentType);
        }

        /// dynamic factory pattern alos can be implemented here
        private Type Find(MailContentType contentType)
        {
            if (_registeredImplementation.ContainsKey(contentType))
                return _registeredImplementation[contentType];


            var assembly = Assembly.GetAssembly(typeof(IMailProvider));

            var types = from type in assembly.GetTypes()
                where Attribute.IsDefined(type, typeof(ContentTypeAttribute))
                select type;

            var providerType = types.Select(p => p).FirstOrDefault(t => t
                .GetCustomAttributes(typeof(ContentTypeAttribute), false)
                .Any(att => ((ContentTypeAttribute) att).Match(contentType)));


            if (providerType == null)
                throw new HandledErrorException(
                    "Could not find a IMailProvider implementation for given MailContentType");

            if (!_registeredImplementation.ContainsKey(contentType))
                _registeredImplementation.Add(contentType, providerType);

            return providerType;
        }
    }
}