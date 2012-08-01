var taskHub = $.connection.taskHub;

function startSignalRConnection(projectId, userName, isNews) {
    $.connection.hub.start(function () {
        taskHub.join($.connection.hub.id, projectId, userName, isNews);
    });
}

taskHub.TaskCreated = function (taskId, managerId) {
     $.ajax({
        type: "POST",
        url: "/Project/TaskView",
        data: { taskId: taskId },
        dataType: 'html',
        success: function (response1) {
            var task = "<div class=\"task-holder\" data-taskid=" + taskId + " id =" + taskId + ">";
            task += response1;
            task += "</div>";
            jQuery("[data-managerid=" + managerId + "]").html(task);
            jQuery("[data-taskid=" + taskId + "]").insertBefore(jQuery("[data-managerid=" + managerId + "]"));
            jQuery("[data-taskid=" + taskId + "]").effect("shake", { times: 3 }, 300);
        }
    });
};

taskHub.TaskMoved = function (movedtaskId, moveToId) {
    jQuery("[data-taskid=" + movedtaskId + "]").fadeOut();
    setTimeout(function () {
        jQuery("[data-taskid=" + movedtaskId + "]").insertBefore(jQuery("[data-managerid=" + moveToId.toString() + "]"));
        jQuery("[data-taskid=" + movedtaskId + "]").fadeIn();
    },
                300);
};


taskHub.SetUnreadNewsesCount = function (count) {;
    jQuery("[data-id = newses_count]").html(count);
    jQuery("[data-id = newses_count]").effect("highlight");
};