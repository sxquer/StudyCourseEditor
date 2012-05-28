function ExampleProcessSuccess(response) {
    ShowNoty(response["message"], (response["success"] == "true") ? "success" : "error");

    if (response["success"] == "true") {
        if (response["actionType"] == "create") Example_AddToView(response["exampleID"], response["body"], response["isAntiExample"] == "true");
        if (response["actionType"] == "delete") Example_DeleteFromView(response["exampleID"]);
    }
}

function ExampleProcessFailed() {
    ShowNoty("Во время выполнения Ajax произошла ошибка", "error");
}

function Example_DeleteFromView(id) {
    $("#example_" + id).remove();
}

function Example_AddToView(id, body, isAntiExample) {
    var newAns = $("#example_template").clone();

    var column;

    if (isAntiExample) column = $("#antiexamples");
    else column = $("#examples");
    
    column.find("#existing").append(newAns);
    
    newAns.attr("id", "example_" + id);
    newAns.css("display", "block");

    newAns.html(newAns.html().replace("value_placeholder", body).replace("id_placeholder", id));

    column.find("#new").find("#Example").val("");

}