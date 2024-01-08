using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BaseApp.Web.Code.Mappers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BaseApp.Tests;

[TestClass]
public class MapperTest
{
    [TestMethod]
    public void Ignore_Success()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Map1Model, Map2Model>()
                .Ignore(x => x.Name);
        });
        config.AssertConfigurationIsValid();
        var source = new Map1Model { EntityID = 1 };
        var dest = config.CreateMapper().Map<Map2Model>(source);
        Assert.AreEqual(source.EntityID, dest.EntityID);
        Assert.IsNull(dest.Name);
    }
    
    [TestMethod]
    public void IgnoreSource_Success()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Map2Model, Map1Model>(MemberList.Source)
                .IgnoreSource(x => x.Name);
        });
        config.AssertConfigurationIsValid();
        var source = new Map2Model { EntityID = 1 };
        var dest = config.CreateMapper().Map<Map1Model>(source);
        Assert.AreEqual(source.EntityID, dest.EntityID);
    }
    
    [TestMethod]
    public void IgnoreSource_EntityID_Not_Mapped()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Map2Model, Map1Model>(MemberList.Source)
                .IgnoreSource(x => x.EntityID)
                .IgnoreSource(x => x.Name);
        });
        config.AssertConfigurationIsValid();
        var source = new Map2Model { EntityID = 1 };
        var dest = config.CreateMapper().Map<Map1Model>(source);
        Assert.AreNotEqual(source.EntityID, dest.EntityID);
        Assert.AreEqual(default, dest.EntityID);
    }
    
    [TestMethod]
    public void IgnoreAllUnmappedComplexTypes_Success()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<SourceModel, DestinationModel>()
                .IgnoreAllUnmappedComplexTypes();
        });
        config.AssertConfigurationIsValid();
        var source = new SourceModel { EntityID = 1, Name = "Test Property", RelatedEntities = [1, 5, 7] };
        var dest = config.CreateMapper().Map<DestinationModel>(source);
        Assert.AreEqual(source.EntityID, dest.EntityID);
        Assert.AreEqual(source.Name, dest.Name);
        Assert.IsTrue(source.RelatedEntities.Intersect(dest.RelatedEntities).Count() == source.RelatedEntities.Count);
        Assert.IsNull(dest.NotMappedField);
    }

    [TestMethod]
    public void IgnoreAllUnmappedComplexTypes_StringProperty_FailToIgnore()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<SourceV2Model, DestinationModel>()
                .IgnoreAllUnmappedComplexTypes();
        });
        try
        {
            config.AssertConfigurationIsValid();
        }
        catch (AutoMapperConfigurationException ex)
        {
            Assert.AreEqual(ex.Errors.Single().UnmappedPropertyNames.Single(), nameof(DestinationModel.Name));
            return;
        }
        Assert.Fail("Mapper assert configuration exception expected.");
    }
    
    private class Map1Model
    {
        public int EntityID { get; set; }
    }
    
    private class Map2Model
    {
        public int EntityID { get; set; }
        public string Name { get; set; }
    }
    
    private class SourceModel
    {
        public int EntityID { get; set; }
        public string Name { get; set; }
        public List<int> RelatedEntities { get; set; }
    }
    
    private class SourceV2Model
    {
        public int EntityID { get; set; }
    }
    
    private class DestinationModel
    {
        public int EntityID { get; set; }
        public string Name { get; set; }
        public List<int> RelatedEntities { get; set; }
        public List<int> NotMappedField { get; set; }
    }
}