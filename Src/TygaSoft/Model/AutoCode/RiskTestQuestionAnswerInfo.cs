using System;

namespace TygaSoft.Model
{
    [Serializable]
    public partial class RiskTestQuestionAnswerInfo
    {
	    public Guid Id { get; set; }

        public Guid UserId { get; set; } 

public Guid QuestionId { get; set; } 

public string AnswerResult { get; set; } 

public DateTime LastUpdatedDate { get; set; } 
    }
}
