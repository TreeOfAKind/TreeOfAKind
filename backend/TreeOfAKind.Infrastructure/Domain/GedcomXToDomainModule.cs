using Autofac;
using TreeOfAKind.Application.DomainServices.GedcomXImport;

namespace TreeOfAKind.Infrastructure.Domain
{
    public class GedcomXToDomainModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<GedcomXDateExtractor>()
                .As<IGedcomXDateExtractor>()
                .InstancePerLifetimeScope();

            builder.RegisterType<GedcomXNameExtractor>()
                .As<IGedcomXNameExtractor>()
                .InstancePerLifetimeScope();

            builder.RegisterType<GedcomXNoteExtractor>()
                .As<IGedcomXNoteExtractor>()
                .InstancePerLifetimeScope();

            builder.RegisterType<GedcomXToDomainGenderConverter>()
                .As<IGedcomXToDomainGenderConverter>()
                .InstancePerLifetimeScope();

            builder.RegisterType<GedcomXToDomainRelationConverter>()
                .As<IGedcomXToDomainRelationConverter>()
                .InstancePerLifetimeScope();

            builder.RegisterType<GedcomXToDomainRelationTypeConverter>()
                .As<IGedcomXToDomainRelationTypeConverter>()
                .InstancePerLifetimeScope();

            builder.RegisterType<GedcomXToDomainTreeConverter>()
                .As<IGedcomXToDomainTreeConverter>()
                .InstancePerLifetimeScope();

            builder.RegisterType<XmlStreamToGedcomXParser>()
                .As<IXmlStreamToGedcomXParser>()
                .InstancePerLifetimeScope();
        }
    }
}
