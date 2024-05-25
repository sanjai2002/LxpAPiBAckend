using AutoMapper;
using LXP.Common.Entities;
using LXP.Common.ViewModels;
using LXP.Core.IServices;
using LXP.Data.IRepository;
namespace LXP.Core.Services
{


    public class MaterialTypeServices : IMaterialTypeServices
    {
        private readonly IMaterialTypeRepository _materialTypeRepository;
        private Mapper _materialTypeMapper;
        public MaterialTypeServices(IMaterialTypeRepository materialTypeRepository)
        {
            var _configMaterialType = new MapperConfiguration(cfg => cfg.CreateMap<MaterialType, MaterialTypeViewModel>().ReverseMap());
            _materialTypeMapper = new Mapper(_configMaterialType);
            _materialTypeRepository = materialTypeRepository;
        }
        public List<MaterialTypeViewModel> GetAllMaterialType()
        {
            return _materialTypeMapper.Map<List<MaterialType>, List<MaterialTypeViewModel>>(_materialTypeRepository.GetAllMaterialTypes());
        }
    }
}
