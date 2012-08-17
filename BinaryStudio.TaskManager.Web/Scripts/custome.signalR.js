var taskHub = $.connection.taskHub;

function startSignalRConnection(projectId, userName, isClient) {
    $.connection.hub.start(function () {
        taskHub.join($.connection.hub.id, projectId, userName, isClient);
    }).done();
}


taskHub.TaskCreated = function (taskIn) {
    $.ajax({
        type: "POST",
        url: "/Project/TaskView",
        data: { taskId: taskIn.Id },
        dataType: 'html',
        success: function (response1) {
            var managerId = taskIn.AssigneeId == null ? 0 : taskIn.AssigneeId;
            var task = "<div class=\"task-holder\" data-taskid=" + taskIn.Id + " id =" + taskIn.Id + ">";
            task += response1;
            task += "</div>";
            jQuery("[data-managerid=" + managerId + "]").html(task);
            jQuery("[data-taskid=" + taskIn.Id + "]").insertBefore(jQuery("[data-managerid=" + managerId + "]"));
            jQuery("[data-taskid=" + taskIn.Id + "]").effect("shake", { times: 2 }, 200);
        }
    });
};

taskHub.TaskMoved = function (movedTaskId, moveToUser) {
    jQuery("[data-taskid=" + movedTaskId + "]").fadeOut();
    var userId = moveToUser.Id != null ? moveToUser.Id.toString() : '0';
    setTimeout(function () {
        jQuery("[data-taskid=" + movedTaskId + "]").insertBefore(jQuery("[data-managerid=" + userId + "]"));
        jQuery("[data-taskid=" + movedTaskId + "]").fadeIn();
    }, 300);
};


taskHub.SetUnreadNewsesCount = function (count) {
    jQuery("[data-id = newses_count]").html(count);
    jQuery("[data-id = newses_count]").effect("highlight");
    if(count == 0) {
        jQuery("[data-id = newses_count]").fadeOut();
    }
};