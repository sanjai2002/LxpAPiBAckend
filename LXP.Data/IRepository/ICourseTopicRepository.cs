﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LXP.Common.Entities;

namespace LXP.Data.IRepository
{
    public interface ICourseTopicRepository
    {
        object GetTopicDetails(string courseId);
        List<Topic> GetAllTopics();
        Task AddCourseTopic(Topic topic);
        bool AnyTopicByTopicName(string topicName);
        Task<Topic> GetTopicDetailsByTopicNameAndCourse(string topicName, Guid courseId);

        Task<int> UpdateCourseTopic(Topic topic);
        object GetAllTopicDetailsByCourseId(string courseId);
        Task<Topic> GetTopicByTopicId(Guid topicId);
        Task<List<Topic>> GetTopicsbycouresId(Guid courseId);
        Task<List<LearnerProgress>> GetTopicsbyLearnerId(Guid courseId, Guid LearnerId);
    }
}
