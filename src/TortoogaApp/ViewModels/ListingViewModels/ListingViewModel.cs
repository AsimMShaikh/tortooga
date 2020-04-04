using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace TortoogaApp.ViewModels.ListingViewModels
{
    public class ListingViewModel
    {
        public Guid? ListingGuid { get; set; }
        public string ReferenceNumber { get; set; }

        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required, MaxLength(1000)]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Departure Location")]
        public string Origin { get; set; }

        [Required]
        [Display(Name = "Arriving Location")]
        public string Destination { get; set; }

        [Required, Range(1, 100)]
        [Display(Name = "Length")]
        public double? Length { get; set; }

        [Required, Range(1, 100)]
        [Display(Name = "Width")]
        public double? Width { get; set; }

        [Required, Range(1, 100)]
        [Display(Name = "Height")]
        public double? Height { get; set; }

        [Required, Range(1, 99999)]
        [Display(Name = "Price")]
        public decimal? Price { get; set; }

        [Required]
        [Display(Name = "Departure Date")]
        public DateTime? DepartureDate { get; set; }

        [Required]
        [Display(Name = "Estimated Arrival Date")]
        public DateTime? EstimatedArrivalDate { get; set; }

        [Required, Range(1, 50)]
        [Display(Name = "Total Transits")]
        public int? TransitStops { get; set; }

        [Required]
        [Display(Name = "Contact Details")]
        public string ContactDetails { get; set; }

        [Required]
        [Display(Name = "Drop Off Date")]
        public DateTime? AppraisalDate { get; set; }

        [Required]
        [Display(Name = "Drop Off Time")]
        public DateTime? AppraisalTime { get; set; }

        [Required]
        [Display(Name = "Drop Off Address")]
        public string DropOffAddress { get; set; }

        [Required]
        [Display(Name = "Pick Up Address")]
        public string PickUpAddress { get; set; }

        public IFormFile ListingImage { get; set; }
    }
}