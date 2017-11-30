using System.Collections.Generic;
using System.Linq;

namespace ScenarioGenerator
{
    // https://github.com/OrchardCMS/OrchardCore
    // Total Projects: 127
    // Total ProjectRefs: 529
    // Total PackageRefs: 237
    // Unique PackageRefs: 84

    public class OrchardCore : ISolution
    {
        public IList<(string Name, IEnumerable<string> ProjectReferences)> Projects => new List<(string Name, IEnumerable<string> ProjectReferences)>
        {
            // Rank 0
            ( "OrchardCore.Application.Targets", Enumerable.Empty<string>() ),
            ( "OrchardCore.BackgroundTasks.Abstractions", Enumerable.Empty<string>() ),
            ( "OrchardCore.Data.Abstractions", Enumerable.Empty<string>() ),
            ( "OrchardCore.DeferredTasks.Abstractions", Enumerable.Empty<string>() ),
            ( "OrchardCore.Entities", Enumerable.Empty<string>() ),
            ( "OrchardCore.Environment.Cache.Abstractions", Enumerable.Empty<string>() ),
            ( "OrchardCore.Environment.Extensions.Abstractions", Enumerable.Empty<string>() ),
            ( "OrchardCore.Liquid.Abstractions", Enumerable.Empty<string>() ),
            ( "OrchardCore.Localization.Abstractions", Enumerable.Empty<string>() ),
            ( "OrchardCore.Lucene.Abstractions", Enumerable.Empty<string>() ),
            ( "OrchardCore.Modules.Abstractions", Enumerable.Empty<string>() ),
            ( "OrchardCore.Parser.Yaml", Enumerable.Empty<string>() ),
            ( "OrchardCore.Queries.Abstractions", Enumerable.Empty<string>() ),
            ( "OrchardCore.Recipes.Abstractions", Enumerable.Empty<string>() ),
            ( "OrchardCore.ResourceManagement.Abstractions", Enumerable.Empty<string>() ),
            ( "OrchardCore.Scripting.Abstractions", Enumerable.Empty<string>() ),
            ( "OrchardCore.Security.Abstractions", Enumerable.Empty<string>() ),
            ( "OrchardCore.Setup.Abstractions", Enumerable.Empty<string>() ),
            ( "OrchardCore.StorageProviders.Abstractions", Enumerable.Empty<string>() ),
            ( "OrchardCore.XmlRpc.Abstractions", Enumerable.Empty<string>() ),

            // Rank 1
            ( "OrchardCore.Admin.Abstractions", new string[] { "OrchardCore.Environment.Extensions.Abstractions", "OrchardCore.Security.Abstractions", } ),
            ( "OrchardCore.Environment.Shell.Abstractions", new string[] { "OrchardCore.Environment.Extensions.Abstractions", } ),
            ( "OrchardCore.Localization.Core", new string[] { "OrchardCore.Localization.Abstractions", } ),
            ( "OrchardCore.Lucene.Core", new string[] { "OrchardCore.Lucene.Abstractions", "OrchardCore.Queries.Abstractions", } ),
            ( "OrchardCore.Module.Targets", new string[] { "OrchardCore.Modules.Abstractions", } ),
            ( "OrchardCore.Mvc.Abstractions", new string[] { "OrchardCore.Entities", "OrchardCore.Modules.Abstractions", } ),
            ( "OrchardCore.ResourceManagement", new string[] { "OrchardCore.ResourceManagement.Abstractions", } ),
            ( "OrchardCore.Scripting.Core", new string[] { "OrchardCore.Scripting.Abstractions", } ),
            ( "OrchardCore.Scripting.JavaScript", new string[] { "OrchardCore.Scripting.Abstractions", } ),
            ( "OrchardCore.StorageProviders.FileSystem", new string[] { "OrchardCore.StorageProviders.Abstractions", } ),
            ( "OrchardCore.Users.Abstractions", new string[] { "OrchardCore.Security.Abstractions", } ),

            // Rank 2
            ( "OrchardCore.BackgroundTasks", new string[] { "OrchardCore.BackgroundTasks.Abstractions", "OrchardCore.Environment.Shell.Abstractions", "OrchardCore.Modules.Abstractions", } ),
            ( "OrchardCore.DeferredTasks", new string[] { "OrchardCore.DeferredTasks.Abstractions", "OrchardCore.Environment.Shell.Abstractions", } ),
            ( "OrchardCore.Diagnostics.Elm", new string[] { "OrchardCore.Module.Targets", } ),
            ( "OrchardCore.Environment.Commands", new string[] { "OrchardCore.Environment.Shell.Abstractions", "OrchardCore.Modules.Abstractions", } ),
            ( "OrchardCore.Environment.Extensions", new string[] { "OrchardCore.Environment.Cache.Abstractions", "OrchardCore.Environment.Extensions.Abstractions", "OrchardCore.Environment.Shell.Abstractions", "OrchardCore.Modules.Abstractions", "OrchardCore.Parser.Yaml", } ),
            ( "OrchardCore.Environment.Shell", new string[] { "OrchardCore.DeferredTasks.Abstractions", "OrchardCore.Environment.Extensions.Abstractions", "OrchardCore.Environment.Shell.Abstractions", "OrchardCore.Modules.Abstractions", "OrchardCore.Parser.Yaml", } ),
            ( "OrchardCore.Feeds.Abstractions", new string[] { "OrchardCore.Mvc.Abstractions", } ),
            ( "OrchardCore.HomeRoute", new string[] { "OrchardCore.Module.Targets", "OrchardCore.Mvc.Abstractions", } ),
            ( "OrchardCore.Logging.NLog", new string[] { "OrchardCore.Environment.Shell.Abstractions", } ),
            ( "OrchardCore.Mvc.Core", new string[] { "OrchardCore.Environment.Extensions.Abstractions", "OrchardCore.Environment.Shell.Abstractions", "OrchardCore.Modules.Abstractions", "OrchardCore.Mvc.Abstractions", } ),
            ( "OrchardCore.Recipes.Implementations", new string[] { "OrchardCore.DeferredTasks.Abstractions", "OrchardCore.Environment.Extensions.Abstractions", "OrchardCore.Environment.Shell.Abstractions", "OrchardCore.Modules.Abstractions", "OrchardCore.Recipes.Abstractions", "OrchardCore.Scripting.Abstractions", } ),
            ( "OrchardCore.ResponseCache", new string[] { "OrchardCore.Module.Targets", } ),
            ( "OrchardCore.Security", new string[] { "OrchardCore.Mvc.Abstractions", "OrchardCore.Security.Abstractions", } ),
            ( "OrchardCore.Theme.Targets", new string[] { "OrchardCore.Module.Targets", } ),
            ( "OrchardCore.Users.Core", new string[] { "OrchardCore.Entities", "OrchardCore.Security.Abstractions", "OrchardCore.Users.Abstractions", } ),
            ( "OrchardCore.XmlRpc", new string[] { "OrchardCore.Environment.Extensions.Abstractions", "OrchardCore.Module.Targets", "OrchardCore.Mvc.Abstractions", "OrchardCore.XmlRpc.Abstractions", } ),

            // Rank 3
            ( "OrchardCore.ContentManagement.Abstractions", new string[] { "OrchardCore.Mvc.Core", } ),
            ( "OrchardCore.Data", new string[] { "OrchardCore.Data.Abstractions", "OrchardCore.Environment.Extensions.Abstractions", "OrchardCore.Environment.Shell", "OrchardCore.Modules.Abstractions", } ),
            ( "OrchardCore.DisplayManagement", new string[] { "OrchardCore.Environment.Cache.Abstractions", "OrchardCore.Environment.Extensions.Abstractions", "OrchardCore.Environment.Shell.Abstractions", "OrchardCore.Modules.Abstractions", "OrchardCore.Mvc.Core", } ),
            ( "OrchardCore.Environment.Navigation", new string[] { "OrchardCore.Environment.Shell", "OrchardCore.Security", } ),
            ( "OrchardCore.Environment.Shell.Data", new string[] { "OrchardCore.Data.Abstractions", "OrchardCore.Environment.Shell", } ),
            ( "OrchardCore.Feeds.Core", new string[] { "OrchardCore.Feeds.Abstractions", } ),
            ( "OrchardCore.Hosting.Console", new string[] { "OrchardCore.Environment.Commands", } ),
            ( "OrchardCore.Modules", new string[] { "OrchardCore.Environment.Extensions", "OrchardCore.Environment.Shell", "OrchardCore.Modules.Abstractions", } ),
            ( "OrchardCore.Mvc.HelloWorld", new string[] { "OrchardCore.Module.Targets", "OrchardCore.Mvc.Core", } ),
            ( "OrchardCore.Scripting", new string[] { "OrchardCore.Module.Targets", "OrchardCore.Mvc.Core", "OrchardCore.Scripting.Core", "OrchardCore.Scripting.JavaScript", } ),

            // Rank 4
            ( "OrchardCore.ContentManagement", new string[] { "OrchardCore.ContentManagement.Abstractions", "OrchardCore.Data.Abstractions", "OrchardCore.Environment.Cache.Abstractions", "OrchardCore.Mvc.Abstractions", "OrchardCore.Recipes.Abstractions", } ),
            ( "OrchardCore.ContentManagement.Display", new string[] { "OrchardCore.ContentManagement.Abstractions", "OrchardCore.DisplayManagement", } ),
            ( "OrchardCore.Deployment.Abstractions", new string[] { "OrchardCore.DisplayManagement", "OrchardCore.Recipes.Abstractions", } ),
            ( "OrchardCore.Diagnostics", new string[] { "OrchardCore.DisplayManagement", "OrchardCore.Module.Targets", "OrchardCore.Mvc.Abstractions", } ),
            ( "OrchardCore.DisplayManagement.Liquid", new string[] { "OrchardCore.DisplayManagement", "OrchardCore.Liquid.Abstractions", } ),
            ( "OrchardCore.Entities.DisplayManagement", new string[] { "OrchardCore.DisplayManagement", "OrchardCore.Entities", } ),
            ( "OrchardCore.Environment.Cache", new string[] { "OrchardCore.Environment.Cache.Abstractions", "OrchardCore.Environment.Extensions.Abstractions", "OrchardCore.Modules", } ),
            ( "OrchardCore.Feeds", new string[] { "OrchardCore.Feeds.Core", "OrchardCore.Module.Targets", "OrchardCore.Mvc.Abstractions", } ),
            ( "OrchardCore.Indexing.Abstractions", new string[] { "OrchardCore.ContentManagement.Abstractions", } ),
            ( "OrchardCore.Media.Abstractions", new string[] { "OrchardCore.ContentManagement.Abstractions", "OrchardCore.StorageProviders.Abstractions", } ),
            ( "OrchardCore.Menu.Abstractions", new string[] { "OrchardCore.ContentManagement.Abstractions", } ),
            ( "OrchardCore.MetaWeblog.Abstractions", new string[] { "OrchardCore.ContentManagement.Abstractions", "OrchardCore.XmlRpc.Abstractions", } ),
            ( "OrchardCore.Mvc", new string[] { "OrchardCore.Module.Targets", "OrchardCore.Modules", "OrchardCore.Mvc.Core", } ),
            ( "OrchardCore.Nancy.Core", new string[] { "OrchardCore.Modules", } ),
            ( "OrchardCore.Navigation", new string[] { "OrchardCore.Data.Abstractions", "OrchardCore.DisplayManagement", "OrchardCore.Environment.Navigation", "OrchardCore.Module.Targets", "OrchardCore.Security", } ),
            ( "OrchardCore.Roles", new string[] { "OrchardCore.Data.Abstractions", "OrchardCore.DisplayManagement", "OrchardCore.Environment.Navigation", "OrchardCore.Module.Targets", "OrchardCore.Recipes.Abstractions", "OrchardCore.Security.Abstractions", } ),
            ( "OrchardCore.Settings", new string[] { "OrchardCore.Data.Abstractions", "OrchardCore.DisplayManagement", "OrchardCore.Entities", "OrchardCore.Environment.Navigation", "OrchardCore.Liquid.Abstractions", "OrchardCore.Module.Targets", "OrchardCore.Recipes.Abstractions", "OrchardCore.Setup.Abstractions", } ),
            ( "OrchardCore.Setup", new string[] { "OrchardCore.Data", "OrchardCore.DisplayManagement", "OrchardCore.Module.Targets", "OrchardCore.Mvc.Abstractions", "OrchardCore.Recipes.Abstractions", "OrchardCore.Setup.Abstractions", } ),
            ( "OrchardCore.Tenants", new string[] { "OrchardCore.Admin.Abstractions", "OrchardCore.DisplayManagement", "OrchardCore.Environment.Navigation", "OrchardCore.Module.Targets", } ),
            ( "SafeMode", new string[] { "OrchardCore.DisplayManagement", "OrchardCore.Theme.Targets", } ),
            ( "TheAdmin", new string[] { "OrchardCore.DisplayManagement", "OrchardCore.Theme.Targets", } ),
            ( "TheAgencyTheme", new string[] { "OrchardCore.DisplayManagement", "OrchardCore.Theme.Targets", } ),

            // Rank 5
            ( "OrchardCore.Admin", new string[] { "OrchardCore.Admin.Abstractions", "OrchardCore.Data.Abstractions", "OrchardCore.DisplayManagement", "OrchardCore.Environment.Navigation", "OrchardCore.Module.Targets", "OrchardCore.Settings", } ),
            ( "OrchardCore.Application.Mvc.Targets", new string[] { "OrchardCore.Application.Targets", "OrchardCore.Mvc", } ),
            ( "OrchardCore.Commons", new string[] { "OrchardCore.BackgroundTasks", "OrchardCore.Data", "OrchardCore.DeferredTasks", "OrchardCore.DisplayManagement", "OrchardCore.DisplayManagement.Liquid", "OrchardCore.Environment.Cache", "OrchardCore.Environment.Extensions", "OrchardCore.Environment.Shell.Data", "OrchardCore.Module.Targets", "OrchardCore.ResourceManagement", } ),
            ( "OrchardCore.ContentPreview", new string[] { "OrchardCore.ContentManagement.Abstractions", "OrchardCore.ContentManagement.Display", "OrchardCore.Module.Targets", "OrchardCore.Security", "OrchardCore.Settings", } ),
            ( "OrchardCore.ContentTypes.Abstractions", new string[] { "OrchardCore.ContentManagement.Abstractions", "OrchardCore.ContentManagement.Display", } ),
            ( "OrchardCore.Deployment.Core", new string[] { "OrchardCore.Deployment.Abstractions", } ),
            ( "OrchardCore.DynamicCache", new string[] { "OrchardCore.ContentManagement", "OrchardCore.DisplayManagement", "OrchardCore.Environment.Cache.Abstractions", "OrchardCore.Environment.Navigation", "OrchardCore.Module.Targets", "OrchardCore.Mvc.Core", } ),
            ( "OrchardCore.Features", new string[] { "OrchardCore.Admin.Abstractions", "OrchardCore.ContentManagement.Abstractions", "OrchardCore.Deployment.Abstractions", "OrchardCore.DisplayManagement", "OrchardCore.Environment.Extensions.Abstractions", "OrchardCore.Environment.Navigation", "OrchardCore.Environment.Shell", "OrchardCore.Environment.Shell.Abstractions", "OrchardCore.Module.Targets", "OrchardCore.Mvc.Abstractions", "OrchardCore.Recipes.Abstractions", "OrchardCore.ResourceManagement.Abstractions", } ),
            ( "OrchardCore.Layers", new string[] { "OrchardCore.Admin.Abstractions", "OrchardCore.ContentManagement", "OrchardCore.ContentManagement.Abstractions", "OrchardCore.ContentManagement.Display", "OrchardCore.Data.Abstractions", "OrchardCore.Entities.DisplayManagement", "OrchardCore.Environment.Navigation", "OrchardCore.Module.Targets", "OrchardCore.Mvc.Abstractions", "OrchardCore.Recipes.Abstractions", "OrchardCore.Scripting.Abstractions", } ),
            ( "OrchardCore.Media.Services", new string[] { "OrchardCore.ContentManagement.Abstractions", "OrchardCore.Media.Abstractions", } ),
            ( "OrchardCore.Menu", new string[] { "OrchardCore.ContentManagement.Abstractions", "OrchardCore.ContentManagement.Display", "OrchardCore.Data.Abstractions", "OrchardCore.DisplayManagement", "OrchardCore.Environment.Navigation", "OrchardCore.Module.Targets", } ),
            ( "OrchardCore.Nancy", new string[] { "OrchardCore.Module.Targets", "OrchardCore.Nancy.Core", } ),
            ( "OrchardCore.Nancy.HelloWorld", new string[] { "OrchardCore.Module.Targets", "OrchardCore.Nancy.Core", } ),
            ( "OrchardCore.Resources", new string[] { "OrchardCore.ContentManagement", "OrchardCore.DisplayManagement", "OrchardCore.Module.Targets", "OrchardCore.ResourceManagement.Abstractions", } ),
            ( "OrchardCore.Themes", new string[] { "OrchardCore.Admin.Abstractions", "OrchardCore.DisplayManagement", "OrchardCore.Environment.Extensions.Abstractions", "OrchardCore.Environment.Navigation", "OrchardCore.Environment.Shell.Abstractions", "OrchardCore.Module.Targets", "OrchardCore.Recipes.Abstractions", "OrchardCore.Settings", } ),
            ( "OrchardCore.Title", new string[] { "OrchardCore.ContentManagement.Abstractions", "OrchardCore.ContentManagement.Display", "OrchardCore.Data.Abstractions", "OrchardCore.Indexing.Abstractions", "OrchardCore.Liquid.Abstractions", "OrchardCore.MetaWeblog.Abstractions", "OrchardCore.Module.Targets", "OrchardCore.Security.Abstractions", } ),
            ( "OrchardCore.Users", new string[] { "OrchardCore.ContentManagement.Display", "OrchardCore.Data.Abstractions", "OrchardCore.Environment.Commands", "OrchardCore.Environment.Navigation", "OrchardCore.Liquid.Abstractions", "OrchardCore.Module.Targets", "OrchardCore.Setup.Abstractions", "OrchardCore.Users.Core", } ),
            ( "TheBlogTheme", new string[] { "OrchardCore.ContentManagement", "OrchardCore.DisplayManagement", "OrchardCore.ResourceManagement", "OrchardCore.Theme.Targets", } ),
            ( "TheTheme", new string[] { "OrchardCore.ContentManagement", "OrchardCore.DisplayManagement", "OrchardCore.ResourceManagement", "OrchardCore.Theme.Targets", } ),

            // Rank 6
            ( "OrchardCore.Alias", new string[] { "OrchardCore.ContentManagement.Abstractions", "OrchardCore.ContentTypes.Abstractions", "OrchardCore.Data.Abstractions", "OrchardCore.Data.Abstractions", "OrchardCore.DisplayManagement", "OrchardCore.Indexing.Abstractions", "OrchardCore.Liquid.Abstractions", "OrchardCore.Module.Targets", } ),
            ( "OrchardCore.Application.Nancy.Targets", new string[] { "OrchardCore.Application.Targets", "OrchardCore.Nancy", } ),
            ( "OrchardCore.Autoroute", new string[] { "OrchardCore.ContentManagement.Abstractions", "OrchardCore.ContentManagement.Display", "OrchardCore.ContentTypes.Abstractions", "OrchardCore.Data.Abstractions", "OrchardCore.Indexing.Abstractions", "OrchardCore.Liquid.Abstractions", "OrchardCore.MetaWeblog.Abstractions", "OrchardCore.Module.Targets", "OrchardCore.Mvc.Abstractions", "OrchardCore.Security", } ),
            ( "OrchardCore.Body", new string[] { "OrchardCore.ContentManagement.Abstractions", "OrchardCore.ContentManagement.Display", "OrchardCore.ContentTypes.Abstractions", "OrchardCore.Data.Abstractions", "OrchardCore.DisplayManagement", "OrchardCore.Indexing.Abstractions", "OrchardCore.Liquid.Abstractions", "OrchardCore.MetaWeblog.Abstractions", "OrchardCore.Module.Targets", "OrchardCore.Mvc.Abstractions", } ),
            ( "OrchardCore.ContentFields", new string[] { "OrchardCore.ContentManagement.Abstractions", "OrchardCore.ContentManagement.Display", "OrchardCore.ContentTypes.Abstractions", "OrchardCore.Indexing.Abstractions", "OrchardCore.Liquid.Abstractions", "OrchardCore.Module.Targets", } ),
            ( "OrchardCore.Contents", new string[] { "OrchardCore.ContentManagement", "OrchardCore.ContentManagement.Display", "OrchardCore.ContentTypes.Abstractions", "OrchardCore.Feeds.Abstractions", "OrchardCore.Indexing.Abstractions", "OrchardCore.Liquid.Abstractions", "OrchardCore.Module.Targets", "OrchardCore.Mvc.Abstractions", "OrchardCore.Navigation", "OrchardCore.Recipes.Abstractions", "OrchardCore.Scripting.Abstractions", "OrchardCore.Settings", } ),
            ( "OrchardCore.ContentTypes", new string[] { "OrchardCore.ContentManagement", "OrchardCore.ContentManagement.Abstractions", "OrchardCore.ContentManagement.Display", "OrchardCore.ContentTypes.Abstractions", "OrchardCore.Deployment.Abstractions", "OrchardCore.Environment.Navigation", "OrchardCore.Module.Targets", "OrchardCore.ResourceManagement", } ),
            ( "OrchardCore.CustomSettings", new string[] { "OrchardCore.ContentManagement.Abstractions", "OrchardCore.ContentTypes.Abstractions", "OrchardCore.DisplayManagement", "OrchardCore.Entities.DisplayManagement", "OrchardCore.Environment.Navigation", "OrchardCore.Module.Targets", } ),
            ( "OrchardCore.Deployment", new string[] { "OrchardCore.Admin.Abstractions", "OrchardCore.ContentManagement", "OrchardCore.Data.Abstractions", "OrchardCore.Deployment.Core", "OrchardCore.DisplayManagement", "OrchardCore.Environment.Navigation", "OrchardCore.Module.Targets", } ),
            ( "OrchardCore.Deployment.Remote", new string[] { "OrchardCore.Admin.Abstractions", "OrchardCore.Data.Abstractions", "OrchardCore.Deployment.Core", "OrchardCore.DisplayManagement", "OrchardCore.Environment.Navigation", "OrchardCore.Module.Targets", "OrchardCore.Recipes.Abstractions", } ),
            ( "OrchardCore.Flows", new string[] { "OrchardCore.ContentManagement.Abstractions", "OrchardCore.ContentManagement.Display", "OrchardCore.ContentTypes.Abstractions", "OrchardCore.Data.Abstractions", "OrchardCore.Liquid.Abstractions", "OrchardCore.Module.Targets", "OrchardCore.Mvc.Abstractions", "OrchardCore.ResourceManagement.Abstractions", } ),
            ( "OrchardCore.Indexing", new string[] { "OrchardCore.ContentTypes.Abstractions", "OrchardCore.Data.Abstractions", "OrchardCore.DisplayManagement", "OrchardCore.Indexing.Abstractions", "OrchardCore.Module.Targets", } ),
            ( "OrchardCore.Liquid", new string[] { "OrchardCore.ContentManagement", "OrchardCore.ContentManagement.Abstractions", "OrchardCore.ContentTypes.Abstractions", "OrchardCore.Data", "OrchardCore.DisplayManagement", "OrchardCore.DisplayManagement.Liquid", "OrchardCore.Indexing.Abstractions", "OrchardCore.Module.Targets", } ),
            ( "OrchardCore.Localization", new string[] { "OrchardCore.Environment.Shell", "OrchardCore.Features", "OrchardCore.Localization.Abstractions", "OrchardCore.Localization.Core", } ),
            ( "OrchardCore.Lucene", new string[] { "OrchardCore.Admin.Abstractions", "OrchardCore.BackgroundTasks.Abstractions", "OrchardCore.ContentManagement", "OrchardCore.ContentManagement.Display", "OrchardCore.ContentTypes.Abstractions", "OrchardCore.Data.Abstractions", "OrchardCore.DisplayManagement", "OrchardCore.Entities.DisplayManagement", "OrchardCore.Environment.Navigation", "OrchardCore.Environment.Shell", "OrchardCore.Indexing.Abstractions", "OrchardCore.Liquid.Abstractions", "OrchardCore.Lucene.Core", "OrchardCore.Module.Targets", "OrchardCore.Security", } ),
            ( "OrchardCore.Markdown", new string[] { "OrchardCore.ContentManagement.Abstractions", "OrchardCore.ContentManagement.Display", "OrchardCore.ContentTypes.Abstractions", "OrchardCore.Data.Abstractions", "OrchardCore.DisplayManagement", "OrchardCore.Indexing.Abstractions", "OrchardCore.Liquid.Abstractions", "OrchardCore.MetaWeblog.Abstractions", "OrchardCore.Module.Targets", } ),
            ( "OrchardCore.Media", new string[] { "OrchardCore.ContentManagement.Abstractions", "OrchardCore.ContentManagement.Display", "OrchardCore.ContentTypes.Abstractions", "OrchardCore.Data.Abstractions", "OrchardCore.DisplayManagement", "OrchardCore.Environment.Navigation", "OrchardCore.Environment.Shell", "OrchardCore.Liquid.Abstractions", "OrchardCore.Media.Abstractions", "OrchardCore.Media.Services", "OrchardCore.MetaWeblog.Abstractions", "OrchardCore.Modules.Abstractions", "OrchardCore.Recipes.Abstractions", "OrchardCore.Security", "OrchardCore.Security.Abstractions", "OrchardCore.StorageProviders.FileSystem", "OrchardCore.XmlRpc.Abstractions", } ),
            ( "OrchardCore.Mvc.Web", new string[] { "OrchardCore.Application.Mvc.Targets", "OrchardCore.Diagnostics.Elm", "OrchardCore.Hosting.Console", "OrchardCore.Mvc.HelloWorld", } ),
            ( "OrchardCore.OpenId", new string[] { "OrchardCore.ContentManagement.Abstractions", "OrchardCore.Data.Abstractions", "OrchardCore.DisplayManagement", "OrchardCore.Entities.DisplayManagement", "OrchardCore.Environment.Navigation", "OrchardCore.Module.Targets", "OrchardCore.Recipes.Abstractions", "OrchardCore.Roles", "OrchardCore.Settings", "OrchardCore.Users", } ),
            ( "OrchardCore.Recipes", new string[] { "OrchardCore.Data.Abstractions", "OrchardCore.Environment.Commands", "OrchardCore.Features", "OrchardCore.Module.Targets", "OrchardCore.Mvc.Abstractions", "OrchardCore.Recipes.Implementations", } ),
            ( "OrchardCore.Templates", new string[] { "OrchardCore.Admin.Abstractions", "OrchardCore.ContentManagement.Abstractions", "OrchardCore.ContentTypes.Abstractions", "OrchardCore.Data.Abstractions", "OrchardCore.DisplayManagement", "OrchardCore.DisplayManagement.Liquid", "OrchardCore.Environment.Navigation", "OrchardCore.Module.Targets", "OrchardCore.Recipes.Abstractions", } ),
            ( "OrchardCore.Widgets", new string[] { "OrchardCore.ContentManagement.Abstractions", "OrchardCore.ContentManagement.Display", "OrchardCore.ContentTypes.Abstractions", "OrchardCore.Data.Abstractions", "OrchardCore.Module.Targets", "OrchardCore.Mvc.Abstractions", } ),

            // Rank 7
            ( "OrchardCore.Demo", new string[] { "OrchardCore.Admin.Abstractions", "OrchardCore.BackgroundTasks.Abstractions", "OrchardCore.ContentManagement.Display", "OrchardCore.Contents", "OrchardCore.Data.Abstractions", "OrchardCore.DeferredTasks.Abstractions", "OrchardCore.Entities.DisplayManagement", "OrchardCore.Environment.Commands", "OrchardCore.Environment.Navigation", "OrchardCore.Module.Targets", "OrchardCore.ResourceManagement", "OrchardCore.ResourceManagement.Abstractions", "OrchardCore.Users.Core", } ),
            ( "OrchardCore.Lists", new string[] { "OrchardCore.ContentManagement.Abstractions", "OrchardCore.ContentManagement.Display", "OrchardCore.Contents", "OrchardCore.ContentTypes.Abstractions", "OrchardCore.Media.Abstractions", "OrchardCore.MetaWeblog.Abstractions", "OrchardCore.Module.Targets", "OrchardCore.Mvc.Abstractions", "OrchardCore.ResourceManagement.Abstractions", "OrchardCore.Users.Abstractions", } ),
            ( "OrchardCore.Nancy.Web", new string[] { "OrchardCore.Application.Nancy.Targets", "OrchardCore.Nancy.HelloWorld", } ),
            ( "OrchardCore.Queries", new string[] { "OrchardCore.ContentManagement.Abstractions", "OrchardCore.ContentManagement.Display", "OrchardCore.Data.Abstractions", "OrchardCore.DisplayManagement", "OrchardCore.Environment.Navigation", "OrchardCore.Liquid.Abstractions", "OrchardCore.Lucene", "OrchardCore.Module.Targets", "OrchardCore.Queries.Abstractions", } ),

            // Rank 8
            ( "OrchardCore.Application.Cms.Targets", new string[] { "OrchardCore.Admin", "OrchardCore.Alias", "OrchardCore.Application.Targets", "OrchardCore.Autoroute", "OrchardCore.Body", "OrchardCore.Commons", "OrchardCore.ContentFields", "OrchardCore.ContentPreview", "OrchardCore.Contents", "OrchardCore.ContentTypes", "OrchardCore.CustomSettings", "OrchardCore.Deployment", "OrchardCore.Deployment.Remote", "OrchardCore.Diagnostics", "OrchardCore.DynamicCache", "OrchardCore.Features", "OrchardCore.Feeds", "OrchardCore.Flows", "OrchardCore.HomeRoute", "OrchardCore.Indexing", "OrchardCore.Layers", "OrchardCore.Liquid", "OrchardCore.Lists", "OrchardCore.Localization", "OrchardCore.Lucene", "OrchardCore.Markdown", "OrchardCore.Media", "OrchardCore.Menu", "OrchardCore.Mvc", "OrchardCore.Navigation", "OrchardCore.Queries", "OrchardCore.Recipes", "OrchardCore.Resources", "OrchardCore.ResponseCache", "OrchardCore.Roles", "OrchardCore.Scripting", "OrchardCore.Settings", "OrchardCore.Setup", "OrchardCore.Templates", "OrchardCore.Tenants", "OrchardCore.Themes", "OrchardCore.Title", "OrchardCore.Users", "OrchardCore.Widgets", "OrchardCore.XmlRpc", "SafeMode", "TheAdmin", "TheAgencyTheme", "TheBlogTheme", "TheTheme", } ),
            ( "OrchardCore.Tests", new string[] { "OrchardCore.ContentManagement", "OrchardCore.Data", "OrchardCore.DisplayManagement", "OrchardCore.Environment.Extensions", "OrchardCore.Environment.Extensions.Abstractions", "OrchardCore.Environment.Shell", "OrchardCore.Hosting.Console", "OrchardCore.Liquid", "OrchardCore.Localization.Abstractions", "OrchardCore.Localization.Core", "OrchardCore.Parser.Yaml", "OrchardCore.Queries", "OrchardCore.Recipes.Implementations", } ),

            // Rank 9
            ( "OrchardCore.Cms.Web", new string[] { "OrchardCore.Application.Cms.Targets", "OrchardCore.Hosting.Console", "OrchardCore.Logging.NLog", } ),
        };

        public string MainProject => "OrchardCore.Cms.Web";

        public Scenario Scenario => Scenario.WebApp;
    }
}
