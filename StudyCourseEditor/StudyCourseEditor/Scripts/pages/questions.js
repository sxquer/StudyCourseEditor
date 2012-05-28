/*
Notifications will work only with answers.js!!!!
For standalone work, include noty files!
*/
$(function () {
    $("#QuestionTypeID").change(function () {
        var val = $("#QuestionTypeID option:selected").val();
        $("#helpButton").attr("href", "/Question/QuestionTypeAjaxHelp?typeId=" + val);

        //TODO: Redo
        if (val == 3) {
            $("#answers :checkbox").each(function () {
                $(this).attr("disabled", "disabled");
            });
        } else {
            $("#answers :checkbox").each(function () {
                $(this).removeAttr("disabled");
            });
        }

    }).change();
});

function QuestionEditSuccess() {
    noty({
        "text": "Вопрос сохранен",
        "layout": "top",
        "type": "success",
        "animateOpen": { "height": "toggle" },
        "animateClose": { "height": "toggle" },
        "speed": 300,
        "timeout": 5000,
        "closeButton": false,
        "closeOnSelfClick": true,
        "closeOnSelfOver": false,
        "modal": false
    });
}

function QuestionEditFailure() {
    noty({
        "text": "Произошла ошибка. Вопрос не был сохранен",
        "layout": "top",
        "type": "error",
        "animateOpen": { "height": "toggle" },
        "animateClose": { "height": "toggle" },
        "speed": 300,
        "timeout": 5000,
        "closeButton": false,
        "closeOnSelfClick": true,
        "closeOnSelfOver": false,
        "modal": false
    });
}

function HelpSuccess(response) {
    noty({ 
        "text": response,
        "layout": "top",
        "type": "information",
        "animateOpen": { "height": "toggle" },
        "animateClose": { "height": "toggle" },
        "speed": 300,
        "timeout": 5000,
        "closeButton": false,
        "closeOnSelfClick": true,
        "closeOnSelfOver": false, 
        "modal": false });
}

function HelpFailure() {
    noty({
        "text": "Произошла непредвиденная ошбика (#HelpFailure)",
        "layout": "top",
        "type": "error",
        "animateOpen": { "height": "toggle" },
        "animateClose": { "height": "toggle" },
        "speed": 300,
        "timeout": 5000,
        "closeButton": false,
        "closeOnSelfClick": true,
        "closeOnSelfOver": false,
        "modal": false
    });
}