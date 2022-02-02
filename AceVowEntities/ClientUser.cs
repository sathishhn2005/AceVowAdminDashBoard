using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AceVowEntities
{
    public partial class ClientUser
    {
        
        public int Id { get; set; }
        public string Industry { get; set; }
        public string UserName { get; set; }
        public string ImageUrl { get; set; }
        public string Comment { get; set; }
        public string ReplyComment { get; set; }
        public string StoreName { get; set; }
        public string StoreURL { get; set; }
        public string ContactName { get; set; }
        public string Positon { get; set; }
        public string PirmaryContact { get; set; }
        public string EmailId { get; set; }
        public string Password { get; set; }
        public string EncryptedPassword { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string TargetedCities { get; set; }
        public string TargetedCommunities { get; set; }
        public string WelcomeMessage { get; set; }
        public Nullable<int> TrolleryCount { get; set; }
        public Nullable<int> BasketCount { get; set; }
        public Nullable<int> UserId { get; set; }
        public string PostalCode { get; set; }
        public string ClientLogo { get; set; }
        public string ClientBanner { get; set; }
        public string ClientFBUrl { get; set; }
        public string ClientInstaUrl { get; set; }
        public string Status { get; set; }
        public string ClientTwitterUrl { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Domain { get; set; }
        public int CatUserId { get; set; }
        public bool IsActive { get; set; }
        public List<Category> lstCategory{ get; set; }
        public List<Industry> lstIndustry { get; set; }
        [JsonIgnore]
        public HttpPostedFileBase ImageFileLogo { get; set; }
        [JsonIgnore]
        public HttpPostedFileBase ImageFileBanner { get; set; }
        [Display(Name = "From Date")]
        [DisplayFormat(DataFormatString = "{0: dd/MM/yyyy}")]

        public DateTime FromDate { get; set; }
        [Display(Name = "To Date")]
        [DisplayFormat(DataFormatString = "{0: dd/MM/yyyy}")]
        public DateTime ToDate { get; set; }
        [Display(Name = "Offer Start Date")]
        [DisplayFormat(DataFormatString = "{0: dd/MM/yyyy}")]

        public DateTime OfferStartDate { get; set; }
        [Display(Name = "Offer End Date")]
        [DisplayFormat(DataFormatString = "{0: dd/MM/yyyy}")]
        public DateTime OfferEndDate { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
