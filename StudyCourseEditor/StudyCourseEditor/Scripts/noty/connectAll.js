﻿function GetFileConection(filename, filetype) {
    if (filetype == "js") {
        return "<script src='" + filename + "'></script>";
    } else if (filetype == "css") {
        return " <link href='" + filename + "' rel='stylesheet' type='text/css' />";
    }
}

document.write(GetFileConection("/Scripts/noty/js/jquery.noty.js", "js"));
document.write(GetFileConection("/Scripts/noty/css/jquery.noty.css", "css"));
document.write(GetFileConection("/Scripts/noty/css/noty_theme_default.css", "css"));

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