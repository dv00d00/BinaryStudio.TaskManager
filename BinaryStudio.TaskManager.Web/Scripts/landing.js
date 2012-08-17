var toDashboard = function (num, where) {
    if (where == "here") {
        $("*").css("cursor", "progress");
        location.href = "/Project/Project/" + num;
    }
    else
        window.open("/Project/Project/" + num);
};
$(function () {
    taskHub.TaskMoved = function (movedTaskId, moveToUser) {
        var thisTask = ko.utils.arrayFirst(modelData.tasks(), function (task) {
            return task.Id == movedTaskId;
        });
        var index = modelData.tasks.indexOf(thisTask);
        modelData.tasks.replace(modelData.tasks()[index], {
            Id: thisTask.Id,
            Name: thisTask.Name,
            Description: thisTask.Description,
            CreatedDate: thisTask.CreatedDate,
            CreatedTime: thisTask.CreatedTime,
            CompareDate: thisTask.CompareDate,
            Creator: thisTask.Creator,
            Priority: thisTask.Priority,
            AssigneeId: moveToUser.Id,
            Assignee: moveToUser.Name,
            AssigneePhoto: moveToUser.Photo
        });
    };

    taskHub.TaskCreated = function (task) {
        var date = new Date(parseInt(task.Created.substr(6)));
        var thisTask = new Object({
            Id: task.Id,
            Name: task.Name,
            Description: task.Description,
            CreatedDate: date.toLocaleDateString(),
            CreatedTime: date.toLocaleTimeString(),
            CompareDate: date,
            Creator: task.Creator,
            Priority: task.Priority,
            AssigneeId: task.AssigneeId,
            Assignee: task.Assignee,
            AssigneePhoto: task.AssigneePhoto
        });
        modelData.tasks.push(thisTask);
    };
    /*********** To dashboard clicks handling ************/
    $(document).on("dblclick", ".project_list .project_name", function () {
        toDashboard($(this).parent("div").attr("data-id"), "here");
    });
    $(document).on("mousedown", ".project_list .project_name", function (e) {
        if (e.which == 2) {
            toDashboard($(this).parent("div").attr("data-id"), "there");
        }
    });

    /************** Assign menu **************/
    $(document).on("mouseover", ".appointive", function () {
        $(this).parent("div").parent("div").children("span").css("left", "2px");
    });
    $(document).on("mouseout", ".appointive", function () {
        $(this).parent("div").parent("div").children("span").css("left", "0px");
    });
    var taskId = null;
    $(document).on("click", ".img_holder", function (e) {
        $(".user_list_title").text("Assign someone to this task");
        modelData.allUsers();
        showUserHolder(e);
        $(".user_list_holder").addClass("instant_task_move");
        taskId = $(this).parent("div").parent("div").attr("data-id");
    });
    $(document).on("click", ".add_task_assignee_photo", function (e) {
        $(".user_list_title").text("Assign someone to this task");
        modelData.allUsers();
        showUserHolder(e);
        $(".user_list_holder").addClass("assignee_to_choose");
    });
    $(document).on("click", ".assignee_btn", function (e) {
        modelData.allUsers();
        var user_list_holder = $(".user_list_holder");
        $(".user_list_clear").hide();
        if (user_list_holder.hasClass("open") == false) {
            $(".user_list_title").text("Show tasks for assignee");
            showUserHolder(e);
            user_list_holder.addClass("open");
            $(".user_list_holder").addClass("assignee_filter");
        }
        else {
            user_list_holder.removeClass("open");
            hideUserHolder();
        }
        $(".user_list_clear").toggle();
    });

    $(document).keyup(".user_list_input", function (e) {
        var code = null;
        code = (e.keyCode ? e.keyCode : e.which);
        if (code == 27) {
            hideUserHolder();
        }
        $("user_list_holder").off();
    });
    $(".user_list_close").click(function () {
        hideUserHolder();
    });

    $(document).on("click.user_list", ".assignee_to_choose li", function () {
        var self = $(this);
        var userId = self.attr('data-id');
        hideUserHolder();
        $(".new_task_name").focus();
        $(".add_task_assignee_photo").html("");
        self.children("img").clone().appendTo($(".add_task_assignee_photo"));
        $(".add_task_assignee").attr("data-id", userId);
        $(".add_task_assignee_name span").html(self.children("span").text());
    });
    $(document).on("click.user_list", ".assignee_to_choose .user_list_clear", function () {
        hideUserHolder();
        $(".add_task_assignee").attr("data-id", "");
        $(".add_task_assignee_photo").html("");
        $(".add_task_assignee span").text("No one");
    });
    $(document).on("click.user_list", ".instant_task_move li", function () {
        var userId = $(this).attr('data-id');
        moveTask(-1, userId, taskId);

    });

    $(document).on("click.user_list", ".assignee_filter li", function () {
        var self = $(this);
        var task = {
            AssigneeId: self.attr('data-id'),
            Assignee: self.children("span").text()
        };
        modelData.filterByAssignee(task);
        hideUserHolder();
    });
    $(document).on("click.user_list", ".instant_task_move .user_list_clear", function () {
        moveTask(-1, -1, taskId);
    });

    $('.user_list_input').keyup(function () {
        scrollPresence();
    });

    /**** Tooltip  ******/
    $('.dashboard_btn').tooltip({
        position: 'left',
        delay: { show: 1000 }
    });

    /******* Shortcuts ********/
    $(document).on("keyup.short", function (e) {
        bindShortcuts(e);
    });

    $(document).on("focus", "input", function () {
        $(document).off(".short");
    });
    $(document).on("blur", "input", function () {
        $(document).on("keyup.short", function (e) {
            bindShortcuts(e);
        });
    });
    function bindShortcuts(e) {
        var code = (e.keyCode ? e.keyCode : e.which);
        switch (code) {
            case 13:
                newTaskInputEnter();
                break;
            case 84:
                newTaskInputEnter();
                break;
            case 70:
                $("#filterByName").focus();
                break;
            case 83:
                $(".sort_btn").toggleClass("open");
                break;
            case 80:
                modelData.sortByPriority();
                break;
            case 65:
                modelData.sortByAssignee();
                break;
            case 67:
                modelData.sortByDate();
                break;
            case 68:
                var num = modelData.Active();
                if ((num != "") && (num != '-1') && (num != null))
                    toDashboard(num, "here");
                break;
            case 78:
                modelData.sortByName();
                break;
            case 75:
                $("#shortcuts_window").modal('show');
                break;
            case 77:
                $(".new_task_btn").hide();
                modelData.Active(-1);
                $(".project_name").removeClass("active_proj");
                $("#my_tasks").addClass("active_proj");
                getTaskGroup("my");
                $(".assignee_btn").hide();
                break;
            case 85:
                $(".new_task_btn").hide();
                modelData.Active(-1);
                $(".project_name").removeClass("active_proj");
                $("#unassigned_tasks").addClass("active_proj");
                getTaskGroup("unassigned");
                $(".assignee_btn").hide();
                break;
        }
    }

    $(".keyboard").click(function () {
        $("#shortcuts_window").modal('show');
    });
    /******** Project name length *******/

    $(".project_name").each(function () {
        var text = $(this).text();
        if (text.length > 20) {
            $(this).attr("title", text);
            $(this).html(text.substr(0, 20) + "...");
        }
    });
    if ($(".project_name").length == 0)
        $(".project_list").html("<span>There are no projects yet</span>");
    /********* Delete project ********/

    $("#delete_box").on('hidden', function () {
        $(".yes").off();
        $(window).off("keydown");
        $(document).on("focus", "input", function () {
            $(document).off(".short");
        });
    });

    $(document).on("click", ".delete_btn", function () {
        var proj = $(this).parent(".proj_row").attr("data-id");
        $(".modal-body").html("<p>Are you sure, that you want to delete project</p>\
            <span> <b>" + $(this).parent(".proj_row").children(".project_name").html() + "</b>?</span>");
        $("#delete_box").modal('show');
        $(document).off(".short");
        $(".yes").on("click", function () {
            deleteProjectQuery(proj);
            $(".yes").off();
        });
        $(window).on("keydown", function (e) {
            var code = null;
            code = (e.keyCode ? e.keyCode : e.which);
            if (code == 13) {
                deleteProjectQuery(proj);
                $("#delete_box").modal('hide');
            }
            $(window).off("keydown");
            setTimeout(function () {
                $(document).on("keyup.short", function (d) {
                    bindShortcuts(d);
                });
            }, 300);
        });
    });

    /******** Add Project *********/
    $(".new_project_btn").click(function () {
        if ($(".newBox").length == 0) {
            $(".project_list").prepend($("#add_proj_box").clone());
            $("#add_proj_box").addClass("newBox");
            $(".newBox").css("display", "block");
        } else {
            $(".newBox").children("input").focus();
        }
        $(".add_cancel").click(function () {
            $(".newBox").remove();
        });
        $(".add_proj_btn").click(function () {
            sendNewProject();
        });
        $(".newBox").children("input").keyup(function (e) {
            var code = null;
            code = (e.keyCode ? e.keyCode : e.which);
            if (code == 27) {
                $(".newBox").remove();
                $("input").blur();
            }
            else if (code == 13) {
                sendNewProject();
                $(".user_list_input").focus();
                setTimeout(function () { $("input").blur(); }, 100);
            }
        });
        $(".add_proj_btn").prop("disabled", "true");
        $("#add_proj_box input").keyup(function () {
            if ($(this).val() != "") {
                $(".add_proj_btn").removeAttr("disabled");
            }
            else {
                $(".add_proj_btn").prop("disabled", "true");
            }
        });
    });

    /********* Project Tasks View  *******/
    var homeProj = -1;
    if (modelData.projects().length != 0) {
        homeProj = modelData.projects()[0].Id;
        modelData.Active(homeProj);
    }
    startSignalRConnection(homeProj, user, false);
    ko.applyBindings(modelData);
    setTimeout(modelData.data.start(false), 200);
    $('body').popover({
        selector: '.task_popover',
        delay: { show: 100 },
        placement: 'top'
    });

    $(document).on("click", ".task_group_list .project_name", function () {
        $(".new_task_btn").hide();
        // $(".project_name").removeClass("active_proj");
        // $(this).addClass("active_proj");
        var url = null;
        switch ($(this).attr("id")) {
            case "all_tasks": url = "all"; break;
            case "my_tasks": url = "my"; break;
            case "unassigned_tasks": url = "unassigned"; break;
        }
        getTaskGroup(url);
        $(".assignee_btn").hide();
    });
    $(document).on("click", ".user_projs,.created_projs", function () {
        $(".new_task_btn").show();
        $(".assignee_btn").show();
        var proj = $(this).parent(".proj_row").attr("data-id");
        modelData.Active(proj);
    });

    /*********** Add Task ****************/
    $(".new_task_btn").click(function () {
        newTaskInputEnter();
    });
    $(".add_task_cancel").click(function () {
        newTaskInputEscape();
    });
    $(".priority_buttons .btn").click(function () {
        $(".prior_name").html($(this).attr("data-prior"));
    });
    $(".add_task_btn").click(function () {
        addTask();
    });
    $(".new_task_name").keyup(function (e) {
        if (e.which == 13) {
            addTask();
        }
    });
    $(".new_task_name").keyup(function (e) {
        if (e.which == 27)
            newTaskInputEscape();
    });

});
/******************** Functions ************************/
function getTaskList(proj) {
    if (modelData.data.start() == false)
        taskHub.changeProject($.connection.hub.id, proj);
    $.ajax({
        data: { projectId: proj },
        dataType: "JSON",
        type: "GET",
        url: "/Landing/GetTasks",
        beforeSend: function () {
            //$("#loader").show();
        },
        success: function (data) {
            var task = null;
            modelData.project(data.Name);
            modelData.projectId(data.Id);
            modelData.tasks.removeAll();
            modelData.users.removeAll();
            for (var i = 0; i < data.Tasks.length; i++) {
                task = data.Tasks[i];
                var date = new Date(parseInt(task.Created.substr(6)));
                var thisTask = new Object({
                    Id: task.Id,
                    Name: task.Name,
                    Description: task.Description,
                    CreatedDate: date.toLocaleDateString(),
                    CreatedTime: date.toLocaleTimeString(),
                    CompareDate: date,
                    Creator: task.Creator,
                    Priority: task.Priority,
                    AssigneeId: task.AssigneeId,
                    Assignee: task.Assignee,
                    AssigneePhoto: task.AssigneePhoto
                });
                modelData.tasks.push(thisTask);
            }
            modelData.allTasks();
            for (var j = 0; j < data.Users.length; j++) {
                var user = data.Users[j];
                var thisUser = new Object({
                    Id: user.Id,
                    Name: user.Name,
                    Photo : user.Photo==true? '/Project/GetImage?UserId='+user.Id : '/Content/images/photo.png'
                });
                modelData.users.push(thisUser);
            }
        }
    });
}

function getTaskGroup(url) {
    $.ajax({
        data: { 'projectId'   : "-1",
                'taskGroup'   : url},
        dataType: "JSON",
        type: "GET",
        url: "/Landing/GetTasks",
        success: function (data) {
            var task = null;
            modelData.project(data.Name);
            modelData.projectId(data.Id);
            modelData.tasks.removeAll();
            modelData.users.removeAll();
            for (var i = 0; i < data.Tasks.length; i++) {
                task = data.Tasks[i];
                var date = new Date(parseInt(task.Created.substr(6)));
                var thisTask = new Object({
                    Id: task.Id,
                    Name: task.Name,
                    Description: task.Description,
                    CreatedDate: date.toLocaleDateString(),
                    CreatedTime: date.toLocaleTimeString(),
                    CompareDate: date,
                    Creator: task.Creator,
                    Priority: task.Priority,
                    AssigneeId: task.AssigneeId,
                    Assignee: task.Assignee,
                    AssigneePhoto: task.AssigneePhoto
                });
                modelData.tasks.push(thisTask);
            }
            modelData.allTasks();
        }
    });
}

function sendNewProject() {
    var val = $(".newBox").children("input").val();
    $(".newBox").remove();
    $.ajax({
        data: { projectName: val },
        dataType: "JSON",
        type: "POST",
        url: "/Landing/AddProject",
        beforeSend: function () {
            $("body").css("cursor", "progress");
        },
        success: function (project) {
            $("body").css("cursor", "default");
            var proj = Object({
                Id: project.Id,
                Name: project.Name,
                CreatedByYou: true
            });
            modelData.projects.push(proj);
            modelData.Active(proj.Id);
        },
        error: function () {
            $("body").css("cursor", "default");
        }
    });
}


function deleteProjectQuery(proj) {
    $.ajax({
        data: { projectId: proj },
        dataType: 'JSON',
        type: 'POST',
        url: '/Landing/DeleteProject',
        beforeSend: function () {
            $("body").css("cursor", "progress");
        },
        success: function () {
            $("body").css("cursor", "default");
            var thisProj = ko.utils.arrayFirst(modelData.projects(), function (project) {
                return project.Id == proj;
            });
            if (modelData.Active() == proj) {
                act = true;
            }
            modelData.projects.remove(thisProj);
            if (act) {
                if (modelData.projects().length > 0) {
                    modelData.Active(modelData.projects()[0].Id);
                }
                else {
                    modelData.Active(-1);
                    modelData.tasks.removeAll();
                }
            }
        },
        error: function () {
            $("body").css("cursor", "default");
        }
    });
}

function scrollPresence() {
    setTimeout(function() {
        var user_list = $(".user_list");
        if (user_list.height() < 250) {
            user_list.css("overflow-y", "hidden");
        } else {
            user_list.css("overflow-y", "scroll");
        }
    }, 50);
}

function newTaskInputEnter() {
    $(".new_task_btn").hide();
    $("#add_task_box").show();
    $(".new_task_name").focus();
}

function newTaskInputEscape() {
    $("input").blur();
    $("#add_task_box").hide();
    $(".new_task_btn").show();
    $(".new_task_name").val("");
    $(".add_task_assignee").attr("data-id", "");
    $(".add_task_assignee_photo").html("");
    $(".add_task_assignee span").text("No one");
} 

function moveTask(projectId, userId, taskId) {
    $.ajax({
        'data': { 'taskId': taskId,
            'senderId': -1,
            'receiverId': userId,
            'projectId': projectId
        },
        'dataType': 'JSON',
        'type': 'POST',
        'url': '/Project/MoveTask',
        beforeSend: function () {
            $("body").css("cursor", "progress");
            $(".user_list_holder").hide();
        },
        success: function () {
            $("body").css("cursor", "default");
        },
        error: function () {
            $("body").css("cursor", "default");
        }
    });
}

function addTask() {
    var task_name = $("#add_task_box input").val();
    var priority = $(".priority_buttons .btn.active").val();
    $("#add_task_box input").val("");
    $("#add_task_box input").focus();
    if (task_name != "") {
        var thisTask = new Object({
            Name: task_name,
            Priority: priority,
            AssigneeId: $(".add_task_assignee").attr("data-id"),
            ProjectId: modelData.Active()
        });
        $.ajax({
            'data': {
                'AssigneeId': thisTask.AssigneeId,
                'Name': thisTask.Name,
                'Priority': thisTask.Priority,
                'ProjectId': thisTask.ProjectId
            },
            'dataType': 'JSON',
            'type': 'POST',
            'url': '/Landing/CreateTask',
            beforeSend: function () {
                $('body').css('cursor', 'progress');
            },
            success: function () {
                $('body').css('cursor', 'default');
            },
            error: function () {
                $('body').css('cursor', 'default');
            }
        });
    }
    else {
        $(".new_task_name").focus();
    }
}

function showUserHolder(e) {
    var top;
    var left;
    var self = $(this);
    var user_list_holder = $('.user_list_holder');
    if ($(window).height() > (e.pageY + 200)) {//> (e.pageY + $('.user_list_holder').height())) {
        top = e.pageY + 25;
    }
    else {
        top = e.pageY - user_list_holder.height() - 25;
    }
    if ($(window).width() > (e.pageX + user_list_holder.width())) {
        left = e.pageX - 30;
    }
    else {
        left = e.pageX - 225;
    }
    if (modelData.Active() != -1)
        user_list_holder.css({
            "display": "block",
            "top": top,
            "left": left
        });
    scrollPresence();
    $(".user_list_input").focus();
    $(".user_list_clear").show();
    user_list_holder.removeClass("instant_task_move");
    user_list_holder.removeClass("assignee_to_choose");
    user_list_holder.removeClass("assignee_filter");
    user_list_holder.removeClass("open");
}

function hideUserHolder() {
    var user_list_holder = $('.user_list_holder');
    user_list_holder.hide();
    user_list_holder.removeClass("open");
    modelData.allUsers();
    $("input").blur();
}

modelData.Active.subscribe(function (newActive) {
    if (newActive != -1)
        getTaskList(newActive);
    else if (modelData.data.start() == false)
        taskHub.changeProject($.connection.hub.id, newActive);
        
});