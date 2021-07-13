// a.snegovoy@gmail.com

using Common.Workers;

using TheStore.Api.Front.Data.Repositories;

namespace TheStore.Api.Front.Data.Helpers
{
    public static class RepositoryFabric
    {
        public static TagsWorkRepository GetTagsWorkRepository() =>
            new TagsWorkRepository( SettingsBuilder.GetDbSettings() );

        public static TagsRepository GetTagsRepository() => new TagsRepository( SettingsBuilder.GetDbSettings() );

    }
}