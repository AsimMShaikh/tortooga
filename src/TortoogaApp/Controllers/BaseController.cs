using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TortoogaApp.Security;
using TortoogaApp.Services;
using Microsoft.AspNetCore.Identity;
using TortoogaApp.ViewModels.BadgeCountsViewModels;

namespace TortoogaApp.Controllers
{
    public class BaseController : Controller
    {
        private ICommonservice _commonservice;
        private UserManager<ApplicationUser> _userManager;

        #region Constructors
        public BaseController(ICommonservice commonservice, UserManager<ApplicationUser> userManager)
        {
            this._commonservice = commonservice;
            this._userManager = userManager;
        }

        #endregion

        #region Methods

        /// <summary>
        /// To Fetch the Parameter Values
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        protected Dictionary<string, string> GetParameters(List<string> columns)
        {
            var queryStrings = Request.Query;
            var sortingColumn = columns[Convert.ToInt32(queryStrings["iSortCol_0"])];
            var sortDir = queryStrings["sSortDir_0"];           
            var pageStart = Convert.ToInt32(queryStrings["iDisplayStart"]);
            var pageSize = Convert.ToInt32(queryStrings["iDisplayLength"]) == 0 ? 10 : Convert.ToInt32(queryStrings["iDisplayLength"]);
            var pageIndex = pageStart == 0 ? 1 : (pageStart / pageSize) + 1;
            var searchBy = System.Net.WebUtility.UrlDecode(queryStrings["srchBy"]);
            var searchTxt = System.Net.WebUtility.UrlDecode(queryStrings["srchTxt"]);
            //var carrierVal = queryStrings["CarrierGuid"];
            var parameters = new Dictionary<string, string>
                {
                    {"sort_by", sortingColumn},
                    {"sort_order", sortDir},
                    {"results_per_page", pageSize.ToString()},
                    {"page_number", pageIndex.ToString()},
                    {"PageStart", pageStart.ToString()},
                };
            if (!string.IsNullOrWhiteSpace(searchBy))
            {
                //searchTxt = CheckIfDate(searchTxt);
                parameters.Add("Key", searchBy);
                parameters.Add("Value", string.IsNullOrEmpty(searchTxt) ? string.Empty : searchTxt.ToString().ToLower());
            }
           
            //if (!string.IsNullOrWhiteSpace(carrierVal))
            //{
            //    //searchTxt = CheckIfDate(searchTxt);                
            //    parameters.Add("Carrier", string.IsNullOrEmpty(carrierVal) ? string.Empty : carrierVal.ToString().ToLower());
            //}


            return parameters;
        }


        /// <summary>
        /// For fetching the BadgeCounts To be shown on the menu
        /// </summary>
        /// <returns></returns>
        public async Task<ContentResult> GetBadgeCounts()
        {
            var data = string.Empty;
            BadgeCountViewModel badgecount = new BadgeCountViewModel();
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            if (await _userManager.IsInRoleAsync(currentUser, RoleType.CARRIER_ADMIN))
            {
                badgecount = _commonservice.GetBadgeCounts(currentUser.Id, currentUser.CarrierGuid, true);
            }
            else
            {
                badgecount = _commonservice.GetBadgeCounts(currentUser.Id, currentUser.CarrierGuid, false);
            }

            data = CommonService.Json.Serialize(new { Success = true, data = badgecount });
            return Content(data, "application/json");
        }

        #endregion
    }
}