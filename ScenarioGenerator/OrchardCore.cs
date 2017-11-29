using System;
using System.Collections.Generic;
using System.Linq;

namespace ScenarioGenerator
{
    // https://github.com/OrchardCMS/OrchardCore
    // Total Projects: 127
    // Total ProjectRefs: 529
    // Total PackageRefs: 237
    // Unique PackageRefs: 84

    public class OrchardCore : IScenario
    {
        public IDictionary<string, IEnumerable<string>> ProjectReferences => new Dictionary<string, IEnumerable<string>>
        {
            // Rank 0
            { "OrchardCore.Application.Targets", Enumerable.Empty<string>() },
            { "OrchardCore.BackgroundTasks.Abstractions", Enumerable.Empty<string>() },
            { "OrchardCore.Data.Abstractions", Enumerable.Empty<string>() },
            { "OrchardCore.DeferredTasks.Abstractions", Enumerable.Empty<string>() },
            { "OrchardCore.Entities", Enumerable.Empty<string>() },
            { "OrchardCore.Environment.Cache.Abstractions", Enumerable.Empty<string>() },
            { "OrchardCore.Environment.Extensions.Abstractions", Enumerable.Empty<string>() },
            { "OrchardCore.Liquid.Abstractions", Enumerable.Empty<string>() },
            { "OrchardCore.Localization.Abstractions", Enumerable.Empty<string>() },
            { "OrchardCore.Lucene.Abstractions", Enumerable.Empty<string>() },
            { "OrchardCore.Modules.Abstractions", Enumerable.Empty<string>() },
            { "OrchardCore.Parser.Yaml", Enumerable.Empty<string>() },
            { "OrchardCore.Queries.Abstractions", Enumerable.Empty<string>() },
            { "OrchardCore.Recipes.Abstractions", Enumerable.Empty<string>() },
            { "OrchardCore.ResourceManagement.Abstractions", Enumerable.Empty<string>() },
            { "OrchardCore.Scripting.Abstractions", Enumerable.Empty<string>() },
            { "OrchardCore.Security.Abstractions", Enumerable.Empty<string>() },
            { "OrchardCore.Setup.Abstractions", Enumerable.Empty<string>() },
            { "OrchardCore.StorageProviders.Abstractions", Enumerable.Empty<string>() },
            { "OrchardCore.XmlRpc.Abstractions", Enumerable.Empty<string>() },

            // Rank 1
            { "OrchardCore.Admin.Abstractions", new string[] { "OrchardCore.Environment.Extensions.Abstractions", "OrchardCore.Security.Abstractions", } },
            { "OrchardCore.Environment.Shell.Abstractions", new string[] { "OrchardCore.Environment.Extensions.Abstractions", } },
            { "OrchardCore.Localization.Core", new string[] { "OrchardCore.Localization.Abstractions", } },
            { "OrchardCore.Lucene.Core", new string[] { "OrchardCore.Lucene.Abstractions", "OrchardCore.Queries.Abstractions", } },
            { "OrchardCore.Module.Targets", new string[] { "OrchardCore.Modules.Abstractions", } },
            { "OrchardCore.Mvc.Abstractions", new string[] { "OrchardCore.Modules.Abstractions", "OrchardCore.Entities", } },
            { "OrchardCore.ResourceManagement", new string[] { "OrchardCore.ResourceManagement.Abstractions", } },
            { "OrchardCore.Scripting.Core", new string[] { "OrchardCore.Scripting.Abstractions", } },
            { "OrchardCore.Scripting.JavaScript", new string[] { "OrchardCore.Scripting.Abstractions", } },
            { "OrchardCore.StorageProviders.FileSystem", new string[] { "OrchardCore.StorageProviders.Abstractions", } },
            { "OrchardCore.Users.Abstractions", new string[] { "OrchardCore.Security.Abstractions", } },

            // Rank 2
            { "OrchardCore.BackgroundTasks", new string[] { "OrchardCore.Modules.Abstractions", "OrchardCore.BackgroundTasks.Abstractions", "OrchardCore.Environment.Shell.Abstractions", } },
            { "OrchardCore.DeferredTasks", new string[] { "OrchardCore.DeferredTasks.Abstractions", "OrchardCore.Environment.Shell.Abstractions", } },
            { "OrchardCore.Diagnostics.Elm", new string[] { "OrchardCore.Module.Targets", } },
            { "OrchardCore.Environment.Commands", new string[] { "OrchardCore.Modules.Abstractions", "OrchardCore.Environment.Shell.Abstractions", } },
            { "OrchardCore.Environment.Extensions", new string[] { "OrchardCore.Modules.Abstractions", "OrchardCore.Environment.Cache.Abstractions", "OrchardCore.Environment.Extensions.Abstractions", "OrchardCore.Environment.Shell.Abstractions", "OrchardCore.Parser.Yaml", } },
            { "OrchardCore.Environment.Shell", new string[] { "OrchardCore.Modules.Abstractions", "OrchardCore.DeferredTasks.Abstractions", "OrchardCore.Environment.Extensions.Abstractions", "OrchardCore.Environment.Shell.Abstractions", "OrchardCore.Parser.Yaml", } },
            { "OrchardCore.Feeds.Abstractions", new string[] { "OrchardCore.Mvc.Abstractions", } },
            { "OrchardCore.HomeRoute", new string[] { "OrchardCore.Module.Targets", "OrchardCore.Mvc.Abstractions", } },
            { "OrchardCore.Logging.NLog", new string[] { "OrchardCore.Environment.Shell.Abstractions", } },
            { "OrchardCore.Mvc.Core", new string[] { "OrchardCore.Modules.Abstractions", "OrchardCore.Mvc.Abstractions", "OrchardCore.Environment.Extensions.Abstractions", "OrchardCore.Environment.Shell.Abstractions", } },
            { "OrchardCore.Recipes.Implementations", new string[] { "OrchardCore.Modules.Abstractions", "OrchardCore.DeferredTasks.Abstractions", "OrchardCore.Environment.Extensions.Abstractions", "OrchardCore.Environment.Shell.Abstractions", "OrchardCore.Recipes.Abstractions", "OrchardCore.Scripting.Abstractions", } },
            { "OrchardCore.ResponseCache", new string[] { "OrchardCore.Module.Targets", } },
            { "OrchardCore.Security", new string[] { "OrchardCore.Mvc.Abstractions", "OrchardCore.Security.Abstractions", } },
            { "OrchardCore.Theme.Targets", new string[] { "OrchardCore.Module.Targets", } },
            { "OrchardCore.Users.Core", new string[] { "OrchardCore.Entities", "OrchardCore.Security.Abstractions", "OrchardCore.Users.Abstractions", } },
            { "OrchardCore.XmlRpc", new string[] { "OrchardCore.Module.Targets", "OrchardCore.Mvc.Abstractions", "OrchardCore.Environment.Extensions.Abstractions", "OrchardCore.XmlRpc.Abstractions", } },

            // Rank 3
            { "OrchardCore.ContentManagement.Abstractions", new string[] { "OrchardCore.Mvc.Core", } },
            { "OrchardCore.Data", new string[] { "OrchardCore.Modules.Abstractions", "OrchardCore.Data.Abstractions", "OrchardCore.Environment.Extensions.Abstractions", "OrchardCore.Environment.Shell", } },
            { "OrchardCore.DisplayManagement", new string[] { "OrchardCore.Modules.Abstractions", "OrchardCore.Environment.Cache.Abstractions", "OrchardCore.Environment.Extensions.Abstractions", "OrchardCore.Environment.Shell.Abstractions", "OrchardCore.Mvc.Core", } },
            { "OrchardCore.Environment.Navigation", new string[] { "OrchardCore.Environment.Shell", "OrchardCore.Security", } },
            { "OrchardCore.Environment.Shell.Data", new string[] { "OrchardCore.Data.Abstractions", "OrchardCore.Environment.Shell", } },
            { "OrchardCore.Feeds.Core", new string[] { "OrchardCore.Feeds.Abstractions", } },
            { "OrchardCore.Hosting.Console", new string[] { "OrchardCore.Environment.Commands", } },
            { "OrchardCore.Modules", new string[] { "OrchardCore.Modules.Abstractions", "OrchardCore.Environment.Extensions", "OrchardCore.Environment.Shell", } },
            { "OrchardCore.Mvc.HelloWorld", new string[] { "OrchardCore.Module.Targets", "OrchardCore.Mvc.Core", } },
            { "OrchardCore.Scripting", new string[] { "OrchardCore.Module.Targets", "OrchardCore.Mvc.Core", "OrchardCore.Scripting.Core", "OrchardCore.Scripting.JavaScript", } },

            // Rank 4
            { "OrchardCore.ContentManagement", new string[] { "OrchardCore.Mvc.Abstractions", "OrchardCore.ContentManagement.Abstractions", "OrchardCore.Data.Abstractions", "OrchardCore.Environment.Cache.Abstractions", "OrchardCore.Recipes.Abstractions", } },
            { "OrchardCore.ContentManagement.Display", new string[] { "OrchardCore.ContentManagement.Abstractions", "OrchardCore.DisplayManagement", } },
            { "OrchardCore.Deployment.Abstractions", new string[] { "OrchardCore.Recipes.Abstractions", "OrchardCore.DisplayManagement", } },
            { "OrchardCore.Diagnostics", new string[] { "OrchardCore.Module.Targets", "OrchardCore.Mvc.Abstractions", "OrchardCore.DisplayManagement", } },
            { "OrchardCore.DisplayManagement.Liquid", new string[] { "OrchardCore.DisplayManagement", "OrchardCore.Liquid.Abstractions", } },
            { "OrchardCore.Entities.DisplayManagement", new string[] { "OrchardCore.DisplayManagement", "OrchardCore.Entities", } },
            { "OrchardCore.Environment.Cache", new string[] { "OrchardCore.Modules", "OrchardCore.Environment.Cache.Abstractions", "OrchardCore.Environment.Extensions.Abstractions", } },
            { "OrchardCore.Feeds", new string[] { "OrchardCore.Module.Targets", "OrchardCore.Mvc.Abstractions", "OrchardCore.Feeds.Core", } },
            { "OrchardCore.Indexing.Abstractions", new string[] { "OrchardCore.ContentManagement.Abstractions", } },
            { "OrchardCore.Media.Abstractions", new string[] { "OrchardCore.ContentManagement.Abstractions", "OrchardCore.StorageProviders.Abstractions", } },
            { "OrchardCore.Menu.Abstractions", new string[] { "OrchardCore.ContentManagement.Abstractions", } },
            { "OrchardCore.MetaWeblog.Abstractions", new string[] { "OrchardCore.XmlRpc.Abstractions", "OrchardCore.ContentManagement.Abstractions", } },
            { "OrchardCore.Mvc", new string[] { "OrchardCore.Module.Targets", "OrchardCore.Modules", "OrchardCore.Mvc.Core", } },
            { "OrchardCore.Nancy.Core", new string[] { "OrchardCore.Modules", } },
            { "OrchardCore.Navigation", new string[] { "OrchardCore.Module.Targets", "OrchardCore.Data.Abstractions", "OrchardCore.DisplayManagement", "OrchardCore.Environment.Navigation", "OrchardCore.Security", } },
            { "OrchardCore.Roles", new string[] { "OrchardCore.Module.Targets", "OrchardCore.Data.Abstractions", "OrchardCore.DisplayManagement", "OrchardCore.Environment.Navigation", "OrchardCore.Recipes.Abstractions", "OrchardCore.Security.Abstractions", } },
            { "OrchardCore.Settings", new string[] { "OrchardCore.Entities", "OrchardCore.Module.Targets", "OrchardCore.Data.Abstractions", "OrchardCore.DisplayManagement", "OrchardCore.Environment.Navigation", "OrchardCore.Liquid.Abstractions", "OrchardCore.Recipes.Abstractions", "OrchardCore.Setup.Abstractions", } },
            { "OrchardCore.Setup", new string[] { "OrchardCore.Module.Targets", "OrchardCore.Mvc.Abstractions", "OrchardCore.Data", "OrchardCore.DisplayManagement", "OrchardCore.Recipes.Abstractions", "OrchardCore.Setup.Abstractions", } },
            { "OrchardCore.Tenants", new string[] { "OrchardCore.Module.Targets", "OrchardCore.Environment.Navigation", "OrchardCore.Admin.Abstractions", "OrchardCore.DisplayManagement", } },
            { "SafeMode", new string[] { "OrchardCore.Theme.Targets", "OrchardCore.DisplayManagement", } },
            { "TheAdmin", new string[] { "OrchardCore.Theme.Targets", "OrchardCore.DisplayManagement", } },
            { "TheAgencyTheme", new string[] { "OrchardCore.Theme.Targets", "OrchardCore.DisplayManagement", } },

            // Rank 5
            { "OrchardCore.Admin", new string[] { "OrchardCore.Module.Targets", "OrchardCore.Admin.Abstractions", "OrchardCore.Data.Abstractions", "OrchardCore.DisplayManagement", "OrchardCore.Settings", "OrchardCore.Environment.Navigation", } },
            { "OrchardCore.Application.Mvc.Targets", new string[] { "OrchardCore.Application.Targets", "OrchardCore.Mvc", } },
            { "OrchardCore.Commons", new string[] { "OrchardCore.Module.Targets", "OrchardCore.DeferredTasks", "OrchardCore.Data", "OrchardCore.Environment.Cache", "OrchardCore.DisplayManagement", "OrchardCore.DisplayManagement.Liquid", "OrchardCore.BackgroundTasks", "OrchardCore.Environment.Extensions", "OrchardCore.ResourceManagement", "OrchardCore.Environment.Shell.Data", } },
            { "OrchardCore.ContentPreview", new string[] { "OrchardCore.Module.Targets", "OrchardCore.ContentManagement.Abstractions", "OrchardCore.ContentManagement.Display", "OrchardCore.Settings", "OrchardCore.Security", } },
            { "OrchardCore.ContentTypes.Abstractions", new string[] { "OrchardCore.ContentManagement.Abstractions", "OrchardCore.ContentManagement.Display", } },
            { "OrchardCore.Deployment.Core", new string[] { "OrchardCore.Deployment.Abstractions", } },
            { "OrchardCore.DynamicCache", new string[] { "OrchardCore.Module.Targets", "OrchardCore.Mvc.Core", "OrchardCore.ContentManagement", "OrchardCore.DisplayManagement", "OrchardCore.Environment.Cache.Abstractions", "OrchardCore.Environment.Navigation", } },
            { "OrchardCore.Features", new string[] { "OrchardCore.Deployment.Abstractions", "OrchardCore.Module.Targets", "OrchardCore.Mvc.Abstractions", "OrchardCore.Admin.Abstractions", "OrchardCore.ContentManagement.Abstractions", "OrchardCore.DisplayManagement", "OrchardCore.Environment.Extensions.Abstractions", "OrchardCore.Environment.Navigation", "OrchardCore.Environment.Shell", "OrchardCore.Environment.Shell.Abstractions", "OrchardCore.Recipes.Abstractions", "OrchardCore.ResourceManagement.Abstractions", } },
            { "OrchardCore.Layers", new string[] { "OrchardCore.Entities.DisplayManagement", "OrchardCore.Module.Targets", "OrchardCore.Mvc.Abstractions", "OrchardCore.Admin.Abstractions", "OrchardCore.ContentManagement", "OrchardCore.ContentManagement.Abstractions", "OrchardCore.ContentManagement.Display", "OrchardCore.Data.Abstractions", "OrchardCore.Environment.Navigation", "OrchardCore.Recipes.Abstractions", "OrchardCore.Scripting.Abstractions", } },
            { "OrchardCore.Media.Services", new string[] { "OrchardCore.ContentManagement.Abstractions", "OrchardCore.Media.Abstractions", } },
            { "OrchardCore.Menu", new string[] { "OrchardCore.Module.Targets", "OrchardCore.ContentManagement.Display", "OrchardCore.Data.Abstractions", "OrchardCore.DisplayManagement", "OrchardCore.ContentManagement.Abstractions", "OrchardCore.Environment.Navigation", } },
            { "OrchardCore.Nancy", new string[] { "OrchardCore.Module.Targets", "OrchardCore.Nancy.Core", } },
            { "OrchardCore.Nancy.HelloWorld", new string[] { "OrchardCore.Module.Targets", "OrchardCore.Nancy.Core", } },
            { "OrchardCore.Resources", new string[] { "OrchardCore.Module.Targets", "OrchardCore.ContentManagement", "OrchardCore.ResourceManagement.Abstractions", "OrchardCore.DisplayManagement", } },
            { "OrchardCore.Themes", new string[] { "OrchardCore.Environment.Extensions.Abstractions", "OrchardCore.Environment.Shell.Abstractions", "OrchardCore.Module.Targets", "OrchardCore.Admin.Abstractions", "OrchardCore.DisplayManagement", "OrchardCore.Environment.Navigation", "OrchardCore.Recipes.Abstractions", "OrchardCore.Settings", } },
            { "OrchardCore.Title", new string[] { "OrchardCore.Liquid.Abstractions", "OrchardCore.Module.Targets", "OrchardCore.ContentManagement.Abstractions", "OrchardCore.ContentManagement.Display", "OrchardCore.Data.Abstractions", "OrchardCore.Indexing.Abstractions", "OrchardCore.MetaWeblog.Abstractions", "OrchardCore.Security.Abstractions", } },
            { "OrchardCore.Users", new string[] { "OrchardCore.Users.Core", "OrchardCore.Module.Targets", "OrchardCore.ContentManagement.Display", "OrchardCore.Data.Abstractions", "OrchardCore.Environment.Commands", "OrchardCore.Environment.Navigation", "OrchardCore.Liquid.Abstractions", "OrchardCore.Setup.Abstractions", } },
            { "TheBlogTheme", new string[] { "OrchardCore.Theme.Targets", "OrchardCore.ContentManagement", "OrchardCore.ResourceManagement", "OrchardCore.DisplayManagement", } },
            { "TheTheme", new string[] { "OrchardCore.Theme.Targets", "OrchardCore.ContentManagement", "OrchardCore.ResourceManagement", "OrchardCore.DisplayManagement", } },

            // Rank 6
            { "OrchardCore.Alias", new string[] { "OrchardCore.Liquid.Abstractions", "OrchardCore.Module.Targets", "OrchardCore.ContentManagement.Abstractions", "OrchardCore.ContentTypes.Abstractions", "OrchardCore.Data.Abstractions", "OrchardCore.DisplayManagement", "OrchardCore.Indexing.Abstractions", "OrchardCore.Data.Abstractions", } },
            { "OrchardCore.Application.Nancy.Targets", new string[] { "OrchardCore.Application.Targets", "OrchardCore.Nancy", } },
            { "OrchardCore.Autoroute", new string[] { "OrchardCore.Liquid.Abstractions", "OrchardCore.Module.Targets", "OrchardCore.Mvc.Abstractions", "OrchardCore.ContentManagement.Abstractions", "OrchardCore.ContentManagement.Display", "OrchardCore.ContentTypes.Abstractions", "OrchardCore.Data.Abstractions", "OrchardCore.Indexing.Abstractions", "OrchardCore.Security", "OrchardCore.MetaWeblog.Abstractions", } },
            { "OrchardCore.Body", new string[] { "OrchardCore.Liquid.Abstractions", "OrchardCore.Module.Targets", "OrchardCore.Mvc.Abstractions", "OrchardCore.ContentManagement.Abstractions", "OrchardCore.ContentManagement.Display", "OrchardCore.ContentTypes.Abstractions", "OrchardCore.Data.Abstractions", "OrchardCore.DisplayManagement", "OrchardCore.Indexing.Abstractions", "OrchardCore.MetaWeblog.Abstractions", } },
            { "OrchardCore.ContentFields", new string[] { "OrchardCore.Liquid.Abstractions", "OrchardCore.Module.Targets", "OrchardCore.ContentManagement.Abstractions", "OrchardCore.ContentManagement.Display", "OrchardCore.ContentTypes.Abstractions", "OrchardCore.Indexing.Abstractions", } },
            { "OrchardCore.Contents", new string[] { "OrchardCore.Module.Targets", "OrchardCore.Mvc.Abstractions", "OrchardCore.ContentManagement", "OrchardCore.ContentManagement.Display", "OrchardCore.ContentTypes.Abstractions", "OrchardCore.Feeds.Abstractions", "OrchardCore.Indexing.Abstractions", "OrchardCore.Liquid.Abstractions", "OrchardCore.Recipes.Abstractions", "OrchardCore.Scripting.Abstractions", "OrchardCore.Navigation", "OrchardCore.Settings", } },
            { "OrchardCore.ContentTypes", new string[] { "OrchardCore.Deployment.Abstractions", "OrchardCore.Module.Targets", "OrchardCore.ContentManagement", "OrchardCore.ContentManagement.Display", "OrchardCore.ContentTypes.Abstractions", "OrchardCore.ContentManagement.Abstractions", "OrchardCore.Environment.Navigation", "OrchardCore.ResourceManagement", } },
            { "OrchardCore.CustomSettings", new string[] { "OrchardCore.Entities.DisplayManagement", "OrchardCore.Environment.Navigation", "OrchardCore.Module.Targets", "OrchardCore.ContentManagement.Abstractions", "OrchardCore.ContentTypes.Abstractions", "OrchardCore.DisplayManagement", } },
            { "OrchardCore.Deployment", new string[] { "OrchardCore.Module.Targets", "OrchardCore.ContentManagement", "OrchardCore.Data.Abstractions", "OrchardCore.Deployment.Core", "OrchardCore.Environment.Navigation", "OrchardCore.Admin.Abstractions", "OrchardCore.DisplayManagement", } },
            { "OrchardCore.Deployment.Remote", new string[] { "OrchardCore.Module.Targets", "OrchardCore.Data.Abstractions", "OrchardCore.Deployment.Core", "OrchardCore.Environment.Navigation", "OrchardCore.Admin.Abstractions", "OrchardCore.DisplayManagement", "OrchardCore.Recipes.Abstractions", } },
            { "OrchardCore.Flows", new string[] { "OrchardCore.Liquid.Abstractions", "OrchardCore.ContentTypes.Abstractions", "OrchardCore.Module.Targets", "OrchardCore.Mvc.Abstractions", "OrchardCore.ContentManagement.Abstractions", "OrchardCore.ContentManagement.Display", "OrchardCore.Data.Abstractions", "OrchardCore.ResourceManagement.Abstractions", } },
            { "OrchardCore.Indexing", new string[] { "OrchardCore.Module.Targets", "OrchardCore.ContentTypes.Abstractions", "OrchardCore.Data.Abstractions", "OrchardCore.DisplayManagement", "OrchardCore.Indexing.Abstractions", } },
            { "OrchardCore.Liquid", new string[] { "OrchardCore.Module.Targets", "OrchardCore.ContentManagement.Abstractions", "OrchardCore.ContentManagement", "OrchardCore.ContentTypes.Abstractions", "OrchardCore.Data", "OrchardCore.DisplayManagement", "OrchardCore.DisplayManagement.Liquid", "OrchardCore.Indexing.Abstractions", } },
            { "OrchardCore.Localization", new string[] { "OrchardCore.Environment.Shell", "OrchardCore.Localization.Abstractions", "OrchardCore.Localization.Core", "OrchardCore.Features", } },
            { "OrchardCore.Lucene", new string[] { "OrchardCore.Admin.Abstractions", "OrchardCore.ContentManagement", "OrchardCore.Entities.DisplayManagement", "OrchardCore.Liquid.Abstractions", "OrchardCore.Module.Targets", "OrchardCore.Indexing.Abstractions", "OrchardCore.DisplayManagement", "OrchardCore.ContentManagement.Display", "OrchardCore.Environment.Shell", "OrchardCore.BackgroundTasks.Abstractions", "OrchardCore.Security", "OrchardCore.ContentTypes.Abstractions", "OrchardCore.Environment.Navigation", "OrchardCore.Lucene.Core", "OrchardCore.Data.Abstractions", } },
            { "OrchardCore.Markdown", new string[] { "OrchardCore.Liquid.Abstractions", "OrchardCore.Module.Targets", "OrchardCore.ContentManagement.Abstractions", "OrchardCore.ContentManagement.Display", "OrchardCore.ContentTypes.Abstractions", "OrchardCore.Data.Abstractions", "OrchardCore.DisplayManagement", "OrchardCore.Indexing.Abstractions", "OrchardCore.MetaWeblog.Abstractions", } },
            { "OrchardCore.Media", new string[] { "OrchardCore.Modules.Abstractions", "OrchardCore.ContentManagement.Abstractions", "OrchardCore.ContentManagement.Display", "OrchardCore.ContentTypes.Abstractions", "OrchardCore.Data.Abstractions", "OrchardCore.DisplayManagement", "OrchardCore.Environment.Navigation", "OrchardCore.Environment.Shell", "OrchardCore.Liquid.Abstractions", "OrchardCore.Media.Abstractions", "OrchardCore.Media.Services", "OrchardCore.MetaWeblog.Abstractions", "OrchardCore.Recipes.Abstractions", "OrchardCore.Security.Abstractions", "OrchardCore.Security", "OrchardCore.StorageProviders.FileSystem", "OrchardCore.XmlRpc.Abstractions", } },
            { "OrchardCore.Mvc.Web", new string[] { "OrchardCore.Diagnostics.Elm", "OrchardCore.Hosting.Console", "OrchardCore.Application.Mvc.Targets", "OrchardCore.Mvc.HelloWorld", } },
            { "OrchardCore.OpenId", new string[] { "OrchardCore.Entities.DisplayManagement", "OrchardCore.Module.Targets", "OrchardCore.ContentManagement.Abstractions", "OrchardCore.Data.Abstractions", "OrchardCore.DisplayManagement", "OrchardCore.Environment.Navigation", "OrchardCore.Recipes.Abstractions", "OrchardCore.Roles", "OrchardCore.Settings", "OrchardCore.Users", } },
            { "OrchardCore.Recipes", new string[] { "OrchardCore.Module.Targets", "OrchardCore.Mvc.Abstractions", "OrchardCore.Data.Abstractions", "OrchardCore.Environment.Commands", "OrchardCore.Features", "OrchardCore.Recipes.Implementations", } },
            { "OrchardCore.Templates", new string[] { "OrchardCore.Module.Targets", "OrchardCore.Admin.Abstractions", "OrchardCore.ContentManagement.Abstractions", "OrchardCore.ContentTypes.Abstractions", "OrchardCore.Environment.Navigation", "OrchardCore.Recipes.Abstractions", "OrchardCore.Data.Abstractions", "OrchardCore.DisplayManagement", "OrchardCore.DisplayManagement.Liquid", } },
            { "OrchardCore.Widgets", new string[] { "OrchardCore.Module.Targets", "OrchardCore.Mvc.Abstractions", "OrchardCore.ContentManagement.Abstractions", "OrchardCore.ContentManagement.Display", "OrchardCore.ContentTypes.Abstractions", "OrchardCore.Data.Abstractions", } },

            // Rank 7
            { "OrchardCore.Demo", new string[] { "OrchardCore.Entities.DisplayManagement", "OrchardCore.Module.Targets", "OrchardCore.Admin.Abstractions", "OrchardCore.BackgroundTasks.Abstractions", "OrchardCore.ContentManagement.Display", "OrchardCore.Data.Abstractions", "OrchardCore.DeferredTasks.Abstractions", "OrchardCore.Environment.Commands", "OrchardCore.Environment.Navigation", "OrchardCore.ResourceManagement", "OrchardCore.ResourceManagement.Abstractions", "OrchardCore.Users.Core", "OrchardCore.Contents", } },
            { "OrchardCore.Lists", new string[] { "OrchardCore.Users.Abstractions", "OrchardCore.Module.Targets", "OrchardCore.Mvc.Abstractions", "OrchardCore.ContentManagement.Abstractions", "OrchardCore.ContentManagement.Display", "OrchardCore.ContentTypes.Abstractions", "OrchardCore.Media.Abstractions", "OrchardCore.ResourceManagement.Abstractions", "OrchardCore.MetaWeblog.Abstractions", "OrchardCore.Contents", } },
            { "OrchardCore.Nancy.Web", new string[] { "OrchardCore.Application.Nancy.Targets", "OrchardCore.Nancy.HelloWorld", } },
            { "OrchardCore.Queries", new string[] { "OrchardCore.ContentManagement.Display", "OrchardCore.Data.Abstractions", "OrchardCore.DisplayManagement", "OrchardCore.ContentManagement.Abstractions", "OrchardCore.Environment.Navigation", "OrchardCore.Liquid.Abstractions", "OrchardCore.Queries.Abstractions", "OrchardCore.Module.Targets", "OrchardCore.Lucene", } },

            // Rank 8
            { "OrchardCore.Application.Cms.Targets", new string[] { "OrchardCore.Application.Targets", "OrchardCore.Localization", "OrchardCore.Liquid", "OrchardCore.Media", "OrchardCore.Queries", "OrchardCore.Admin", "OrchardCore.Alias", "OrchardCore.Autoroute", "OrchardCore.Body", "OrchardCore.Commons", "OrchardCore.ContentFields", "OrchardCore.ContentPreview", "OrchardCore.Contents", "OrchardCore.ContentTypes", "OrchardCore.CustomSettings", "OrchardCore.Deployment", "OrchardCore.Deployment.Remote", "OrchardCore.Diagnostics", "OrchardCore.DynamicCache", "OrchardCore.Feeds", "OrchardCore.Flows", "OrchardCore.HomeRoute", "OrchardCore.Indexing", "OrchardCore.Layers", "OrchardCore.Lists", "OrchardCore.Lucene", "OrchardCore.Markdown", "OrchardCore.Menu", "OrchardCore.Features", "OrchardCore.Mvc", "OrchardCore.Navigation", "OrchardCore.Recipes", "OrchardCore.Resources", "OrchardCore.ResponseCache", "OrchardCore.Roles", "OrchardCore.Scripting", "OrchardCore.Settings", "OrchardCore.Setup", "OrchardCore.Templates", "OrchardCore.Tenants", "OrchardCore.Themes", "OrchardCore.Title", "OrchardCore.Users", "OrchardCore.Widgets", "OrchardCore.XmlRpc", "SafeMode", "TheAdmin", "TheBlogTheme", "TheAgencyTheme", "TheTheme", } },
            { "OrchardCore.Tests", new string[] { "OrchardCore.Liquid", "OrchardCore.Queries", "OrchardCore.ContentManagement", "OrchardCore.Data", "OrchardCore.DisplayManagement", "OrchardCore.Environment.Extensions", "OrchardCore.Environment.Extensions.Abstractions", "OrchardCore.Environment.Shell", "OrchardCore.Hosting.Console", "OrchardCore.Localization.Abstractions", "OrchardCore.Localization.Core", "OrchardCore.Parser.Yaml", "OrchardCore.Recipes.Implementations", } },

            // Rank 9
            { "OrchardCore.Cms.Web", new string[] { "OrchardCore.Hosting.Console", "OrchardCore.Logging.NLog", "OrchardCore.Application.Cms.Targets", } },
        };

        public string MainProject => "OrchardCore.Cms.Web";
    }
}
