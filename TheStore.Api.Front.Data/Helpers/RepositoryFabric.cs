// a.snegovoy@gmail.com

using Common.Settings;
using Common.Workers;

using TheStore.Api.Front.Data.Repositories;

namespace TheStore.Api.Front.Data.Helpers
{
    public static class RepositoryFabric
    {
        public static TagsWorkRepository GetTagsWorkRepository() => new TagsWorkRepository(GetDbSettings());
        public static TagsRepository GetTagsRepository() => new TagsRepository(GetDbSettings());
        public static CategoryRepository GetCategoryRepository() => new CategoryRepository(GetDbSettings());
        public static CategoryMapRepository GetCategoryMapRepository() => new CategoryMapRepository(GetDbSettings());

        private static DbSettings GetDbSettings() => SettingsBuilder.GetDbSettings();

    }
}