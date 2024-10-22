﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// Application user class with the Indentitiy user options, extends with Bio and Displayname
    /// </summary>
    public class AppUser : IdentityUser<Guid>
    {
        public DateTime DateOfBirth { get; set; }
        public string DisplayName { get; set; }
        public string Bio { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime LastActive { get; set; } = DateTime.Now;
        public ICollection<Photo> Photos { get; set; }
        public ICollection<WorkoutEventAttendee> WorkoutEvents { get; set; }
        public ICollection<UserFollowing> Followings { get; set; } // User is following them
        public ICollection<UserFollowing> Followers { get; set; } // They are following the user
        public ICollection<UserLike> LikedByUsers { get; set; } // They liked the user
        public ICollection<UserLike> LikedUsers { get; set; } // The user liked them
        public ICollection<AppUserRole> UserRoles { get; set; }
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
