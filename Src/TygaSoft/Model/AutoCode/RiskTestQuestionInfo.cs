using System;

namespace TygaSoft.Model
{
    [Serializable]
    public partial class RiskTestQuestionInfo
    {
	    public Guid Id { get; set; }

        public string QuestionXml { get; set; } 

public DateTime LastUpdatedDate { get; set; } 
    }
}
