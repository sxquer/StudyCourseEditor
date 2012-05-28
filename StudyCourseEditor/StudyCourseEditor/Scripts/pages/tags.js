function TagProcessSuccess(response) {
    ShowNoty(response["message"], (response["success"] == "true") ? "success" : "error");

    if (response["success"] == "true") {
        if (response["actionType"] == "create") Tag_AddToView(response["id"], response["body"]);
        if (response["actionType"] == "delete") Tag_DeleteFromView(response["id"]);
    }
}

function TagProcessFailed() {
    ShowNoty("Во время выполнения Ajax произошла ошибка", "error");
}


function Tag_DeleteFromView(id) {
    $("#tag_" + id).remove();
}

function Tag_AddToView(id, body, isAntiExample) {
    var newAns = $("#tag_template").clone();

    var column = $("#tags");

    column.find("#existing").append(newAns);

    newAns.attr("id", "tag_" + id);
    newAns.css("display", "block");

    newAns.html(newAns.html().replace("value_placeholder", body).replace("id_placeholder", id));

    column.find("#new").find("#tagName").val("");

}