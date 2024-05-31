using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using LXP.Common.Entities;
using LXP.Common.ViewModels;
using LXP.Core.IServices;
using LXP.Data.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Core.Services
{
    public class MaterialServices : IMaterialServices
    {
        private readonly IMaterialRepository _materialRepository;
        private readonly ICourseTopicRepository _courseTopicRepository;
       private readonly IMaterialTypeRepository _materialTypeRepository;
        private readonly IWebHostEnvironment _environment;
        private readonly IHttpContextAccessor _contextAccessor;
        private Mapper _courseMaterialMapper;



        public MaterialServices(IMaterialTypeRepository materialTypeRepository,IMaterialRepository materialRepository,ICourseTopicRepository courseTopicRepository, IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor)
        {
            _materialRepository = materialRepository;
            _courseTopicRepository = courseTopicRepository;
            _materialTypeRepository = materialTypeRepository;
            _environment = environment;
            _contextAccessor = httpContextAccessor;
            var _configCourseMaterial = new MapperConfiguration(cfg => cfg.CreateMap<Material, MaterialListViewModel>().ReverseMap());
            _courseMaterialMapper = new Mapper(_configCourseMaterial);


        }
        public async Task<MaterialListViewModel> AddMaterial(MaterialViewModel material)
        {
            Topic topic = await _courseTopicRepository.GetTopicByTopicId(Guid.Parse(material.TopicId));
            MaterialType materialType = _materialTypeRepository.GetMaterialTypeByMaterialTypeId(Guid.Parse(material.MaterialTypeId));
            bool isMaterialExists = await _materialRepository.AnyMaterialByMaterialNameAndTopic(material.Name, topic);
            if (!isMaterialExists)
            {
                // Generate a unique file name
                var uniqueFileName = $"{Guid.NewGuid()}_{material.Material.FileName}";

                // Save the image to a designated folder (e.g., wwwroot/images)
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "CourseMaterial"); // Use WebRootPath
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    material.Material.CopyTo(stream); // Use await
                }
                Material materialCreation = new Material()
                {
                    MaterialId = Guid.NewGuid(),
                    Name = material.Name,
                    MaterialType = materialType,

                    CreatedBy = material.CreatedBy,
                    CreatedAt = DateTime.Now,
                    FilePath = uniqueFileName,

                    IsActive = true,
                    IsAvailable = true,
                    Duration = material.Duration,
                    Topic = topic,
                    ModifiedAt = null,
                    ModifiedBy = null
                };
                await _materialRepository.AddMaterial(materialCreation);
                return _courseMaterialMapper.Map<Material, MaterialListViewModel>(materialCreation);
            }
            else
            {
                return null;
            }
        }

        public async Task<List<MaterialListViewModel>> GetAllMaterialDetailsByTopicAndType(string topicId,string materialTypeId)
        {
            Topic topic = await _courseTopicRepository.GetTopicByTopicId(Guid.Parse(topicId));
            MaterialType materialType = _materialTypeRepository.GetMaterialTypeByMaterialTypeId(Guid.Parse(materialTypeId));

            List<Material> material= _materialRepository.GetAllMaterialDetailsByTopicAndType(topic,materialType);

            List<MaterialListViewModel> materialLists = new List<MaterialListViewModel>();

            foreach (var item in material)
            {

                
                MaterialListViewModel materialList = new MaterialListViewModel()
                {
                    MaterialId = item.MaterialId,
                    TopicName = item.Topic.Name,
                    MaterialType = item.MaterialType.Type,
                    Name = item.Name,
                   // FilePath = item.FilePath,


                    FilePath = String.Format("{0}://{1}{2}/wwwroot/CourseMaterial/{3}",
                                             _contextAccessor.HttpContext.Request.Scheme,
                                             _contextAccessor.HttpContext.Request.Host,
                                             _contextAccessor.HttpContext.Request.PathBase,
                                             item.FilePath),


                    Duration = item.Duration,
                    IsActive = item.IsActive,
                    IsAvailable = item.IsAvailable,
                    CreatedAt = item.CreatedAt,
                    CreatedBy = item.CreatedBy,
                    ModifiedAt = item.ModifiedAt.ToString(),
                    ModifiedBy = item.ModifiedBy





                };
                materialLists.Add(materialList);
            }
            return materialLists;
        }

        public async Task<MaterialListViewModel> GetMaterialByMaterialNameAndTopic(string materialName, string topicId)
        {
            Topic topic = await _courseTopicRepository.GetTopicByTopicId(Guid.Parse(topicId));
            Material material = await _materialRepository.GetMaterialByMaterialNameAndTopic(materialName, topic);
            MaterialListViewModel materialView = new MaterialListViewModel()
            {
                MaterialId = material.MaterialId,
                TopicName = material.Topic.Name,
                MaterialType = material.MaterialType.Type,
                Name = material.Name,
                FilePath = material.FilePath,
                Duration = material.Duration,
                IsActive = material.IsActive,
                IsAvailable = material.IsAvailable,
                CreatedAt = material.CreatedAt,
                ModifiedAt = material.ModifiedAt.ToString(),
                ModifiedBy = material.ModifiedBy,
                CreatedBy = material.CreatedBy







            };
            return materialView;
        }
    }
}
