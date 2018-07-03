using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StubAPI.Models
{
    public class NotificationDetails
    {
       
       public int ? uID { get; set; }
        public string  notificationType { get; set; }
        public string notificationTitle { get; set; }
        public string text { get; set; }
       // public string data { get; set; }
        
        public string notificationPhoto { get; set; }
        public bool ? IsDone { get; set; }
        public int ? notificationID { get; set; }
        public bool ? dismiss { get; set; }
        public int  contactId { get; set; }
        public string timeSent { get; set; }
        

        public ViewData data { get; set; }


    }
    public class ViewData
    {
        public string target { get; set; }
        public int ? LocationId { get; set; }
        public int ? CatId { get; set; }
        public int ? SubCatId { get; set; }
        public int ? MCId { get; set; }
        public string SubCategoryName { get; set; }
        public string CategoryName { get; set; }
        public string LocationName { get; set; }
        public string MCName { get; set; }
        public string addedBy { get; set; }

        public string addedWhen { get; set; }
        public string redirectTo { get; set; }

    }
    public class PushNotification
    {
        public string[] registration_ids { get; set; }
        public Notifications notification { get; set; }
        public PushData data { get; set; }

    }
    public class Notifications
    {
        public string title { get; set; }
        public string body { get; set; }
        public string sound { get; set; }
        public string vibrate { get; set; }

        public string priority { get; set; }

    }
    public class PushData
    {
        public string title { get; set; }
        public string body { get; set; }
        public string date { get; set; }
        public int? catId { get; set; }
        public int? subCatId { get; set; }
        public int? microId { get; set; }
        public int? locationId { get; set; }


    }
}