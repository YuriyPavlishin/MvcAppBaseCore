namespace BaseApp.Web.Code.Mappers
{
    public abstract class MapperBase : AutoMapper.Profile
    {
        protected override void Configure()
        {
            CreateMaps();
        }

        protected abstract void CreateMaps();
    }
}