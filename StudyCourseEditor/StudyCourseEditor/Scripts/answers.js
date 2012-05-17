function GetFileConection(filename, filetype) {
    var fileref;
    if (filetype == "js") {
        return "<script src='" + filename + "'></script>";
    } else if (filetype == "css") { 
        return " <link href='" + filename + "' rel='stylesheet' type='text/css' />";
    }
}

document.write(GetFileConection("/Scripts/noty/js/jquery.noty.js", "js"));
document.write(GetFileConection("/Scripts/noty/css/jquery.noty.css", "css"));
document.write(GetFileConection("/Scripts/noty/css/noty_theme_default.css", "css"));

/*
function AnswerProcessSuccess(ajaxResponse) {
    ShowNoty(xhr[2], (xhr[3] == "0") ? "success" : "error");

    if (xhr[4] == "add") AddToView(xhr[5], xhr[6]);
    if (xhr[4] == "delete") DeleteFromView(xhr[5]);
}*/

function AnswerProcessSuccess(response) {
    ShowNoty(response["message"], (response["success"] == "true") ? "success" : "error");

    if (response["success"] == "true") {
        if (response["actionType"] == "create") AddToView(response["answerID"], response["body"]);
        if (response["actionType"] == "delete") DeleteFromView(response["answerID"]);
    }
}

function AnswerProcessFailed() {
    ShowNoty("Во время выполнения Ajax произошла ошибка", "error");
}

function ShowNoty(text, type) {
    noty({
        "text": text,
        "layout": "bottomRight",
        "type": type,
        "animateOpen": { "height": "toggle" },
        "animateClose": { "height": "toggle" },
        "speed": 300,
        "timeout": 2000,
        "closeButton": false,
        "closeOnSelfClick": true,
        "closeOnSelfOver": true,
        "modal": false
    });
}

function DeleteFromView(id) {
    $("#answer_" + id).remove();
}

function AddToView(id, body) {
    var newAns = $("#answer_template").clone();

    $("#existing-answers-placeholder").append(newAns);
    newAns.attr("id", "answer_" + id);
    newAns.css("display", "block");

    newAns.html(newAns.html().replace("value_placeholder", body).replace("id_placeholder", id));
    
    $("#NewAnswer").val("");
}