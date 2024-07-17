using Business.Models;
using Data.Entities;
using Data.Entities.Identity;
using NUnit.Framework.Legacy;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MathQuest.Tests
{
    internal class PersonEqualityComparer : IEqualityComparer<Person>
    {
        public bool Equals([AllowNull]  Person x, [AllowNull] Person y)
        {
            if (x == null && y == null)
            {
                return true;
            }
            if (x == null || y == null)
            {
                return false;
            }

            return x.Id == y.Id
                && x.FirstName == y.FirstName
                && x.LastName == y.LastName
                && x.BirthDate == y.BirthDate
                && x.Gender == y.Gender;
        }

        public int GetHashCode([DisallowNull] Person obj)
        {
            return obj.GetHashCode();
        }
    }

    internal class UserEqualityComparer : IEqualityComparer<User>
    {
        public bool Equals([AllowNull]  User x, [AllowNull] User y)
        {
            if(x == null && y == null)
            {
                return true;
            }
            if(x == null || y == null)
            {
                return false;
            }

            return x.Id == y.Id
                && x.PersonId == y.PersonId
                && x.RegistrationDate == y.RegistrationDate
                && x.GradeId == y.GradeId;
        }

        public int GetHashCode([DisallowNull] User obj)
        {
            return obj.GetHashCode();
        }
    }

    internal class ExerciseEqualityComparer : IEqualityComparer<Exercise>
    {
        public bool Equals([AllowNull] Exercise x, [AllowNull] Exercise y)
        {
            if (x == null && y == null)
            {
                return true;
            }
            if (x == null || y == null)
            {
                return false;
            }

            return x.Id == y.Id
                && x.Title == y.Title
                && x.Description == y.Description
                && x.Question == y.Question
                && x.PossibleAnswers.SequenceEqual(y.PossibleAnswers)
                && x.CorrectAnswer == y.CorrectAnswer
                && x.DifficultyLevel == y.DifficultyLevel
                && x.GradeId == y.GradeId;
        }

        public int GetHashCode([DisallowNull] Exercise obj)
        {
            return obj.GetHashCode();
        }
    }

    internal class ExerciseRatingEqualityComparer : IEqualityComparer<ExerciseRating>
    {
        public bool Equals([AllowNull] ExerciseRating x, [AllowNull] ExerciseRating y)
        {
            if (x == null && y == null)
            {
                return true;
            }
            if (x == null || y == null)
            {
                return false;
            }

            return x.UserId == y.UserId
                && x.ExerciseId == y.ExerciseId
                && x.Rating == y.Rating
                && x.RatingDate == y.RatingDate
                && x.FeedbackText == y.FeedbackText;
        }

        public int GetHashCode([DisallowNull] ExerciseRating obj)
        {
            return obj.GetHashCode();
        }
    }

    internal class LeaderboardEqualityComparer : IEqualityComparer<Leaderboard>
    {
        public bool Equals([AllowNull] Leaderboard x, [AllowNull] Leaderboard y)
        {
            if (x == null && y == null)
            {
                return true;
            }
            if (x == null || y == null)
            {
                return false;
            }

            return x.Id == y.Id
                && x.UserId == y.UserId
                && x.Points == y.Points;
        }

        public int GetHashCode([DisallowNull] Leaderboard obj)
        {
            return obj.GetHashCode();
        }
    }

    internal class TheoryEqualityComparer : IEqualityComparer<Theory>
    {
        public bool Equals([AllowNull] Theory x, [AllowNull] Theory y)
        {
            if (x == null && y == null)
            {
                return true;
            }
            if (x == null || y == null)
            {
                return false;
            }

            return x.Id == y.Id
                && x.Title == y.Title
                && x.Description == y.Description
                && x.Content == y.Content
                && x.GradeId == y.GradeId;
        }

        public int GetHashCode([DisallowNull] Theory obj)
        {
            return obj.GetHashCode();
        }
    }

    internal class TheoryRatingEqualityComparer : IEqualityComparer<TheoryRating>
    {
        public bool Equals([AllowNull] TheoryRating x, [AllowNull] TheoryRating y)
        {
            if (x == null && y == null)
            {
                return true;
            }
            if (x == null || y == null)
            {
                return false;
            }

            return x.UserId == y.UserId
                && x.TheoryId == y.TheoryId
                && x.Rating == y.Rating
                && x.RatingDate == y.RatingDate
                && x.FeedbackText == y.FeedbackText;
        }

        public int GetHashCode([DisallowNull] TheoryRating obj)
        {
            return obj.GetHashCode();
        }
    }

    internal class GradeEqualityComparer : IEqualityComparer<Grade>
    {
        public bool Equals([AllowNull] Grade? x, [AllowNull] Grade? y)
        {
            if (x == null && y == null)
            {
                return true;
            }
            if (x == null || y == null)
            {
                return false;
            }

            return x.Id == y.Id
                && x.GradeName == y.GradeName
                && x.GradeLevel == y.GradeLevel;
        }

        public int GetHashCode([DisallowNull] Grade obj)
        {
            return obj.GetHashCode();
        }
    }
    /*

    internal class PictureEqualityComparer : IEqualityComparer<Picture>
    {
        public bool Equals(Picture? x, Picture? y)
        {
            if (x == null && y == null)
            {
                return true;
            }
            if (x == null || y == null)
            {
                return false;
            }

            return x.Id == y.Id
                && x.Title == y.Title
                && x.Description == y.Description
                && x.Width == y.Width
                && x.Height == y.Height
                && x.Size == y.Size
                && x.Tags == y.Tags
                && x.Privacy == y.Privacy
                && x.ContentType == y.ContentType
                && x.UploadDate == y.UploadDate
                && x.ImageData.SequenceEqual(y.ImageData);
        }

        public int GetHashCode([DisallowNull] Picture obj)
        {
            return obj.GetHashCode();
        }
    }
    */

    internal class NotificationEqualityComparer : IEqualityComparer<Notification>
    {
        public bool Equals([AllowNull] Notification? x, [AllowNull] Notification? y)
        {
            if (x == null && y == null)
            {
                return true;
            }
            if (x == null || y == null)
            {
                return false;
            }

            return x.Id == y.Id
                && x.NotificationDate == y.NotificationDate
                && x.NotificationType == y.NotificationType
                && x.NotificationText == y.NotificationText;
        }

        public int GetHashCode([DisallowNull] Notification obj)
        {
            return obj.GetHashCode();
        }
    }

    internal class MessageEqualityComparer : IEqualityComparer<Message>
    {
        public bool Equals([AllowNull] Message? x, [AllowNull] Message? y)
        {
            if (x == null && y == null)
            {
                return true;
            }
            if (x == null || y == null)
            {
                return false;
            }

            return x.SenderId == y.SenderId
                && x.ReceiverId == y.ReceiverId
                && x.SentDate == y.SentDate
                && x.Text == y.Text;
        }

        public int GetHashCode([DisallowNull] Message obj)
        {
            return obj.GetHashCode();
        }
    }

    internal class FriendshipEqualityComparer : IEqualityComparer<Friend>
    {
        public bool Equals([AllowNull] Friend? x, [AllowNull] Friend? y)
        {
            if (x == null && y == null)
            {
                return true;
            }
            if (x == null || y == null)
            {
                return false;
            }

            return x.RequestedById == y.RequestedById
                && x.RequestedToId == y.RequestedToId
                && x.RequestTime == y.RequestTime
                && x.FriendRequestStatus == y.FriendRequestStatus
                && x.BecameFriendsDate == y.BecameFriendsDate
                && x.RequestTime == y.RequestTime;
        }

        public int GetHashCode([DisallowNull] Friend obj)
        {
            return obj.GetHashCode();
        }
    }

    internal class FeedbackEqualityComparer : IEqualityComparer<Feedback>
    {
        public bool Equals([AllowNull] Feedback? x, [AllowNull] Feedback? y)
        {
            if (x == null && y == null)
            {
                return true;
            }
            if (x == null || y == null)
            {
                return false;
            }

            return x.Id == y.Id
                && x.UserId == y.UserId
                && x.FeedbackDate == y.FeedbackDate
                && x.FeedbackText == y.FeedbackText
                && x.FeedbackDescription == y.FeedbackDescription;
        }

        public int GetHashCode([DisallowNull] Feedback obj)
        {
            return obj.GetHashCode();
        }
    }

    internal class AchievementEqualityComparer : IEqualityComparer<Achievement>
    {
        public bool Equals([AllowNull] Achievement? x, [AllowNull] Achievement? y)
        {
            if (x == null && y == null)
            {
                return true;
            }
            if (x == null || y == null)
            {
                return false;
            }

            return x.Id == y.Id
                && x.AchievementName == y.AchievementName
                && x.AchievementDate == y.AchievementDate
                && x.AchievementType == y.AchievementType
                && x.AchievementDescription == y.AchievementDescription;
        }

        public int GetHashCode([DisallowNull] Achievement obj)
        {
            return obj.GetHashCode();
        }
    }

    internal class UserModelEqualityComparer : IEqualityComparer<UserModel>
    {
        public bool Equals([AllowNull] UserModel x, [AllowNull] UserModel y)
        {
            if (x == null && y == null)
            {
                return true;
            }
            if (x == null || y == null)
            {
                return false;
            }

            return x.Id == y.Id
                && x.PersonId == y.PersonId
                && x.FirstName == y.FirstName
                && x.LastName == y.LastName
                && x.BirthDate == y.BirthDate
                && x.Gender == y.Gender;
        }

        public int GetHashCode([DisallowNull] UserModel obj)
        {
            return obj.GetHashCode();
        }
    }
}
