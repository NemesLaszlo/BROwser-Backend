using Application.Photos.DTOs;
using Application.WorkoutEvents.DTOs;
using AutoMapper;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mapping
{
    /// <summary>
    /// Mapping definitons for AutoMapper
    /// </summary>
    public class MappingProfiles : Profile
    {
        // Mapping configurations
        public MappingProfiles()
        {
            CreateMap<WorkoutEvent, WorkoutEvent>();

            CreateMap<WorkoutEvent, WorkoutEventDTO>();

            CreateMap<Photo, PhotoDTO>()
                .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.AppUser.DisplayName))
                .ForMember(d => d.MainImage, o => o.MapFrom(s => s.AppUser.Photos.FirstOrDefault(x => x.IsMain).Url));
        }
    }
}
