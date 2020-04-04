using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TortoogaApp.Models;
using TortoogaApp.Security;

namespace TortoogaApp.Data
{
    public class TortoogaSeedData
    {
        private ApplicationDbContext _context;
        private RoleManager<Role> _roleManager;
        private UserManager<ApplicationUser> _userManager;
        private readonly string sysAdminEmail = "sysadmin@tortooga.com";
        private readonly string carrierAdminEmail1 = "carrier1@email.com";
        private readonly string carrierAdminEmail2 = "carrier2@email.com";
        private readonly string normalUserEmail = "dan@email.com";

        /// <summary>
        /// Initializes a new instance of the <see cref="TortoogaSeedData"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="userManager">The user manager.</param>
        /// <param name="roleManager">The role manager.</param>
        public TortoogaSeedData(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<Role> roleManager)
        {
            this._context = context;
            this._userManager = userManager;
            this._roleManager = roleManager;
        }

        public async void DevEnvSeedData()
        {
            await SetupUserRoles();
            SetupListings();
            await SetupUsersAndAssignRoles();

            _context.SaveChanges();
        }

        /// <summary>
        /// Setups the listings.
        /// </summary>
        /// <returns></returns>
        private void SetupListings()
        {
            if (!_context.Carriers.Any())
            {
                List<Carrier> seedCarriers = null;

                seedCarriers = new List<Carrier>()
                {
                    new Carrier()
                    {
                        AccountNumber = "EB928445",
                        AddressLine1= "425 S. Palos Verdes Street",
                        PostCode = "90731",
                        City = "San Pedro",
                        State = "Oregon",
                        Country = "United States",
                        BusinessName = "Poseidon Shipping Pty Ltd",
                        Email = "support@poseidonshippingtor.com",
                        PhoneNumber = "1300228444",
                        MobileNumber = "+512232322",
                        CompanyBio = "We are the best company in the world and Lorem ipsum dolor sit amet, consectetur adipisicing elit. Voluptatem, exercitationem, suscipit, distinctio, qui sapiente aspernatur molestiae non corporis magnisuscipit",
                        ContactPerson = "Jimmy Borat",
                        MCDotNumber = "582231F",
                        SiteUrl = "http://www.poseidonshipping.com",
                        BankingDetails = new CarrierBankingDetails()
                        {
                            AccountName = "Poseiden Business Funds",
                            BankIdentificationNumber = "044122",
                            AccountNumber = "10288811"
                        },
                        Listings = new List<Listing>() {
                            new Listing
                            {
                                Title = "From Los Angelas to Seattle, ample space available",
                                ListingGuid = Guid.NewGuid(),
                                Price = 2500.00m,
                                AppraisalDateTime = new DateTime(2016,12,12,13,00,00),
                                ContactDetails = "support@poseidonshippingtor.com",
                                DepartureDate = new DateTime(2016,12,20),
                                Destination = "Seattle",
                                EstimatedArrivalDate = new DateTime(2016,12,30),
                                TransitStops = 1,
                                Description = @"Lorem ipsum dolor sit amet, consectetur adipisicing elit. Voluptatem, exercitationem, suscipit, distinctio, qui sapiente aspernatur molestiae non corporis magnisuscipit, distinctio, qui sapiente aspernatur molestiae non corporis magni consectetur adipisicing elit. Voluptatem, exercitationem, suscipit, distinctio, qui sapiente",
                                Height = 6, Width = 7.5, Length = 8,
                                Origin = "Los Angelas",
                                DropOffAddress = "425 S. Palos Verdes Street, San Pedro, 90731, CA, United States",
                                PickUpAddress = "2203 Alaskan Way, Seattle, 98121, WA, United States",
                                ReferenceNumber = "100000000"
                            }
                        }
                    },
                    new Carrier()
                    {
                        AccountNumber = "UD148383",
                        AddressLine1= "2203 Alaskan Way",
                        PostCode = "98121",
                        City = "Seattle",
                        State = "California",
                        Country = "United States",
                        BusinessName = "Sheddex Freight",
                        Email = "shipping-enquires@sheddextor.com",
                        PhoneNumber = "1300773554",
                        MobileNumber = "+144444444",
                        CompanyBio = "Sheddex Freight has been serving the industry for 23 ... ipsum dolor sit amet, consectetur adipisicing elit. Voluptatem, exercitationem, suscipit, distinctio, qui sapiente aspernatur molestiae non corporis magnisuscipit",
                        ContactPerson = "Arapama Jihud",
                        MCDotNumber = "881193D",
                        SiteUrl = "http://www.sheddex-freight.com",
                        BankingDetails = new CarrierBankingDetails()
                        {
                            AccountName = "Ararpama Pty Ltd",
                            BankIdentificationNumber = "098212",
                            AccountNumber = "04221232"
                        },
                        Listings = new List<Listing>() {
                            new Listing
                            {
                                Title = "Moving? Seattle to Los Angelas",
                                ListingGuid = Guid.NewGuid(),
                                Price = 788.00m,
                                AppraisalDateTime = new DateTime(2016,12,5,10,30,00),
                                ContactDetails = "1300773554",
                                DepartureDate = new DateTime(2016,12,10),
                                Destination = "Los Angelas",
                                EstimatedArrivalDate = new DateTime(2016,12,24),
                                Description = @"Lorem consectetur adipisicing elit. Voluptatem, exercitationem, suscipit, distinctio, qui sapiente aspernatur molestiae non corporis magniconsectetur adipisicing elit. Voluptatem, exercitationem, suscipit, distinctio, qui sapiente aspernatur molestiae non corporis magni",
                                TransitStops = 3,
                                Height = 10, Width = 10, Length = 7,
                                Origin = "Seattle",
                                DropOffAddress = "2203 Alaskan Way, Seattle, 98121, WA, United States",
                                PickUpAddress = "425 S. Palos Verdes Street, San Pedro, 90731, CA, United States",
                                ReferenceNumber = "200000000"
                            }
                        }
                    }
                };

                _context.Carriers.AddRange(seedCarriers);
                seedCarriers.ForEach(v => _context.Listings.Add(v.Listings.FirstOrDefault()));
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Setups the users and assign roles to them.
        /// </summary>
        /// <returns></returns>
        private async Task SetupUsersAndAssignRoles()
        {
            if (!_context.Users.Any())
            {
                var carriers = _context.Carriers.ToArray();
                var allSamePassword = "password";

                var normalUser = await _userManager.FindByNameAsync(normalUserEmail);
                if (normalUser == null)
                {
                    var user = new ApplicationUser { UserName = normalUserEmail, Email = normalUserEmail };
                    await _userManager.CreateAsync(user, allSamePassword);
                }

                var sysAdmin = await _userManager.FindByNameAsync(sysAdminEmail);
                if (sysAdmin == null)
                {
                    var user = new ApplicationUser { UserName = sysAdminEmail, Email = sysAdminEmail };
                    await _userManager.CreateAsync(user, allSamePassword);

                    //TODO: Use Roles for now, as claim based role needs to do policy etc, investigate later
                    //await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, RoleType.SYSTEM_ADMIN));

                    if (!await _userManager.IsInRoleAsync(user, RoleType.SYSTEM_ADMIN))
                    {
                        await _userManager.AddToRoleAsync(user, RoleType.SYSTEM_ADMIN);
                    }
                }

                var carrierAdmin1 = await _userManager.FindByNameAsync(carrierAdminEmail1);
                if (carrierAdmin1 == null)
                {
                    var user = new ApplicationUser { UserName = carrierAdminEmail1, Email = carrierAdminEmail1, Carrier = carriers[0] };
                    await _userManager.CreateAsync(user, allSamePassword);

                    if (!await _userManager.IsInRoleAsync(user, RoleType.CARRIER_ADMIN))
                    {
                        await _userManager.AddToRoleAsync(user, RoleType.CARRIER_ADMIN);
                    }
                }

                var carrierAdmin2 = await _userManager.FindByNameAsync(carrierAdminEmail2);
                if (carrierAdmin2 == null)
                {
                    var user = new ApplicationUser { UserName = carrierAdminEmail2, Email = carrierAdminEmail2, Carrier = carriers[1] };
                    await _userManager.CreateAsync(user, allSamePassword);

                    if (!await _userManager.IsInRoleAsync(user, RoleType.CARRIER_ADMIN))
                    {
                        await _userManager.AddToRoleAsync(user, RoleType.CARRIER_ADMIN);
                    }
                }
            }
        }

        /// <summary>
        /// Setups the foundation user roles
        /// </summary>
        /// <returns></returns>
        private async Task SetupUserRoles()
        {
            var sysAdminRole = await _roleManager.FindByNameAsync(RoleType.SYSTEM_ADMIN);
            if (sysAdminRole == null)
            {
                sysAdminRole = new Role(RoleType.SYSTEM_ADMIN);
                await _roleManager.CreateAsync(sysAdminRole);
            }

            var carrierAdminRole = await _roleManager.FindByNameAsync(RoleType.CARRIER_ADMIN);
            if (carrierAdminRole == null)
            {
                carrierAdminRole = new Role(RoleType.CARRIER_ADMIN);
                await _roleManager.CreateAsync(carrierAdminRole);
            }
        }
    }
}