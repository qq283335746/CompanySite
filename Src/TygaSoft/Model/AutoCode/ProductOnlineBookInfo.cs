using System;

namespace TygaSoft.Model
{
    [Serializable]
    public partial class ProductOnlineBookInfo
    {
	    public Guid Id { get; set; }

        public string CustomerName { get; set; } 

public string ClientType { get; set; } 

public string TelPhone { get; set; } 

public string MobilePhone { get; set; } 

public string Fax { get; set; } 

public string Email { get; set; } 

public string Address { get; set; } 

public string BookProduct { get; set; } 

public Decimal Price { get; set; } 

public DateTime LastUpdatedDate { get; set; } 
    }
}
