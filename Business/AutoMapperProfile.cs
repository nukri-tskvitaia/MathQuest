using AutoMapper;
using Business.Models;
using Data.Entities;
using Data.Entities.Identity;

namespace Business
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<RegisterModel, Person>()
                .ForMember(rm => rm.FirstName, r => r.MapFrom(x => x.FirstName))
                .ForMember(rm => rm.LastName, r => r.MapFrom(x => x.LastName))
                .ForMember(rm => rm.BirthDate, r => r.MapFrom(x => x.BirthDate))
                .ForMember(rm => rm.Gender, r => r.MapFrom(x => x.Gender));

            CreateMap<RegisterModel, User>()
                .ForMember(rm => rm.UserName, r => r.MapFrom(x => x.UserName))
                .ForMember(rm => rm.Email, r => r.MapFrom(x => x.Email))
                .ForMember(rm => rm.PhoneNumber, r => r.MapFrom(x => x.PhoneNumber ?? ""))
                .ForMember(rm => rm.ProfilePictureUrl, r => r.MapFrom(x => x.ProfilePictureUrl ?? null));


            CreateMap<User, UserModel>()
                .ForMember(rm => rm.Id, r => r.MapFrom(x => x.Id))
                .ForMember(rm => rm.PersonId, r => r.MapFrom(x => x.PersonId))
                .ForMember(rm => rm.FirstName, r => r.MapFrom(x => x.Person!.FirstName))
                .ForMember(rm => rm.LastName, r => r.MapFrom(x => x.Person!.LastName))
                .ForMember(rm => rm.BirthDate, r => r.MapFrom(x => x.Person!.BirthDate))
                .ForMember(rm => rm.Gender, r => r.MapFrom(x => x.Person!.Gender));

            CreateMap<UserModel, User>()
                .ForMember(rm => rm.Id, r => r.MapFrom(x => x.Id))
                .ForMember(rm => rm.PersonId, r => r.MapFrom(x => x.PersonId));

            CreateMap<UserModel, Person>()
                .ForMember(rm => rm.Id, r => r.MapFrom(x => x.PersonId))
                .ForMember(rm => rm.FirstName, r => r.MapFrom(x => x.FirstName))
                .ForMember(rm => rm.LastName, r => r.MapFrom(x => x.LastName))
                .ForMember(rm => rm.BirthDate, r => r.MapFrom(x => x.BirthDate))
                .ForMember(rm => rm.Gender, r => r.MapFrom(x => x.Gender));

            CreateMap<Person, UserInfoModel>()
                .ForMember(rm => rm.FirstName, r => r.MapFrom(x => x.FirstName))
                .ForMember(rm => rm.LastName, r => r.MapFrom(x => x.LastName))
                .ForMember(rm => rm.BirthDate, r => r.MapFrom(x => x.BirthDate))
                .ForMember(rm => rm.Gender, r => r.MapFrom(x => x.Gender));
            CreateMap<User, UserInfoModel>()
                .ForMember(rm => rm.Email, r => r.MapFrom(x => x.Email));

            CreateMap<UserInfoModel, Person>()
                .ForMember(rm => rm.FirstName, r => r.MapFrom(x => x.FirstName))
                .ForMember(rm => rm.LastName, r => r.MapFrom(x => x.LastName))
                .ForMember(rm => rm.BirthDate, r => r.MapFrom(x => x.BirthDate))
                .ForMember(rm => rm.Gender, r => r.MapFrom(x => x.Gender))
                .ForMember(rm => rm.Id, r => r.Ignore());
            CreateMap<UserInfoModel, User>()
                .ForMember(rm => rm.Email, r => r.MapFrom(x => x.Email))
                .ForAllMembers(rm => rm.Ignore());

            CreateMap<User, FriendInfoModel>()
                .ForMember(rm => rm.UserName, r => r.MapFrom(x => x.UserName))
                .ForMember(rm => rm.ProfilePictureUrl, r => r.MapFrom(x => x.ProfilePictureUrl));
            CreateMap<User, FriendInfoExtendedModel>()
                .ForMember(rm => rm.Id, r => r.MapFrom(x => x.Id))
                .ForMember(rm => rm.UserName, r => r.MapFrom(x => x.UserName))
                .ForMember(rm => rm.ProfilePicture, r => r.MapFrom(x => x.ProfilePictureUrl));

            CreateMap<Message, MessageModel>()
                .ForMember(rm => rm.Text, r => r.MapFrom(x => x.Text))
                .ForMember(rm => rm.SentDate, r => r.MapFrom( x => x.SentDate));

            CreateMap<MessageModel, Message>()
                .ForMember(rm => rm.SenderId, r => r.MapFrom(x => x.SenderId))
                .ForMember(rm => rm.ReceiverId, r => r.MapFrom(x => x.ReceiverId))
                .ForMember(rm => rm.Text, r => r.MapFrom(x => x.Text))
                .ForMember(rm => rm.SentDate, r => r.MapFrom(x => x.SentDate));

            CreateMap<Leaderboard, LeaderboardDataModel>()
                .ForMember(rm => rm.Id, r => r.MapFrom(x => x.Id))
                .ForMember(rm => rm.UserId, r => r.MapFrom(x => x.UserId))
                .ForMember(rm => rm.Points, r => r.MapFrom(x => x.Points))
                .ForMember(rm => rm.UserName, r => r.MapFrom(x => x.User!.UserName));
            CreateMap<LeaderboardDataModel, Leaderboard>()
                .ForMember(rm => rm.UserId, r => r.MapFrom(x => x.UserId))
                .ForMember(rm => rm.Points, r => r.MapFrom(x => x.Points));

            CreateMap<Feedback, FeedbackModel>()
                .ForMember(rm => rm.Id, r => r.MapFrom(x => x.Id))
                .ForMember(rm => rm.UserId, r => r.MapFrom(x => x.UserId))
                .ForMember(rm => rm.FeedbackDescription, r => r.MapFrom(x => x.FeedbackDescription))
                .ForMember(rm => rm.FeedbackText, r => r.MapFrom(x => x.FeedbackText))
                .ForMember(rm => rm.FeedbackDate, r => r.MapFrom(x => x.FeedbackDate));
            CreateMap<FeedbackModel, Feedback>()
                .ForMember(rm => rm.Id, r => r.MapFrom(x => x.Id))
                .ForMember(rm => rm.UserId, r => r.MapFrom(x => x.UserId))
                .ForMember(rm => rm.FeedbackDescription, r => r.MapFrom(x => x.FeedbackDescription))
                .ForMember(rm => rm.FeedbackText, r => r.MapFrom(x => x.FeedbackText))
                .ForMember(rm => rm.FeedbackDate, r => r.MapFrom(x => x.FeedbackDate));

        }
    }
}
