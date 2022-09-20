"use strict";
var connection = new signalR.HubConnectionBuilder().withUrl("/ProgressHub").build();
connection.on("sendToUser", (tasknumber, totalTasks) => {
    var heading = document.createElement("h3");
    var pct = Math.round(tasknumber / totalTasks * 100);

    heading.textContent = tasknumber + "##############" + totalTasks;
    var div = document.createElement("div");
    div.appendChild(heading);

    document.getElementById("articleList").innerHTML = tasknumber + " out of " + totalTasks + " ( " + pct + "% )";
    if (pct == 100) document.getElementById("vd").style.visibility = "visible";
    $('#progBarValidation').css('width', pct + '%').attr('aria-valuenow', pct);
});

connection.on("sendToProcessing", (tasknumber, totalTasks) => {
    var heading = document.createElement("h3");
    var pct = Math.round(tasknumber / totalTasks * 100);

    heading.textContent = tasknumber + "##############" + totalTasks;
    var div = document.createElement("div");
    div.appendChild(heading);

    document.getElementById("processingList").innerHTML = tasknumber + " out of " + totalTasks + " ( " + pct + "% )";
    if (pct === 100) document.getElementById("pd").style.visibility = "visible";
    $('#progBarProcessing').css('width', pct + '%').attr('aria-valuenow', pct);
});
connection.start().catch(function (err) {
    return console.error(err.toString());
});