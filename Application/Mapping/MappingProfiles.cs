using Application.Core;
using Application.Photos.DTOs;
using Application.UserProfile.DTOs;
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
            string currentUsername = null; // Current logged in user following this profile or not

            CreateMap<WorkoutEvent, WorkoutEvent>();

            CreateMap<WorkoutEvent, WorkoutEventDTO>()
                .ForMember(d => d.HostUsername, o => o.MapFrom(s => s.Attendees.FirstOrDefault(x => x.IsHost).AppUser.UserName));

            CreateMap<WorkoutEventAttendee, AttendeeDTO>()
                .ForMember(d => d.Username, o => o.MapFrom(s => s.AppUser.UserName))
                .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.AppUser.DisplayName))
                .ForMember(d => d.Email, o => o.MapFrom(s => s.AppUser.Email))
                .ForMember(d => d.Bio, o => o.MapFrom(s => s.AppUser.Bio))
                .ForMember(d => d.Image, o => o.MapFrom(s => s.AppUser.Photos.FirstOrDefault(x => x.IsMain).Url));

            CreateMap<Photo, PhotoDTO>()
                .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.AppUser.DisplayName))
                .ForMember(d => d.MainImage, o => o.MapFrom(s => s.AppUser.Photos.FirstOrDefault(x => x.IsMain).Url));

            CreateMap<AppUser, ProfileDTO>()
                .ForMember(d => d.Age, o => o.MapFrom(s => s.DateOfBirth.CalculateAge()))
                .ForMember(d => d.Image, o => o.MapFrom(s => s.Photos.FirstOrDefault(x => x.IsMain).Url))
                .ForMember(d => d.Following, o => o.MapFrom(s => s.Followers.Any(x => x.Observer.UserName == currentUsername)))
                .ForMember(d => d.FollowersCount, o => o.MapFrom(s => s.Followers.Count))
                .ForMember(d => d.FollowingCount, o => o.MapFrom(s => s.Followings.Count));

            CreateMap<WorkoutEventAttendee, UserEventDTO>()
                .ForMember(d => d.Event_Id, o => o.MapFrom(s => s.WorkoutEvent.WorkoutEvent_Id))
                .ForMember(d => d.Title, o => o.MapFrom(s => s.WorkoutEvent.Title))
                .ForMember(d => d.Category, o => o.MapFrom(s => s.WorkoutEvent.Category))
                .ForMember(d => d.Date, o => o.MapFrom(s => s.WorkoutEvent.Date))
                .ForMember(d => d.HostUsername, o => o.MapFrom(s => s.WorkoutEvent.Attendees.FirstOrDefault(x => x.IsHost).AppUser.UserName));
        }
    }
}
