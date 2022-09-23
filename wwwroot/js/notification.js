"use strict";
var connection = new signalR.HubConnectionBuilder().withUrl("/ProgressHub").build();
connection.on("sendToUser", (validation,tasknumber, totalTasks) => {
    var pct = Math.round(tasknumber / totalTasks * 100);
    document.getElementById("articleList").innerHTML = validation + "  ~  "+tasknumber + " out of " + totalTasks + " ( " + pct + "% )";
    if (pct === 100) document.getElementById("vd").style.visibility = "visible";
    $('#progBarValidation').css('width', pct + '%').attr('aria-valuenow', pct);
});

connection.on("sendToProcessing", (tankid,tasknumber, totalTasks) => {
    var pct = Math.round(tasknumber / totalTasks * 100);
    document.getElementById("processingList").innerHTML = "TankID: "+tankid+ "  ~  "+ tasknumber + " out of " + totalTasks + " ( " + pct + "% )";
    if (pct === 100) document.getElementById("pd").style.visibility = "visible";
    $('#progBarProcessing').css('width', pct + '%').attr('aria-valuenow', pct);
});
connection.start().catch(function (err) {
    return console.error(err.toString());
});