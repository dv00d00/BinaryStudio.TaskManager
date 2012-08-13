var modelData = new TaskModel();
var toDashboard = function (element, where) {
    var num = element.parent("div").attr("data-id");
    if (where == "here"){
        $("*").css("cursor", "progress");
        location.href = "/Project/Project/" + num;
    }
    else
        window.open("/Project/Project/" + num);
};
$(function () {
    /*********** To dashboard clicks handling ************/
    $(document).on("dblclick", ".project_list .project_name", function () {
        toDashboard($(this), "here");
    });
    $(document).on("mousedown", ".project_list .project_name", function (e) {
        if (e.which == 2) {
            toDashboard($(this), "there");
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
    $(document).on("click", ".img_holder,.add_task_assignee_photo", function (e) {
        var top;
        var left;
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
        if ($("#content h2").attr("data-id") != -1)
            user_list_holder.css({
                "display": "block",
                "top": top,
                "left": left
            });
        scrollPresence();
        $(".user_list_input").focus();
        taskId = $(this).parent("div").parent("div").attr("data-id");
        user_list_holder.removeClass("instant_task_move");
        user_list_holder.removeClass("assignee_to_choose");
        if ($(this).hasClass("img_holder")) {
            user_list_holder.addClass("instant_task_move");
        }
        else {
            user_list_holder.addClass("assignee_to_choose");
        }
    });
    $(document).keyup(function (e) {
        var code = null;
        code = (e.keyCode ? e.keyCode : e.which);
        if (code == 27) {
            $(".user_list_holder").hide();
            clearInput();
        }
        $("user_list_holder").off();
    });
    $(".user_list_close").click(function () {
        $(".user_list_holder").hide();
        clearInput();
    });

    $(document).on("click.user_list", ".assignee_to_choose li", function () {
        var self = $(this);
        var userId = self.attr('data-id');
        $(".user_list_holder").hide();
        $(".add_task_assignee_photo").html("");
        self.children("img").clone().appendTo($(".add_task_assignee_photo"));
        $(".add_task_assignee").attr("data-id", userId);
        $(".add_task_assignee_name span").html(self.children("span").text());
    });

    $(document).on("click.user_list", ".instant_task_move li", function () {
        var userId = $(this).attr('data-id');
        moveTask(-1, userId, taskId);

    });
    $(document).on("click.user_list", ".instant_task_move .user_list_clear", function () {
        moveTask(-1, -1, taskId);
    });

    $('.user_list_input').keyup(function () {
        var search_val = $(this).val();
        for (var i = 0; i < modelData.users().length; i++) {
            modelData.users()[i]._destroy = false;
        }
        modelData.users.destroy(function (item) {
            return item.Name.indexOf(search_val) == -1;
        });
        scrollPresence();
    });

    /**** Tooltip  ******/
    $('.dashboard_btn').tooltip({
        position: 'left',
        delay: { show: 1000 }
    });
    /***** Filter  *******/
    $("#left_panel>input").val("Filter tasks");

    $("#left_panel>input").on("blur", function () {
        if ($(this).val() == "")
            $(this).val("Filter tasks");
    });

    $("#left_panel>input").on("click", function () {
        if ($(this).val() == "Filter tasks")
            $(this).val("");
    });

    /******** Project name length *******/

    $(".project_name").each(function () {
        var text = $(this).html();
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
    });

    $(document).on("click", ".delete_btn", function () {
        var proj = $(this).parent(".proj_row").attr("data-id");
        $(".modal-body").html("<p>Are you sure, that you want to delete project</p>\
            <span> <b>" + $(this).parent(".proj_row").children(".project_name").html() + "</b>?</span>");
        $("#delete_box").modal('show');
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
            } else if (code == 13) {
                sendNewProject();
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
    var homeProj = $("#projects .proj_row:first-child").attr("data-id");
    $("#projects .proj_row:first-child").children(".project_name").addClass("active_proj");
    getTaskList(homeProj);
    ko.applyBindings(modelData);
    $('body').popover({
        selector: '.task_popover',
        delay: { show: 100 },
        placement: 'top'
    });

    $(document).on("click", ".task_group_list .project_name", function () {
        $(".new_task_btn").hide();
        $(".project_name").removeClass("active_proj");
        $(this).addClass("active_proj");
        var url = null;
        switch ($(this).attr("id")) {
            case "all_tasks": url = "all"; break;
            case "my_tasks": url = "my"; break;
            case "unassigned_tasks": url = "unassigned"; break;
        }
        var groupName = $(this).html();
        getTaskGroup(url, groupName);
    });
    $(document).on("click", ".user_projs,.created_projs", function () {
        $(".new_task_btn").show();
        $(".project_name").removeClass("active_proj");
        $(this).addClass("active_proj");
        var proj = $(this).parent(".proj_row").attr("data-id");
        getTaskList(proj);
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
    $(document).on("keyup", function (e) {
        if (e.which == 13) {
            newTaskInputEnter();
        }
    });
    $(".new_task_name").keyup(function (e) {
        if (e.which == 27)
            newTaskInputEscape();
    });
});

/******************** Functions ************************/
function getTaskList(proj) {
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
                    Creator: task.Creator,
                    Priority: task.Priority,
                    AssigneeId: task.AssigneeId,
                    Assignee: task.Assignee,
                    AssigneePhoto: task.AssigneePhoto
                });
                modelData.tasks.push(thisTask);
            }
            modelData.allTasks();
            for (var i = 0; i < data.Users.length; i++) {
                var user = data.Users[i];
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

function getTaskGroup(url, groupName) {
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
            $("#loader").show();
        },
        success: function (response) {
            $("#loader").hide();
            listProjects(response);
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
            $("#loader").show();
        },
        success: function (response) {
            $("#loader").hide();
            listProjects(response);
        }
    });
}

function listProjects(response) {
    var list = $(".project_list");
    list.html("");
    var name = null;
    var creatorProjectsCount = projectsOutput(response.CreatorProjects, "created_projs");
    var userProjectsCount = projectsOutput(response.UserProjects, "user_projs");
    if (userProjectsCount + creatorProjectsCount == 0) {
        $(".project_list").html("<span>There are no projects yet</span>");
    }
    $('.project_name').each(function () {
        if ($(this).parent("div").attr("data-id") == $('#content h2').attr("data-id")) {
            $(this).addClass('active_proj');
        }
    });
}

function projectsOutput(projects, li_class) {
    var name = null;
    for (var i = 0; i < projects.length; i++) {
        if (projects[i].Name.length < 20)
            name = projects[i].Name;
        else
            name = projects[i].Name.substr(0, 20) + '...';
        var ownProjDelete = (li_class == 'user_projs') ? "" : "<div class='delete_btn'></div>";
        $(".project_list").append("\
        <div class='proj_row' data-id='" + projects[i].Id + "'>\
                         " + ownProjDelete + "\
                         <div  class='" + li_class + " project_name'>" + name + "</div>\
                     </div>");
         if (projects[i].Name.length >= 20)
            $("li[data-id=" + projects[i].Id + "]").attr("title", projects[i].Name);
        $('.dashboard_btn').tooltip({
            'placement': 'right',
            delay: { show: 1000 }
        });
    }
    return i;
}

function scrollPresence()
{
    var user_list = $(".user_list");
    if (user_list.height() < 250) {
        user_list.css("overflow-y", "hidden");
    }
    else {
        user_list.css("overflow-y", "scroll");
    }
}

function clearInput() {
    $(".user_list_input").val("");
    for (var i = 0; i < modelData.users().length; i++) {
        modelData.users()[i]._destroy = false;
    }
}

function newTaskInputEnter() {
    $(".new_task_btn").hide();
    $("#add_task_box").show();
    $(".new_task_name").focus();
}

function newTaskInputEscape() {
    $("#add_task_box").hide();
    $(".new_task_btn").show();
    $(".new_task_name").val("");
    $(".add_task_assignee_photo").html("");
    $(".add_task_assignee span").html("");
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
            location.reload(true);
        }
    });
}

function addTask() {
    var task_name = $("#add_task_box input").val();
    var priority = $(".priority_buttons .btn.active").val();
    if (task_name != "") {
        var thisTask = new Object({
            Name: task_name,
            Priority: priority,
            AssigneeId: $(".add_task_assignee").attr("data-id"),
            ProjectId: $("#content h2").attr("data-id")
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
                location.reload(true);
            }
        });
    }
    else {
        $(".new_task_name").focus();
    }
}