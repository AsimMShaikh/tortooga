using AutoMapper;
using System;
using TortoogaApp.Models;
using TortoogaApp.ViewModels.CarrierViewModels;
using TortoogaApp.ViewModels.ListingViewModels;
using TortoogaApp.ViewModels.UserViewModels;

namespace TortoogaApp
{
    public class AutoMapperProfileConfig : Profile
    {
        public AutoMapperProfileConfig()
        {
            CreateMap<Listing, DetailListingViewModel>()
                .ForMember(dest => dest.CarrierName, opt => opt.MapFrom(x => x.Carrier.BusinessName))
                .ForMember(dest => dest.CarrierRatings, opt => opt.Ignore())
                .ForMember(dest => dest.TotalNumberOfRatings, opt => opt.Ignore());

            CreateMap<Booking, BookingSummaryViewModel>()
                .ForMember(dest => dest.BusinessName, opt => opt.Ignore())
                .ForMember(dest => dest.DepartureDate, opt => opt.MapFrom(x => x.Listing.DepartureDate))
                .ForMember(dest => dest.Destination, opt => opt.MapFrom(x => x.Listing.Destination))
                .ForMember(dest => dest.Origin, opt => opt.MapFrom(x => x.Listing.Origin))
                .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(x => x.CreatedByUser.Email));

            CreateMap<ListingViewModel, Listing>()
                .ForMember(dest => dest.Carrier, opt => opt.Ignore())
                .ForMember(dest => dest.CarrierGuid, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.ReferenceNumber, opt => opt.Ignore())
                .ForMember(dest => dest.SquareFeet, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.AppraisalDateTime, opt => opt.MapFrom(
                      src => new DateTime(
                          src.AppraisalDate.Value.Year, src.AppraisalDate.Value.Month, src.AppraisalDate.Value.Day,
                          src.AppraisalTime.Value.Hour, src.AppraisalTime.Value.Minute, src.AppraisalTime.Value.Second)
                      )
                );

            CreateMap<Listing, ListingViewModel>()
                .ForMember(dest => dest.AppraisalDate, opt => opt.MapFrom(src => src.AppraisalDateTime.Date))
                .ForMember(dest => dest.AppraisalTime, opt => opt.MapFrom(src => src.AppraisalDateTime))
                .ForMember(dest => dest.ListingImage, opt => opt.Ignore());


            CreateMap<UserImageModel, ProfileImage>()
               .ForMember(dest => dest.ImageForUser, opt => opt.Ignore())
               .ForMember(dest => dest.UserId, opt => opt.Ignore())
               .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
               .ForMember(dest => dest.isDeleted, opt => opt.Ignore())
               .ForMember(dest => dest.CreatedOn, opt => opt.Ignore())
               .ForMember(dest => dest.ImageGuid, opt => opt.Ignore());


            CreateMap<Carrier, CarrierProfileViewModel>()
             .ForMember(dest => dest.Countries, opt => opt.Ignore())
             .ForMember(dest => dest.Provincestates, opt => opt.Ignore())
             .ForMember(dest => dest.BankAccountNumber, opt => opt.Ignore())
             .ForMember(dest => dest.BankIdentificationNumber, opt => opt.Ignore())
             .ForMember(dest => dest.AccountName, opt => opt.Ignore())
             .ForMember(dest => dest.ShippingItemsList, opt => opt.Ignore())
             .ForMember(dest => dest.ImageUrl, opt => opt.Ignore());


            CreateMap<UserImageModel, CompanyLogo>()
              .ForMember(dest => dest.CarrierLogo, opt => opt.Ignore())
              .ForMember(dest => dest.CarrierGuid, opt => opt.Ignore())
              .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
              .ForMember(dest => dest.isDeleted, opt => opt.Ignore())
              .ForMember(dest => dest.CreatedOn, opt => opt.Ignore())
              .ForMember(dest => dest.ImageGuid, opt => opt.Ignore());



        }
    }
}