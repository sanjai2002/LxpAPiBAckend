using AutoMapper;
using LXP.Common.Entities;
using LXP.Core.IServices;
using LXP.Data.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LXP.Common.ViewModels;
namespace LXP.Core.Services
{
    public class MaterialTypeServices:IMaterialTypeServices                                        // inheriting with Imaterial services
    {
        private readonly IMaterialTypeRepository _materialTypeRepository;
        private Mapper _materialTypeMapper;
        public MaterialTypeServices(IMaterialTypeRepository materialTypeRepository) 
        {
            var _configMaterialType = new MapperConfiguration(cfg => cfg.CreateMap<MaterialType,MaterialTypeViewModel>().ReverseMap());
            _materialTypeMapper = new Mapper(_configMaterialType);
         _materialTypeRepository = materialTypeRepository;
        }
        public List<MaterialTypeViewModel> GetAllMaterialType() 
        {
            return _materialTypeMapper.Map<List<MaterialType>,List<MaterialTypeViewModel>>(_materialTypeRepository.GetAllMaterialTypes());
        }
    }
}
