var ShowRiskTestQuestionAnswer = {
    Init: function () {
        this.InitAnswer();
    },
    InitAnswer: function () {
        if ($("#hQuestionId").val() != $("#hAnswerQuestionId").val()) {
            $.messager.alert('错误提醒', "当前题目与答题题目已不一致，无法查看结果", 'error');
            return false;
        }
        var xml = $("#xmlAnswer").html();
        var rows = $("#questionList").find(".row");
        $(xml).find("xel").each(function (i, item) {
            var title = $.trim($(item).find("title").text());
            var answer = $.trim($(item).find("answer").text());

            rows.each(function () {
                var currRow = $(this);
                var qTitle = $.trim(currRow.find(".title").text());
                if ($.trim(currRow.find(".title").text()) == title) {
                    currRow.find("label").each(function () {
                        if ($.trim($(this).text()) == answer) {
                            $(this).prev().attr("checked", "checked");
                        }
                    })
                }
            })
        })
    }
}